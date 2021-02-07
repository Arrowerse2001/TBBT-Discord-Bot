using Discord.Commands;
using System.Threading.Tasks;
using Discord.Addons.Interactive;
using Discord;
using System.Collections.Generic;
using System.Linq;
using TBBTDiscordBot.Handlers;
using Discord.WebSocket;

namespace TBBTDiscordBot.Commands
{
    [RequireContext(ContextType.Guild)]
    public class Misc : InteractiveBase
    {

        [Command("icon")]
        public async Task UploadIcon() => await Context.Channel.SendMessageAsync(Context.Guild.IconUrl);

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

        [Command("speak")]
        public async Task SaySomething([Remainder]string msg)
        {
            await Context.Message.DeleteAsync();
            var ch = Context.Channel.Id;
            var client = Context.Client;
            ulong channelID = 784578754785312828; // break room
            var c = client.GetChannel(channelID) as SocketTextChannel;
            await c.SendMessageAsync($"{msg}");
            await Context.Channel.SendMessageAsync("Posted!");
        }
    }
}