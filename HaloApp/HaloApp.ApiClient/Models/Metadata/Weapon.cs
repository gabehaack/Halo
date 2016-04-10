namespace HaloApp.ApiClient.Models.Metadata
{
    public class Weapon
    {
        public string name { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public string largeIconImageUrl { get; set; }
        public string smallIconImageUrl { get; set; }
        public bool isUsableByPlayer { get; set; }
        public long id { get; set; }
    }
}
