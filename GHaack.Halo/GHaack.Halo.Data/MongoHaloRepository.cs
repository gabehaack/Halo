using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GHaack.Halo.Domain.Models.Dto;
using GHaack.Halo.Domain.Models.Metadata;
using GHaack.Halo.Domain.Services;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace GHaack.Halo.Data
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
                .Where(m => 
                    m.Players.Any(p => 
                        String.Equals(p.Name, player, 
                            StringComparison.OrdinalIgnoreCase)))
                .ToListAsync();
        }

        private static void RegisterClasses()
        {
            // Metadata
            RegisterClassMap<CsrDesignation>();
            RegisterClassMap<CsrTier>();
            RegisterClassMap<FlexibleStat>();
            RegisterClassMap<GameBaseVariant>();
            RegisterClassMap<GameVariant>();
            RegisterClassMap<Impulse>();
            RegisterClassMap<Map>();
            RegisterClassMap<MapVariant>();
            RegisterClassMap<Medal>();
            RegisterClassMap<MedalSpriteLocation>();
            RegisterClassMap<Playlist>();
            RegisterClassMap<Season>();
            RegisterClassMap<SpartanRank>();
            RegisterClassMap<TeamColor>();
            RegisterClassMap<Vehicle>();
            RegisterClassMap<Weapon>();

            // Domain DTOs
            RegisterClassMap<CsrDto>();
            RegisterClassMap<MatchDto>();
            RegisterClassMap<PlayerDto>();
        }

        private static void RegisterClassMap<T>()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
                BsonClassMap.RegisterClassMap<T>();
        }
    }
}
