using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using TBBTDiscordBot.Handlers;
using TBBTDiscordBot.Preconditions;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net.Http;
using TBBTDiscordBot.Handlers.DataHandling;


namespace TBBTDiscordBot.Commands
{
    [RequireContext(ContextType.Guild)]
    public class MinigameCommands : InteractiveBase
    {
        // RPS
        [Command("rps")]
        [Summary("Rock Paper Scissors")]
        public async Task StartRPS() => await MinigameHandler.RPS.StartRPS(Context);

        // CAH
        [Command("cah")]
        public async Task PlayCah()
        {
            await MinigameHandler.CAH.TryToStartGame(ArrayHandler.QuestionCards[Utilities.GetRandomNumber(0, ArrayHandler.QuestionCards.Length)], (SocketGuildUser)Context.User, Context, 10).ConfigureAwait(false);
        }

        [Command("join cah")]
        [Alias("cah join")]
        public async Task JoinNG() => await MinigameHandler.CAH.JoinGame((SocketGuildUser)Context.User, Context);

        [Command("cah start")]
        public async Task StartCah() => await MinigameHandler.CAH.StartGame(Context);
    }
}
