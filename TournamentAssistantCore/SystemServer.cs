﻿using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Open.Nat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TournamentAssistantCore.Discord;
using TournamentAssistantCore.Discord.Helpers;
using TournamentAssistantCore.Discord.Services;
using TournamentAssistantShared;
using TournamentAssistantShared.Models;
using TournamentAssistantShared.Models.Packets;
using TournamentAssistantShared.Sockets;
using TournamentAssistantShared.Utillities;
using static TournamentAssistantShared.Models.GameplayModifiers.Types;
using static TournamentAssistantShared.Models.Packets.Response.Types;
using static TournamentAssistantShared.Models.PlayerSpecificSettings.Types;
using static TournamentAssistantShared.SharedConstructs;

namespace TournamentAssistantCore
{
    public class SystemServer : INotifyPropertyChanged
    {
        Server server;
        WsServer wsServer;

        public event Func<Player, Task> PlayerConnected;
        public event Func<Player, Task> PlayerDisconnected;
        public event Func<Player, Task> PlayerInfoUpdated;
        public event Func<Match, Task> MatchInfoUpdated;
        public event Func<Match, Task> MatchCreated;
        public event Func<Match, Task> MatchDeleted;

        public event Func<SongFinished, Task> PlayerFinishedSong;

        public event Func<Acknowledgement, Guid, Task> AckReceived;

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        //Tournament State can be modified by ANY client thread, so definitely needs thread-safe accessing
        private State _state;
        public State State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                NotifyPropertyChanged(nameof(State));
            }
        }

        public User Self { get; set; }

        public QualifierBot QualifierBot { get; private set; }
        public Discord.Database.QualifierDatabaseContext Database { get; private set; }

        //Reference to self as a server, if we are eligible for the Master Lists
        public CoreServer CoreServer { get; private set; }

        //Server settings
        private Config config;
        private string address;
        private int port;
        private ServerSettings settings;
        private string botToken;

        //Update checker
        private CancellationTokenSource updateCheckToken = new();

        //Overlay settings
        private int overlayPort;

        public SystemServer(string botTokenArg = null)
        {
            config = new Config("serverConfig.json");

            var portValue = config.GetString("port");
            if (portValue == string.Empty)
            {
                portValue = "10156";
                config.SaveString("port", portValue);
            }

            var nameValue = config.GetString("serverName");
            if (nameValue == string.Empty)
            {
                nameValue = "Default Server Name";
                config.SaveString("serverName", nameValue);
            }

            var passwordValue = config.GetString("password");
            if (passwordValue == string.Empty || passwordValue == "[Password]")
            {
                passwordValue = string.Empty;
                config.SaveString("password", "[Password]");
            }

            var addressValue = config.GetString("serverAddress");
            if (addressValue == string.Empty || addressValue == "[serverAddress]")
            {
                addressValue = "[serverAddress]";
                config.SaveString("serverAddress", addressValue);
            }

            var scoreUpdateFrequencyValue = config.GetString("scoreUpdateFrequency");
            if (scoreUpdateFrequencyValue == string.Empty)
            {
                scoreUpdateFrequencyValue = "30";
                config.SaveString("scoreUpdateFrequency", scoreUpdateFrequencyValue);
            }

            var overlayPortValue = config.GetString("overlayPort");
            if (overlayPortValue == string.Empty || overlayPortValue == "[overlayPort]")
            {
                overlayPortValue = "0";
                config.SaveString("overlayPort", "[overlayPort]");
            }

            var botTokenValue = config.GetString("botToken");
            if (botTokenValue == string.Empty || botTokenValue == "[botToken]")
            {
                botTokenValue = botTokenArg;
                config.SaveString("botToken", "[botToken]");
            }

            var bannedModsValue = config.GetBannedMods();
            if (bannedModsValue.Length == 0)
            {
                bannedModsValue = new string[] { "IntroSkip", "AutoPauseStealth", "NoteSliceVisualizer", "SongChartVisualizer", "Custom Notes" };
                config.SaveBannedMods(bannedModsValue);
            }

            var enableTeamsValue = config.GetBoolean("enableTeams");

            var teamsValue = config.GetTeams();
            if (teamsValue.Length == 0)
            {
                //Default teams
                teamsValue = new Team[]
                {
                    new Team()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Team Green"
                    },
                    new Team()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Team Spicy"
                    }
                };
                config.SaveTeams(teamsValue);
            }

            settings = new ServerSettings
            {
                ServerName = nameValue,
                Password = passwordValue,
                EnableTeams = enableTeamsValue,
                ScoreUpdateFrequency = Convert.ToInt32(scoreUpdateFrequencyValue),
            };
            settings.Teams.AddRange(teamsValue);
            settings.BannedMods.AddRange(bannedModsValue);

            address = addressValue;
            port = int.Parse(portValue);
            overlayPort = int.Parse(overlayPortValue);
            botToken = botTokenValue;
        }

        //Blocks until socket server begins to start (note that this is not "until server is started")
        public async void Start()
        {
            State = new State();
            State.ServerSettings = settings;
            State.KnownHosts.Add(config.GetHosts());

            Logger.Info($"Running on {Update.osType}");

            //Check for updates
            Logger.Info("Checking for updates...");
            var newVersion = await Update.GetLatestRelease();
            if (System.Version.Parse(SharedConstructs.Version) < newVersion)
            {
                Logger.Error($"Update required! You are on \'{SharedConstructs.Version}\', new version is \'{newVersion}\'");
                Logger.Info("Attempting AutoUpdate...");
                bool UpdateSuccess = await Update.AttemptAutoUpdate();
                if (!UpdateSuccess)
                {
                    Logger.Error("AutoUpdate Failed. Please Update Manually. Shutting down");
                    //Moon's note / TODO: Can't do this from shared. Screw the threads
                    //SystemHost.MainThreadStop.Set(); //Release the main thread, so we don't leave behind threads
                    Environment.Exit(0);
                }
                else
                {
                    Logger.Warning("Update was successful, exitting...");
                    //SystemHost.MainThreadStop.Set(); //Release the main thread, so we don't leave behind threads
                    Environment.Exit(0);
                }
            }
            else Logger.Success($"You are on the most recent version! ({SharedConstructs.Version})");

            if (overlayPort != 0)
            {
                OpenPort(overlayPort);
                wsServer = new WsServer(overlayPort);
                #pragma warning disable CS4014
                Task.Run(wsServer.Start);
                #pragma warning restore CS4014
            }

            //If we have a token, start a qualifier bot
            if (!string.IsNullOrEmpty(botToken) && botToken != "[botToken]")
            {
                //We need to await this so the DI framework has time to load the database service
                QualifierBot = new QualifierBot(botToken: botToken, server: this);
                await QualifierBot.Start();
            }

            //Set up the database
            if (QualifierBot != null)
            {
                Database = QualifierBot.Database;
            }
            else
            {
                //If the bot's not running, we need to start the service manually
                var service = new DatabaseService();
                Database = service.DatabaseContext;
            }

            //Translate Event and Songs from database to model format
            var events = Database.Events.Where(x => !x.Old);
            Func<string, List<GameplayParameters>> getSongsForEvent = (string eventId) =>
            {
                return Database.Songs.Where(x => !x.Old && x.EventId == eventId).Select(x => new GameplayParameters
                {
                    Beatmap = new Beatmap
                    {
                        LevelId = x.LevelId,
                        Characteristic = new Characteristic
                        {
                            SerializedName = x.Characteristic
                        },
                        Difficulty = x.BeatmapDifficulty,
                        Name = x.Name
                    },
                    GameplayModifiers = new GameplayModifiers
                    {
                        Options = (GameOptions)x.GameOptions
                    },
                    PlayerSettings = new PlayerSpecificSettings
                    {
                        Options = (PlayerOptions)x.PlayerOptions
                    }
                }).ToList() ?? new List<GameplayParameters> { };
            };
            State.Events.AddRange(events.Select(x => Database.ConvertDatabaseToModel(getSongsForEvent(x.EventId).ToArray(), x)).ToArray());

            //Give our new server a sense of self :P
            Self = new User()
            {
                Id = Guid.Empty.ToString(),
                Name = "HOST"
            };

            async Task scrapeServersAndStart(CoreServer core)
            {
                CoreServer = core ?? new CoreServer
                {
                    Address = "127.0.0.1",
                    Port = 0,
                    Name = "Unregistered Server"
                };

                //Scrape hosts. Unreachable hosts will be removed
                Logger.Info("Reaching out to other hosts for updated Master Lists...");

                //Commented out is the code that makes this act as a mesh network
                //var hostStatePairs = await HostScraper.ScrapeHosts(State.KnownHosts, settings.ServerName, 0, core);

                //The uncommented duplicate here makes this act as a hub and spoke network, since networkauditor.org is the domain of the master server
                var hostStatePairs = await HostScraper.ScrapeHosts(State.KnownHosts.Where(x => x.Address.Contains(MasterServer)).ToArray(), settings.ServerName, 0, core);

                hostStatePairs = hostStatePairs.Where(x => x.Value != null).ToDictionary(x => x.Key, x => x.Value);
                var newHostList = hostStatePairs.Values.Where(x => x.KnownHosts != null).SelectMany(x => x.KnownHosts).Union(hostStatePairs.Keys);
                State.KnownHosts.Clear();
                State.KnownHosts.AddRange(newHostList.ToArray());

                //The current server will always remove itself from its list thanks to it not being up when
                //it starts. Let's fix that. Also, add back the Master Server if it was removed.
                //We accomplish this by triggering the default-on-empty function of GetHosts()
                if (State.KnownHosts.Count == 0) State.KnownHosts.AddRange(config.GetHosts());
                if (core != null)
                {
                    State.KnownHosts.Clear();
                    State.KnownHosts.AddRange(State.KnownHosts.Union(new CoreServer[] { core }).ToArray());
                }

                config.SaveHosts(State.KnownHosts.ToArray());
                Logger.Info("Server list updated.");

                OpenPort(port);

                server = new Server(port);
                server.PacketReceived += Server_PacketReceived;
                server.ClientConnected += Server_ClientConnected;
                server.ClientDisconnected += Server_ClientDisconnected;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                server.Start();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                //Start a regular check for updates
                Update.PollForUpdates(() =>
                { 
                    server.Shutdown();
                    //SystemHost.MainThreadStop.Set(); //Release the main thread, so we don't leave behind threads
                    Environment.Exit(0);
                }, updateCheckToken.Token);
            };

            //Verify that the provided address points to our server
            if (IPAddress.TryParse(address, out _))
            {
                Logger.Warning($"\'{address}\' seems to be an IP address. You'll need a domain pointed to your server for it to be added to the Master Lists");
                await scrapeServersAndStart(null);
            }
            else if (address != "[serverAddress]")
            {
                Logger.Info("Verifying that \'serverAddress\' points to this server...");

                var connected = new AutoResetEvent(false);
                var keyName = $"{address}:{port}";
                bool verified = false;

                var verificationServer = new Server(port);
                verificationServer.PacketReceived += (_, packet) =>
                {
                    if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.Connect")
                    {
                        var connect = packet.SpecificPacket.Unpack<Connect>();
                        if (connect.Name == keyName)
                        {
                            verified = true;
                            connected.Set();
                        }
                    }
                    return Task.CompletedTask;
                };

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                verificationServer.Start();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                var client = new TemporaryClient(address, port, keyName, "0", Connect.Types.ConnectTypes.TemporaryConnection);
                await client.Start();

                connected.WaitOne(6000);

                client.Shutdown();
                verificationServer.Shutdown();

                if (verified)
                {
                    Logger.Success("Verified address! Server should be added to the Lists of all servers that were scraped for hosts");

                    await scrapeServersAndStart(new CoreServer
                    {
                        Address = address,
                        Port = port,
                        Name = State.ServerSettings.ServerName
                    });
                }
                else
                {
                    Logger.Warning("Failed to verify address. Continuing server startup, but note that this server was not added to the Master Lists, if it wasn't already there");
                    await scrapeServersAndStart(null);
                }
            }
            else
            {
                Logger.Warning("If you provide a value for \'serverAddress\' in the configuration file, your server can be added to the Master Lists");
                await scrapeServersAndStart(null);
            }
        }

        //Courtesy of andruzzzhka's Multiplayer
        async void OpenPort(int port)
        {
            Logger.Info($"Trying to open port {port} using UPnP...");
            try
            {
                NatDiscoverer discoverer = new NatDiscoverer();
                CancellationTokenSource cts = new CancellationTokenSource(2500);
                NatDevice device = await discoverer.DiscoverDeviceAsync(PortMapper.Upnp, cts);

                await device.CreatePortMapAsync(new Mapping(Protocol.Tcp, port, port, ""));

                Logger.Info($"Port {port} is open!");
            }
            catch (Exception)
            {
                Logger.Warning($"Can't open port {port} using UPnP! (This is only relevant for people behind NAT who don't port forward. If you're being hosted by an actual server, or you've set up port forwarding manually, you can safely ignore this message. As well as any other yellow messages... Yellow means \"warning\" folks.");
            }
        }

        private async Task Server_ClientDisconnected(ConnectedUser client)
        {
            Logger.Debug("Client Disconnected!");

            if (State.Players.Any(x => x.User.Id == client.id.ToString()))
            {
                var player = State.Players.First(x => x.User.Id == client.id.ToString());
                await RemovePlayer(player);
            }
            else if (State.Coordinators.Any(x => x.User.Id == client.id.ToString()))
            {
                await RemoveCoordinator(State.Coordinators.First(x => x.User.Id == client.id.ToString()));
            }
        }

        private Task Server_ClientConnected(ConnectedUser client)
        {
            return Task.CompletedTask;
        }

        public async Task Send(Guid id, Packet packet)
        {
            #region LOGGING
            string secondaryInfo = string.Empty;
            if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.PlaySong")
            {
                var playSong = packet.SpecificPacket.Unpack<PlaySong>();
                secondaryInfo = playSong.GameplayParameters.Beatmap.LevelId + " : " + playSong.GameplayParameters.Beatmap.Difficulty;
            }
            if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.LoadSong")
            {
                var loadSong = packet.SpecificPacket.Unpack<LoadSong>();
                secondaryInfo = loadSong.LevelId;
            }
            if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.Command")
            {
                var command = packet.SpecificPacket.Unpack<Command>();
                secondaryInfo = command.CommandType.ToString();
            }
            if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.Event")
            {
                var @event = packet.SpecificPacket.Unpack<Event>();

                secondaryInfo = @event.Type.ToString();
                if (@event.Type == Event.Types.EventType.PlayerUpdated)
                {
                    var player = @event.ChangedObject.Unpack<Player>();
                    secondaryInfo = $"{secondaryInfo} from ({player.User.Name} : {player.DownloadState}) : ({player.PlayState} : {player.Score} : {player.StreamDelayMs})";
                }
                else if (@event.Type == Event.Types.EventType.MatchUpdated)
                {
                    var match = @event.ChangedObject.Unpack<Match>();
                    secondaryInfo = $"{secondaryInfo} ({match.SelectedDifficulty})";
                }
            }
            if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.ForwardingPacket")
            {
                var typeUrl = packet.SpecificPacket.Unpack<ForwardingPacket>().SpecificPacket.TypeUrl;
                secondaryInfo = $"{typeUrl.Substring(typeUrl.LastIndexOf("."))}";
            }
            Logger.Debug($"Sending {packet.ToBytes().Length} bytes: ({packet.SpecificPacket.TypeUrl.Substring(packet.SpecificPacket.TypeUrl.LastIndexOf("."))}) ({secondaryInfo})");
            #endregion LOGGING

            packet.From = Guid.Parse(Self?.Id ?? Guid.Empty.ToString());
            await server.Send(id, packet.ToBytes());
            await wsServer.Send(id, JsonConvert.SerializeObject(packet));
        }

        public async Task Send(Guid[] ids, Packet packet)
        {
            #region LOGGING
            string secondaryInfo = string.Empty;
            if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.PlaySong")
            {
                var playSong = packet.SpecificPacket.Unpack<PlaySong>();
                secondaryInfo = playSong.GameplayParameters.Beatmap.LevelId + " : " + playSong.GameplayParameters.Beatmap.Difficulty;
            }
            if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.LoadSong")
            {
                var loadSong = packet.SpecificPacket.Unpack<LoadSong>();
                secondaryInfo = loadSong.LevelId;
            }
            if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.Command")
            {
                var command = packet.SpecificPacket.Unpack<Command>();
                secondaryInfo = command.CommandType.ToString();
            }
            if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.Event")
            {
                var @event = packet.SpecificPacket.Unpack<Event>();

                secondaryInfo = @event.Type.ToString();
                if (@event.Type == Event.Types.EventType.PlayerUpdated)
                {
                    var player = @event.ChangedObject.Unpack<Player>();
                    secondaryInfo = $"{secondaryInfo} from ({player.User.Name} : {player.DownloadState}) : ({player.PlayState} : {player.Score} : {player.StreamDelayMs})";
                }
                else if (@event.Type == Event.Types.EventType.MatchUpdated)
                {
                    var match = @event.ChangedObject.Unpack<Match>();
                    secondaryInfo = $"{secondaryInfo} ({match.SelectedDifficulty})";
                }
            }
            if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.ForwardingPacket")
            {
                var typeUrl = packet.SpecificPacket.Unpack<ForwardingPacket>().SpecificPacket.TypeUrl;
                secondaryInfo = $"{typeUrl.Substring(typeUrl.LastIndexOf("."))}";
            }

            var toIds = string.Empty;
            foreach (var id in ids) toIds += $"{id}, ";
            toIds = toIds.Substring(0, toIds.Length - 2);

            Logger.Debug($"Sending {packet.ToBytes().Length} bytes: ({packet.SpecificPacket.TypeUrl.Substring(packet.SpecificPacket.TypeUrl.LastIndexOf("."))}) ({secondaryInfo})");
            #endregion LOGGING

            packet.From = Guid.Parse(Self?.Id ?? Guid.Empty.ToString());
            await server.Send(ids, packet.ToBytes());
            await wsServer.Send(ids, JsonConvert.SerializeObject(packet));
        }

        public async Task ForwardTo(Guid[] ids, Guid from, Packet packet)
        {
            packet.From = from;

            #region LOGGING
            string secondaryInfo = string.Empty;
            if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.PlaySong")
            {
                var playSong = packet.SpecificPacket.Unpack<PlaySong>();
                secondaryInfo = playSong.GameplayParameters.Beatmap.LevelId + " : " + playSong.GameplayParameters.Beatmap.Difficulty;
            }
            if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.LoadSong")
            {
                var loadSong = packet.SpecificPacket.Unpack<LoadSong>();
                secondaryInfo = loadSong.LevelId;
            }
            if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.Command")
            {
                var command = packet.SpecificPacket.Unpack<Command>();
                secondaryInfo = command.CommandType.ToString();
            }
            if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.Event")
            {
                var @event = packet.SpecificPacket.Unpack<Event>();

                secondaryInfo = @event.Type.ToString();
                if (@event.Type == Event.Types.EventType.PlayerUpdated)
                {
                    var player = @event.ChangedObject.Unpack<Player>();
                    secondaryInfo = $"{secondaryInfo} from ({player.User.Name} : {player.DownloadState}) : ({player.PlayState} : {player.Score} : {player.StreamDelayMs})";
                }
                else if (@event.Type == Event.Types.EventType.MatchUpdated)
                {
                    var match = @event.ChangedObject.Unpack<Match>();
                    secondaryInfo = $"{secondaryInfo} ({match.SelectedDifficulty})";
                }
            }
            if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.ForwardingPacket")
            {
                var typeUrl = packet.SpecificPacket.Unpack<ForwardingPacket>().SpecificPacket.TypeUrl;
                secondaryInfo = $"{typeUrl.Substring(typeUrl.LastIndexOf("."))}";
            }

            var toIds = string.Empty;
            foreach (var id in ids) toIds += $"{id}, ";
            if (!string.IsNullOrEmpty(toIds)) toIds = toIds.Substring(0, toIds.Length - 2);

            Logger.Debug($"Forwarding {packet.ToBytes().Length} bytes ({packet.SpecificPacket.TypeUrl.Substring(packet.SpecificPacket.TypeUrl.LastIndexOf("."))}) ({secondaryInfo}) TO ({toIds}) FROM ({packet.From})");
            #endregion LOGGING

            await server.Send(ids, packet.ToBytes());
            await wsServer.Send(ids, JsonConvert.SerializeObject(packet));
        }

        private async Task BroadcastToAllClients(Packet packet, bool toOverlay = true)
        {
            #region LOGGING
            string secondaryInfo = string.Empty;
            if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.PlaySong")
            {
                var playSong = packet.SpecificPacket.Unpack<PlaySong>();
                secondaryInfo = playSong.GameplayParameters.Beatmap.LevelId + " : " + playSong.GameplayParameters.Beatmap.Difficulty;
            }
            if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.LoadSong")
            {
                var loadSong = packet.SpecificPacket.Unpack<LoadSong>();
                secondaryInfo = loadSong.LevelId;
            }
            if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.Command")
            {
                var command = packet.SpecificPacket.Unpack<Command>();
                secondaryInfo = command.CommandType.ToString();
            }
            if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.Event")
            {
                var @event = packet.SpecificPacket.Unpack<Event>();

                secondaryInfo = @event.Type.ToString();
                if (@event.Type == Event.Types.EventType.PlayerUpdated)
                {
                    var player = @event.ChangedObject.Unpack<Player>();
                    secondaryInfo = $"{secondaryInfo} from ({player.User.Name} : {player.DownloadState}) : ({player.PlayState} : {player.Score} : {player.StreamDelayMs})";
                }
                else if (@event.Type == Event.Types.EventType.MatchUpdated)
                {
                    var match = @event.ChangedObject.Unpack<Match>();
                    secondaryInfo = $"{secondaryInfo} ({match.SelectedDifficulty})";
                }
            }
            if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.ForwardingPacket")
            {
                var typeUrl = packet.SpecificPacket.Unpack<ForwardingPacket>().SpecificPacket.TypeUrl;
                secondaryInfo = $"{typeUrl.Substring(typeUrl.LastIndexOf("."))}";
            }
            Logger.Debug($"Sending {packet.ToBytes().Length} bytes ({packet.SpecificPacket.TypeUrl.Substring(packet.SpecificPacket.TypeUrl.LastIndexOf("."))}) ({secondaryInfo})");
            #endregion LOGGING

            packet.From = Guid.Parse(Self.Id);
            await server.Broadcast(packet.ToBytes());
            await wsServer.Broadcast(JsonConvert.SerializeObject(packet));
        }

        #region EventManagement
        public async Task AddPlayer(Player player)
        {
            lock (State)
            {
                State.Players.Add(player);
            }
            
            NotifyPropertyChanged(nameof(State));

            var @event = new Event
            {
                Type = Event.Types.EventType.PlayerAdded,
                ChangedObject = Any.Pack(player)
            };
            await BroadcastToAllClients(new Packet(@event));

            if (PlayerConnected != null) await PlayerConnected.Invoke(player);
        }

        public async Task UpdatePlayer(Player player)
        {
            lock (State)
            {
                var playerToReplace = State.Players.FirstOrDefault(x => x.User.UserEquals(player.User));
                State.Players.Remove(playerToReplace);
                State.Players.Add(player);
            }

            NotifyPropertyChanged(nameof(State));

            var @event = new Event
            {
                Type = Event.Types.EventType.PlayerUpdated,
                ChangedObject = Any.Pack(player)
            };
            await BroadcastToAllClients(new Packet(@event));

            if (PlayerInfoUpdated != null) await PlayerInfoUpdated.Invoke(player);
        }

        public async Task RemovePlayer(Player player)
        {
            lock (State)
            {
                var playerToRemove = State.Players.FirstOrDefault(x => x.User.UserEquals(player.User));
                State.Players.Remove(playerToRemove);
            }
            
            NotifyPropertyChanged(nameof(State));

            var @event = new Event
            {
                Type = Event.Types.EventType.PlayerLeft,
                ChangedObject = Any.Pack(player)
            };
            await BroadcastToAllClients(new Packet(@event));

            if (PlayerDisconnected != null) await PlayerDisconnected.Invoke(player);
        }

        public async Task AddCoordinator(Coordinator coordinator)
        {
            lock (State)
            {
                State.Coordinators.Add(coordinator);
            }
            
            NotifyPropertyChanged(nameof(State));

            var @event = new Event
            {
                Type = Event.Types.EventType.CoordinatorAdded,
                ChangedObject = Any.Pack(coordinator)
            };
            await BroadcastToAllClients(new Packet(@event));
        }

        public async Task RemoveCoordinator(Coordinator coordinator)
        {
            lock (State)
            {
                var coordinatorToRemove = State.Coordinators.FirstOrDefault(x => x.User.UserEquals(coordinator.User));
                State.Coordinators.Remove(coordinatorToRemove);
            }

            NotifyPropertyChanged(nameof(State));

            var @event = new Event
            {
                Type = Event.Types.EventType.CoordinatorLeft,
                ChangedObject = Any.Pack(coordinator)
            };
            await BroadcastToAllClients(new Packet(@event));
        }

        public async Task CreateMatch(Match match)
        {
            lock (State)
            {
                State.Matches.Add(match);
            }

            NotifyPropertyChanged(nameof(State));

            var @event = new Event
            {
                Type = Event.Types.EventType.MatchCreated,
                ChangedObject = Any.Pack(match)
            };
            await BroadcastToAllClients(new Packet(@event));

            if (MatchCreated != null) await MatchCreated.Invoke(match);
        }

        public async Task UpdateMatch(Match match)
        {
            lock (State)
            {
                var matchToReplace = State.Matches.FirstOrDefault(x => x.Guid == match.Guid);
                State.Matches.Remove(matchToReplace);
                State.Matches.Add(match);
            }

            NotifyPropertyChanged(nameof(State));

            var @event = new Event
            {
                Type = Event.Types.EventType.MatchUpdated,
                ChangedObject = Any.Pack(match)
            };

            var updatePacket = new Packet(@event);

            await BroadcastToAllClients(updatePacket);

            if (MatchInfoUpdated != null) await MatchInfoUpdated.Invoke(match);
        }

        public async Task DeleteMatch(Match match)
        {
            lock (State)
            {
                var matchToRemove = State.Matches.FirstOrDefault(x => x.Guid == match.Guid);
                State.Matches.Remove(matchToRemove);
            }

            NotifyPropertyChanged(nameof(State));

            var @event = new Event
            {
                Type = Event.Types.EventType.MatchDeleted,
                ChangedObject = Any.Pack(match)
            };
            await BroadcastToAllClients(new Packet(@event));

            if (MatchDeleted != null) await MatchDeleted.Invoke(match);
        }

        public async Task<Response> SendCreateQualifierEvent(CoreServer host, QualifierEvent qualifierEvent)
        {
            if (host == CoreServer)
            {
                return await CreateQualifierEvent(qualifierEvent);
            }
            else
            {
                var result = await HostScraper.RequestResponse(host, new Packet(new Event
                {
                    Type = Event.Types.EventType.QualifierEventCreated,
                    ChangedObject = Any.Pack(qualifierEvent)
                }), "type.googleapis.com/TournamentAssistantShared.Models.Packets.Response", $"{CoreServer.Address}:{CoreServer.Port}", 0);
                return result?.SpecificPacket.Unpack<Response>() ?? new Response
                {
                    Type = ResponseType.Fail,
                    Message = "The request to the designated server timed out. The server is offline or otherwise unreachable"
                };
            }
        }

        public async Task<Response> SendUpdateQualifierEvent(CoreServer host, QualifierEvent qualifierEvent)
        {
            if (host == CoreServer)
            {
                return await UpdateQualifierEvent(qualifierEvent);
            }
            else
            {
                var result = await HostScraper.RequestResponse(host, new Packet(new Event
                {
                    Type = Event.Types.EventType.QualifierEventUpdated,
                    ChangedObject = Any.Pack(qualifierEvent)
                }), "type.googleapis.com/TournamentAssistantShared.Models.Packets.Response", $"{ CoreServer.Address}:{CoreServer.Port}", 0);
                return result?.SpecificPacket.Unpack<Response>() ?? new Response
                {
                    Type = ResponseType.Fail,
                    Message = "The request to the designated server timed out. The server is offline or otherwise unreachable"
                };
            }
        }

        public async Task<Response> SendDeleteQualifierEvent(CoreServer host, QualifierEvent qualifierEvent)
        {
            if (host == CoreServer)
            {
                return await DeleteQualifierEvent (qualifierEvent);
            }
            else
            {
                var result = await HostScraper.RequestResponse(host, new Packet(new Event
                {
                    Type = Event.Types.EventType.QualifierEventDeleted,
                    ChangedObject = Any.Pack(qualifierEvent)
                }), "type.googleapis.com/TournamentAssistantShared.Models.Packets.Response", $"{ CoreServer.Address}:{CoreServer.Port}", 0);
                return result?.SpecificPacket.Unpack<Response>() ?? new Response
                {
                    Type = ResponseType.Fail,
                    Message = "The request to the designated server timed out. The server is offline or otherwise unreachable"
                };
            }
        }

        public async Task<Response> CreateQualifierEvent(QualifierEvent qualifierEvent)
        {
            if (Database.Events.Any(x => !x.Old && x.GuildId == (ulong)qualifierEvent.Guild.Id))
            {
                return new Response
                {
                    Type = ResponseType.Fail,
                    Message = "There is already an event running for your guild"
                };
            }

            var databaseEvent = Database.ConvertModelToEventDatabase(qualifierEvent);
            Database.Events.Add(databaseEvent);
            await Database.SaveChangesAsync();

            lock (State)
            {
                State.Events.Add(qualifierEvent);
            }

            NotifyPropertyChanged(nameof(State));

            var @event = new Event
            {
                Type = Event.Types.EventType.QualifierEventCreated,
                ChangedObject = Any.Pack(qualifierEvent)
            };
            await BroadcastToAllClients(new Packet(@event));

            return new Response
            {
                Type = ResponseType.Success,
                Message = $"Successfully created event: {databaseEvent.Name} with settings: {(QualifierEvent.Types.EventSettings)databaseEvent.Flags}"
            };
        }

        public async Task<Response> UpdateQualifierEvent(QualifierEvent qualifierEvent)
        {
            if (!Database.Events.Any(x => !x.Old && x.GuildId == (ulong)qualifierEvent.Guild.Id))
            {
                return new Response
                {
                    Type = ResponseType.Fail,
                    Message = "There is not an event running for your guild"
                };
            }

            //Update Event entry
            var newDatabaseEvent = Database.ConvertModelToEventDatabase(qualifierEvent);
            Database.Entry(Database.Events.First(x => x.EventId == qualifierEvent.EventId.ToString())).CurrentValues.SetValues(newDatabaseEvent);

            //Check for removed songs
            foreach (var song in Database.Songs.Where(x => x.EventId == qualifierEvent.EventId.ToString() && !x.Old))
            {
                if (!qualifierEvent.QualifierMaps.Any(x => song.LevelId == x.Beatmap.LevelId &&
                    song.Characteristic == x.Beatmap.Characteristic.SerializedName &&
                    song.BeatmapDifficulty == x.Beatmap.Difficulty &&
                    song.GameOptions == (int)x.GameplayModifiers.Options &&
                    song.PlayerOptions == (int)x.PlayerSettings.Options))
                {
                    song.Old = true;
                }
            }

            //Check for newly added songs
            foreach (var song in qualifierEvent.QualifierMaps)
            {
                if (!Database.Songs.Any(x => !x.Old &&
                    x.LevelId == song.Beatmap.LevelId &&
                    x.Characteristic == song.Beatmap.Characteristic.SerializedName &&
                    x.BeatmapDifficulty == song.Beatmap.Difficulty &&
                    x.GameOptions == (int)song.GameplayModifiers.Options &&
                    x.PlayerOptions == (int)song.PlayerSettings.Options))
                {
                    Database.Songs.Add(new Discord.Database.Song
                    {
                        EventId = qualifierEvent.EventId.ToString(),
                        LevelId = song.Beatmap.LevelId,
                        Name = song.Beatmap.Name,
                        Characteristic = song.Beatmap.Characteristic.SerializedName,
                        BeatmapDifficulty = song.Beatmap.Difficulty,
                        GameOptions = (int)song.GameplayModifiers.Options,
                        PlayerOptions = (int)song.PlayerSettings.Options
                    });
                }
            }

            await Database.SaveChangesAsync();

            lock (State)
            {
                var eventToReplace = State.Events.FirstOrDefault(x => x.EventId == qualifierEvent.EventId);
                State.Events.Remove(eventToReplace);
                State.Events.Add(qualifierEvent);
            }

            NotifyPropertyChanged(nameof(State));

            var @event = new Event
            {
                Type = Event.Types.EventType.QualifierEventUpdated,
                ChangedObject = Any.Pack(qualifierEvent)
            };

            var updatePacket = new Packet(@event);

            await BroadcastToAllClients(updatePacket);

            return new Response
            {
                Type = ResponseType.Success,
                Message = $"Successfully updated event: {newDatabaseEvent.Name}"
            };
        }

        public async Task<Response> DeleteQualifierEvent(QualifierEvent qualifierEvent)
        {
            if (!Database.Events.Any(x => !x.Old && x.GuildId == (ulong)qualifierEvent.Guild.Id))
            {
                return new Response
                {
                    Type = ResponseType.Fail,
                    Message = "There is not an event running for your guild"
                };
            }

            //Mark all songs and scores as old
            await Database.Events.Where(x => x.EventId == qualifierEvent.EventId.ToString()).ForEachAsync(x => x.Old = true);
            await Database.Songs.Where(x => x.EventId == qualifierEvent.EventId.ToString()).ForEachAsync(x => x.Old = true);
            await Database.Scores.Where(x => x.EventId == qualifierEvent.EventId.ToString()).ForEachAsync(x => x.Old = true);
            await Database.SaveChangesAsync();

            lock (State)
            {
                var eventToRemove = State.Events.FirstOrDefault(x => x.EventId == qualifierEvent.EventId);
                State.Events.Remove(eventToRemove);
            }

            NotifyPropertyChanged(nameof(State));

            var @event = new Event
            {
                Type = Event.Types.EventType.QualifierEventDeleted,
                ChangedObject = Any.Pack(qualifierEvent)
            };
            await BroadcastToAllClients(new Packet(@event));

            return new Response
            {
                Type = ResponseType.Success,
                Message = $"Successfully ended event: {qualifierEvent.Name}"
            };
        }

        public async Task AddHost(CoreServer host)
        {
            lock (State)
            {
                State.KnownHosts.Add(host);

                //Save to disk
                config.SaveHosts(State.KnownHosts.ToArray());
            }

            NotifyPropertyChanged(nameof(State));

            var @event = new Event
            {
                Type = Event.Types.EventType.HostAdded,
                ChangedObject = Any.Pack(host)
            };
            await BroadcastToAllClients(new Packet(@event));
        }

        public async Task RemoveHost(CoreServer host)
        {
            lock (State)
            {
                var hostToRemove = State.KnownHosts.FirstOrDefault(x => x.CoreServerEquals(host));
                State.KnownHosts.Remove(hostToRemove);
            }

            NotifyPropertyChanged(nameof(State));

            var @event = new Event
            {
                Type = Event.Types.EventType.HostRemoved,
                ChangedObject = Any.Pack(host)
            };
            await BroadcastToAllClients(new Packet(@event));
        }
        #endregion EventManagement

        private async Task Server_PacketReceived(ConnectedUser player, Packet packet)
        {
            #region LOGGING
            string secondaryInfo = string.Empty;
            if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.PlaySong")
            {
                var playSong = packet.SpecificPacket.Unpack<PlaySong>();
                secondaryInfo = playSong.GameplayParameters.Beatmap.LevelId + " : " + playSong.GameplayParameters.Beatmap.Difficulty;
            }
            if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.LoadSong")
            {
                var loadSong = packet.SpecificPacket.Unpack<LoadSong>();
                secondaryInfo = loadSong.LevelId;
            }
            if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.Command")
            {
                var command = packet.SpecificPacket.Unpack<Command>();
                secondaryInfo = command.CommandType.ToString();
            }
            if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.Event")
            {
                var @event = packet.SpecificPacket.Unpack<Event>();

                secondaryInfo = @event.Type.ToString();
                if (@event.Type == Event.Types.EventType.PlayerUpdated)
                {
                    var changedPlayer = @event.ChangedObject.Unpack<Player>();
                    secondaryInfo = $"{secondaryInfo} from ({changedPlayer.User.Name} : {changedPlayer.DownloadState}) : ({changedPlayer.PlayState} : {changedPlayer.Score} : {changedPlayer.StreamDelayMs})";
                }
                else if (@event.Type == Event.Types.EventType.MatchUpdated)
                {
                    var match = @event.ChangedObject.Unpack<Match>();
                    secondaryInfo = $"{secondaryInfo} ({match.SelectedDifficulty})";
                }
            }
            if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.ForwardingPacket")
            {
                var typeUrl = packet.SpecificPacket.Unpack<ForwardingPacket>().SpecificPacket.TypeUrl;
                secondaryInfo = $"{typeUrl.Substring(typeUrl.LastIndexOf("."))}";
            }
            Logger.Debug($"Received {packet.ToBytes().Length} bytes: ({packet.SpecificPacket.TypeUrl.Substring(packet.SpecificPacket.TypeUrl.LastIndexOf("."))}) ({secondaryInfo})");
            #endregion LOGGING

            //Ready to go, only disabled since it is currently unusued
            /*if (packet.Type != PacketType.Acknowledgement)
            {
                Send(packet.From, new Packet(new Acknowledgement()
                {
                    PacketId = packet.Id
                }));
            }*/

            if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.Acknowledgement")
            {
                Acknowledgement acknowledgement = packet.SpecificPacket.Unpack<Acknowledgement>();
                AckReceived?.Invoke(acknowledgement, packet.From);
            }
            /*else if (packet.Type == PacketType.SongList)
            {
                SongList songList = packet.SpecificPacket as SongList;
            }*/
            /*else if (packet.Type == PacketType.LoadedSong)
            {
                LoadedSong loadedSong = packet.SpecificPacket as LoadedSong;
            }*/
            else if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.Connect")
            {
                Connect connect = packet.SpecificPacket.Unpack<Connect>();

                if (connect.ClientVersion != VersionCode)
                {
                    await Send(player.id, new Packet(new ConnectResponse()
                    {
                        Response = new Response()
                        {
                            Type = ResponseType.Fail,
                            Message = $"Version mismatch, this server is on version {SharedConstructs.Version}",
                        },
                        Self = null,
                        State = null,
                        ServerVersion = VersionCode
                    }));
                }
                else if (connect.ClientType == Connect.Types.ConnectTypes.Player)
                {
                    var newPlayer = new Player()
                    {
                        User = new User()
                        {

                            Id = player.id.ToString(),
                            Name = connect.Name
                        },
                        UserId = connect.UserId,
                        Team = new Team() { Id = Guid.Empty.ToString(), Name = "None"}
                    };

                    await AddPlayer(newPlayer);

                    //Give the newly connected player their Self and State
                    await Send(player.id, new Packet(new ConnectResponse()
                    {
                        Response = new Response()
                        {
                            Type = ResponseType.Success,
                            Message = $"Connected to {settings.ServerName}!"
                        },
                        Self = newPlayer.User,
                        State = State,
                        ServerVersion = VersionCode
                    }));
                }
                else if (connect.ClientType == Connect.Types.ConnectTypes.Coordinator)
                {
                    if (string.IsNullOrWhiteSpace(settings.Password) || connect.Password == settings.Password)
                    {
                        var coordinator = new Coordinator()
                        {
                            User = new User()
                            {
                                Id = player.id.ToString(),
                                Name = connect.Name
                            }
                        };
                        await AddCoordinator(coordinator);

                        //Give the newly connected coordinator their Self and State
                        await Send(player.id, new Packet(new ConnectResponse()
                        {
                            Response = new Response()
                            {
                                Type = ResponseType.Success,
                                Message = $"Connected to {settings.ServerName}!"
                            },
                            Self = coordinator.User,
                            State = State,
                            ServerVersion = VersionCode
                        }));
                    }
                    else
                    {
                        await Send(player.id, new Packet(new ConnectResponse()
                        {
                            Response = new Response()
                            {
                                Type = ResponseType.Fail,
                                Message = $"Incorrect password for {settings.ServerName}!"
                            },
                            State = State,
                            ServerVersion = VersionCode
                        }));
                    }
                }
                else if (connect.ClientType == Connect.Types.ConnectTypes.TemporaryConnection)
                {
                    //A scraper just wants a copy of our state, so let's give it to them
                    await Send(player.id, new Packet(new ConnectResponse()
                    {
                        Response = new Response()
                        {
                            Type = ResponseType.Success,
                            Message = $"Connected to {settings.ServerName} (scraper)!"
                        },
                        Self = null,
                        State = State,
                        ServerVersion = VersionCode
                    }));
                }
            }
            else if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.ScoreRequest")
            {
                ScoreRequest request = packet.SpecificPacket.Unpack<ScoreRequest>();

                var scores = Database.Scores
                    .Where(x => x.EventId == request.EventId.ToString() &&
                        x.LevelId == request.Parameters.Beatmap.LevelId &&
                        x.Characteristic == request.Parameters.Beatmap.Characteristic.SerializedName &&
                        x.BeatmapDifficulty == request.Parameters.Beatmap.Difficulty &&
                        x.GameOptions == (int)request.Parameters.GameplayModifiers.Options &&
                        //x.PlayerOptions == (int)request.Parameters.PlayerSettings.Options &&
                        !x.Old).OrderByDescending(x => x._Score)
                    .Select(x => new Score
                {
                    EventId = request.EventId,
                    Parameters = request.Parameters,
                    Username = x.Username,
                    UserId = x.UserId.ToString(),
                    Score_ = x._Score,
                    FullCombo = x.FullCombo,
                    Color = x.Username == "Moon" ? "#00ff00" : "#ffffff"
                });

                //If scores are disabled for this event, don't return them
                var @event = Database.Events.FirstOrDefault(x => x.EventId == request.EventId.ToString());
                if (((QualifierEvent.Types.EventSettings)@event.Flags).HasFlag(QualifierEvent.Types.EventSettings.HideScoresFromPlayers))
                {
                    await Send(player.id, new Packet(new ScoreRequestResponse()));
                }
                else
                {
                    var scoreRequestResponse = new ScoreRequestResponse();
                    scoreRequestResponse.Scores.AddRange(scores);
                    await Send(player.id, new Packet(scoreRequestResponse));
                }
            }
            else if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.SubmitScore")
            {
                SubmitScore submitScore = packet.SpecificPacket.Unpack<SubmitScore>();

                //Check to see if the song exists in the database
                var song = Database.Songs.FirstOrDefault(x => x.EventId == submitScore.Score.EventId.ToString() &&
                        x.LevelId == submitScore.Score.Parameters.Beatmap.LevelId &&
                        x.Characteristic == submitScore.Score.Parameters.Beatmap.Characteristic.SerializedName &&
                        x.BeatmapDifficulty == submitScore.Score.Parameters.Beatmap.Difficulty &&
                        x.GameOptions == (int)submitScore.Score.Parameters.GameplayModifiers.Options &&
                        //x.PlayerOptions == (int)submitScore.Score.Parameters.PlayerSettings.Options &&
                        !x.Old);

                if (song != null)
                {
                    //Mark all older scores as old
                    var scores = Database.Scores
                        .Where(x => x.EventId == submitScore.Score.EventId.ToString() &&
                            x.LevelId == submitScore.Score.Parameters.Beatmap.LevelId &&
                            x.Characteristic == submitScore.Score.Parameters.Beatmap.Characteristic.SerializedName &&
                            x.BeatmapDifficulty == submitScore.Score.Parameters.Beatmap.Difficulty &&
                            x.GameOptions == (int)submitScore.Score.Parameters.GameplayModifiers.Options &&
                            //x.PlayerOptions == (int)submitScore.Score.Parameters.PlayerSettings.Options &&
                            !x.Old &&
                            x.UserId == ulong.Parse(submitScore.Score.UserId));

                    var oldHighScore = (scores.OrderBy(x => x._Score).FirstOrDefault()?._Score ?? -1);
                    if (oldHighScore < submitScore.Score.Score_)
                    {
                        foreach (var score in scores) score.Old = true;

                        Database.Scores.Add(new Discord.Database.Score
                        {
                            EventId = submitScore.Score.EventId.ToString(),
                            UserId = ulong.Parse(submitScore.Score.UserId),
                            Username = submitScore.Score.Username,
                            LevelId = submitScore.Score.Parameters.Beatmap.LevelId,
                            Characteristic = submitScore.Score.Parameters.Beatmap.Characteristic.SerializedName,
                            BeatmapDifficulty = submitScore.Score.Parameters.Beatmap.Difficulty,
                            GameOptions = (int)submitScore.Score.Parameters.GameplayModifiers.Options,
                            PlayerOptions = (int)submitScore.Score.Parameters.PlayerSettings.Options,
                            _Score = submitScore.Score.Score_,
                            FullCombo = submitScore.Score.FullCombo,
                        });
                        await Database.SaveChangesAsync();
                    }

                    var newScores = Database.Scores
                        .Where(x => x.EventId == submitScore.Score.EventId.ToString() &&
                            x.LevelId == submitScore.Score.Parameters.Beatmap.LevelId &&
                            x.Characteristic == submitScore.Score.Parameters.Beatmap.Characteristic.SerializedName &&
                            x.BeatmapDifficulty == submitScore.Score.Parameters.Beatmap.Difficulty &&
                            x.GameOptions == (int)submitScore.Score.Parameters.GameplayModifiers.Options &&
                            //x.PlayerOptions == (int)submitScore.Score.Parameters.PlayerSettings.Options &&
                            !x.Old).OrderByDescending(x => x._Score).Take(10)
                        .Select(x => new Score
                        {
                            EventId = submitScore.Score.EventId,
                            Parameters = submitScore.Score.Parameters,
                            Username = x.Username,
                            UserId = x.UserId.ToString(),
                            Score_ = x._Score,
                            FullCombo = x.FullCombo,
                            Color = "#ffffff"
                        });

                    //Return the new scores for the song so the leaderboard will update immediately
                    //If scores are disabled for this event, don't return them
                    var @event = Database.Events.FirstOrDefault(x => x.EventId == submitScore.Score.EventId.ToString());
                    var hideScores = ((QualifierEvent.Types.EventSettings)@event.Flags).HasFlag(QualifierEvent.Types.EventSettings.HideScoresFromPlayers);
                    var enableLeaderboardMessage = ((QualifierEvent.Types.EventSettings)@event.Flags).HasFlag(QualifierEvent.Types.EventSettings.EnableLeaderboardMessage);

                    var scoreRequestResponse = new ScoreRequestResponse();
                    scoreRequestResponse.Scores.AddRange(hideScores ? new Score[] { } : newScores.ToArray());
                    await Send(player.id, new Packet(scoreRequestResponse));

                    if (oldHighScore < submitScore.Score.Score_ && @event.InfoChannelId != default && !hideScores && QualifierBot != null)
                    {
                        QualifierBot.SendScoreEvent(@event.InfoChannelId, submitScore);

                        if (enableLeaderboardMessage)
                        {
                            var eventSongs = Database.Songs.Where(x => x.EventId == submitScore.Score.EventId.ToString() && !x.Old);
                            var eventScores = Database.Scores.Where(x => x.EventId == submitScore.Score.EventId.ToString() && !x.Old);
                            var newMessageId = await QualifierBot.SendLeaderboardUpdate(@event.InfoChannelId, @event.LeaderboardMessageId, eventScores.ToList(), eventSongs.ToList());
                            if (@event.LeaderboardMessageId != newMessageId)
                            {
                                @event.LeaderboardMessageId = newMessageId;
                                await Database.SaveChangesAsync();
                            }
                        }
                    }
                }
            }
            else if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.Event")
            {
                Event @event = packet.SpecificPacket.Unpack<Event>();
                switch (@event.Type)
                {
                    case Event.Types.EventType.CoordinatorAdded:
                        await AddCoordinator(@event.ChangedObject.Unpack<Coordinator>());
                        break;
                    case Event.Types.EventType.CoordinatorLeft:
                        await RemoveCoordinator(@event.ChangedObject.Unpack<Coordinator>());
                        break;
                    case Event.Types.EventType.MatchCreated:
                        await CreateMatch(@event.ChangedObject.Unpack<Match>());
                        break;
                    case Event.Types.EventType.MatchUpdated:
                        await UpdateMatch(@event.ChangedObject.Unpack<Match>());
                        break;
                    case Event.Types.EventType.MatchDeleted:
                        await DeleteMatch(@event.ChangedObject.Unpack<Match>());
                        break;
                    case Event.Types.EventType.PlayerAdded:
                        await AddPlayer(@event.ChangedObject.Unpack<Player>());
                        break;
                    case Event.Types.EventType.PlayerUpdated:
                        await UpdatePlayer(@event.ChangedObject.Unpack<Player>());
                        break;
                    case Event.Types.EventType.PlayerLeft:
                        await RemovePlayer(@event.ChangedObject.Unpack<Player>());
                        break;
                    case Event.Types.EventType.QualifierEventCreated:
                        await Send(player.id, new Packet(await CreateQualifierEvent(@event.ChangedObject.Unpack<QualifierEvent>())));
                        break;
                    case Event.Types.EventType.QualifierEventUpdated:
                        await Send(player.id, new Packet(await UpdateQualifierEvent(@event.ChangedObject.Unpack<QualifierEvent>())));
                        break;
                    case Event.Types.EventType.QualifierEventDeleted:
                        await Send(player.id, new Packet(await DeleteQualifierEvent(@event.ChangedObject.Unpack<QualifierEvent>())));
                        break;
                    case Event.Types.EventType.HostAdded:
                        await AddHost(@event.ChangedObject.Unpack<CoreServer>());
                        break;
                    case Event.Types.EventType.HostRemoved:
                        await RemoveHost(@event.ChangedObject.Unpack<CoreServer>());
                        break;
                    default:
                        Logger.Error($"Unknown command received from {player.id}!");
                        break;
                }
            }
            else if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.SongFinished")
            {
                await BroadcastToAllClients(packet, false);
                PlayerFinishedSong?.Invoke(packet.SpecificPacket.Unpack<SongFinished>());
            }
            else if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.ForwardingPacket")
            {
                var forwardingPacket = packet.SpecificPacket.Unpack<ForwardingPacket>();
                var forwardedPacket = new Packet(forwardingPacket.SpecificPacket);

                await ForwardTo(forwardingPacket.ForwardTo.Select(x => Guid.Parse(x)).ToArray(), packet.From, forwardedPacket);
            }
            else if (packet.SpecificPacket.TypeUrl == "type.googleapis.com/TournamentAssistantShared.Models.Packets.SendBotMessage")
            {
                var sendBotMessage = packet.SpecificPacket.Unpack<SendBotMessage>();
                QualifierBot.SendMessage(sendBotMessage.Channel, sendBotMessage.Message);
            }
        }
    }
}
