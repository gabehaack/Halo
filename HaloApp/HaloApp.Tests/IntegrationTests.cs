using AutoMapper;
using HaloApp.ApiClient;
using HaloApp.Data;
using HaloApp.Domain;
using HaloApp.Domain.Services;
using MongoDB.Driver;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
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

        private const string Player = "shockRocket";

        private static HaloDataManager HaloDataManager()
        {
            return new HaloDataManager(HaloApiClient(), HaloRepository());
        }

        private static IHaloApi HaloApiClient()
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
            });
        }

        [Fact]
        public async Task ReplaceAllMetadata()
        {
            var haloDataManager = HaloDataManager();

            await haloDataManager.ReplaceAllMetadataAsync();
        }

        [Fact]
        public async Task StoreMatches()
        {
            var haloDataManager = HaloDataManager();

            await haloDataManager.StoreMatchesAsync(Player);
        }

        [Fact]
        public async Task GetMatches()
        {
            var haloDataManager = HaloDataManager();

            var matches = await haloDataManager.GetMatchesAsync(Player);
            var playerStats = haloDataManager.GetPlayerStats(matches.ToList(), Player);

            var player = matches.First().GetPlayer(Player);
            double totalWeaponDamage = player.WeaponDamage;
            double othertotal = player.WeaponsStats
                .Sum(w => w.DamageDealt);
            double diff = totalWeaponDamage - othertotal;
        }
    }
}
