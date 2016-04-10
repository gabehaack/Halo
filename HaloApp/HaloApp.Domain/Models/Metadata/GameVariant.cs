using System;

namespace HaloApp.Domain.Models.Metadata
{
    public class GameVariant
    {
        public string Description { get; set; }
        public Guid GameBaseVariantId { get; set; }
        public Uri IconUrl { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
