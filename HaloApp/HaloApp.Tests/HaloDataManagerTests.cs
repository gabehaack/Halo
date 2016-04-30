using GHaack.Utilities;
using HaloApp.ApiClient;
using HaloApp.Data;
using HaloApp.Domain;
using HaloApp.Domain.Enums;
using HaloApp.Domain.Models;
using HaloApp.Domain.Services;
using MongoDB.Driver;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace HaloApp.Tests
{
    [ExcludeFromCodeCoverage]
    public class HaloDataManagerTests
    {
        private static Random Random = new Random();
        private static DataGenerator DataGenerator = new DataGenerator(Random);
        private const string Player = "shockRocket";

        private static HaloDataManager HaloDataManager()
        {
            var haloApi = Substitute.For<IHaloApi>();
            var haloRepository = Substitute.For<IHaloRepository>();
            return new HaloDataManager(haloApi, haloRepository);
        }

        [Fact]
        public void Ctor()
        {
            IHaloApi haloApi = new HaloApi(new Uri(""), "");
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

            var playerStats = haloDataManager.GetPlayerStats(matches, Player);
            Assert.Equal(0.5, playerStats.Accuracy);
        }

        private IList<Match> MatchData()
        {
            return new List<Match>
            {
                new Match
                {
                    Duration = TimeSpan.FromMinutes(10),
                    GameMode = GameMode.Arena,
                    Players = MatchPlayerData(),
                    TeamGame = true,
                },
            };
        }

        private IList<MatchPlayer> MatchPlayerData()
        {
            return new List<MatchPlayer>
            {
                new MatchPlayer
                {
                    AvgLifeTime = TimeSpan.FromSeconds(60),
                    Name = "shockRocket",
                    Team = 1,
                    Assassinations = 1,
                    Assists = 10,
                    Deaths = 10,
                    GrenadeDamage = 200,
                    GrenadeKills = 2,
                    GroundPoundDamage = 100,
                    GroundPoundKills = 1,
                    Headshots = 5,
                    Kills = 20,
                    MeleeDamage = 300,
                    MeleeKills = 3,
                    PowerWeaponDamage = 400,
                    PowerWeaponKills = 4,
                    ShotsFired = 200,
                    ShotsLanded = 100,
                    ShoulderBashDamage = 100,
                    ShoulderBashKills = 1,
                    WeaponDamage = 1500,
                    WeaponsStats = WeaponStatsData(),
                },
            };
        }

        private IList<WeaponStats> WeaponStatsData()
        {
            return new List<WeaponStats>
            {
                new WeaponStats
                {
                    DamageDealt = 1000,
                    Headshots = 5,
                    Kills = 10,
                    PossessionTime = TimeSpan.FromMinutes(5),
                    ShotsFired = 100,
                    ShotsLanded = 50,
                },
                new WeaponStats
                {
                    DamageDealt = 500,
                    Headshots = 0,
                    Kills = 5,
                    PossessionTime = TimeSpan.FromMinutes(5),
                    ShotsFired = 100,
                    ShotsLanded = 25,
                },
            };
        }
    }
}
