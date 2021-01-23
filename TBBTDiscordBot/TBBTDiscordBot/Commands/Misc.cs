using Discord.Commands;
using System.Threading.Tasks;
using Discord.Addons.Interactive;
using Discord;
using System.Collections.Generic;
using System.Linq;
using TBBTDiscordBot.Handlers;

namespace TBBTDiscordBot.Commands
{
    [RequireContext(ContextType.Guild)]
    public class Misc : InteractiveBase
    {

        [Command("help")]
        [Summary("Displays Help Menu")]
        public async Task HelpMenu()
        {
            
        }

        [Command("ping")]
        [Summary("Replies with pong to see if the bot is online.")]
        public async Task PingPong() => await Context.Channel.SendMessageAsync("Pong!");

        // Reddit
        [Command("bbtr")]
        [Alias("big bang reddit", "br", "bigbangtheoryreddit", "big bang theory reddit")]
        public async Task DisplayBBTR() => await TheBigBangTheoryReddit.PrintRandomThought(Context.Channel);
    }
}