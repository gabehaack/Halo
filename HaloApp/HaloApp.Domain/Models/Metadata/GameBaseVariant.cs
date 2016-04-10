using HaloApp.Domain.Enums;
using System;
using System.Collections.Generic;

namespace HaloApp.Domain.Models.Metadata
{
    public class GameBaseVariant
    {
        public Uri IconUrl { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<GameMode> SupportedGameModes { get; set; }
    }
}
