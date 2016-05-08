using System;

namespace GHaack.Halo.Domain.Models.Dto
{
    public class WeaponStatsDto
    {
        public long WeaponId { get; set; }
        public int ShotsFired { get; set; }
        public int ShotsLanded { get; set; }
        public int Headshots { get; set; }
        public int Kills { get; set; }
        public double DamageDealt { get; set; }
        public TimeSpan PossessionTime { get; set; }
    }
}
