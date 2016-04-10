using System;
using System.Collections.Generic;

namespace HaloApp.ApiClient.Models.Metadata
{
    public class Map
    {
        public string name { get; set; }
        public string description { get; set; }
        public List<string> supportedGameModes { get; set; }
        public string imageUrl { get; set; }
        public Guid id { get; set; }
    }
}
