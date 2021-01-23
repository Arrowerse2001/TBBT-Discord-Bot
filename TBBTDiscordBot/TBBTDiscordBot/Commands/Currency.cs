using Discord.Addons.Interactive;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBBTDiscordBot.Handlers;

namespace TBBTDiscordBot.Commands
{
    [RequireContext(ContextType.Guild)]
    public class Currency : InteractiveBase
    {
        // https://www.amandaclegg.co.uk/wp-content/uploads/2019/11/Nerds.jpg

        [Command("comics")]
        [Alias("comic books", "books", "cb", "comics book", "comics books")]
        [Summary("Displays the amount of comics a user has.")]
        public async Task DisplayComics() => await ComicBooksHandler.DisplayComicBooks(Context, (Discord.WebSocket.SocketGuildUser)Context.User, Context.Channel);
    }
}
