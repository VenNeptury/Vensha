using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using Vensha.Services;

namespace Vensha.Handlers
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;

        public CommandHandler(IServiceProvider services)
        {
            _services = services;
            _commands = services.GetRequiredService<CommandService>();
            _client = services.GetRequiredService<DiscordSocketClient>();

            _client.MessageReceived += HandleCommands;
            _commands.Log += Logging.Log;
        }

        public Task Initialise() => _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        private async Task HandleCommands(SocketMessage socketMessage)
        {
            var message = socketMessage as SocketUserMessage;
            if (message == null || message.Author.IsBot || message.Author.IsWebhook) return;

            int argPos = 0;
            if (!message.HasStringPrefix(Constants.Config.Prefix, ref argPos) && !message.HasMentionPrefix(_client.CurrentUser, ref argPos)) return;

            var context = new CommandContext(_client, message);

            var result = await _commands.ExecuteAsync(context, argPos, _services, MultiMatchHandling.Best);

            if (!result.IsSuccess && result.ErrorReason != "Unknown command.") await Logging.Log("CommandHandler", LogSeverity.Error, result.ErrorReason);
            else if (result.IsSuccess) await Logging.Log("CommandHandler", LogSeverity.Debug, $"Command successfully executed: {message.Content.Substring(0, Math.Min(20, message.Content.Length))}");
        }
    }
}