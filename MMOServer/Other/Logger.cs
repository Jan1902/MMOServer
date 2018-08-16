using MMOServer.ConsoleStuff;
using System;
using System.IO;

namespace MMOServer.Other
{
    public static class Logger
    {
        private static StreamWriter _writer;

        public static void Initialize()
        {
            var path = GetNewLogFilePath();
            _writer = File.AppendText(path);
            ConsoleUtils.Info("Log file set up at {0}", path);
        }

        public static void LogInfo(string message)
        {
            _writer.WriteLine("[INFO] " + DateTime.Now + ": " + message);
            _writer.Flush();
        }

        public static void LogWarning(string message)
        {
            _writer.WriteLine("[WARNING] " + DateTime.Now + ": " + message);
            _writer.Flush();
        }

        public static void LogError(string message)
        {
            _writer.WriteLine("[ERROR] " + DateTime.Now + ": " + message);
            _writer.Flush();
        }

        private static string GetNewLogFilePath()
        {
            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs")))
            {
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs"));
            }

            var path = String.Format(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "log_{0}.txt"), DateTime.Today.ToShortDateString().Replace('.', '_'));

            var index = 1;
            while (File.Exists(path))
            {
                path = String.Format(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "log_{0}_{1}.txt"), DateTime.Today.ToShortDateString().Replace('.', '_'), index);
                index++;
            }

            return path;
        }
    }
}
