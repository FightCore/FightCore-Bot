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

        /// <summary>
        /// Starts the main application.
        /// </summary>
        /// <returns>An infinite task.</returns>
        private async Task MainAsync()
        {
            _logger = new SerilogLogger();
            _logger.Initialize();
            
            _discordClient = new DiscordSocketClient();
            _commandService = new CommandService();

            _discordClient.Log += Log;
            
            var commandHandler = new CommandHandler(_discordClient, _commandService);
            await commandHandler.InstallCommandsAsync(BuildServiceProvider());

            var token = Environment.GetEnvironmentVariable("DiscordToken");
            await _discordClient.LoginAsync(TokenType.Bot, token);
            await _discordClient.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        /// <summary>
        /// Builds the dependencies into a <see cref="IServiceProvider"/>.
        /// </summary>
        /// <returns>The <see cref="IServiceProvider"/> containing all the services.</returns>
        private IServiceProvider BuildServiceProvider() => new ServiceCollection()
            .AddSingleton(_discordClient)
            .AddSingleton(_commandService)
            .AddSingleton(_logger)
            .BuildServiceProvider();


        /// <summary>
        /// Static logging method to be able to use a non-static class.
        /// This is so there can be a logger interface as it may be switched from time to time.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        /// <returns>An awaitable task.</returns>
        private static Task Log(LogMessage message)
        {
            _logger.LogMessage(message);
            return Task.CompletedTask;
        }
    }
}