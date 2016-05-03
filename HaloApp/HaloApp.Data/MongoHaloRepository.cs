using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloApp.Domain.Models.Dto;
using HaloApp.Domain.Models.Metadata;
using HaloApp.Domain.Services;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

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

        public async Task ReplaceMetadataAsync<TMetadata>(IEnumerable<TMetadata> metadata)
        {
            string metadataCollectionName = typeof(TMetadata).Name + 's';
            await _haloDb.DropCollectionAsync(metadataCollectionName);
            var metadataCollection = _haloDb.GetCollection<TMetadata>(metadataCollectionName);
            await metadataCollection.InsertManyAsync(metadata);
        }

        public async Task<IEnumerable<TMetadata>> GetMetadataAsync<TMetadata>()
        {
            string metadataCollectionName = typeof(TMetadata).Name + 's';
            var metadataCollection = _haloDb.GetCollection<TMetadata>(metadataCollectionName);
            return await metadataCollection
                .AsQueryable()
                .ToListAsync();
        }

        public async Task AddMatchesAsync(IEnumerable<MatchDto> matches)
        {
            var matchCollection = _haloDb.GetCollection<MatchDto>("Matches");
            await matchCollection.InsertManyAsync(matches);
        }

        public async Task<IEnumerable<MatchDto>> GetMatchesAsync(string player)
        {
            var matchCollection = _haloDb.GetCollection<MatchDto>("Matches");
            return await matchCollection
                .AsQueryable()
                .Where(m => m.Players.Any(p => p.Name == player))
                .ToListAsync();
        }

        private void RegisterClasses()
        {
            // Metadata
            BsonClassMap.RegisterClassMap<CsrDesignation>();
            BsonClassMap.RegisterClassMap<CsrTier>();
            BsonClassMap.RegisterClassMap<FlexibleStat>();
            BsonClassMap.RegisterClassMap<GameBaseVariant>();
            BsonClassMap.RegisterClassMap<GameVariant>();
            BsonClassMap.RegisterClassMap<Impulse>();
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

            BsonClassMap.RegisterClassMap<CsrDto>();
            BsonClassMap.RegisterClassMap<MatchDto>();
            BsonClassMap.RegisterClassMap<PlayerDto>();
        }
    }
}
