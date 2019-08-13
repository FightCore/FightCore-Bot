using System.Threading.Tasks;
using Discord;

namespace FightCore.DiscordBot.Logging
{
    public interface ILogger
    {
        Task LogMessage(LogMessage message);
    }
}