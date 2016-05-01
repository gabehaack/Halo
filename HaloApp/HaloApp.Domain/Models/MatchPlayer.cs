using System;
using System.Collections.Generic;

namespace HaloApp.Domain.Models
{
    public class MatchPlayer
    {
        public TimeSpan AvgLifeTime { get; set; }
        public string Name { get; set; }
        public int Team { get; set; }
        public int Rank { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Assists { get; set; }
        public Csr PreviousCsr { get; set; }
        public Csr CurrentCsr { get; set; }
        public bool Dnf { get; set; }
        public int Headshots { get; set; }
        public double WeaponDamage { get; set; }
        public int ShotsFired { get; set; }
        public int ShotsLanded { get; set; }
        public int MeleeKills { get; set; }
        public double MeleeDamage { get; set; }
        public int Assassinations { get; set; }
        public int GroundPoundKills { get; set; }
        public double GroundPoundDamage { get; set; }
        public int ShoulderBashKills { get; set; }
        public double ShoulderBashDamage { get; set; }
        public double GrenadeDamage { get; set; }
        public int PowerWeaponKills { get; set; }
        public double PowerWeaponDamage { get; set; }
        public int PowerWeaponGrabs { get; set; }
        public TimeSpan PowerWeaponPossessionTime { get; set; }
        public int GrenadeKills { get; set; }
        public IList<WeaponStats> WeaponsStats { get; set; }

        public double DamageDealt
        {
            get
            {
                return HaloDataManager.Round(
                    GrenadeDamage +
                    GroundPoundDamage +
                    MeleeDamage +
                    PowerWeaponDamage +
                    ShoulderBashDamage +
                    WeaponDamage);
            }
        }
    }
}
