using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Vensha.Handlers;
using Victoria;

namespace Vensha.Services
{
    public class DiscordService
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandHandler _commandHandler;
        private readonly LavaNode _lavaNode;
        private readonly ServiceProvider _services;

        public DiscordService()
        {
            _services = ConfigureServices();
            _client = _services.GetRequiredService<DiscordSocketClient>();
            _commandHandler = _services.GetRequiredService<CommandHandler>();
            _lavaNode = _services.GetRequiredService<LavaNode>();

            _client.Log += Logging.Log;
            _client.Ready += Ready;
        }

        public async Task InitializeAsync()
        {
            Constants.Initialise();

            await _client.LoginAsync(TokenType.Bot, Constants.Config.Token);
            await _client.StartAsync();

            await _commandHandler.Initialise();

            await Task.Delay(-1);
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<Constants>()
                .AddSingleton<LavaNode>()
                .AddSingleton(new LavaConfig())
                .BuildServiceProvider();
        }

        private Task Ready()
        {
            _client.SetStatusAsync(UserStatus.DoNotDisturb);
            _client.SetGameAsync(Constants.Config.BotStatus);
            return Task.CompletedTask;
        }
    }
}