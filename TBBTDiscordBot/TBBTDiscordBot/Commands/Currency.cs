using Discord.Addons.Interactive;
using Discord.Commands;
using System.Threading.Tasks;
using TBBTDiscordBot.Handlers;
using Discord.WebSocket;
using TBBTDiscordBot.Handlers.DataHandling;
using System;
using Discord;

namespace TBBTDiscordBot.Commands
{
    [RequireContext(ContextType.Guild)]
    public class Currency : InteractiveBase
    {
        // https://www.amandaclegg.co.uk/wp-content/uploads/2019/11/Nerds.jpg

        [Command("comics")]
        [Alias("comic books", "books", "cb", "comics book", "comics books")]
        [Summary("Displays the amount of comics a user has.")]
        public async Task DisplayComics() => await ComicBooksHandler.DisplayComicBooks(Context, (SocketGuildUser)Context.User, Context.Channel);

        [Command("rob")]
        [Alias("pickpocket")]
        [Summary("Steal nickels from another user")]
        public async Task Rob(SocketGuildUser user) => await ComicBooksHandler.PickPocket(Context, user);

        // Spawn Comics
        [Command("spawn")]
        [RequireOwner]
        public async Task SpawnComics(int amount, SocketGuildUser user)
        {
            ComicBooksHandler.AdjustComicBooks(user, amount);
            await Context.Channel.SendMessageAsync($"Spawned {user.Mention} {amount} comics.");
        }

        [Command("gamble")]
        [Summary("Take a risk to increase your comics but you may lose them!")]
        public async Task GambleComics(int amount)
        {
            var account = UserAccounts.GetAccount(Context.User);

            if (amount > account.ComicBooks)
            {
                await Utilities.PrintError(Context.Channel, "Did you take a marijuana!? You do not have that many comics.");
            }
            else if (amount < 0 || amount.ToString().StartsWith("-"))
            {
                await Utilities.PrintError(Context.Channel, "Did you take a marijuana!? You cannot gamble with negative numbers");
            }
            else if (amount > 0 && amount <= account.ComicBooks)
            {
                Random random = new Random();
                int rollNumber = random.Next(0, 99);
                if (rollNumber > 50)
                {
                    ComicBooksHandler.AdjustComicBooks(Context.User, amount * 2);
                    ComicBooksHandler.AdjustComicsWonFromG((SocketGuildUser)Context.User, amount * 2);
                    ComicBooksHandler.AdjustGwins(Context.User, 1);
                    await Utilities.PrintSuccess(Context.Channel, $"Well Done, you rolled {rollNumber}/100 and won {amount * 2}");
                }
                else if (rollNumber < 50)
                {
                    ComicBooksHandler.AdjustGLosts((SocketGuildUser)Context.User, 1);
                    ComicBooksHandler.AdjustComicsLostFromG((SocketGuildUser)Context.User, amount);
                    ComicBooksHandler.AdjustComicBooks(Context.User, -amount);
                    await Utilities.PrintErrorGamble(Context.Channel, $"You lost! You rolled {rollNumber}/100 and lost {amount}");
                }
                else if (rollNumber == 50)
                {
                    await Context.Channel.SendMessageAsync($"You broke even!");
                }
            }
        }

        [Command("comics lb")]
        [Alias("comic lb", "clb")]
        public async Task DisplayLB() => await ComicBooksHandler.PrintComicBooksLeaderboard(Context);

        [Command("spawntoall")]
        [RequireOwner]
        public async Task GiveToAll([Remainder] int a)
        {

            foreach (var user in Context.Guild.Users)
            {
                try
                {
                    ComicBooksHandler.AdjustComicBooks(user, a);
                    await Context.Channel.SendMessageAsync($"Spawned {user} {a} comics.");
                }
                catch { }
            }
            await Context.Channel.SendMessageAsync($"Spawned all");
        }

        [Command("gamble all")]
        public async Task GambleAll()
        {
            var account = UserAccounts.GetAccount(Context.User);

            Random random = new Random();
            int amount = account.ComicBooks;
            int rollNumber = random.Next(0, 99);
            if (rollNumber > 50)
            {
                ComicBooksHandler.AdjustGwins((SocketGuildUser)Context.User, 1);
                ComicBooksHandler.AdjustComicBooks(Context.User, amount * 2);
                ComicBooksHandler.AdjustComicsWonFromG((SocketGuildUser)Context.User, amount * 2);
                await Utilities.PrintSuccess(Context.Channel, $"Well Done, you rolled {rollNumber}/100 and won {amount * 2}");
            }
            else if (rollNumber < 50)
            {
                ComicBooksHandler.AdjustComicBooks(Context.User, -amount);
                ComicBooksHandler.AdjustComicsLostFromG((SocketGuildUser)Context.User, amount);
                ComicBooksHandler.AdjustGLosts((SocketGuildUser)Context.User, 1);
                await Utilities.PrintErrorGamble(Context.Channel, $"You lost! You rolled {rollNumber}/100 and lost {amount}");
            }
            else if (rollNumber == 50)
            {
                await Context.Channel.SendMessageAsync($"You broke even!");
            }
        }

        [Command("gwins")]
        public async Task DisplayGWins()
        {
            await ComicBooksHandler.DisplayGambleWins(Context, (SocketGuildUser)Context.User, Context.Channel);
        }
    }
}