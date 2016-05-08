using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using GHaack.Halo.Api;
using GHaack.Halo.Data;
using GHaack.Halo.Domain;
using GHaack.Halo.Domain.Enums;
using GHaack.Halo.Domain.Models;
using GHaack.Halo.Domain.Models.Dto;
using GHaack.Halo.Domain.Models.Metadata;
using GHaack.Halo.Domain.Services;
using MongoDB.Driver;
using NSubstitute;
using Xunit;

namespace GHaack.Halo.Tests
{
    [ExcludeFromCodeCoverage]
    public class HaloDataManagerTests
    {
        private static HaloDataManager HaloDataManager()
        {
            var haloApi = Substitute.For<IHaloApi>();
            var haloRepository = Substitute.For<IHaloRepository>();
            return new HaloDataManager(haloApi, haloRepository);
        }

        private static HaloDataManager HaloDataManager(IEnumerable<Weapon> weapons)
        {
            var haloApi = Substitute.For<IHaloApi>();
            var haloRepository = Substitute.For<IHaloRepository>();
            haloRepository.GetMetadataAsync<Weapon>().Returns(weapons);
            return new HaloDataManager(haloApi, haloRepository);
        }

        #region Tests

        [Fact]
        public void Ctor()
        {
            IHaloApi haloApi = new HaloApi(new Uri("http://fakeUri"), "key");
            IMongoClient mongoClient = new MongoClient();
            IHaloRepository haloRepository = new MongoHaloRepository(mongoClient);
            Assert.Throws<ArgumentNullException>(() => new HaloDataManager(haloApi, null));
            Assert.Throws<ArgumentNullException>(() => new HaloDataManager(null, haloRepository));
        }

        [Fact]
        public void GetPlayerStats()
        {
            var haloDataManager = HaloDataManager();
            var matches = MatchData();

            var playerStats = haloDataManager.GetPlayerStats(matches, "shockRocket");
            Assert.Equal(10, playerStats.Assists);
            Assert.Equal(2600, playerStats.DamageDealt);
            Assert.Equal(8, playerStats.Deaths);
            Assert.Equal(2, playerStats.GamesPlayed);
            Assert.Equal(20, playerStats.Kills);
            Assert.Equal("shockRocket", playerStats.Name);
            Assert.Equal(200, playerStats.ShotsFired);
            Assert.Equal(100, playerStats.ShotsLanded);
            Assert.True(TimeSpan.FromMinutes(10).Equals(playerStats.TimePlayed));

            var weapon1 = playerStats.WeaponsStats.First();
            Assert.Equal(1000, weapon1.DamageDealt);
            Assert.Equal(5, weapon1.Headshots);
            Assert.Equal(10, weapon1.Kills);
            Assert.True(TimeSpan.FromMinutes(5).Equals(weapon1.PossessionTime));
            Assert.Equal(150, weapon1.ShotsFired);
            Assert.Equal(80, weapon1.ShotsLanded);
            Assert.Equal(1, weapon1.Weapon.Id);

            var weapon2 = playerStats.WeaponsStats.ElementAt(1);
            Assert.Equal(500, weapon2.DamageDealt);
            Assert.Equal(1, weapon2.Headshots);
            Assert.Equal(5, weapon2.Kills);
            Assert.True(TimeSpan.FromMinutes(3).Equals(weapon2.PossessionTime));
            Assert.Equal(50, weapon2.ShotsFired);
            Assert.Equal(20, weapon2.ShotsLanded);
            Assert.Equal(2, weapon2.Weapon.Id);
        }

        [Fact]
        public async Task MatchesAsync()
        {
            var haloDataManager = HaloDataManager(WeaponData());

            var matches = await haloDataManager.MatchesAsync(MatchData());

            var player1weapon1stats = matches.First().Players.First()
                .WeaponsStats.First(w => w.Weapon.Id == 1);
            Assert.Equal("weaponName1", player1weapon1stats.Weapon.Name);
            Assert.Equal("weaponDescription1", player1weapon1stats.Weapon.Description);

            var player1weapon2stats = matches.First().Players.First()
                .WeaponsStats.First(w => w.Weapon.Id == 2);
            Assert.Equal("weaponName2", player1weapon2stats.Weapon.Name);
            Assert.Equal("weaponDescription2", player1weapon2stats.Weapon.Description);

            var player2weapon1stats = matches.First().Players.First()
                .WeaponsStats.First(w => w.Weapon.Id == 1);
            Assert.Equal("weaponName1", player2weapon1stats.Weapon.Name);
            Assert.Equal("weaponDescription1", player2weapon1stats.Weapon.Description);

            var player2weapon2stats = matches.First().Players.First()
                .WeaponsStats.First(w => w.Weapon.Id == 2);
            Assert.Equal("weaponName2", player2weapon2stats.Weapon.Name);
            Assert.Equal("weaponDescription2", player2weapon2stats.Weapon.Description);
        }

        #endregion

        #region

        private static IList<Match> MatchData()
        {
            return new List<Match>
            {
                new Match
                {
                    Duration = TimeSpan.FromMinutes(6),
                    GameMode = GameMode.Arena,
                    Players = PlayerData1(),
                    TeamGame = true,
                },
                new Match
                {
                    Duration = TimeSpan.FromMinutes(4),
                    GameMode = GameMode.Arena,
                    Players = PlayerData2(),
                    TeamGame = true,
                },
            };
        }

        private static IList<Player> PlayerData1()
        {
            return new List<Player>
            {
                new Player
                {
                    Name = "shockRocket",
                    Assists = 4,
                    Deaths = 3,
                    GrenadeDamage = 50,
                    GroundPoundDamage = 40,
                    Headshots = 2,
                    Kills = 8,
                    MeleeDamage = 100,
                    PowerWeaponDamage = 150,
                    ShotsFired = 80,
                    ShotsLanded = 40,
                    ShoulderBashDamage = 40,
                    WeaponDamage = 500,
                    WeaponsStats = WeaponStatsData1(),
                },
            };
        }

        private static IList<Player> PlayerData2()
        {
            return new List<Player>
            {
                new Player
                {
                    Name = "shockRocket",
                    Assists = 6,
                    Deaths = 5,
                    GrenadeDamage = 150,
                    GroundPoundDamage = 60,
                    Headshots = 3,
                    Kills = 12,
                    MeleeDamage = 200,
                    PowerWeaponDamage = 250,
                    ShotsFired = 120,
                    ShotsLanded = 60,
                    ShoulderBashDamage = 60,
                    WeaponDamage = 1000,
                    WeaponsStats = WeaponStatsData2(),
                },
            };
        }

        private static IList<WeaponStats> WeaponStatsData1()
        {
            return new List<WeaponStats>
            {
                new WeaponStats
                {
                    DamageDealt = 400,
                    Headshots = 2,
                    Kills = 4,
                    PossessionTime = TimeSpan.FromMinutes(2),
                    ShotsFired = 50,
                    ShotsLanded = 30,
                    Weapon = new Weapon
                    {
                        Id = 1,
                    },
                },
                new WeaponStats
                {
                    DamageDealt = 200,
                    Headshots = 0,
                    Kills = 2,
                    PossessionTime = TimeSpan.FromMinutes(1),
                    ShotsFired = 20,
                    ShotsLanded = 5,
                     Weapon = new Weapon
                    {
                        Id = 2,
                    },
                },
            };
        }

        private static IList<WeaponStats> WeaponStatsData2()
        {
            return new List<WeaponStats>
            {
                new WeaponStats
                {
                    DamageDealt = 600,
                    Headshots = 3,
                    Kills = 6,
                    PossessionTime = TimeSpan.FromMinutes(3),
                    ShotsFired = 100,
                    ShotsLanded = 50,
                     Weapon = new Weapon
                    {
                        Id = 1,
                    },
                },
                new WeaponStats
                {
                    DamageDealt = 300,
                    Headshots = 1,
                    Kills = 3,
                    PossessionTime = TimeSpan.FromMinutes(2),
                    ShotsFired = 30,
                    ShotsLanded = 15,
                     Weapon = new Weapon
                    {
                        Id = 2,
                    },
                },
            };
        }

        private static IList<Weapon> WeaponData()
        {
            return new List<Weapon>
            {
                new Weapon
                {
                    Description = "weaponDescription1",
                    Id = 1,
                    Name = "weaponName1",
                },
                new Weapon
                {
                    Description = "weaponDescription2",
                    Id = 2,
                    Name = "weaponName2",
                },
            };
        }

        #endregion
    }
}
