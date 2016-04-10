using HaloApp.Domain.Models;
using HaloApp.Domain.Models.Metadata;
using HaloApp.Domain.Services;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
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

        public async Task ReplaceMetadata<TMetadata>(IList<TMetadata> metadata)
        {
            string metadataCollectionName = typeof(TMetadata).Name + 's';
            await _haloDb.DropCollectionAsync(metadataCollectionName);
            var metadataCollection = _haloDb.GetCollection<TMetadata>(metadataCollectionName);
            await metadataCollection.InsertManyAsync(metadata);
        }

        private void RegisterClasses()
        {
            #region Metadata

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

            #endregion

            BsonClassMap.RegisterClassMap<Csr>();
            BsonClassMap.RegisterClassMap<Match>();
            BsonClassMap.RegisterClassMap<MatchPlayer>();
        }
    }
}
