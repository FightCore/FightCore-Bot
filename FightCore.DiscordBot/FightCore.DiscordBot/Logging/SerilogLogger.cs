using System;
using System.Threading.Tasks;
using Discord;
using Serilog;

namespace FightCore.DiscordBot.Logging
{
    public class SerilogLogger : ILogger
    {
        private Serilog.ILogger _logger;
        public Task LogMessage(LogMessage message)
        {
            switch (message.Severity)
            {
                case LogSeverity.Error:
                case LogSeverity.Critical:
                    _logger.Fatal($"{message.Source}: {message.Message} {message.Exception}");
                    break;
                case LogSeverity.Warning:
                    _logger.Warning($"{message.Source}: {message.Message} {message.Exception}");
                    break;
                case LogSeverity.Info:
                    _logger.Information($"{message.Source}: {message.Message} {message.Exception}");
                    break;
                case LogSeverity.Verbose:
                    _logger.Verbose($"{message.Source}: {message.Message} {message.Exception}");
                    break;
                case LogSeverity.Debug:
                    _logger.Debug($"{message.Source}: {message.Message} {message.Exception}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            return Task.CompletedTask;
        }

        public void Initialize()
        {
            _logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}