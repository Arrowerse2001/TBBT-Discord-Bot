using Discord;
using System.Text;
using System.Linq;
using System.Timers;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Collections.Generic;
using TBBTDiscordBot.Handlers.DataHandling;

namespace TBBTDiscordBot.Handlers
{
    class RankHandler
    {
        private static List<ulong> UsersGivenXPInLastMinute = new List<ulong>();

        public static void Start()
        {
            Timer timer = new Timer()
            {
                Interval = (1000 * 60),
                AutoReset = true,
                Enabled = true,
            };
            timer.Elapsed += Reset;
        }

        // Every minute, empty the list of users that got xp in the last minute
        private static void Reset(object sender, ElapsedEventArgs e) => UsersGivenXPInLastMinute.Clear();

        // Give a user XP when they talk (15-25 xp once a minute) and then check if they can level up
        public static async Task TryToGiveUserXP(SocketCommandContext context, SocketUser user)
        {
            if (UsersGivenXPInLastMinute.Contains(user.Id)) return;
            GiveUserXP(user, Utilities.GetRandomNumber(15, 25));
            UsersGivenXPInLastMinute.Add(user.Id);
            await CheckXP(context, user);
        }

        public static void GiveUserXP(SocketUser user, int xp)
        {
            UserAccounts.GetAccount(user).xp += xp;
            UserAccounts.SaveAccounts();
        }

        public static void RemoveXP(SocketUser user, int xp)
        {
            UserAccounts.GetAccount(user).xp -= xp;
            UserAccounts.SaveAccounts();
        }

        private static async Task CompareAndSet(SocketCommandContext context, SocketUser user, UserAccount account, uint level, uint targetXP)
        {
            if (account.xp >= targetXP && account.level < level)
            {
                account.level = level;
                UserAccounts.SaveAccounts();
                await Rankup(context, user, level);
            }
        }

        private static async Task AddRole(SocketCommandContext context, SocketUser user, string oldRole, string newRole)
        {
            await (user as IGuildUser).AddRoleAsync(context.Guild.Roles.FirstOrDefault(x => x.Name == newRole));
            await (user as IGuildUser).RemoveRoleAsync(context.Guild.Roles.FirstOrDefault(x => x.Name == oldRole));
        }

        private static async Task Rankup(SocketCommandContext context, SocketUser user, uint level)
        {
            StringBuilder description = new StringBuilder().Append($"{user.Mention} has leveled up to {level}.");
            if (level == 5)
            {
                description.Append($" {user.Mention} has been promoted and is now a Busboy! Congratulations!");
                await (user as IGuildUser).AddRoleAsync(context.Guild.Roles.FirstOrDefault(x => x.Name == "Busboy"));
            }
            else if (level == 7)
            {
                description.Append($" {user.Mention} has been  promoted and is now a waitress in the cheesecake factory!");
                await AddRole(context, user, "Busboy", "Waitress");
            }
            else if (level == 10)
            {
                description.Append($" {user.Mention} has quit their job as a waitress and is now a wannabe Actress! Well Done!");
                await AddRole(context, user, "Waitress", "Actress");
            }
            else if (level == 12)
            {
                description.Append($" {user.Mention} has become self-aware and know that they are officially a Geek! Congrats!");
                await AddRole(context, user, "Actress", "Geek");
            }
            else if (level == 14)
            {
                description.Append($" {user.Mention} has opened their own Comic Book Store and has leveled up!");
                await AddRole(context, user, "Geek", "Comic Book Store Owner");
            }
            else if (level == 16)
            {
                description.Append($" {user.Mention} has found the seedy unberbelly of science is now a Geologist.");
                await AddRole(context, user, "Comic Book Store Owner", "Geologist");
            }
            else if (level == 19)
            {
                description.Append($" {user.Mention} has now become a Sales Rep for a large company! Congratulations on your promotion");
                await AddRole(context, user, "Geologist", "Sales Rep");
            }
            else if (level == 21)
            {
                description.Append($" {user.Mention} is an oompa loompa of science and is now an Engineer.");
                await AddRole(context, user, "Sales Rep", "Engineer");
            }
            else if (level == 23)
            {
                description.Append($" {user.Mention} has channeled their inner Raj and is now an Astronomer!");
                await AddRole(context, user, "Engineer", "Astronomer");
            }
            else if (level == 24)
            {
                description.Append($" {user.Mention} has been talking to Penny's brother and is now a Chemist!");
                await AddRole(context, user, "Astronomer", "Chemist");
            }
            else if (level == 25)
            {
                description.Append($" {user.Mention} has took their Chemist career to another level and is now a Biochemist!");
                await AddRole(context, user, "Chemist", "Biochemist");
            }
            else if (level == 26)
            {
                description.Append($" {user.Mention} has channeled their inner Bernedette and is now a Micro-Biologist!");
                await AddRole(context, user, "Biochemist", "Micro-Biologist");
            }
            else if (level == 27)
            {
                description.Append($" {user.Mention} you have advanced and are now a Biologist!");
                await AddRole(context, user, "Micro-Biologist", "Biologist");
            }
            else if (level == 28)
            {
                description.Append($" {user.Mention} has just returned from the International Space Station and is now an Astronaut!");
                await AddRole(context, user, "Biologist", "Astronaut");
            }
            else if (level == 29)
            {
                description.Append($" {user.Mention} you have talked to Leonard and you now work beside him for a week. You have leveled up and are now an Experimental Physicist");
                await AddRole(context, user, "Astronaut", "Experimental Physicist");
            }
            else if (level == 33)
            {
                description.Append($" {user.Mention} You are a lot like Sheldon and are now a Theoretical Physicist!");
                await AddRole(context, user, "Experimental Physicist", "Theoretical Physicist");
            }
            else if (level == 40)
            {
                description.Append($" {user.Mention} You have achieved your life long dream and are now a Nobel Prize Winner!");
                await AddRole(context, user, "Theoretical Physicist", "Nobel Prize Winners");
            }
            await Utilities.SendEmbed(context.Channel, "Level Up", description.ToString(), Utilities.DomColorFromURL(user.GetAvatarUrl()), "", user.GetAvatarUrl());
        }

        // MEE6
        static uint[] xpLevel = { 0, 100, 255, 475, 770, 1150, 1625, 2205, 2900, 3720, 4675, 5775, 7030, 8450, 10045, 11825, 13800, 15900, 18375, 20995, 23850,
        26950, 30305, 33925, 37820, 42000, 46475, 51255, 56350, 61770, 67525, 73625, 80080, 86900, 94095, 101675, 109650, 118030, 126825, 136045, 145700};

        public static async Task CheckXP(SocketCommandContext context, SocketUser user)
        {
            UserAccount account = UserAccounts.GetAccount(user);
            for (uint i = 0; i < xpLevel.Length; i++)
                await CompareAndSet(context, user, account, i, xpLevel[i]);
        }

        public static async Task DisplayLevelAndXP(SocketCommandContext context, SocketUser user)
        {
            UserAccount account = UserAccounts.GetAccount(user);
            StringBuilder description = new StringBuilder()
                .AppendLine($"Rank: {LevelToRank(account.level)}").AppendLine()
                .AppendLine($"Level: {account.level}").AppendLine()
                .AppendLine($"Total XP: {account.xp.ToString("#,##0")}").AppendLine()
                .AppendLine($"XP until next level: {(account.xp - xpLevel[account.level]).ToString("#,##0")}/{(xpLevel[account.level + 1] - xpLevel[account.level]).ToString("#,##0")}").AppendLine();
            var rankColor = Colours.Blue;
            await Utilities.SendEmbed(context.Channel, $"{(user as SocketGuildUser).Nickname ?? user.Username}'s Level", description.ToString(), rankColor, "", user.GetAvatarUrl());
        }

        public static string LevelToRank(uint level)
        {

            if (level == 5 && level < 7)
                return "Busboy";
            else if (level >= 7 && level <= 9)
                return "Waitress";
            else if (level >= 10 && level <= 11)
                return "Actress";
            else if (level >= 12 && level <= 13)
                return "Geek";
            else if (level >= 14 && level <= 15)
                return "Comic Book Store Owner";
            else if (level >= 16 && level <= 18)
                return "Geologist";
            else if (level >= 19 && level <= 20)
                return "Sales Rep";
            else if (level >= 21 && level <= 22)
                return "Engineer";
            else if (level >= 23 && level < 24)
                return "Astronomer";
            else if (level >= 24 && level < 25)
                return "Chemist";
            else if (level >= 25 && level < 26)
                return "Biochemist";
            else if (level >= 26 && level < 27)
                return "Micro-Biologist";
            else if (level >= 27 && level < 28)
                return "Biologist";
            else if (level >= 28 && level < 29)
                return "Astronaut";
            else if (level >= 20 && level < 33)
                return "Experimental Physicist";
            else if (level >= 33 && level < 40)
                return "Theoretical Physicist";
            else if (level >= 40)
                return "Nobel Prize Winners";
            else return "User has no rank.";
        }

        public static async Task PrintComicBooksLeaderboard(SocketCommandContext context)
        {
            List<int> xpList = new List<int>();
            for (int i = 0; i < context.Guild.Users.Count; i++)
                xpList.Add(UserAccounts.GetAccount(context.Guild.Users.ElementAt(i)).xp);

            xpList.Sort();
            xpList.Reverse();

            StringBuilder description = new StringBuilder();
            List<SocketGuildUser> PeopleOnLB = new List<SocketGuildUser>();
            for (int xpListIndex = 0; xpListIndex < 10; xpListIndex++)
            {
                for (int userIndex = 0; userIndex < context.Guild.Users.Count; userIndex++)
                {
                    if (UserAccounts.GetAccount(context.Guild.Users.ElementAt(userIndex)).xp == xpList[xpListIndex] && !PeopleOnLB.Contains(context.Guild.Users.ElementAt(userIndex)))
                    {
                        string name = context.Guild.Users.ElementAt(userIndex).Nickname ?? context.Guild.Users.ElementAt(userIndex).Username;
                        description.AppendLine($"{xpListIndex + 1}. **{name}**, `{xpList[xpListIndex]} XP`");
                        PeopleOnLB.Add(context.Guild.Users.ElementAt(userIndex));
                        break;
                    }
                }
            }
            await Utilities.SendEmbed(context.Channel, "Top 10 Users With The Most XP", description.ToString(), Colours.Gold, "", "");
        }
    }
}