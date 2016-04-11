using System;
using System.Collections.Generic;

namespace HaloApp.Domain.Models
{
    public class MatchPlayer
    {
        public TimeSpan AverageLifeTime { get; set; }
        public string Name { get; set; }
        public int Team { get; set; }
        public int Rank { get; set; }
        public int TotalKills { get; set; }
        public int TotalDeaths { get; set; }
        public int TotalAssists { get; set; }
        public Csr PreviousCsr { get; set; }
        public Csr CurrentCsr { get; set; }
        public bool Dnf { get; set; }
        public int TotalHeadshots { get; set; }
        public double TotalWeaponDamage { get; set; }
        public int TotalShotsFired { get; set; }
        public int TotalShotsLanded { get; set; }
        public int TotalMeleeKills { get; set; }
        public double TotalMeleeDamage { get; set; }
        public int TotalAssassinations { get; set; }
        public int TotalGroundPoundKills { get; set; }
        public double TotalGroundPoundDamage { get; set; }
        public int TotalShoulderBashKills { get; set; }
        public double TotalShoulderBashDamage { get; set; }
        public double TotalGrenadeDamage { get; set; }
        public int TotalPowerWeaponKills { get; set; }
        public double TotalPowerWeaponDamage { get; set; }
        public int TotalPowerWeaponGrabs { get; set; }
        public int TotalGrenadeKills { get; set; }
        public IList<WeaponStats> WeaponsStats { get; set; }

        public double DamageDealt
        {
            get
            {
                return HaloDataManager.Round(
                    TotalGrenadeDamage +
                    TotalGroundPoundDamage +
                    TotalMeleeDamage +
                    TotalPowerWeaponDamage +
                    TotalShoulderBashDamage +
                    TotalWeaponDamage);
            }
        }
    }
}
