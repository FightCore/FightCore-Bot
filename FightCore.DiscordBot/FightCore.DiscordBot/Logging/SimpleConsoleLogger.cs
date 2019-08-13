using System;
using System.Threading.Tasks;
using Discord;

namespace FightCore.DiscordBot.Logging
{
    public class SimpleConsoleLogger : ILogger
    {
        /// <inheritdoc />
        public Task LogMessage(LogMessage message)
        {
            Console.WriteLine($"{message.Severity} || {message.Source}: {message.Message}");
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public void Initialize()
        {
            // Ignored as console doesn't need to prepare.
        }
    }
}