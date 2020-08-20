using System;
using System.Threading.Tasks;
using Discord;

namespace Vensha.Services {
    public static class Logging {
        private static LogSeverity logLevel = LogSeverity.Info;
        public static async Task Log (string source, LogSeverity severity, string message, Exception error = null) {
            if (severity > logLevel) return;

            await AppendMessage ((ConsoleColor) (-1), DateTime.Now.ToLongTimeString () + ' ');

            await AppendMessage (GetColour (severity), $"[{source}] ");

            if (!String.IsNullOrEmpty (message)) await AppendMessage (ConsoleColor.White, message + ' ');
            else if (error != null && !String.IsNullOrEmpty (error.Message)) await AppendMessage (ConsoleColor.Red, error.Message);

            Console.WriteLine ();
        }
        public static Task Log (LogMessage msg) => Log (msg.Source, msg.Severity, msg.Message, msg.Exception);

        private static Task AppendMessage (ConsoleColor color, string message) {
            Console.ForegroundColor = color;
            Console.Write (message);
            Console.ResetColor ();
            return Task.CompletedTask;
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