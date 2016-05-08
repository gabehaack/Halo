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
                .Where(m => m.Players.Any(p => p.Name == player))
                .ToListAsync();
        }

        public static void RegisterClasses()
        {
            // Metadata
            if (!BsonClassMap.IsClassMapRegistered(typeof(CsrDesignation)))
                BsonClassMap.RegisterClassMap<CsrDesignation>();
            if (!BsonClassMap.IsClassMapRegistered(typeof(CsrTier)))
                BsonClassMap.RegisterClassMap<CsrTier>();
            if (!BsonClassMap.IsClassMapRegistered(typeof(FlexibleStat)))
                BsonClassMap.RegisterClassMap<FlexibleStat>();
            if (!BsonClassMap.IsClassMapRegistered(typeof(GameBaseVariant)))
                BsonClassMap.RegisterClassMap<GameBaseVariant>();
            if (!BsonClassMap.IsClassMapRegistered(typeof(GameVariant)))
                BsonClassMap.RegisterClassMap<GameVariant>();
            if (!BsonClassMap.IsClassMapRegistered(typeof(Impulse)))
                BsonClassMap.RegisterClassMap<Impulse>();
            if (!BsonClassMap.IsClassMapRegistered(typeof(Map)))
                BsonClassMap.RegisterClassMap<Map>();
            if (!BsonClassMap.IsClassMapRegistered(typeof(MapVariant)))
                BsonClassMap.RegisterClassMap<MapVariant>();
            if (!BsonClassMap.IsClassMapRegistered(typeof(Medal)))
                BsonClassMap.RegisterClassMap<Medal>();
            if (!BsonClassMap.IsClassMapRegistered(typeof(MedalSpriteLocation)))
                BsonClassMap.RegisterClassMap<MedalSpriteLocation>();
            if (!BsonClassMap.IsClassMapRegistered(typeof(Playlist)))
                BsonClassMap.RegisterClassMap<Playlist>();
            if (!BsonClassMap.IsClassMapRegistered(typeof(Season)))
                BsonClassMap.RegisterClassMap<Season>();
            if (!BsonClassMap.IsClassMapRegistered(typeof(SpartanRank)))
                BsonClassMap.RegisterClassMap<SpartanRank>();
            if (!BsonClassMap.IsClassMapRegistered(typeof(TeamColor)))
                BsonClassMap.RegisterClassMap<TeamColor>();
            if (!BsonClassMap.IsClassMapRegistered(typeof(Vehicle)))
                BsonClassMap.RegisterClassMap<Vehicle>();
            if (!BsonClassMap.IsClassMapRegistered(typeof(Weapon)))
                BsonClassMap.RegisterClassMap<Weapon>();

            if (!BsonClassMap.IsClassMapRegistered(typeof(CsrDto)))
                BsonClassMap.RegisterClassMap<CsrDto>();
            if (!BsonClassMap.IsClassMapRegistered(typeof(MatchDto)))
                BsonClassMap.RegisterClassMap<MatchDto>();
            if (!BsonClassMap.IsClassMapRegistered(typeof(PlayerDto)))
                BsonClassMap.RegisterClassMap<PlayerDto>();
        }
    }
}
