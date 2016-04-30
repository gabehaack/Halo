using HaloApp.Domain.Models;
using HaloApp.Domain.Models.Metadata;
using HaloApp.Domain.Services;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloApp.Data
{
    public class MongoHaloRepository : IHaloRepository
    {
        private readonly IMongoClient _mongoClient;
        private readonly IMongoDatabase _haloDb;

        public MongoHaloRepository(IMongoClient mongoClient)
        {
            if (mongoClient == null)
                throw new ArgumentNullException(nameof(mongoClient));
            _mongoClient = mongoClient;
            RegisterClasses();

            _haloDb = _mongoClient.GetDatabase("Halo");
        }

        public async Task ReplaceMetadataAsync<TMetadata>(IList<TMetadata> metadata)
        {
            string metadataCollectionName = typeof(TMetadata).Name + 's';
            await _haloDb.DropCollectionAsync(metadataCollectionName);
            var metadataCollection = _haloDb.GetCollection<TMetadata>(metadataCollectionName);
            await metadataCollection.InsertManyAsync(metadata);
        }

        public async Task<IList<TMetadata>> GetMetadataAsync<TMetadata>()
        {
            string metadataCollectionName = typeof(TMetadata).Name + 's';
            var metadataCollection = _haloDb.GetCollection<TMetadata>(metadataCollectionName);
            return await metadataCollection.AsQueryable()
                .ToListAsync();
        }

        public async Task AddMatchesAsync(IList<Match> matches)
        {
            var matchCollection = _haloDb.GetCollection<Match>("Matches");
            await matchCollection.InsertManyAsync(matches);
        }

        public async Task<IList<Match>> GetMatchesAsync(string player)
        {
            var matchCollection = _haloDb.GetCollection<Match>("Matches");
            var matches = await matchCollection.AsQueryable()
                .Where(m => m.Players.Any(p => p.Name == player))
                .ToListAsync();
            var weapons = await GetMetadataAsync<Weapon>();
            foreach (var match in matches)
            {
                foreach (var matchPlayer in match.Players)
                {
                    foreach (var weaponStat in matchPlayer.WeaponsStats)
                    {
                        weaponStat.Weapon = weapons
                            .FirstOrDefault(w => w.Id == weaponStat.Weapon.Id);
                    }
                }
            }
            return matches;
        }

        private void RegisterClasses()
        {
            // Metadata
            BsonClassMap.RegisterClassMap<CsrDesignation>();
            BsonClassMap.RegisterClassMap<CsrTier>();
            BsonClassMap.RegisterClassMap<FlexibleStat>();
            BsonClassMap.RegisterClassMap<GameBaseVariant>();
            BsonClassMap.RegisterClassMap<GameVariant>();
            //BsonClassMap.RegisterClassMap<Impulse>();
            BsonClassMap.RegisterClassMap<Map>();
            BsonClassMap.RegisterClassMap<MapVariant>();
            BsonClassMap.RegisterClassMap<Medal>();
            BsonClassMap.RegisterClassMap<MedalSpriteLocation>();
            BsonClassMap.RegisterClassMap<Playlist>();
            BsonClassMap.RegisterClassMap<Season>();
            BsonClassMap.RegisterClassMap<SpartanRank>();
            BsonClassMap.RegisterClassMap<TeamColor>();
            BsonClassMap.RegisterClassMap<Vehicle>();
            BsonClassMap.RegisterClassMap<Weapon>();

            BsonClassMap.RegisterClassMap<Csr>();
            BsonClassMap.RegisterClassMap<Match>();
            BsonClassMap.RegisterClassMap<MatchPlayer>();
        }
    }
}
