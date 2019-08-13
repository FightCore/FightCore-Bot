using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using FightCore.DiscordBot.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace FightCore.DiscordBot
{
    public class Program
    {
        private DiscordSocketClient _discordClient;
        private CommandService _commandService;
        private static ILogger _logger;
        
        static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync()
        {
            _logger = new SerilogLogger();
            _logger.Initialize();
            
            _discordClient = new DiscordSocketClient();
            _commandService = new CommandService();

            _discordClient.Log += Log;
            
            var commandHandler = new CommandHandler(_discordClient, _commandService);
            await commandHandler.InstallCommandsAsync(BuildServiceProvider());

            var token = Environment.GetEnvironmentVariable("DiscordToken");
            await _discordClient.LoginAsync(TokenType.Bot, token
                );
            await _discordClient.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        public IServiceProvider BuildServiceProvider() => new ServiceCollection()
            .AddSingleton(_discordClient)
            .AddSingleton(_commandService)
            .AddSingleton(_logger)
            .BuildServiceProvider();


        private static Task Log(LogMessage message)
        {
            _logger.LogMessage(message);
            return Task.CompletedTask;
        }
    }
}