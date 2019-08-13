using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace FightCore.DiscordBot
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private IServiceProvider _serviceProvider;

        public CommandHandler(DiscordSocketClient client, CommandService commands)
        {
            _commands = commands;
            _client = client;
        }

        /// <summary>
        /// Installs the commands into the <see cref="DiscordSocketClient"/>
        /// registers the Dependency Injection and all Modules.
        /// </summary>
        /// <param name="serviceProvider">The service provider containing the services.</param>
        /// <returns>an awaitable task.</returns>
        public async Task InstallCommandsAsync(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            // Hook the MessageReceived event into our command handler
            _client.MessageReceived += HandleCommandAsync;

            // Add the dependencies to DI and grab all Modules and register them.
            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
                services: serviceProvider);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process the command if it was a system message
            if (!(messageParam is SocketUserMessage message)) return;

            // Create a number to track where the prefix ends and the command begins
            var argumentPosition = 0;

            // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            if (!(message.HasCharPrefix('-', ref argumentPosition) ||
                  message.HasMentionPrefix(_client.CurrentUser, ref argumentPosition)) ||
                message.Author.IsBot)
                return;

            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(_client, message);

            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.

            // Keep in mind that result does not indicate a return value
            // rather an object stating if the command executed successfully.
            var result = await _commands.ExecuteAsync(
                context: context,
                argPos: argumentPosition,
                services: _serviceProvider);

            // Optionally, we may inform the user if the command fails
            // to be executed; however, this may not always be desired,
            // as it may clog up the request queue should a user spam a
            // command.
            // if (!result.IsSuccess)
            // await context.Channel.SendMessageAsync(result.ErrorReason);
        }
    }
}