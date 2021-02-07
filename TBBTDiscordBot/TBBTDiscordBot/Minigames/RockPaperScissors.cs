using Discord;
using System.Linq;
using Discord.Rest;
using Discord.Commands;
using Discord.WebSocket;
using TBBTDiscordBot.Handlers;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TBBTDiscordBot.Minigames
{
    class RockPaperScissors
    {
        private RestUserMessage gameMessage;
        public ulong MessageID;
        private bool isPlaying;
        public SocketGuildUser Player;
        private readonly List<string> Plays = new List<string>(new[] { "Rock", "Paper", "Scissors", "Lizard", "Spock" });

        private Embed Embed(string description, string footer) => Utilities.Embed("Rock-Paper-Scissors-Lizard-Spock", description, Colours.Blue, footer, "https://cdn.discordapp.com/attachments/784578667333419008/807795384759222322/LIZARDSPOCK.png");

        public async Task StartRPS(SocketCommandContext context)
        {
            string name = ((SocketGuildUser)context.User).Nickname ?? context.User.Username;
            gameMessage = await context.Channel.SendMessageAsync("", false, Embed("Pick your play and see if you can beat me!", $"{name} is playing."));
            await gameMessage.AddReactionAsync(new Emoji("📰"));
            await Task.Delay(1000);
            await gameMessage.AddReactionAsync(new Emoji("✂"));
            await Task.Delay(1000);
            await gameMessage.AddReactionAsync(new Emoji("🌑"));
            await Task.Delay(1000);
            await gameMessage.AddReactionAsync(new Emoji("🦎"));
            await Task.Delay(1000);
            await gameMessage.AddReactionAsync(new Emoji("🖖"));
            isPlaying = true;
            MessageID = gameMessage.Id;
            Player = (SocketGuildUser)context.User;
        }

        public async Task ViewPlay(string emote)
        {
            if (!isPlaying) return;

            string playOne = "";
            if (emote == "📰")
                playOne = "Paper";
            else if (emote == "✂")
                playOne = "Scissors";
            else if (emote == "🌑")
                playOne = "Rock";
            else if (emote == "🦎")
                playOne = "Lizard";
            else if (emote == "🖖")
                playOne = "ZSpock";

            string playTwo = Plays.ElementAt(Utilities.GetRandomNumber(0, 4));
            string result = GetWinner(playOne[0], playTwo[0]);

            // Update the game message to show the winner and remove the reactions
            await gameMessage.ModifyAsync(m => { m.Embed = Embed($"{Player.Mention} chose {playOne.Replace("Z", "")}\n\nI chose {playTwo}.\n\n{result}", ""); });
            await gameMessage.RemoveAllReactionsAsync();

            if (result.Contains("lose"))
                ComicBooksHandler.AdjustComicBooks(Player, -3);
            else if (result.Contains("got"))
                ComicBooksHandler.AdjustComicBooks(Player, 3);

            Player = null;
            isPlaying = false;
        }

        private string GetWinner(char p1, char p2)
        {
            if (p1 == p2) return "It's a draw!";
            if ((p1 == 'S' && p2 == 'P') ||
                (p1 == 'P' && p2 == 'R') ||
                (p1 == 'R' && p2 == 'S') ||
                (p1 == 'R' && p2 == 'L') ||
                (p1 == 'L' && p2 == 'Z') ||
                (p1 == 'Z' && p2 == 'S') ||
                (p1 == 'S' && p2 == 'L') ||
                (p1 == 'L' && p2 == 'P') ||
                (p1 == 'P' && p2 == 'Z') ||
                (p1 == 'Z' && p2 == 'R'))
                return $"{Player.Mention} won and got 3 Comic Books!";
            return "I won! You lose 3 Comic Books.";
        }
    }
}