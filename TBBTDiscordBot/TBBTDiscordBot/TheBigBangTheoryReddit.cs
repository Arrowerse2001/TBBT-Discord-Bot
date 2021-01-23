using Discord;
using System.Net;
using Newtonsoft.Json;
using Discord.WebSocket;
using System.Threading.Tasks;


namespace TBBTDiscordBot
{
    static class TheBigBangTheoryReddit
    {
        // Print a random Shower Thought from Reddit
        public static async Task PrintRandomThought(ISocketMessageChannel Channel)
        {
            dynamic stuff = null;
            using (WebClient client = new WebClient())
                stuff = JsonConvert.DeserializeObject(client.DownloadString("https://www.reddit.com/r/bigbangtheory/top.json?sort=top&t=week&limit=100"));

            stuff = stuff.data.children[Utilities.GetRandomNumber(0, 1)].data;

            await Channel.SendMessageAsync(null, false, new EmbedBuilder()
                .WithAuthor(new EmbedAuthorBuilder()
                    .WithIconUrl("https://styles.redditmedia.com/t5_2s85x/styles/communityIcon_8w5ipx6edet51.jpg")
                    .WithName("r/BigBangTheory")
                    .WithUrl($"https://www.reddit.com{stuff.permalink}"))
                .WithColor(Colours.Blue)
                .WithDescription(stuff.title.ToString())
                .Build());
        }
    }
}
