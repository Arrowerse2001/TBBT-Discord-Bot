using Discord.Commands;
using System.Threading.Tasks;
using Discord.Addons.Interactive;
using Discord;
using System.Collections.Generic;
using System.Linq;
using TBBTDiscordBot.Handlers;
using Discord.WebSocket;
using System;
using System.Text;
using TBBTDiscordBot.Preconditions;

/*
  Put your head down and work hard. Never wait for things to happen, make them happen through hard graft and not givinig up.
 */
namespace TBBTDiscordBot.Commands
{
    [RequireContext(ContextType.Guild)]
    public class Misc : InteractiveBase
    {
        public static IDictionary<string, DateTimeOffset> timeList = new Dictionary<string, DateTimeOffset>();

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
        public async Task ComicStore() => await ComicBooksHandler.DisplayCoinsStore(Context, (SocketGuildUser)Context.User, Context.Channel);

        [Command("speak")]
        public async Task SaySomething([Remainder]string msg)
        {
            await Context.Message.DeleteAsync();
            var client = Context.Client;
            ulong channelID = 784578754785312828; // general
            var c = client.GetChannel(channelID) as SocketTextChannel;
            await c.SendMessageAsync($"{msg}");
            await Context.Channel.SendMessageAsync("Posted!");
        }

        [Command("dm")]
        public async Task DMUser(SocketGuildUser user, [Remainder]string msg)
        {
            await Context.Message.DeleteAsync();
            await user.SendMessageAsync(msg);
            await Context.Channel.SendMessageAsync($"Message Sent!");
        }

        [Command("xp")]
        public async Task DisplayRank(SocketUser user = null) => await RankHandler.DisplayLevelAndXP(Context, user ?? Context.User);

        [Command("ranks")]
        [Alias("levels", "roles")]
        public async Task ViewRanks()
        {
            StringBuilder roles = new StringBuilder()
                .AppendLine("Level 5: Busboy")
                .AppendLine("Level 7: Waitress")
                .AppendLine("Level 10: Actress")
                .AppendLine("Level 12: Geek")
                .AppendLine("Level 14: Comic Book Store Owner")
                .AppendLine("Level 16: Geologist")
                .AppendLine("Level 19: Sales Rep")
                .AppendLine("Level 21: Engineer")
                .AppendLine("Level 23: Astronomer")
                .AppendLine("Level 24: Chemist")
                .AppendLine("Level 25: Biochemist")
                .AppendLine("Level 26: Micro-Biologist")
                .AppendLine("Level 27: Biologist")
                .AppendLine("Level 28: Astronaut")
                .AppendLine("Level 29: Experimental Physicist")
                .AppendLine("Level 33: Theoretical Physicist")
                .AppendLine("Level 40: Nobel Prize Winners");
            await Utilities.SendEmbed(Context.Channel, "XP Roles", roles.ToString(), Colours.Blue, "You get 15-25 xp for sending a message, but only once a minute.", "");
        }

        [Command("xp add")]
        [RequireRole("Tenure Committee")]
        public async Task AddXP(SocketUser user, int xp)
        {
            RankHandler.GiveUserXP(user, xp);
            await RankHandler.CheckXP(Context, user);
            await Context.Channel.SendMessageAsync($"Added {xp} to {user.Mention}.");
        }

        [Command("xp remove")]
        [RequireRole("Tenure Committee")]
        public async Task RemoveXP(SocketUser user, int xp)
        {
            RankHandler.RemoveXP(user, xp);
            await RankHandler.CheckXP(Context, user);
            await Context.Channel.SendMessageAsync($"Removed {xp} from {user.Mention}");
        }

        [Command("xp lb")]
        public async Task DisplayXPLB() => await RankHandler.PrintComicBooksLeaderboard(Context);
    }
}