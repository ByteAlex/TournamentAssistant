﻿using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using static TournamentAssistantShared.GlobalConstants;

/**
 * Modified by Moon on ?/?/2018 (Originally taken from andruzzzhka's work)
 * Simple wrapper for Console.Log which makes logging
 * a little prettier
 */

namespace TournamentAssistantShared
{
    public class Logger
    {
        //Enable communication between pluginlogger class and shared.
        public static event Action<object, LogType> PluginLog;
        private static object LockObject = new object();

        private static string[] LogFiles = 
        {
            "Error.txt",
            "Warning.txt",
            "Info.txt",
            "Success.txt",
            "Debug.txt",
            "All.txt"
        };

        //Added for the purpose of viewing log info in the UI
        public enum LogType
        {
            Error,
            Warning,
            Info,
            Success,
            Debug
        }

        public static event Action<LogType, string> MessageLogged;

        public static void LoggerFileInit()
        {
            if (LogAllToFile)
            {
                string path;
                if (IsServer) path = ServerDataLogs;
                else path = AppDataLogs;

                foreach (var fileName in LogFiles)
                {
                    if (!File.Exists($"{path}{fileName}")) File.Create($"{path}{fileName}");
                }
            }
        }

        public static void ArchiveLogs()
        {
            string path;
            if (IsServer) path = ServerDataLogs;
            else path = AppDataLogs;

            string archivePath = $"{path}";
            string folderName = $"{DateTime.UtcNow}";

            foreach (char illegalChar in IllegalPathCharacters)
                folderName = folderName.Replace(illegalChar, '_');

            archivePath += folderName;

            Directory.CreateDirectory($"{archivePath}");

            foreach (var fileName in LogFiles)
            {
                while (File.Exists($"{path}{fileName}"))
                {
                    try
                    {
                        File.Move($"{path}{fileName}", $"{archivePath}{Path.DirectorySeparatorChar}{fileName}");
                    }
                    catch (IOException)
                    {
                        //dont do anything, just try again (we want to close the logger, so thats why not log this)
                    }
                }
            }
        }

        public static void Error(object message)
        {
            MessageLogged?.Invoke(LogType.Error, message.ToString());
            LogString(message, LogType.Error);
        }

        public static void Warning(object message)
        {
            MessageLogged?.Invoke(LogType.Warning, message.ToString());
            LogString(message, LogType.Warning);
        }

        public static void Info(object message)
        {
            MessageLogged?.Invoke(LogType.Info, message.ToString());
            LogString(message, LogType.Info);
        }

        public static void Success(object message)
        {
            MessageLogged?.Invoke(LogType.Success, message.ToString());
            LogString(message, LogType.Success);
        }

        public static void Debug(object message)
        {
#if DEBUG
            MessageLogged?.Invoke(LogType.Debug, message.ToString());
            LogString(message, LogType.Debug);
#endif
        }

        public static void ColoredLog(object message, ConsoleColor color, LogType type)
        {
            MessageLogged?.Invoke(type, message.ToString());
            LogString(message, type, color);
        }

        private static void LogString(object message, LogType type, ConsoleColor color = default)
        {
            message = $"[{NameOfCallingClass().Replace("TournamentAssistant", string.Empty)}]: {message}";

            //Prevent two threads writing to console at the same time (and to a file too)
            lock (LockObject)
            {
                if (IsPlugin)
                {
                    PluginLog?.Invoke(message, type); //God this is SO ugly
                    return;
                }

                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ForegroundColor = default;
                if (LogAllToFile)
                {
                    string path;
                    if (IsServer) path = ServerDataLogs;
                    else path = AppDataLogs;

                    while (true)
                    {
                        try
                        {
                            //Moon, I know you hate unawaited async code, but please leave these async,
                            //because we dont want to for instance wait for a drive to spin up or smth on main thread
                            //(this code is called from basically everywhere, including main thread)
                            //In fact, if this was an app with more logs I would go straight for a logger reserved thread with exec queue as to not clog up any other threads by writing to COUT
                            //Which would in that case also prevent two threads from writing to COUT at the same time mashing the logs together
                            StreamWriter writer = new($"{path}All.txt", true);
                            switch (type)
                            {
                                case LogType.Error:
                                    StreamWriter errorWriter = new($"{path}Error.txt", true);
                                    errorWriter.WriteLineAsync($"[{type}]: {message}");
                                    writer.WriteLineAsync($"[{type}]: {message}");
                                    errorWriter.Close();
                                    break;
                                case LogType.Warning:
                                    StreamWriter warningWriter = new($"{path}Warning.txt", true);
                                    warningWriter.WriteLineAsync($"[{type}]: {message}");
                                    writer.WriteLineAsync($"[{type}]: {message}");
                                    warningWriter.Close();
                                    break;
                                case LogType.Info:
                                    StreamWriter infoWriter = new($"{path}Info.txt", true);
                                    infoWriter.WriteLineAsync($"[{type}]: {message}");
                                    writer.WriteLineAsync($"[{type}]: {message}");
                                    infoWriter.Close();
                                    break;
                                case LogType.Success:
                                    StreamWriter successWriter = new($"{path}Success.txt", true);
                                    successWriter.WriteLineAsync($"[{type}]: {message}");
                                    writer.WriteLineAsync($"[{type}]: {message}");
                                    successWriter.Close();
                                    break;
                                case LogType.Debug:
                                    StreamWriter debugWriter = new($"{path}Debug.txt", true);
                                    debugWriter.WriteLineAsync($"[{type}]: {message}");
                                    writer.WriteLineAsync($"[{type}]: {message}");
                                    debugWriter.Close();
                                    break;
                                default:
                                    writer.WriteLineAsync($"[{type}]: {message}");
                                    break;
                            }
                            writer.Close();
                            break;
                        }
                        catch (IOException)
                        {
                            //Dont do anything, just try again
                        }
                    }
                }
            }
        }

        //Stolen from StackOverflow
        public static string NameOfCallingClass()
        {
            string fullName;
            Type declaringType;
            int skipFrames = 3;
            do
            {
                MethodBase method = new StackFrame(skipFrames, false).GetMethod();
                declaringType = method.DeclaringType;
                if (declaringType == null)
                {
                    return method.Name;
                }
                skipFrames++;
                fullName = declaringType.FullName;
            }
            while (declaringType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase));

            return fullName;
        }
    }
}
