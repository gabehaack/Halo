using HaloApp.Domain.Enums;
using System;
using System.Collections.Generic;

namespace HaloApp.Domain.Models.Metadata
{
    public class Map
    {
        public string Description { get; set; }
        public Guid Id { get; set; }
        public Uri ImageUrl { get; set; }
        public string Name { get; set; }
        public IEnumerable<GameMode> SupportedGameModes { get; set; }
    }
}
