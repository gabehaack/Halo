using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using GHaack.Halo.Domain.Models;
using Xunit;

namespace GHaack.Halo.Tests
{
    [ExcludeFromCodeCoverage]
    public class HaloDomainModelTests
    {
        [Fact]
        public void PlayerStatsProperties()
        {
            var playerStats = PlayerStatsData()[0];
            Assert.Equal(0.5, playerStats.Accuracy);
            Assert.Equal(10.3, playerStats.Kda);
            Assert.Equal(1.8, playerStats.KillsPerDeath);
            Assert.Equal(3, playerStats.KillsPerMinute);
            Assert.Equal(180, playerStats.DamagePerDeath);
            Assert.Equal(300, playerStats.DamagePerMinute);
            Assert.Equal(100, playerStats.DamagePerKill);
            Assert.Equal(10, playerStats.ShotsFiredPerKill);
            Assert.Equal(5, playerStats.ShotsLandedPerKill);
        }

        [Fact]
        public void WeaponStatsProperties()
        {
            var weaponStats = WeaponStatsData()[0];
            Assert.Equal(0.5, weaponStats.Accuracy);
            Assert.Equal(0.03, weaponStats.HeadshotPercentage);
            Assert.Equal(3, weaponStats.KillsPerMinute);
            Assert.Equal(300, weaponStats.DamagePerMinute);
            Assert.Equal(100, weaponStats.DamagePerKill);
            Assert.Equal(10, weaponStats.ShotsFiredPerKill);
            Assert.Equal(5, weaponStats.ShotsLandedPerKill);
        }

        private static IList<PlayerStats> PlayerStatsData()
        {
            return new List<PlayerStats>
            {
                new PlayerStats
                {
                    Assists = 7,
                    DamageDealt = 1800,
                    Deaths = 10,
                    GamesPlayed = 1,
                    Kills = 18,
                    ShotsFired = 180,
                    ShotsLanded = 90,
                    TimePlayed = TimeSpan.FromMinutes(6),
                },
            };
        }

        private static IList<WeaponStats> WeaponStatsData()
        {
            return new List<WeaponStats>
            {
                new WeaponStats
                {
                    DamageDealt = 1800,
                    Headshots = 6,
                    Kills = 18,
                    PossessionTime = TimeSpan.FromMinutes(6),
                    ShotsFired = 180,
                    ShotsLanded = 90,
                },
            };
        }
    }
}
