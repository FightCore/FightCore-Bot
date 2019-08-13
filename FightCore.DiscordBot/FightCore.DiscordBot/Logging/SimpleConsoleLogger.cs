using System;
using System.Threading.Tasks;
using Discord;

namespace FightCore.DiscordBot.Logging
{
    public class SimpleConsoleLogger : ILogger
    {
        public Task LogMessage(LogMessage message)
        {
            Console.WriteLine($"{message.Severity} || {message.Source}: {message.Message}");
            
            return Task.CompletedTask;
        }
    }
}