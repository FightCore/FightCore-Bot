using System.Threading.Tasks;
using Discord.Commands;

namespace FightCore.DiscordBot.Modules
{
    [Group("test")]
    public class TestModule : ModuleBase
    {
        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync("Pong!");
        }
    }
}