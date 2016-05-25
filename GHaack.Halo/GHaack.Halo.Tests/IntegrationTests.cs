using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GHaack.Halo.Api;
using GHaack.Halo.Data;
using GHaack.Halo.Domain;
using GHaack.Halo.Domain.Models;
using GHaack.Halo.Domain.Services;
using MongoDB.Driver;
using Xunit;

namespace GHaack.Halo.Tests
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
            return new HaloDataManager(HaloApi(), HaloRepository(), Mapper());
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

        private static IMapper Mapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<HaloApiMapProfile>();
                cfg.AddProfile<HaloDomainMapProfile>();
            });
            config.AssertConfigurationIsValid();
            return config.CreateMapper();
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

            await haloDataManager.ReplaceAllMetadataAsync();
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

            var playerStats = haloDataManager.GetPlayerStats(match, "shockRocket");
            var player = match.First().GetPlayer("shockRocket");
            double totalWeaponDamage = player.WeaponDamage;
            double othertotal = player.WeaponsStats
                .Sum(w => w.DamageDealt);
            double diff = totalWeaponDamage - othertotal;
        }

        [Fact]
        public async Task MatchesAsync()
        {
            var haloRepository = HaloRepository();
            var haloApi = HaloApi();
            var haloDataManager = new HaloDataManager(haloApi, haloRepository);

            var matchDtos = await haloRepository.GetMatchesAsync("shockRocket");
            var matches = matchDtos.Take(1)
                .Select(Mapper.Map<Match>);
            var preMatch = matches.First();

            var postMatches =  await haloDataManager.MatchesAsync(matches);
            var match = postMatches.First();
            var player = match.Players.First(p => p.Name == "shockRocket");
            var weaponStats = player.WeaponsStats
                .Select(w => new { w.Weapon.Name, w.Kills, w.Accuracy });
            { }
        }

        [Fact]
        public async Task ProfileImages()
        {
            var haloApi = HaloApi();
            var emblem = await haloApi.GetEmblemImageUriAsync("asldkfjatlasjf");
            { }
        }
    }
}
