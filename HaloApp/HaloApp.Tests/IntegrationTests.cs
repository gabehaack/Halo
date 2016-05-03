using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaloApp.ApiClient;
using HaloApp.Data;
using HaloApp.Domain;
using HaloApp.Domain.Services;
using MongoDB.Driver;
using Xunit;

namespace HaloApp.Tests
{
    public class IntegrationTests
    {
        private static readonly Uri HaloApiUri = new Uri(
            ConfigurationManager.AppSettings["HaloApiUri"]);
        private static readonly string SubscriptionKey =
            ConfigurationManager.AppSettings["SubscriptionKey"];
        private static readonly string MongoDbConnection =
            ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString;

        private static HaloDataManager HaloDataManager()
        {
            return new HaloDataManager(HaloApi(), HaloRepository());
        }

        private static IHaloApi HaloApi()
        {
            return new HaloApi(HaloApiUri, SubscriptionKey);
        }

        private static IHaloRepository HaloRepository()
        {
            IMongoClient mongoClient = new MongoClient(MongoDbConnection);
            return new MongoHaloRepository(mongoClient);
        }

        public IntegrationTests()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<HaloApiMapProfile>();
                cfg.AddProfile<HaloDomainMapProfile>();
            });
        }

        [Fact]
        public async Task GetMatches()
        {
            var haloApi = HaloApi();

            var matches = await haloApi.GetMatchesAsync("shockRocket", 0, 1);
            var match = matches.First();
            var players = await haloApi.GetMatchStatsAsync(match.Id);
            match.Players = players;

        }

        [Fact]
        public async Task ReplaceAllMetadata()
        {
            var haloDataManager = HaloDataManager();

            await haloDataManager.ReplaceImpulseMetadataAsync();
        }

        [Fact]
        public async Task StoreMatches()
        {
            var haloDataManager = HaloDataManager();

            await haloDataManager.StoreMatchesAsync("shockRocket", 0, 100);
        }

        [Fact]
        public async Task RetrieveStoredMatches()
        {
            var haloDataManager = HaloDataManager();

            var matches = await haloDataManager.RetrieveStoredMatchesAsync("shockRocket");
            var match = matches.Take(1);
            var playerStats = haloDataManager.GetPlayerStats(matches, "shockRocket");

            var player = matches.First().GetPlayer("shockRocket");
            double totalWeaponDamage = player.WeaponDamage;
            double othertotal = player.WeaponsStats
                .Sum(w => w.DamageDealt);
            double diff = totalWeaponDamage - othertotal;
        }
    }
}
