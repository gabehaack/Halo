using GHaack.Halo.Domain.Enums;
using System;

namespace GHaack.Halo.Domain.Models.Metadata
{
    public class Weapon
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public WeaponType Type { get; set; }
        public Uri LargeIconImageUrl { get; set; }
        public Uri SmallIconImageUrl { get; set; }
        public bool UsableByPlayer { get; set; }
        public long Id { get; set; }
    }
}
