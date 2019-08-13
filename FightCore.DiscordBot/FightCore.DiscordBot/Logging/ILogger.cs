using System.Threading.Tasks;
using Discord;

namespace FightCore.DiscordBot.Logging
{
    public interface ILogger
    {
        /// <summary>
        /// Logs a message to the specified logger.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        /// <returns>An awaitable task.</returns>
        Task LogMessage(LogMessage message);

        /// <summary>
        /// Initializes the logger to be ready for use.
        /// </summary>
        void Initialize();
    }
}