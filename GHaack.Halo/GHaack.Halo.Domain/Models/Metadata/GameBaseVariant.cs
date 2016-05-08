using GHaack.Halo.Domain.Enums;
using System;
using System.Collections.Generic;

namespace GHaack.Halo.Domain.Models.Metadata
{
    public class GameBaseVariant
    {
        public Uri IconUrl { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<GameMode> SupportedGameModes { get; set; }
    }
}
