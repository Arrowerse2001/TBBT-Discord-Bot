using System;
using Discord;
using System.Linq;
using Discord.Commands;
using System.Reflection;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Timers;
using System.Runtime.Remoting.Contexts;
using System.Text.RegularExpressions;
using Discord.Rest;
using System.Collections.Generic;



namespace TBBTDiscordBot.Handlers
{
    class EventHandler
    {
        DiscordSocketClient _client;
        CommandService _service;
        readonly IServiceProvider serviceProdiver;

        public EventHandler(IServiceProvider services) => serviceProdiver = services;
        public static IDictionary<string, DateTimeOffset> timeList = new Dictionary<string, DateTimeOffset>();

        public async Task InitializeAsync(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();

            await _service.AddModulesAsync(Assembly.GetEntryAssembly(), serviceProdiver);

            _client.MessageReceived += HandleCommandAsync;
            _client.ReactionAdded += HandleReactionAsync;


            _service.Log += Log;

        }
        public async Task Help(SocketUserMessage msg)
        {
            var context = new SocketCommandContext(_client, msg);
            List<CommandInfo> commands = _service.Commands.ToList();
            EmbedBuilder embedBuilder = new EmbedBuilder();
            embedBuilder.WithColor(Colours.Blue);
            foreach (CommandInfo command in commands)
            {
                // Get the command Summary attribute information
                string embedFieldText = command.Summary ?? "No description available\n";

                embedBuilder.AddField(command.Name, embedFieldText);
            }

            await context.Channel.SendMessageAsync("Here's a list of commands and their description: ", false, embedBuilder.Build());
        }
        
        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            SocketUserMessage msg = s as SocketUserMessage;
            if (msg == null) return;

            var context = new SocketCommandContext(_client, msg);
            int argPos = 0;
            if (msg.HasStringPrefix(".", ref argPos))
                await _service.ExecuteAsync(context, argPos, serviceProdiver, MultiMatchHandling.Exception);

            string m = msg.Content.ToLower();

            if (m.Contains("retard") || m.Contains("fag"))
                await msg.DeleteAsync();
          
          

            // mute someone if they say the N word.
            if (m.Contains("nigger") || m.Contains("nigga"))
            {
                await msg.DeleteAsync();
                var role = ((ITextChannel)context.Channel).Guild.GetRole(792423525503336478);
                var user = context.User;

                await (user as IGuildUser).AddRoleAsync(role); //add the Shunned role
               
                
                await context.Channel.SendMessageAsync($"{user.Mention} you have been muted. You will be unmuted when a mod comes online.");
                var client = context.Client;
                ulong channelID = 784578613269626960;
                var c = client.GetChannel(channelID) as SocketTextChannel;
                await c.SendMessageAsync($"{user.Mention} has been muted. They will need to be manually unmuted.");
            }
          

            if (Regex.IsMatch(m, "[Hh]ow.*suggest.*", RegexOptions.IgnoreCase))
            {
                DateTimeOffset dt;
                if (timeList.ContainsKey("Suggest"))
                {
                    dt = timeList["Suggest"];
                    if (DateTimeOffset.Now >= dt.AddSeconds(10))
                    {
                        timeList["Suggest"] = DateTimeOffset.Now;
                        await s.Channel.SendMessageAsync("Refer to the pins in <#784578922553540659> for how to make a suggestion.");
                    }
                }
                else
                {
                    timeList["Suggest"] = DateTimeOffset.Now;
                    await s.Channel.SendMessageAsync("Refer to the pins in <#784578922553540659> for how to make a suggestion.");
                }
            }

            if (Regex.IsMatch(m, "[Bb]azinga*", RegexOptions.IgnoreCase))
            {
                DateTimeOffset dt;
                if (timeList.ContainsKey("Bazinga"))
                {
                    dt = timeList["Bazinga"];
                    if (DateTimeOffset.Now >= dt.AddSeconds(10))
                    {
                        timeList["Bazinga"] = DateTimeOffset.Now;
                        await s.Channel.SendMessageAsync(ArrayHandler.Bazinga[Utilities.GetRandomNumber(0, ArrayHandler.Bazinga.Length)]);
                    }
                }
                else
                {
                    timeList["Bazinga"] = DateTimeOffset.Now;
                    await s.Channel.SendMessageAsync(ArrayHandler.Bazinga[Utilities.GetRandomNumber(0, ArrayHandler.Bazinga.Length)]);
                }
            }

            Emote BazingaEmote = Emote.Parse("<:Bazinga:793545893230936064>");

            foreach (var message in ArrayHandler.BazingaEmote)
            {
                if (m.Contains(message))
                {
                    await context.Message.AddReactionAsync(BazingaEmote);
                }
            }

        }

        private async Task HandleReactionAsync(Cacheable<IUserMessage, ulong> cachedMessage,
                        ISocketMessageChannel originChannel, SocketReaction reaction)
        {
            var message = await cachedMessage.GetOrDownloadAsync();

           
          
        }

    }
}
