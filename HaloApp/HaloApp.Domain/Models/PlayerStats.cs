using System;
using System.Collections.Generic;

namespace HaloApp.Domain.Models
{
    public class PlayerStats
    {
        public string Name { get; set; }
        public int GamesPlayed { get; set; }
        public TimeSpan TimePlayed { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Assists { get; set; }
        public double DamageDealt { get; set; }
        public int ShotsLanded { get; set; }
        public int ShotsFired { get; set; }
        public IList<WeaponStats> WeaponsStats { get; set; }

        public double Accuracy
        {
            get { return HaloDataManager.RoundPercentage((double) ShotsLanded / ShotsFired); }
        }

        public double Kda
        {
            get { return HaloDataManager.Round(Kills + (double) Assists / 3 - Deaths); }
        }

        public double KillsPerDeath
        {
            get { return HaloDataManager.Round((double) Kills / Deaths); }
        }

        public double KillsPerMinute
        {
            get { return HaloDataManager.Round(Kills / TimePlayed.TotalMinutes); }
        }

        public double DamagePerDeath
        {
            get { return HaloDataManager.Round(DamageDealt / Deaths); }
        }

        public double DamagePerMinute
        {
            get { return HaloDataManager.Round(DamageDealt / TimePlayed.TotalMinutes); }
        }

        public double DamagePerKill
        {
            get { return HaloDataManager.Round(DamageDealt / Kills); }
        }

        public double ShotsFiredPerKill
        {
            get { return HaloDataManager.Round((double) ShotsFired / Kills); }
        }

        public double ShotsLandedPerKill
        {
            get { return HaloDataManager.Round((double) ShotsLanded / Kills); }
        }
    }
}
