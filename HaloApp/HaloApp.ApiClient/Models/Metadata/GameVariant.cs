using System;

namespace HaloApp.ApiClient.Models.Metadata
{
    public class GameVariant
    {
        public string name { get; set; }
        public string description { get; set; }
        public Guid gameBaseVariantId { get; set; }
        public string iconUrl { get; set; }
        public Guid id { get; set; }
    }
}
