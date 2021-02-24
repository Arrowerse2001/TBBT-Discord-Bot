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

namespace TBBTDiscordBot.Minigames
{
    class Player : IEquatable<Player>
    {
        public SocketGuildUser User { get; set; }
        public bool HasAnswered { get; set; }
        public int Czae { get; set; }

        public bool Equals(Player other) => User.Id == other.User.Id;

        public override bool Equals(object obj) => Equals(obj as Player);

        public override int GetHashCode() => 0;
    }

    class CardsAgainstHumanity
    {
        private string cardPlayed;
        private int playerSlots = 10;
        private readonly List<Player> Players = new List<Player>();

        public bool isGamingGoing;
        private readonly List<string> Emojis = new List<string>(new[] { "🇦", "🇧", "🇨", "🇩", "🇪", "🇫", "🇬", "🇭" });


        private Embed Embed(string description, string footer, bool showPlayers)
        {
            var embed = new EmbedBuilder()
                .WithTitle("Office CAH")
                .WithDescription(description)
                .WithColor(Colours.Blue)
                .WithFooter(footer);
            if (showPlayers)
            {
                StringBuilder PlayerDesc = new StringBuilder();
                for (int i = 0; i < Players.Count; i++)
                    PlayerDesc.AppendLine($"Player {i + 1}: {(Players.ElementAt(i).User.Nickname ?? Players.ElementAt(i).User.Username)}");
                embed.AddField("Players", PlayerDesc);
            }
            return embed.Build();
        }

        private void AddPlayer(SocketGuildUser user) => Players.Add(new Player { HasAnswered = false, User = user });

        public async Task TryToStartGame(string randomcard, SocketGuildUser user, SocketCommandContext context, int players)
        {
            if (isGamingGoing) return;
            isGamingGoing = true;
            AddPlayer(user);
            playerSlots = players;
            await context.Channel.SendMessageAsync("", false, Embed($"{context.User.Mention} has started a game of CAH!\n\nType `.join cah` to join!", "", true));
        }

        public async Task JoinGame(SocketGuildUser user, SocketCommandContext context)
        {
            if (!isGamingGoing)
            {
                await context.Channel.SendMessageAsync("", false, Embed("There is no game currently going.", "", false)).ConfigureAwait(false);
                return;
            }
            if (playerSlots == Players.Count)
                return;
            foreach (Player p in Players)
                if (p.User == user)
                    return;
            AddPlayer(user);
            if (playerSlots == Players.Count)
                await StartGame(context).ConfigureAwait(false);
            else if (Players.Count <= 2)
                await context.Channel.SendMessageAsync("", false, Embed($"{playerSlots - (Players.Count)} more player(s) needed!", "", true));
        }

        public async Task StartGame(SocketCommandContext context)
        {
           var description = ArrayHandler.QuestionCards[Utilities.GetRandomNumber(0, ArrayHandler.QuestionCards.Length)];
            var czar = Players[Utilities.GetRandomNumber(0, Players.Count)];
            var embed = new EmbedBuilder()
                .WithTitle("Office CAH")
                .WithDescription($"Prompt: **{description}**\nCzar: {czar.User.Mention}")
                .WithColor(Colours.Blue)
                .WithFooter("");
            await context.Channel.SendMessageAsync("", false, embed.Build());

            foreach (var p in Players)
            {
                var card1 = ArrayHandler.PlayerCards[Utilities.GetRandomNumber(0, ArrayHandler.PlayerCards.Length)];
                var card2 = ArrayHandler.PlayerCards[Utilities.GetRandomNumber(0, ArrayHandler.PlayerCards.Length)];
                var card3 = ArrayHandler.PlayerCards[Utilities.GetRandomNumber(0, ArrayHandler.PlayerCards.Length)];
                var card4 = ArrayHandler.PlayerCards[Utilities.GetRandomNumber(0, ArrayHandler.PlayerCards.Length)];
                var card5 = ArrayHandler.PlayerCards[Utilities.GetRandomNumber(0, ArrayHandler.PlayerCards.Length)]; // I AM SORRY
                var card6 = ArrayHandler.PlayerCards[Utilities.GetRandomNumber(0, ArrayHandler.PlayerCards.Length)]; // THIS IS HORRIFIC
                var card7 = ArrayHandler.PlayerCards[Utilities.GetRandomNumber(0, ArrayHandler.PlayerCards.Length)];
                var card8 = ArrayHandler.PlayerCards[Utilities.GetRandomNumber(0, ArrayHandler.PlayerCards.Length)];

                while (card1 == card2 || card1 == card3 || card1 == card4 || card1 == card5 || card1 == card6 || card1 == card6 || card1 == card7)
                {
                    card1 = ArrayHandler.PlayerCards[Utilities.GetRandomNumber(0, ArrayHandler.PlayerCards.Length)];
                }

                EmbedBuilder builder = new EmbedBuilder();
                builder.WithTitle("Your Cards")
                    .WithDescription($":regional_indicator_a:: {card1}\n:regional_indicator_b:: {card2}\n:regional_indicator_c:: {card3}\n:regional_indicator_d:: {card4}\n:regional_indicator_e:: {card5}\n:regional_indicator_f:: {card6}\n:regional_indicator_g:: {card7}\n:regional_indicator_h:: {card8}")
                    .WithColor(Colours.Blue)
                    .WithFooter("React with the coresponding letter to use that card!");
                builder.Build();
                var m =  await p.User.SendMessageAsync("", false, builder.Build());
                foreach (var emote in Emojis)
                {
                    var e = new Emoji(emote);
                    await m.AddReactionAsync(e);
                }    
                   
            }
        }

        public void Reset()
        {
            isGamingGoing = false;
            Players.Clear();
        }

        public async Task HandleReactionAsync(Cacheable<IUserMessage, ulong> cachedMessage,
                      ISocketMessageChannel originChannel, SocketReaction reaction, SocketCommandContext context, SocketMessage m)
        {
            var message = await cachedMessage.GetOrDownloadAsync();

            IEmote e = reaction.Emote;

            if (m.Channel.Name.StartsWith("@") && e.Name == "🇦")
            {
               
            }
        }
    }
}
