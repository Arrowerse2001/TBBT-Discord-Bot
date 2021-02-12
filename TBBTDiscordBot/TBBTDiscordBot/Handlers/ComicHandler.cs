using System;
using Discord;
using System.Linq;
using System.Text;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Collections.Generic;
using TBBTDiscordBot.Handlers.DataHandling;


namespace TBBTDiscordBot.Handlers
{
    [RequireContext(ContextType.Guild)]
    static class ComicBooksHandler
    {


        private static readonly Color Gold = new Color(215, 154, 14);
        private const string icon = "https://www.amandaclegg.co.uk/wp-content/uploads/2019/11/Nerds.jpg";

        private static async Task PrintEmbed(ISocketMessageChannel channel, string description) => await Utilities.SendEmbed(channel, "Comic Books", description, Gold, "", icon);
        private static async Task PrintEmbedNoFooter(ISocketMessageChannel channel, string description) => await Utilities.SendEmbed(channel, "Comic Books", description, Gold, "", "");

        // Give ComicBooks to another user (from your own amount)
        public static async Task GiveComicBooks(SocketCommandContext context, SocketGuildUser sender, SocketGuildUser reciever, int amount)
        {
            var SenderAccount = UserAccounts.GetAccount(sender);
            var RecieverAccount = UserAccounts.GetAccount(reciever);
            if (amount < 1)
            {
                await PrintEmbed(context.Channel, $"You must enter an amount greater than 1, {sender.Mention}.").ConfigureAwait(false);
                return;
            }
            else if (amount > SenderAccount.ComicBooks)
            {
                await PrintEmbed(context.Channel, $"You do not have that many Comic Books to send, {sender.Mention}.").ConfigureAwait(false);
                return;
            }
            SenderAccount.ComicBooks -= amount;
            RecieverAccount.ComicBooks += amount;
            UserAccounts.SaveAccounts();
            await PrintEmbedNoFooter(context.Channel, $"{sender.Mention} gave {reciever.Mention} {amount} Comic Books.").ConfigureAwait(false);
        }

        // Spawn ComicBooks for a user
        public static async Task SpawnComicBooks(SocketCommandContext context, SocketGuildUser user, int amount)
        {
            UserAccounts.GetAccount(user).ComicBooks += amount;
            UserAccounts.SaveAccounts();
            await PrintEmbedNoFooter(context.Channel, $"Spawned {user.Mention} {amount} Comic Books.").ConfigureAwait(false);
        }

        // Remove ComicBooks from a user
        public static async Task RemoveComicBooks(SocketCommandContext context, SocketGuildUser user, int amount)
        {
            AdjustComicBooks(user, -amount);
            await PrintEmbedNoFooter(context.Channel, $"{user.Mention} lost {amount} Comic Books.").ConfigureAwait(false);
        }

        // Give or take ComicBooks from a user
        public static void AdjustComicBooks(SocketUser user, int amount) => AdjustComicBooks((SocketGuildUser)user, amount);
        public static void AdjustGwins(SocketUser user, int amount) => AdjustGwins((SocketGuildUser)user, amount);

        public static void AdjustComicBooks(SocketGuildUser user, int amount)
        {
            var account = UserAccounts.GetAccount(user);
            account.ComicBooks += amount;
            if (account.ComicBooks < 0)
                account.ComicBooks = 0;
            UserAccounts.SaveAccounts();
        }

        public static void AdjustComicsWonFromG(SocketGuildUser user, int amount)
        {
            var account = UserAccounts.GetAccount(user);
            account.ComicsWonFromG += amount;
            if (account.ComicsWonFromG < 0)
                account.ComicsWonFromG = 0;
            UserAccounts.SaveAccounts();
        }

        public static void AdjustComicsLostFromG(SocketGuildUser user, int amount)
        {
            var account = UserAccounts.GetAccount(user);
            account.ComicsLostFromG += amount;
            if (account.ComicsLostFromG < 0)
                account.ComicsLostFromG = 0;
            UserAccounts.SaveAccounts();
        }

        public static void AdjustGwins(SocketGuildUser user, int amount)
        {
            var account = UserAccounts.GetAccount(user);
            account.Gwins += amount;
            if (account.Gwins < 0)
                account.Gwins = 0;
            UserAccounts.SaveAccounts();
        }

        public static void AdjustGLosts(SocketGuildUser user, int amount)
        {
            var account = UserAccounts.GetAccount(user);
            account.Glost += amount;
            if (account.Glost < 0)
                account.Glost = 0;
            UserAccounts.SaveAccounts();
        }

        // Display how many ComicBooks a user has
        public static async Task DisplayComicBooks(SocketCommandContext context, SocketGuildUser user, ISocketMessageChannel channel)
        {
            await Utilities.SendEmbed(channel, user.Nickname ?? user.Username, $"{UserAccounts.GetAccount(user).ComicBooks.ToString("#,##0")} Comic Books", Colours.Blue, "", icon);
        }

        public static async Task DisplayGambleWins(SocketCommandContext context, SocketGuildUser user, ISocketMessageChannel channel)
        {
             int total = UserAccounts.GetAccount(user).Glost + UserAccounts.GetAccount(user).Gwins;
             int wins = UserAccounts.GetAccount(user).Gwins;
             //decimal percentage = wins / total * 100;
            // await context.Channel.SendMessageAsync(percentage.ToString("0.##%"));
            decimal value = (decimal)(((double)wins / total) * 100);
            decimal percentage = Convert.ToInt32(Math.Round(value, 2));
             await Utilities.SendEmbed(channel, user.Nickname ?? user.Username, $"{UserAccounts.GetAccount(user).Gwins.ToString("#,##0")} Gamble Wins\n{UserAccounts.GetAccount(user).Glost.ToString("#,##0")} Gamble Losts\nYou have won {UserAccounts.GetAccount(user).ComicsWonFromG.ToString("#,##0")} comics from gambling\nYou have lost {UserAccounts.GetAccount(user).ComicsLostFromG.ToString("#,##0")} comics from gambling\nOn average you have a win rate of {percentage.ToString("#.##")}%", Colours.Blue, "", context.User.GetAvatarUrl());

        }

        #region Pickpocket Related
        private static List<PickPocketUser> PickPocketHistory = new List<PickPocketUser>();
        public static async Task PickPocket(SocketCommandContext context, SocketGuildUser target)
        {
            if (target == null)
            {
                await Utilities.SendEmbed(context.Channel, "Rob", "Attempt to pickpocket others with `/rob @user`", Gold, "", icon);
                return;
            }

            SocketGuildUser self = (SocketGuildUser)context.User;
            if (self == target)
            {
                await PrintEmbed(context.Channel, "You cannot rob yourself").ConfigureAwait(false);
                return;
            }
            foreach (PickPocketUser ppu in PickPocketHistory)
            {
                if (ppu.User == self)
                {
                    if ((DateTime.Now - ppu.TimeStamp).TotalHours <= 24)
                    {
                        string timeLeft = "";
                        if ((12 - ((DateTime.Now - ppu.TimeStamp).TotalHours)) < 1)
                            timeLeft = $"{Math.Round(12 - ((DateTime.Now - ppu.TimeStamp).TotalMinutes), 0)} minutes";
                        else
                            timeLeft = $"{Math.Round(12 - ((DateTime.Now - ppu.TimeStamp).TotalHours), 0)} hours";
                        await Utilities.SendEmbed(context.Channel, "Rob", $"You must wait {timeLeft} before robbing again.", Gold, "", icon);
                        return;
                    }
                    PickPocketHistory.Remove(ppu);
                }
            }
            if (Utilities.GetRandomNumber(0, 2) == 0)
            {
                // Successful pickpocket
                int ComicBooksGained = (int)(UserAccounts.GetAccount(target).ComicBooks * 0.1);
                await Utilities.SendEmbed(context.Channel, "Rob", $"{self.Mention} successfully stole {ComicBooksGained} Comic Books from {target.Mention}.", Gold,"", icon);
                AdjustComicBooks(self, ComicBooksGained);
                AdjustComicBooks(target, -ComicBooksGained);
            }
            else
            {
                // Failed pickpocket
                int ComicBooksLost = (int)(UserAccounts.GetAccount(self).ComicBooks * 0.1);
                await Utilities.SendEmbed(context.Channel, "PickPocket", $"{self.Mention} attempted to pickpocket {target.Mention} and failed, losing {ComicBooksLost} Comic Books.", Gold,
                    "Just poopin’. You know how I be.", icon);
                AdjustComicBooks(self, -ComicBooksLost);
            }
            PickPocketHistory.Add(new PickPocketUser(self, DateTime.Now));
        }
        #endregion

        // Print the top 10 users with the most ComicBooks (and how much they have)
        public static async Task PrintComicBooksLeaderboard(SocketCommandContext context)
        {
            List<int> coinList = new List<int>();
            for (int i = 0; i < context.Guild.Users.Count; i++)
                coinList.Add(UserAccounts.GetAccount(context.Guild.Users.ElementAt(i)).ComicBooks);

            coinList.Sort();
            coinList.Reverse();

            StringBuilder description = new StringBuilder();
            List<SocketGuildUser> PeopleOnLB = new List<SocketGuildUser>();
            for (int coinListIndex = 0; coinListIndex < 10; coinListIndex++)
            {
                for (int userIndex = 0; userIndex < context.Guild.Users.Count; userIndex++)
                {
                    if (UserAccounts.GetAccount(context.Guild.Users.ElementAt(userIndex)).ComicBooks == coinList[coinListIndex] && !PeopleOnLB.Contains(context.Guild.Users.ElementAt(userIndex)))
                    {
                        string name = context.Guild.Users.ElementAt(userIndex).Nickname ?? context.Guild.Users.ElementAt(userIndex).Username;
                        description.AppendLine($"{coinListIndex + 1}. **{name}**, `{coinList[coinListIndex]} Comic Books`");
                        PeopleOnLB.Add(context.Guild.Users.ElementAt(userIndex));
                        break;
                    }
                }
            }
            await Utilities.SendEmbed(context.Channel, "Top 10 Users With The Most Comic Books", description.ToString(), Colours.Gold, "", "");
        }


        private static List<DailyUser> DailyHistory = new List<DailyUser>();
        public static async Task Daily(SocketCommandContext context, SocketUser user)
        {

            SocketUser self = (SocketUser)context.User;

            foreach (DailyUser du in DailyHistory)
            {
                if (du.User == self)
                {
                    if ((DateTime.Now - du.TimeStamp).TotalHours <= 1)
                    {
                        string timeLeft = "";
                        if ((11 - ((DateTime.Now - du.TimeStamp).TotalHours)) < 1)
                            timeLeft = $"{Math.Round(24 - ((DateTime.Now - du.TimeStamp).TotalMinutes), 0)} minutes";
                        else
                            timeLeft = $"{Math.Round(24 - ((DateTime.Now - du.TimeStamp).TotalHours), 0)} hours";
                        await Utilities.SendEmbed(context.Channel, "Daily", $"You must wait {timeLeft} before claiming your daily 50 Comic Books", Gold, "", icon);
                        return;
                    }
                    DailyHistory.Remove(du);
                }
            }
            var account = UserAccounts.GetAccount(context.User);
            ComicBooksHandler.AdjustComicBooks(self, 50);


            var title = "Daily!";
            var a = "You have successfully claimed your daily 50 comic books. You now have " + account.ComicBooks;
            await Utilities.SendEmbed(context.Channel, title, a, Colours.Blue, "Remember to use /store when you have enough comic books!", "");



            DailyHistory.Add(new DailyUser(self, DateTime.Now));
        }
        // A user that has pickpocketed
        public class PickPocketUser : IEquatable<PickPocketUser>
        {
            public SocketGuildUser User { get; }
            public DateTime TimeStamp { get; }

            public PickPocketUser(SocketGuildUser user, DateTime timeStamp)
            {
                User = user;
                TimeStamp = timeStamp;
            }

            public bool Equals(PickPocketUser other) => User.Id == other.User.Id;

            public override bool Equals(object obj) => Equals(obj as PickPocketUser);

            public override int GetHashCode() => 0; // Sorry
        }

        public class DailyUser : IEquatable<DailyUser>
        {
            public SocketUser User { get; }
            public DateTime TimeStamp { get; }

            public DailyUser(SocketUser user, DateTime timeStamp)
            {
                User = user;
                TimeStamp = timeStamp;
            }
            public bool Equals(DailyUser other) => User.Id == User.Id;
            public override bool Equals(object obj) => Equals(obj as DailyUser);
            public override int GetHashCode() => 0;

        }


    }
}