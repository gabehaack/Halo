namespace HaloApp.ApiClient.Models.Metadata
{
    public class Medal
    {
        public string name { get; set; }
        public string description { get; set; }
        public string classification { get; set; }
        public int difficulty { get; set; }
        public MedalSpriteLocation spriteLocation { get; set; }
        public int id { get; set; }

    }

    public class MedalSpriteLocation
    {
        public string spriteSheetUri { get; set; }
        public int left { get; set; }
        public int top { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int spriteWidth { get; set; }
        public int spriteHeight { get; set; }
    }
}
