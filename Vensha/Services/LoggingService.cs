using System;
using Discord;

namespace Vensha.Services {
    public static class Logging {
        private static LogSeverity logLevel = LogSeverity.Info;
        public static void Log (string source, LogSeverity severity, string message, Exception error = null) {
            if (severity > logLevel) return;

            AppendMessage ((ConsoleColor) (-1), DateTime.Now.ToLongTimeString () + ' ');

            AppendMessage (GetColour (severity), $"[{source}] ");

            if (!String.IsNullOrEmpty (message)) AppendMessage (ConsoleColor.White, message + ' ');
            else if (error != null && !String.IsNullOrEmpty (error.Message)) AppendMessage (ConsoleColor.Red, error.Message);

            Console.WriteLine ();
        }
        public static void Log (LogMessage msg) => Log (msg.Source, msg.Severity, msg.Message, msg.Exception);

        private static void AppendMessage (ConsoleColor color, string message) {
            Console.ForegroundColor = color;
            Console.Write (message);
            Console.ResetColor ();
        }

        private static ConsoleColor GetColour (LogSeverity severity) {
            switch (severity) {
                case LogSeverity.Critical:
                    return ConsoleColor.Red;
                case LogSeverity.Error:
                    return ConsoleColor.DarkRed;
                case LogSeverity.Warning:
                    return ConsoleColor.Yellow;
                case LogSeverity.Info:
                    return ConsoleColor.Cyan;
                case LogSeverity.Verbose:
                    return ConsoleColor.DarkCyan;
                case LogSeverity.Debug:
                    return ConsoleColor.Gray;
                default:
                    return ConsoleColor.White;
            }
        }
    }
}