namespace TBBTDiscordBot
{
    public class UserAccount
    {
        internal double localTime;

        public ulong UserID { get; set; }
        public int ComicBooks { get; set; }
        public int Gwins { get; set; }
        public int Glost { get; set; }
        public int ComicsWonFromG { get; set; }
        public int ComicsLostFromG { get; set; }

        public int xp { get; set; }
        public uint level { get; set; }
    }
}