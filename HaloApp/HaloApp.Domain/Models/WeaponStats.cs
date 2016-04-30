using HaloApp.Domain.Models.Metadata;
using System;

namespace HaloApp.Domain.Models
{
    public class WeaponStats
    {
        public Weapon Weapon { get; set; }
        public int ShotsFired { get; set; }
        public int ShotsLanded { get; set; }
        public int Headshots { get; set; }
        public int Kills { get; set; }
        public double DamageDealt { get; set; }
        public TimeSpan PossessionTime { get; set; }

        public double Accuracy
        {
            get { return HaloDataManager.RoundPercentage(ShotsLanded / ShotsFired); }
        }

        public double HeadshotPercentage
        {
            get { return HaloDataManager.RoundPercentage(Headshots / ShotsFired); }
        }

        public double DamagePerKill
        {
            get { return HaloDataManager.Round(DamageDealt / Kills); }
        }

        public double ShotsFiredPerKill
        {
            get { return HaloDataManager.Round(ShotsFired / Kills); }
        }

        public double ShotsLandedPerKill
        {
            get { return HaloDataManager.Round(ShotsFired / Kills); }
        }

        public double KillsPerMinute
        {
            get { return HaloDataManager.Round(Kills / PossessionTime.TotalMinutes); }
        }

        public double DamagePerMinute
        {
            get { return HaloDataManager.Round(DamageDealt / PossessionTime.TotalMinutes); }
        }
    }
}
