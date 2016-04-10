using HaloApp.Domain.Enums;
using System;

namespace HaloApp.Domain.Models.Metadata
{
    public class FlexibleStat
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public FlexibleStatType Type { get; set; }
    }
}
