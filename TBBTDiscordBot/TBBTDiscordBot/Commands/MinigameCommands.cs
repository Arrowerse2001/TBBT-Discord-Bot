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

namespace DunderMifflinPrizeWheel.Commands
{
    [RequireContext(ContextType.Guild)]
    public class MinigameCommands : InteractiveBase
    {
        // RPS
        [Command("rps")]
        [Summary("Rock Paper Scissors")]
        public async Task StartRPS() => await MinigameHandler.RPS.StartRPS(Context);
    }
}
