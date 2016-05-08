using GHaack.Halo.Domain.Enums;
using System;

namespace GHaack.Halo.Domain.Models.Metadata
{
    public class FlexibleStat
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public FlexibleStatType Type { get; set; }
    }
}
