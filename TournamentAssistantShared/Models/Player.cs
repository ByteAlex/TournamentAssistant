// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: player.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace TournamentAssistantShared.Models {

  /// <summary>Holder for reflection information generated from player.proto</summary>
  public static partial class PlayerReflection {

    #region Descriptor
    /// <summary>File descriptor for player.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static PlayerReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CgxwbGF5ZXIucHJvdG8SIFRvdXJuYW1lbnRBc3Npc3RhbnRTaGFyZWQuTW9k",
            "ZWxzGgp1c2VyLnByb3RvGgp0ZWFtLnByb3RvGg9zb25nX2xpc3QucHJvdG8i",
            "3wUKBlBsYXllchI0CgR1c2VyGAEgASgLMiYuVG91cm5hbWVudEFzc2lzdGFu",
            "dFNoYXJlZC5Nb2RlbHMuVXNlchIPCgd1c2VyX2lkGAIgASgJEjQKBHRlYW0Y",
            "AyABKAsyJi5Ub3VybmFtZW50QXNzaXN0YW50U2hhcmVkLk1vZGVscy5UZWFt",
            "EkcKCnBsYXlfc3RhdGUYBCABKA4yMy5Ub3VybmFtZW50QXNzaXN0YW50U2hh",
            "cmVkLk1vZGVscy5QbGF5ZXIuUGxheVN0YXRlcxJPCg5kb3dubG9hZF9zdGF0",
            "ZRgFIAEoDjI3LlRvdXJuYW1lbnRBc3Npc3RhbnRTaGFyZWQuTW9kZWxzLlBs",
            "YXllci5Eb3dubG9hZFN0YXRlcxINCgVzY29yZRgGIAEoBRINCgVjb21ibxgH",
            "IAEoBRIQCghhY2N1cmFjeRgIIAEoARIVCg1zb25nX3Bvc2l0aW9uGAkgASgB",
            "EkUKCXNvbmdfbGlzdBgKIAEoCzIyLlRvdXJuYW1lbnRBc3Npc3RhbnRTaGFy",
            "ZWQuTW9kZWxzLlBhY2tldHMuU29uZ0xpc3QSEAoIbW9kX2xpc3QYCyADKAkS",
            "UQoZc3RyZWFtX3NjcmVlbl9jb29yZGluYXRlcxgMIAEoCzIuLlRvdXJuYW1l",
            "bnRBc3Npc3RhbnRTaGFyZWQuTW9kZWxzLlBsYXllci5Qb2ludBIXCg9zdHJl",
            "YW1fZGVsYXlfbXMYDSABKAMSHAoUc3RyZWFtX3N5bmNfc3RhcnRfbXMYDiAB",
            "KAMaHQoFUG9pbnQSCQoBeBgBIAEoBRIJCgF5GAIgASgFIiUKClBsYXlTdGF0",
            "ZXMSCwoHV2FpdGluZxAAEgoKBkluR2FtZRABIk4KDkRvd25sb2FkU3RhdGVz",
            "EggKBE5vbmUQABIPCgtEb3dubG9hZGluZxABEg4KCkRvd25sb2FkZWQQAhIR",
            "Cg1Eb3dubG9hZEVycm9yEANCI6oCIFRvdXJuYW1lbnRBc3Npc3RhbnRTaGFy",
            "ZWQuTW9kZWxzYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::TournamentAssistantShared.Models.UserReflection.Descriptor, global::TournamentAssistantShared.Models.TeamReflection.Descriptor, global::TournamentAssistantShared.Models.Packets.SongListReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::TournamentAssistantShared.Models.Player), global::TournamentAssistantShared.Models.Player.Parser, new[]{ "User", "UserId", "Team", "PlayState", "DownloadState", "Score", "Combo", "Accuracy", "SongPosition", "SongList", "ModList", "StreamScreenCoordinates", "StreamDelayMs", "StreamSyncStartMs" }, null, new[]{ typeof(global::TournamentAssistantShared.Models.Player.Types.PlayStates), typeof(global::TournamentAssistantShared.Models.Player.Types.DownloadStates) }, null, new pbr::GeneratedClrTypeInfo[] { new pbr::GeneratedClrTypeInfo(typeof(global::TournamentAssistantShared.Models.Player.Types.Point), global::TournamentAssistantShared.Models.Player.Types.Point.Parser, new[]{ "X", "Y" }, null, null, null, null)})
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class Player : pb::IMessage<Player>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<Player> _parser = new pb::MessageParser<Player>(() => new Player());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<Player> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::TournamentAssistantShared.Models.PlayerReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public Player() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public Player(Player other) : this() {
      user_ = other.user_ != null ? other.user_.Clone() : null;
      userId_ = other.userId_;
      team_ = other.team_ != null ? other.team_.Clone() : null;
      playState_ = other.playState_;
      downloadState_ = other.downloadState_;
      score_ = other.score_;
      combo_ = other.combo_;
      accuracy_ = other.accuracy_;
      songPosition_ = other.songPosition_;
      songList_ = other.songList_ != null ? other.songList_.Clone() : null;
      modList_ = other.modList_.Clone();
      streamScreenCoordinates_ = other.streamScreenCoordinates_ != null ? other.streamScreenCoordinates_.Clone() : null;
      streamDelayMs_ = other.streamDelayMs_;
      streamSyncStartMs_ = other.streamSyncStartMs_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public Player Clone() {
      return new Player(this);
    }

    /// <summary>Field number for the "user" field.</summary>
    public const int UserFieldNumber = 1;
    private global::TournamentAssistantShared.Models.User user_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::TournamentAssistantShared.Models.User User {
      get { return user_; }
      set {
        user_ = value;
      }
    }

    /// <summary>Field number for the "user_id" field.</summary>
    public const int UserIdFieldNumber = 2;
    private string userId_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string UserId {
      get { return userId_; }
      set {
        userId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "team" field.</summary>
    public const int TeamFieldNumber = 3;
    private global::TournamentAssistantShared.Models.Team team_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::TournamentAssistantShared.Models.Team Team {
      get { return team_; }
      set {
        team_ = value;
      }
    }

    /// <summary>Field number for the "play_state" field.</summary>
    public const int PlayStateFieldNumber = 4;
    private global::TournamentAssistantShared.Models.Player.Types.PlayStates playState_ = global::TournamentAssistantShared.Models.Player.Types.PlayStates.Waiting;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::TournamentAssistantShared.Models.Player.Types.PlayStates PlayState {
      get { return playState_; }
      set {
        playState_ = value;
      }
    }

    /// <summary>Field number for the "download_state" field.</summary>
    public const int DownloadStateFieldNumber = 5;
    private global::TournamentAssistantShared.Models.Player.Types.DownloadStates downloadState_ = global::TournamentAssistantShared.Models.Player.Types.DownloadStates.None;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::TournamentAssistantShared.Models.Player.Types.DownloadStates DownloadState {
      get { return downloadState_; }
      set {
        downloadState_ = value;
      }
    }

    /// <summary>Field number for the "score" field.</summary>
    public const int ScoreFieldNumber = 6;
    private int score_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Score {
      get { return score_; }
      set {
        score_ = value;
      }
    }

    /// <summary>Field number for the "combo" field.</summary>
    public const int ComboFieldNumber = 7;
    private int combo_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Combo {
      get { return combo_; }
      set {
        combo_ = value;
      }
    }

    /// <summary>Field number for the "accuracy" field.</summary>
    public const int AccuracyFieldNumber = 8;
    private double accuracy_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public double Accuracy {
      get { return accuracy_; }
      set {
        accuracy_ = value;
      }
    }

    /// <summary>Field number for the "song_position" field.</summary>
    public const int SongPositionFieldNumber = 9;
    private double songPosition_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public double SongPosition {
      get { return songPosition_; }
      set {
        songPosition_ = value;
      }
    }

    /// <summary>Field number for the "song_list" field.</summary>
    public const int SongListFieldNumber = 10;
    private global::TournamentAssistantShared.Models.Packets.SongList songList_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::TournamentAssistantShared.Models.Packets.SongList SongList {
      get { return songList_; }
      set {
        songList_ = value;
      }
    }

    /// <summary>Field number for the "mod_list" field.</summary>
    public const int ModListFieldNumber = 11;
    private static readonly pb::FieldCodec<string> _repeated_modList_codec
        = pb::FieldCodec.ForString(90);
    private readonly pbc::RepeatedField<string> modList_ = new pbc::RepeatedField<string>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<string> ModList {
      get { return modList_; }
    }

    /// <summary>Field number for the "stream_screen_coordinates" field.</summary>
    public const int StreamScreenCoordinatesFieldNumber = 12;
    private global::TournamentAssistantShared.Models.Player.Types.Point streamScreenCoordinates_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::TournamentAssistantShared.Models.Player.Types.Point StreamScreenCoordinates {
      get { return streamScreenCoordinates_; }
      set {
        streamScreenCoordinates_ = value;
      }
    }

    /// <summary>Field number for the "stream_delay_ms" field.</summary>
    public const int StreamDelayMsFieldNumber = 13;
    private long streamDelayMs_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public long StreamDelayMs {
      get { return streamDelayMs_; }
      set {
        streamDelayMs_ = value;
      }
    }

    /// <summary>Field number for the "stream_sync_start_ms" field.</summary>
    public const int StreamSyncStartMsFieldNumber = 14;
    private long streamSyncStartMs_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public long StreamSyncStartMs {
      get { return streamSyncStartMs_; }
      set {
        streamSyncStartMs_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as Player);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(Player other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(User, other.User)) return false;
      if (UserId != other.UserId) return false;
      if (!object.Equals(Team, other.Team)) return false;
      if (PlayState != other.PlayState) return false;
      if (DownloadState != other.DownloadState) return false;
      if (Score != other.Score) return false;
      if (Combo != other.Combo) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Accuracy, other.Accuracy)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(SongPosition, other.SongPosition)) return false;
      if (!object.Equals(SongList, other.SongList)) return false;
      if(!modList_.Equals(other.modList_)) return false;
      if (!object.Equals(StreamScreenCoordinates, other.StreamScreenCoordinates)) return false;
      if (StreamDelayMs != other.StreamDelayMs) return false;
      if (StreamSyncStartMs != other.StreamSyncStartMs) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (user_ != null) hash ^= User.GetHashCode();
      if (UserId.Length != 0) hash ^= UserId.GetHashCode();
      if (team_ != null) hash ^= Team.GetHashCode();
      if (PlayState != global::TournamentAssistantShared.Models.Player.Types.PlayStates.Waiting) hash ^= PlayState.GetHashCode();
      if (DownloadState != global::TournamentAssistantShared.Models.Player.Types.DownloadStates.None) hash ^= DownloadState.GetHashCode();
      if (Score != 0) hash ^= Score.GetHashCode();
      if (Combo != 0) hash ^= Combo.GetHashCode();
      if (Accuracy != 0D) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Accuracy);
      if (SongPosition != 0D) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(SongPosition);
      if (songList_ != null) hash ^= SongList.GetHashCode();
      hash ^= modList_.GetHashCode();
      if (streamScreenCoordinates_ != null) hash ^= StreamScreenCoordinates.GetHashCode();
      if (StreamDelayMs != 0L) hash ^= StreamDelayMs.GetHashCode();
      if (StreamSyncStartMs != 0L) hash ^= StreamSyncStartMs.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (user_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(User);
      }
      if (UserId.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(UserId);
      }
      if (team_ != null) {
        output.WriteRawTag(26);
        output.WriteMessage(Team);
      }
      if (PlayState != global::TournamentAssistantShared.Models.Player.Types.PlayStates.Waiting) {
        output.WriteRawTag(32);
        output.WriteEnum((int) PlayState);
      }
      if (DownloadState != global::TournamentAssistantShared.Models.Player.Types.DownloadStates.None) {
        output.WriteRawTag(40);
        output.WriteEnum((int) DownloadState);
      }
      if (Score != 0) {
        output.WriteRawTag(48);
        output.WriteInt32(Score);
      }
      if (Combo != 0) {
        output.WriteRawTag(56);
        output.WriteInt32(Combo);
      }
      if (Accuracy != 0D) {
        output.WriteRawTag(65);
        output.WriteDouble(Accuracy);
      }
      if (SongPosition != 0D) {
        output.WriteRawTag(73);
        output.WriteDouble(SongPosition);
      }
      if (songList_ != null) {
        output.WriteRawTag(82);
        output.WriteMessage(SongList);
      }
      modList_.WriteTo(output, _repeated_modList_codec);
      if (streamScreenCoordinates_ != null) {
        output.WriteRawTag(98);
        output.WriteMessage(StreamScreenCoordinates);
      }
      if (StreamDelayMs != 0L) {
        output.WriteRawTag(104);
        output.WriteInt64(StreamDelayMs);
      }
      if (StreamSyncStartMs != 0L) {
        output.WriteRawTag(112);
        output.WriteInt64(StreamSyncStartMs);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (user_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(User);
      }
      if (UserId.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(UserId);
      }
      if (team_ != null) {
        output.WriteRawTag(26);
        output.WriteMessage(Team);
      }
      if (PlayState != global::TournamentAssistantShared.Models.Player.Types.PlayStates.Waiting) {
        output.WriteRawTag(32);
        output.WriteEnum((int) PlayState);
      }
      if (DownloadState != global::TournamentAssistantShared.Models.Player.Types.DownloadStates.None) {
        output.WriteRawTag(40);
        output.WriteEnum((int) DownloadState);
      }
      if (Score != 0) {
        output.WriteRawTag(48);
        output.WriteInt32(Score);
      }
      if (Combo != 0) {
        output.WriteRawTag(56);
        output.WriteInt32(Combo);
      }
      if (Accuracy != 0D) {
        output.WriteRawTag(65);
        output.WriteDouble(Accuracy);
      }
      if (SongPosition != 0D) {
        output.WriteRawTag(73);
        output.WriteDouble(SongPosition);
      }
      if (songList_ != null) {
        output.WriteRawTag(82);
        output.WriteMessage(SongList);
      }
      modList_.WriteTo(ref output, _repeated_modList_codec);
      if (streamScreenCoordinates_ != null) {
        output.WriteRawTag(98);
        output.WriteMessage(StreamScreenCoordinates);
      }
      if (StreamDelayMs != 0L) {
        output.WriteRawTag(104);
        output.WriteInt64(StreamDelayMs);
      }
      if (StreamSyncStartMs != 0L) {
        output.WriteRawTag(112);
        output.WriteInt64(StreamSyncStartMs);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (user_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(User);
      }
      if (UserId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(UserId);
      }
      if (team_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Team);
      }
      if (PlayState != global::TournamentAssistantShared.Models.Player.Types.PlayStates.Waiting) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) PlayState);
      }
      if (DownloadState != global::TournamentAssistantShared.Models.Player.Types.DownloadStates.None) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) DownloadState);
      }
      if (Score != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Score);
      }
      if (Combo != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Combo);
      }
      if (Accuracy != 0D) {
        size += 1 + 8;
      }
      if (SongPosition != 0D) {
        size += 1 + 8;
      }
      if (songList_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(SongList);
      }
      size += modList_.CalculateSize(_repeated_modList_codec);
      if (streamScreenCoordinates_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(StreamScreenCoordinates);
      }
      if (StreamDelayMs != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(StreamDelayMs);
      }
      if (StreamSyncStartMs != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(StreamSyncStartMs);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(Player other) {
      if (other == null) {
        return;
      }
      if (other.user_ != null) {
        if (user_ == null) {
          User = new global::TournamentAssistantShared.Models.User();
        }
        User.MergeFrom(other.User);
      }
      if (other.UserId.Length != 0) {
        UserId = other.UserId;
      }
      if (other.team_ != null) {
        if (team_ == null) {
          Team = new global::TournamentAssistantShared.Models.Team();
        }
        Team.MergeFrom(other.Team);
      }
      if (other.PlayState != global::TournamentAssistantShared.Models.Player.Types.PlayStates.Waiting) {
        PlayState = other.PlayState;
      }
      if (other.DownloadState != global::TournamentAssistantShared.Models.Player.Types.DownloadStates.None) {
        DownloadState = other.DownloadState;
      }
      if (other.Score != 0) {
        Score = other.Score;
      }
      if (other.Combo != 0) {
        Combo = other.Combo;
      }
      if (other.Accuracy != 0D) {
        Accuracy = other.Accuracy;
      }
      if (other.SongPosition != 0D) {
        SongPosition = other.SongPosition;
      }
      if (other.songList_ != null) {
        if (songList_ == null) {
          SongList = new global::TournamentAssistantShared.Models.Packets.SongList();
        }
        SongList.MergeFrom(other.SongList);
      }
      modList_.Add(other.modList_);
      if (other.streamScreenCoordinates_ != null) {
        if (streamScreenCoordinates_ == null) {
          StreamScreenCoordinates = new global::TournamentAssistantShared.Models.Player.Types.Point();
        }
        StreamScreenCoordinates.MergeFrom(other.StreamScreenCoordinates);
      }
      if (other.StreamDelayMs != 0L) {
        StreamDelayMs = other.StreamDelayMs;
      }
      if (other.StreamSyncStartMs != 0L) {
        StreamSyncStartMs = other.StreamSyncStartMs;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            if (user_ == null) {
              User = new global::TournamentAssistantShared.Models.User();
            }
            input.ReadMessage(User);
            break;
          }
          case 18: {
            UserId = input.ReadString();
            break;
          }
          case 26: {
            if (team_ == null) {
              Team = new global::TournamentAssistantShared.Models.Team();
            }
            input.ReadMessage(Team);
            break;
          }
          case 32: {
            PlayState = (global::TournamentAssistantShared.Models.Player.Types.PlayStates) input.ReadEnum();
            break;
          }
          case 40: {
            DownloadState = (global::TournamentAssistantShared.Models.Player.Types.DownloadStates) input.ReadEnum();
            break;
          }
          case 48: {
            Score = input.ReadInt32();
            break;
          }
          case 56: {
            Combo = input.ReadInt32();
            break;
          }
          case 65: {
            Accuracy = input.ReadDouble();
            break;
          }
          case 73: {
            SongPosition = input.ReadDouble();
            break;
          }
          case 82: {
            if (songList_ == null) {
              SongList = new global::TournamentAssistantShared.Models.Packets.SongList();
            }
            input.ReadMessage(SongList);
            break;
          }
          case 90: {
            modList_.AddEntriesFrom(input, _repeated_modList_codec);
            break;
          }
          case 98: {
            if (streamScreenCoordinates_ == null) {
              StreamScreenCoordinates = new global::TournamentAssistantShared.Models.Player.Types.Point();
            }
            input.ReadMessage(StreamScreenCoordinates);
            break;
          }
          case 104: {
            StreamDelayMs = input.ReadInt64();
            break;
          }
          case 112: {
            StreamSyncStartMs = input.ReadInt64();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            if (user_ == null) {
              User = new global::TournamentAssistantShared.Models.User();
            }
            input.ReadMessage(User);
            break;
          }
          case 18: {
            UserId = input.ReadString();
            break;
          }
          case 26: {
            if (team_ == null) {
              Team = new global::TournamentAssistantShared.Models.Team();
            }
            input.ReadMessage(Team);
            break;
          }
          case 32: {
            PlayState = (global::TournamentAssistantShared.Models.Player.Types.PlayStates) input.ReadEnum();
            break;
          }
          case 40: {
            DownloadState = (global::TournamentAssistantShared.Models.Player.Types.DownloadStates) input.ReadEnum();
            break;
          }
          case 48: {
            Score = input.ReadInt32();
            break;
          }
          case 56: {
            Combo = input.ReadInt32();
            break;
          }
          case 65: {
            Accuracy = input.ReadDouble();
            break;
          }
          case 73: {
            SongPosition = input.ReadDouble();
            break;
          }
          case 82: {
            if (songList_ == null) {
              SongList = new global::TournamentAssistantShared.Models.Packets.SongList();
            }
            input.ReadMessage(SongList);
            break;
          }
          case 90: {
            modList_.AddEntriesFrom(ref input, _repeated_modList_codec);
            break;
          }
          case 98: {
            if (streamScreenCoordinates_ == null) {
              StreamScreenCoordinates = new global::TournamentAssistantShared.Models.Player.Types.Point();
            }
            input.ReadMessage(StreamScreenCoordinates);
            break;
          }
          case 104: {
            StreamDelayMs = input.ReadInt64();
            break;
          }
          case 112: {
            StreamSyncStartMs = input.ReadInt64();
            break;
          }
        }
      }
    }
    #endif

    #region Nested types
    /// <summary>Container for nested types declared in the Player message type.</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static partial class Types {
      public enum PlayStates {
        [pbr::OriginalName("Waiting")] Waiting = 0,
        [pbr::OriginalName("InGame")] InGame = 1,
      }

      public enum DownloadStates {
        [pbr::OriginalName("None")] None = 0,
        [pbr::OriginalName("Downloading")] Downloading = 1,
        [pbr::OriginalName("Downloaded")] Downloaded = 2,
        [pbr::OriginalName("DownloadError")] DownloadError = 3,
      }

      public sealed partial class Point : pb::IMessage<Point>
      #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
          , pb::IBufferMessage
      #endif
      {
        private static readonly pb::MessageParser<Point> _parser = new pb::MessageParser<Point>(() => new Point());
        private pb::UnknownFieldSet _unknownFields;
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        public static pb::MessageParser<Point> Parser { get { return _parser; } }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        public static pbr::MessageDescriptor Descriptor {
          get { return global::TournamentAssistantShared.Models.Player.Descriptor.NestedTypes[0]; }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        pbr::MessageDescriptor pb::IMessage.Descriptor {
          get { return Descriptor; }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        public Point() {
          OnConstruction();
        }

        partial void OnConstruction();

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        public Point(Point other) : this() {
          x_ = other.x_;
          y_ = other.y_;
          _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        public Point Clone() {
          return new Point(this);
        }

        /// <summary>Field number for the "x" field.</summary>
        public const int XFieldNumber = 1;
        private int x_;
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        public int X {
          get { return x_; }
          set {
            x_ = value;
          }
        }

        /// <summary>Field number for the "y" field.</summary>
        public const int YFieldNumber = 2;
        private int y_;
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        public int Y {
          get { return y_; }
          set {
            y_ = value;
          }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        public override bool Equals(object other) {
          return Equals(other as Point);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        public bool Equals(Point other) {
          if (ReferenceEquals(other, null)) {
            return false;
          }
          if (ReferenceEquals(other, this)) {
            return true;
          }
          if (X != other.X) return false;
          if (Y != other.Y) return false;
          return Equals(_unknownFields, other._unknownFields);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        public override int GetHashCode() {
          int hash = 1;
          if (X != 0) hash ^= X.GetHashCode();
          if (Y != 0) hash ^= Y.GetHashCode();
          if (_unknownFields != null) {
            hash ^= _unknownFields.GetHashCode();
          }
          return hash;
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        public override string ToString() {
          return pb::JsonFormatter.ToDiagnosticString(this);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        public void WriteTo(pb::CodedOutputStream output) {
        #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
          output.WriteRawMessage(this);
        #else
          if (X != 0) {
            output.WriteRawTag(8);
            output.WriteInt32(X);
          }
          if (Y != 0) {
            output.WriteRawTag(16);
            output.WriteInt32(Y);
          }
          if (_unknownFields != null) {
            _unknownFields.WriteTo(output);
          }
        #endif
        }

        #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
          if (X != 0) {
            output.WriteRawTag(8);
            output.WriteInt32(X);
          }
          if (Y != 0) {
            output.WriteRawTag(16);
            output.WriteInt32(Y);
          }
          if (_unknownFields != null) {
            _unknownFields.WriteTo(ref output);
          }
        }
        #endif

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        public int CalculateSize() {
          int size = 0;
          if (X != 0) {
            size += 1 + pb::CodedOutputStream.ComputeInt32Size(X);
          }
          if (Y != 0) {
            size += 1 + pb::CodedOutputStream.ComputeInt32Size(Y);
          }
          if (_unknownFields != null) {
            size += _unknownFields.CalculateSize();
          }
          return size;
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        public void MergeFrom(Point other) {
          if (other == null) {
            return;
          }
          if (other.X != 0) {
            X = other.X;
          }
          if (other.Y != 0) {
            Y = other.Y;
          }
          _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        public void MergeFrom(pb::CodedInputStream input) {
        #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
          input.ReadRawMessage(this);
        #else
          uint tag;
          while ((tag = input.ReadTag()) != 0) {
            switch(tag) {
              default:
                _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
                break;
              case 8: {
                X = input.ReadInt32();
                break;
              }
              case 16: {
                Y = input.ReadInt32();
                break;
              }
            }
          }
        #endif
        }

        #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
          uint tag;
          while ((tag = input.ReadTag()) != 0) {
            switch(tag) {
              default:
                _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
                break;
              case 8: {
                X = input.ReadInt32();
                break;
              }
              case 16: {
                Y = input.ReadInt32();
                break;
              }
            }
          }
        }
        #endif

      }

    }
    #endregion

  }

  #endregion

}

#endregion Designer generated code
