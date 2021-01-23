using System;
using Discord;
using System.IO;
using System.Net;
using System.Drawing;
using ColorThiefDotNet;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace TBBTDiscordBot
{
    static class Utilities
    {
        // Universal Web Client
        public static readonly WebClient webClient = new WebClient();

        // Color Thief (gets the dominant color of an image, makes my embeds look pretty)
        private static ColorThief colorThief = new ColorThief();

        // Get a random number
        public static readonly Random getrandom = new Random();
        public static int GetRandomNumber(int min, int max)
        {
            lock (getrandom) { return getrandom.Next(min, max); }
        }

        // Generic Embed template
        public static Embed Embed(string t, string d, Discord.Color c, string f, string thURL) => new EmbedBuilder()
            .WithTitle(t)
            .WithDescription(d)
            .WithColor(c)
            .WithFooter(f)
            .WithThumbnailUrl(thURL)
            .Build();

        // Generic Image Embed template
        public static Embed ImageEmbed(string t, string d, Discord.Color c, string f, string imageURL) => new EmbedBuilder()
            .WithTitle(t)
            .WithDescription(d)
            .WithColor(c)
            .WithFooter(f)
            .WithImageUrl(imageURL)
            .Build();

        // Print a success message
        public static async Task PrintSuccess(this ISocketMessageChannel channel, string message) => await channel.PrintEmbed("Success", message, Colours.Green).ConfigureAwait(false);

        // Print an error
        public static async Task PrintError(this ISocketMessageChannel channel, string message) => await channel.PrintEmbed("Error", message, Colours.Red).ConfigureAwait(false);

        // Get a dominant color from an image (url)
        public static Discord.Color DomColorFromURL(string url)
        {
            byte[] bytes = webClient.DownloadData(url);
            using (webClient)
            using (MemoryStream ms = new MemoryStream(bytes))
            using (Bitmap bitmap = new Bitmap(System.Drawing.Image.FromStream(ms)))
            {
                // Remove the '#' from the string and get the hexadecimal
                return HexToRGB(colorThief.GetColor(bitmap).Color.ToString().Substring(1));
            }
        }

        // Convert a hexidecimal to an RGB value (input does not include the '#')
        public static Discord.Color HexToRGB(string hex)
        {
            // First two values of the hex
            int r = int.Parse(hex.Substring(0, hex.Length - 4), System.Globalization.NumberStyles.AllowHexSpecifier);

            // Get the middle two values of the hex
            int g = int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);

            // Final two values
            int b = int.Parse(hex.Substring(4), System.Globalization.NumberStyles.AllowHexSpecifier);

            return new Discord.Color(r, g, b);
        }


        public static async Task PrintEmbed(this ISocketMessageChannel channel, string title, string message, Discord.Color color) => await channel.SendMessageAsync("", false, new EmbedBuilder()
                .WithTitle(title)
                .WithColor(color)
                .WithDescription(message)
                .Build());

        /// <summary>
        /// Format a time into h:mm tt
        /// </summary>
        /// <param name="offset">The amount of hours to add to the current time.</param>
        /// <returns></returns>
        public static string GetTime(double offset) => DateTime.Now.AddHours(offset).ToString("h:mm tt");

        // Checks if the current channel is the required channel (like minigames)
        public static async Task<bool> CheckForChannel(SocketCommandContext context, ulong requiredChannel, SocketUser user)
        {
            if (context.Channel.Id == requiredChannel)
                return true;
            await PrintError(context.Channel, $"Please use the {context.Guild.GetTextChannel(requiredChannel).Mention} chat for that, {user.Mention}.").ConfigureAwait(false);
            return false;
        }

        // Send an embed to a channel
        public static async Task SendEmbed(ISocketMessageChannel channel, string title, string description, Discord.Color color, string footer, string thumbnailURL)
        {
            await channel.SendMessageAsync(null, false, Embed(title, description, color, footer, thumbnailURL)).ConfigureAwait(false);
        }

        // Send an embed to a channel
        public static async Task SendDomColorEmbed(ISocketMessageChannel channel, string title, string description, string imageURL, string footer = null)
        {
            await channel.SendMessageAsync(null, false, Embed(title, description, DomColorFromURL(imageURL), footer, imageURL)).ConfigureAwait(false);
        }
    }
}