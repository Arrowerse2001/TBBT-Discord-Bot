using System;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using TBBTDiscordBot.Handlers;
using Discord.Addons.Interactive;
using Microsoft.Extensions.DependencyInjection;

namespace TBBTDiscordBot
{
    class Program
    {

        static void Main(string[] args) => new Program().StartAsync().GetAwaiter().GetResult();
        public static DiscordSocketClient _client;
        

        private IServiceProvider services;
        public async Task StartAsync()
        {
            if (Config.bot.DisordBotToken == "" || Config.bot.DisordBotToken == null) return;
            _client = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = LogSeverity.Verbose, AlwaysDownloadUsers = true });
            _client.Log += Log;

            await _client.LoginAsync(TokenType.Bot, Config.bot.DisordBotToken);
            await _client.StartAsync();

            services = new ServiceCollection()
               .AddSingleton(_client)
               .AddSingleton<InteractiveService>()
               .BuildServiceProvider();


            await _client.SetGameAsync(ArrayHandler.Activites[Utilities.GetRandomNumber(0, ArrayHandler.Activites.Length)], null, ActivityType.Playing);
          


            var _handler = new Handlers.EventHandler(services);
            await _handler.InitializeAsync(_client);
            await Task.Delay(-1);
        }

        private static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
            return Task.CompletedTask;
        }
    }
}