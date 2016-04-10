using System;

namespace HaloApp.ApiClient.Models.Metadata
{
    public class Playlist
    {
        public string name { get; set; }
        public string description { get; set; }
        public bool isRanked { get; set; }
        public string imageUrl { get; set; }
        public string gameMode { get; set; }
        public bool isActive { get; set; }
        public Guid id { get; set; }
    }
}
