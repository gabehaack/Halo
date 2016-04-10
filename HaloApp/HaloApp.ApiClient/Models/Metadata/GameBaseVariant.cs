using System;
using System.Collections.Generic;

namespace HaloApp.ApiClient.Models.Metadata
{
    public class GameBaseVariant
    {
        public string name { get; set; }
        public string iconUrl { get; set; }
        public List<string> supportedGameModes { get; set; }
        public string id { get; set; }
    }
}
