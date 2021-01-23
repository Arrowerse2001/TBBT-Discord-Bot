using System.IO;
using System.Reflection;
using System.Diagnostics;
using TBBTDiscordBot.Handlers;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TBBTDiscordBot
{
    static class Config
    {
        public static readonly BotConfig bot;
        public const ulong MiniGamesChannel = 618549807623045132;

        public static readonly List<ulong> MyBots = new List<ulong> {
           629768109829390352
        };

        static Config()
        {



            if (!Directory.Exists("Resources"))
                Directory.CreateDirectory("Resources");

            // If the file doesn't exist, WriteAllText with the json
            // If it exists, deserialize the json into the corresponding object

            // config.json
            if (!File.Exists("Resources/config.json"))
                File.WriteAllText("Resources/config.json", JsonConvert.SerializeObject(bot, Formatting.Indented));
            else
                bot = JsonConvert.DeserializeObject<BotConfig>(File.ReadAllText("Resources/config.json"));
        }

       
        public struct BotConfig
        {
            public string DisordBotToken;
        }
    }
}