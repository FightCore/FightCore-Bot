using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using FightCore.DiscordBot.Logging;

namespace FightCore.DiscordBot.Modules
{
    [Group("bot")]
    public class BotModule : ModuleBase
    {
        private readonly DiscordSocketClient _discordClient;
        private readonly ILogger _logger;
        
        public BotModule(DiscordSocketClient client, ILogger logger)
        {
            _discordClient = client;
            _logger = logger;
        }
        
        [Command("game")]
        public async Task SetGame([Remainder]string game)
        {
            await _discordClient.SetGameAsync(game);
            await _logger.LogMessage(new LogMessage(LogSeverity.Debug, nameof(BotModule),
                $"{Context.User.Username} set the game to {game}"));
            await ReplyAsync("Done!");
        }
    }
}