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
using Xunit;

namespace HaloApp.Tests
{
    [ExcludeFromCodeCoverage]
    public class HaloDataManagerTests
    {
        private static Random Random = new Random();
        private static DataGenerator DataGenerator = new DataGenerator(Random);
        private const string Player = "shockRocket";

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
            var haloApi = Substitute.For<IHaloApi>();
            var haloRepository = Substitute.For<IHaloRepository>();
            var haloDataManager = new HaloDataManager(haloApi, haloRepository);

            var matches = MatchData(Player, 5);
            // TODO - actually write the unit test
        }

        private IList<Match> MatchData(string player, int quantity)
        {
            var matches = new List<Match>();
            for (int i = 0; i < quantity; i++)
            {
                matches.Add(MatchDatum(player));
            }
            return matches;
        }

        private Match MatchDatum(string player)
        {
            return new Match
            {
                Completed = DateTime.Now,
                Duration = TimeSpan.FromMinutes(DataGenerator.Int(10, 20)),
                GameBaseVariantId = Guid.NewGuid(),
                GameMode = EnumUtility.GetRandomEnumValue<GameMode>(),
                GameVariantId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                MapId = Guid.NewGuid(),
                MapVariant = Guid.NewGuid(),
                Players = MatchPlayerData(player),
                PlaylistId = Guid.NewGuid(),
                SeasonId = Guid.NewGuid(),
                TeamGame = DataGenerator.Bool(),
            };
        }

        private IList<MatchPlayer> MatchPlayerData(string player)
        {
            var matchPlayers = new List<MatchPlayer>();
            int team1 = DataGenerator.PositiveInt();
            matchPlayers.Add(MatchPlayerDatum(player, team1));
            for (int i = 0; i < 3; i++)
            {
                matchPlayers.Add(MatchPlayerDatum(DataGenerator.String(10, 15), team1));
            }
            int team2 = DataGenerator.PositiveInt();
            for (int i = 0; i < 4; i++)
            {
                matchPlayers.Add(MatchPlayerDatum(DataGenerator.String(10, 15), team2));
            }
            return matchPlayers;
        }

        private MatchPlayer MatchPlayerDatum(string player, int team)
        {
            return new MatchPlayer
            {
                AverageLifeTime = TimeSpan.FromSeconds(DataGenerator.PositiveInt(60)),
                CurrentCsr = CsrDatum(),
                Dnf = DataGenerator.Bool(),
                Name = player,
                PreviousCsr = CsrDatum(),
                Rank = DataGenerator.Int(),
                Team = team,
                TotalAssassinations = DataGenerator.Int(),
                TotalAssists = DataGenerator.Int(),
                TotalDeaths = DataGenerator.Int(),
                TotalGrenadeDamage = DataGenerator.Double(1000),
                TotalGrenadeKills = DataGenerator.Int(),
                TotalGroundPoundDamage = DataGenerator.Double(1000),
                TotalGroundPoundKills = DataGenerator.Int(),
                TotalHeadshots = DataGenerator.Int(),
                TotalKills = DataGenerator.Int(),
                TotalMeleeDamage = DataGenerator.Double(1000),
                TotalMeleeKills = DataGenerator.Int(),
                TotalPowerWeaponDamage = DataGenerator.Double(1000),
                TotalPowerWeaponGrabs = DataGenerator.Int(),
                TotalPowerWeaponKills = DataGenerator.Int(),
                TotalShotsFired = DataGenerator.Int(),
                TotalShotsLanded = DataGenerator.Int(),
                TotalShoulderBashDamage = DataGenerator.Double(1000),
                TotalShoulderBashKills = DataGenerator.Int(),
                TotalWeaponDamage = DataGenerator.Double(5000),
                WeaponsStats = WeaponsStatsData(10),
            };
        }

        private Csr CsrDatum()
        {
            return new Csr
            {
                CsrDesignationId = DataGenerator.Int(),
                CsrDesignationTierId = DataGenerator.Int(),
                PercentToNextTier = DataGenerator.Int(100),
                Rank = DataGenerator.Int(),
                Value = DataGenerator.Int(),
            };
        }

        private IList<WeaponStats> WeaponsStatsData(int quantity)
        {
            var weaponsStats = new List<WeaponStats>();
            for (int i = 0; i < quantity; i++)
            {
                weaponsStats.Add(WeaponStatsData());
            }
            return weaponsStats;
        }

        private WeaponStats WeaponStatsData()
        {
            return new WeaponStats
            {
                DamageDealt = DataGenerator.Double(1000),
                Headshots = DataGenerator.Int(10),
                Id = DataGenerator.Int(),
                Kills = DataGenerator.Int(),
                PossessionTime = TimeSpan.FromMinutes(DataGenerator.Int(5)),
                ShotsFired = DataGenerator.Int(),
                ShotsLanded = DataGenerator.Int(),
            };
        }
    }
}
