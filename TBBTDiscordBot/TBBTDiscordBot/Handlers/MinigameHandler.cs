using System;
using System.IO;
using Newtonsoft.Json;
using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;
using TBBTDiscordBot.Minigames;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TBBTDiscordBot.Handlers
{
    class MinigameHandler
    {

        public static RockPaperScissors RPS = new RockPaperScissors();
        public static CardsAgainstHumanity CAH = new CardsAgainstHumanity();


        // Display available minigames
        public static async Task DisplayGames(SocketCommandContext context)
        {
            await Utilities.SendEmbed(context.Channel, "MiniGames", "rps`", Colours.Green, "", "");
        }



 

     

        // Reset a game
        public static async Task ResetGame(SocketCommandContext context, string game)
        {
           if (game == "rps")
            {
                await Utilities.SendEmbed(context.Channel, "Minigames", $"{context.User.Mention} has reset RPS.", Colours.Blue, "", "");


            }
           
            else if (game == "")
                await Utilities.PrintError(context.Channel, "Please specify a game to reset.");
            else
                await Utilities.PrintError(context.Channel, $"I was unable to find the `{game}` game.\n\nAvailable games to reset:\nRPS`");
        }
    }
}