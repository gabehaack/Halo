using System;

namespace HaloApp.ApiClient.Models.Metadata
{
    public class MapVariant
    {
        public string name { get; set; }
        public string description { get; set; }
        public string mapImageUrl { get; set; }
        public Guid mapId { get; set; }
        public Guid id { get; set; }
    }
}
