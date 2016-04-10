using System;

namespace HaloApp.Domain.Models.Metadata
{
    public class MapVariant
    {
        public string Description { get; set; }
        public Guid Id { get; set; }
        public Guid MapId { get; set; }
        public Uri MapImageUrl { get; set; }
        public string Name { get; set; }
    }
}
