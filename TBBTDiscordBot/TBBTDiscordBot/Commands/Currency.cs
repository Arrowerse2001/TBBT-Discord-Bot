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
        public async Task DisplayComics() => await ComicBooksHandler.DisplayComicBooks(Context, (Discord.WebSocket.SocketGuildUser)Context.User, Context.Channel);

        [Command("rob")]
        [Alias("pickpocket")]
        [Summary("Steal nickels from another user")]
        public async Task Rob(SocketGuildUser user) => await ComicBooksHandler.PickPocket(Context, user);

        [Command("gamble")]
        public async Task GambleNickels([Remainder] int ammount)
        {
            if (ammount <= 0)
            {
                EmbedBuilder builder = new EmbedBuilder();

                builder.WithTitle("Gamble!")
                    .WithColor(Colours.Red)
                    .WithDescription($"You can't gamble for " + (ammount == 0 ? "Comics!" : "negative comics!"))
                    .WithFooter("Did you take a marijuana!?");

                await ReplyAsync("", false, builder.Build());
                return;
            }

            var channel = Context.Channel.Id;
            var account = UserAccounts.GetAccount(Context.User);
            Random random = new Random();
            int randomNumber = random.Next(0, 100);
            if (account.ComicBooks < ammount)
            {
                await Context.Channel.SendMessageAsync("You do not have enough comics for that!");
            }
            if (account.ComicBooks >= ammount)
            {
                if (randomNumber > 50)
                {
                    EmbedBuilder builder = new EmbedBuilder();

                    builder.WithTitle("Gamble!")
                        .WithColor(Colours.Green)
                        .WithDescription($"Well Done you rolled {randomNumber}/100! and have won {ammount}")
                        .WithFooter("Use /comics to see how many you have now!");
                    await ReplyAsync("", false, builder.Build());
                    ComicBooksHandler.AdjustComicBooks(Context.User, ammount);
                }
                if (randomNumber == 50)
                {
                    EmbedBuilder builder = new EmbedBuilder();

                    builder.WithTitle("Gamble!")
                        .WithColor(Colours.Blue)
                        .WithDescription($"You broke even! you rolled {randomNumber}/100! and have retained the same ammount")
                        .WithFooter("Use /comics to see how many you have now!");
                    await ReplyAsync("", false, builder.Build());
                }
                if (randomNumber < 50)
                {
                    EmbedBuilder builder = new EmbedBuilder();

                    builder.WithTitle("Gamble!")
                        .WithColor(Colours.Red)
                        .WithDescription($"You took a risk and lost! you rolled {randomNumber}/100! and have lost {ammount}")
                        .WithFooter("Use /comics to see how many you have now!");
                    await ReplyAsync("", false, builder.Build());
                    ComicBooksHandler.AdjustComicBooks(Context.User, -ammount);
                }
                if (randomNumber.ToString().StartsWith("-"))
                {
                    await Context.Channel.SendMessageAsync("You can't gamble with negative numbers");
                }
                if (randomNumber == 100)
                {
                    await Context.Channel.SendMessageAsync($"{Context.User.Mention} YOU TRIPPLED IT!");
                    EmbedBuilder builder = new EmbedBuilder();

                    builder.WithTitle("Gamble!")
                        .WithColor(Colours.Green)
                        .WithDescription($"Well Done you rolled {randomNumber}/100! and have won {ammount}")
                        .WithFooter("Use /comics to see how many you have now!");
                    await ReplyAsync("", false, builder.Build());
                    ComicBooksHandler.AdjustComicBooks(Context.User, ammount * 3);
                }
            }
        }
    }
}