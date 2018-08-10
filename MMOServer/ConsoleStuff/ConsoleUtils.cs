using MMOServer.Other;
using System;

namespace MMOServer.ConsoleStuff
{
    public static class ConsoleUtils
    {
        /// <summary>
        /// Outputs an error to the console
        /// </summary>
        /// <param name="error"></param>
        /// <param name="args"></param>
        public static void Error(string error, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            error = string.Format(error, args);
            Console.WriteLine("[!] " + error);
            Console.ResetColor();
            Logger.LogError(error);
        }

        /// <summary>
        /// Outputs an info to the console
        /// </summary>
        /// <param name="info"></param>
        /// <param name="args"></param>
        public static void Info(string info, params object[] args)
        {
            info = string.Format(info, args);
            Console.WriteLine("[*] " + info);
            Logger.LogInfo(info);
        }

        /// <summary>
        /// Outputs a warning to the console
        /// </summary>
        /// <param name="warning"></param>
        /// <param name="args"></param>
        public static void Warning(string warning, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            warning = string.Format(warning, args);
            Console.WriteLine("[?] " + warning);
            Console.ResetColor();
            Logger.LogWarning(warning);
        }
    }
}
