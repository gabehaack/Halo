using System;

namespace HaloApp.Domain.Models.Metadata
{
    public class Vehicle
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Uri LargeIconImageUrl { get; set; }
        public Uri SmallIconImageUrl { get; set; }
        public bool UsableByPlayer { get; set; }
        public int Id { get; set; }
    }
}
