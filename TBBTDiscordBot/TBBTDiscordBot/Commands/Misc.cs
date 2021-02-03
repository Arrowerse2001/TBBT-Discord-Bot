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

       [Command("members")]
       public async Task CountMembers()
       {
            await Context.Channel.SendMessageAsync($"{Context.Guild.MemberCount.ToString()} members");
       }

        [Command("daily")]
        public async Task GetDailyComics() => await ComicBooksHandler.Daily(Context, Context.User);

        [Command("store")]
        public async Task ComicStore() => await Context.Channel.SendMessageAsync("This is still in development.");
    }
}