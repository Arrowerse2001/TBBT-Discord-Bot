using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using TBBTDiscordBot.Preconditions;
using System.Threading.Tasks;

namespace TBBTDiscordBot.Commands
{
    [RequireContext(ContextType.Guild)]
    [Group("rule")]
    public class ServerRules : InteractiveBase
    {
        [Command("1")]
        [Summary("Displays Rule 1")]
        public async Task DisplayRuleOne()
        {
            await Context.Message.DeleteAsync();
            EmbedBuilder builder = new EmbedBuilder();
            builder.Color = Colours.Blue;
            builder.WithTitle("The Big Bang Theory Discord Rule One");
            builder.WithDescription("1. Respect every user no matter what.");
            await ReplyAsync("", false, builder.Build());
        }

        [Command("2")]
        [Summary("Displays Rule 2")]
        public async Task DisplayRuleTwo()
        {
            await Context.Message.DeleteAsync();
            EmbedBuilder builder = new EmbedBuilder();
            builder.Color = Colours.Blue;
            builder.WithTitle("The Big Bang Theory Discord Rule 2");
            builder.WithDescription("2. No political talk, unless it's related to the show.");
            await ReplyAsync("", false, builder.Build());
        }

        [Command("3")]
        [Summary("Displays Rule 3")]
        public async Task DisplayRuleThree()
        {
            await Context.Message.DeleteAsync();
            EmbedBuilder builder = new EmbedBuilder();
            builder.Color = Colours.Blue;
            builder.WithTitle("The Big Bang Theory Discord Rule 3");
            builder.WithDescription("3. No random DM advertisements or random invite posting in any channel.");
            await ReplyAsync("", false, builder.Build());
        }

        [Command("4")]
        [Summary("Displays Rule 4")]
        public async Task DisplayRuleFour()
        {
            await Context.Message.DeleteAsync();
            EmbedBuilder builder = new EmbedBuilder();
            builder.Color = Colours.Blue;
            builder.WithTitle("The Big Bang Theory Discord Rule 4");
            builder.WithDescription("4. Use the designated channels please.");
            await ReplyAsync("", false, builder.Build());
        }

        [Command("5")]
        [Summary("Displays Rule 5")]
        public async Task DisplayRuleFive()
        {
            await Context.Message.DeleteAsync();
            EmbedBuilder builder = new EmbedBuilder();
            builder.Color = Colours.Blue;
            builder.WithTitle("The Big Bang Theory Discord Rule 5");
            builder.WithDescription("5. No Roleplaying.");
            await ReplyAsync("", false, builder.Build());
        }

        [Command("6")]
        [Summary("Displays Rule 6")]
        public async Task DisplayRuleSix()
        {
            await Context.Message.DeleteAsync();
            EmbedBuilder builder = new EmbedBuilder();
            builder.Color = Colours.Blue;
            builder.WithTitle("The Big Bang Theory Discord Rule 6");
            builder.WithDescription("6. No promotion of piracy.");
            await ReplyAsync("", false, builder.Build());
        }

        [Command("7")]
        [Summary("Displays Rule 7")]
        public async Task DisplayRuleSeven()
        {
            await Context.Message.DeleteAsync();
            EmbedBuilder builder = new EmbedBuilder();
            builder.Color = Colours.Blue;
            builder.WithTitle("The Big Bang Theory Discord Rule 7");
            builder.WithDescription("7. No farming XP it will have a cooldown so there is no point in spamming");
            await ReplyAsync("", false, builder.Build());
        }

        [Command("8")]
        [Summary("Displays Rule 8")]
        public async Task DisplayRuleEight()
        {
            await Context.Message.DeleteAsync();
            EmbedBuilder builder = new EmbedBuilder();
            builder.Color = Colours.Blue;
            builder.WithTitle("The Big Bang Theory Discord Rule 8");
            builder.WithDescription("8. Follow the Terms of Service from Discord");
            await ReplyAsync("", false, builder.Build());
        }

        [Command("9")]
        [Summary("Displays Rule 9")]
        public async Task DisplayRuleNine()
        {
            await Context.Message.DeleteAsync();
            EmbedBuilder builder = new EmbedBuilder();
            builder.Color = Colours.Blue;
            builder.WithTitle("The Big Bang Theory Discord Rule 9");
            builder.WithDescription("9. This is a big rule: Do not lie about your age! If we find out you are under 13 of age you will be banned");
            await ReplyAsync("", false, builder.Build());
        }


        [Command("10")]
        [Summary("Displays Rule 10")]
        public async Task DisplayRuleTen()
        {
            await Context.Message.DeleteAsync();
            EmbedBuilder builder = new EmbedBuilder();
            builder.Color = Colours.Blue;
            builder.WithTitle("The Big Bang Theory Discord Rule 10");
            builder.WithDescription("10. Have Fun and Enjoy the server. If you have any questions just ping staff (Admins/Mods)");
            await ReplyAsync("", false, builder.Build());
        }
    }
}