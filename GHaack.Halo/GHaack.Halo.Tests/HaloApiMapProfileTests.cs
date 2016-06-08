using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Xml;
using AutoMapper;
using GHaack.Halo.Api;
using GHaack.Halo.Domain.Enums;
using GHaack.Halo.Domain.Models.Dto;
using GHaack.Halo.Domain.Models.Metadata;
using GHaack.Utilities;
using Xunit;
using ApiMetadata = GHaack.Halo.Api.Models.Metadata;
using ApiModels = GHaack.Halo.Api.Models;

namespace GHaack.Halo.Tests
{
    [ExcludeFromCodeCoverage]
    public class HaloApiMapProfileTests
    {
        private static IMapper Mapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<HaloApiMapProfile>();
            });
            config.AssertConfigurationIsValid();
            return config.CreateMapper();
        }

        #region Type Conversion

        [Fact]
        public void UriTypeConversion()
        {
            string data = "http://uri";
            var expected = new Uri(data);

            var actual = Mapper().Map<Uri>(data);
            Assert.True(expected.Equals(actual));
        }

        [Fact]
        public void UriTypeConversion_Null()
        {
            string data = null;

            var actual = Mapper().Map<Uri>(data);
            Assert.Null(actual);
        }

        [Fact]
        public void GuidTypeConversion()
        {
            var expected = Guid.NewGuid();
            var data = expected.ToString();

            var actual = Mapper().Map<Guid>(data);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GuidTypeConversion_Null()
        {
            string data = null;
            var expected = Guid.Empty;

            var actual = Mapper().Map<Guid>(data);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TimeSpanTypeConversion()
        {
            string data = "PT1M30S";
            var expected = TimeSpan.FromSeconds(90);

            var actual = Mapper().Map<TimeSpan>(data);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TimeSpanTypeConversion_Null()
        {
            string data = null;
            var expected = TimeSpan.Zero;

            var actual = Mapper().Map<TimeSpan>(data);
            Assert.Equal(expected, actual);
        }

        #endregion

        #region Metatdata

        [Fact]
        public void CsrTier()
        {
            var data = CsrTierData()[0];
            var expected = new CsrTier
            {
                IconImageUrl = new Uri(data.iconImageUrl),
                Id = data.id,
            };

            var actual = Mapper().Map<CsrTier>(data);
            Equal(expected, actual);
        }

        private static void Equal(CsrTier expected, CsrTier actual)
        {
            Assert.True(expected.IconImageUrl.Equals(actual.IconImageUrl));
            Assert.Equal(expected.Id, actual.Id);
        }

        [Fact]
        public void CsrDesignation()
        {
            var data = CsrDesignationData()[0];
            var expected = new CsrDesignation
            {
                BannerImageUrl = new Uri(data.bannerImageUrl),
                Id = data.id,
                Name = data.name,
                Tiers = new List<CsrTier>
                {
                    new CsrTier
                    {
                        IconImageUrl = new Uri(data.tiers.First().iconImageUrl),
                        Id = data.tiers.First().id,
                    },
                },
            };

            var actual = Mapper().Map<CsrDesignation>(data);
            Equal(expected, actual);
        }

        private static void Equal(CsrDesignation expected, CsrDesignation actual)
        {
            Assert.True(expected.BannerImageUrl.Equals(actual.BannerImageUrl));
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Equal(expected.Tiers.First(), actual.Tiers.First());
        }

        [Fact]
        public void FlexibleStat()
        {
            var data = FlexibleStatData()[0];
            var expected = new FlexibleStat
            {
                Id = Guid.Parse(data.id),
                Name = data.name,
                Type = EnumUtility.Parse<FlexibleStatType>(data.type),
            };

            var actual = Mapper().Map<FlexibleStat>(data);
            Equal(expected, actual);
        }

        private static void Equal(FlexibleStat expected, FlexibleStat actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Type, actual.Type);
        }

        [Fact]
        public void GameBaseVariant()
        {
            var data = GameBaseVariantData()[0];
            var expected = new GameBaseVariant
            {
                Id = Guid.Parse(data.id),
                IconUrl = new Uri(data.iconUrl),
                Name = data.name,
                SupportedGameModes = new List<GameMode>
                {
                    EnumUtility.Parse<GameMode>(data.supportedGameModes.First())
                },
            };

            var actual = Mapper().Map<GameBaseVariant>(data);
            Equal(expected, actual);
        }

        private static void Equal(GameBaseVariant expected, GameBaseVariant actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.True(expected.IconUrl.Equals(actual.IconUrl));
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.SupportedGameModes.First(), actual.SupportedGameModes.First());
        }

        [Fact]
        public void GameVariant()
        {
            var data = GameVariantData()[0];
            var expected = new GameVariant
            {
                Description = data.description,
                GameBaseVariantId = Guid.Parse(data.gameBaseVariantId),
                IconUrl = new Uri(data.iconUrl),
                Id = Guid.Parse(data.id),
                Name = data.name,
            };

            var actual = Mapper().Map<GameVariant>(data);
            Equal(expected, actual);
        }

        private static void Equal(GameVariant expected, GameVariant actual)
        {
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.GameBaseVariantId, actual.GameBaseVariantId);
            Assert.True(expected.IconUrl.Equals(actual.IconUrl));
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
        }

        [Fact]
        public void Impulse()
        {
            var data = ImpulseData()[0];
            var expected = new Impulse
            {
                Id = data.id,
                Name = data.internalName,
            };

            var actual = Mapper().Map<Impulse>(data);
            Equal(expected, actual);
        }

        private static void Equal(Impulse expected, Impulse actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
        }

        [Fact]
        public void Map()
        {
            var data = MapData()[0];
            var expected = new Map
            {
                Description = data.description,
                ImageUrl = new Uri(data.imageUrl),
                Id = Guid.Parse(data.id),
                Name = data.name,
                SupportedGameModes = new List<GameMode>
                {
                    EnumUtility.Parse<GameMode>(data.supportedGameModes.First())
                },
            };

            var actual = Mapper().Map<Map>(data);
            Equal(expected, actual);
        }

        private static void Equal(Map expected, Map actual)
        {
            Assert.Equal(expected.Description, actual.Description);
            Assert.True(expected.ImageUrl.Equals(actual.ImageUrl));
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.SupportedGameModes.First(), actual.SupportedGameModes.First());
        }

        [Fact]
        public void MapVariant()
        {
            var data = MapVariantData()[0];
            var expected = new MapVariant
            {
                Description = data.description,
                Id = Guid.Parse(data.id),
                Name = data.name,
                MapId = Guid.Parse(data.mapId),
                MapImageUrl = new Uri(data.mapImageUrl),
            };

            var actual = Mapper().Map<MapVariant>(data);
            Equal(expected, actual);
        }

        private static void Equal(MapVariant expected, MapVariant actual)
        {
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.MapId, actual.MapId);
            Assert.True(expected.MapImageUrl.Equals(actual.MapImageUrl));
        }

        [Fact]
        public void MedalSpriteLocation()
        {
            var data = MedalSpriteLocationData()[0];
            var expected = new MedalSpriteLocation
            {
                Height = data.height,
                Left = data.left,
                SpriteHeight = data.spriteHeight,
                SpriteSheetUri = new Uri(data.spriteSheetUri),
                SpriteWidth = data.spriteWidth,
                Top = data.top,
                Width = data.width,
            };

            var actual = Mapper().Map<MedalSpriteLocation>(data);
            Equal(expected, actual);
        }

        private static void Equal(MedalSpriteLocation expected, MedalSpriteLocation actual)
        {
            Assert.Equal(expected.Height, actual.Height);
            Assert.Equal(expected.Left, actual.Left);
            Assert.Equal(expected.SpriteHeight, actual.SpriteHeight);
            Assert.True(expected.SpriteSheetUri.Equals(actual.SpriteSheetUri));
            Assert.Equal(expected.SpriteWidth, actual.SpriteWidth);
            Assert.Equal(expected.Top, actual.Top);
            Assert.Equal(expected.Width, actual.Width);
        }

        [Fact]
        public void Medal()
        {
            var data = MedalData()[0];
            var expected = new Medal
            {
                Classification = EnumUtility.Parse<MedalClassification>(data.classification),
                Description = data.description,
                Difficulty = data.difficulty,
                Id = data.id,
                Name = data.name,
                SpriteLocation = new MedalSpriteLocation
                {
                    Height = data.spriteLocation.height,
                    Left = data.spriteLocation.left,
                    SpriteHeight = data.spriteLocation.spriteHeight,
                    SpriteSheetUri = new Uri(data.spriteLocation.spriteSheetUri),
                    SpriteWidth = data.spriteLocation.spriteWidth,
                    Top = data.spriteLocation.top,
                    Width = data.spriteLocation.width,
                }
            };

            var actual = Mapper().Map<Medal>(data);
            Equal(expected, actual);
        }

        private static void Equal(Medal expected, Medal actual)
        {
            Assert.Equal(expected.Classification, actual.Classification);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.Difficulty, actual.Difficulty);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Equal(expected.SpriteLocation, actual.SpriteLocation);
        }

        [Fact]
        public void Playlist()
        {
            var data = PlaylistData()[0];
            var expected = new Playlist
            {
                Active = data.isActive,
                Description = data.description,
                GameMode = EnumUtility.Parse<GameMode>(data.gameMode),
                Id = Guid.Parse(data.id),
                ImageUrl = new Uri(data.imageUrl),
                Name = data.name,
                Ranked = data.isRanked,
            };

            var actual = Mapper().Map<Playlist>(data);
            Equal(expected, actual);
        }

        private static void Equal(Playlist expected, Playlist actual)
        {
            Assert.Equal(expected.Active, actual.Active);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.GameMode, actual.GameMode);
            Assert.Equal(expected.Id, actual.Id);
            Assert.True(expected.ImageUrl.Equals(actual.ImageUrl));
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Ranked, actual.Ranked);
        }

        [Fact]
        public void Season()
        {
            var data = SeasonData()[0];
            var expected = new Season
            {
                Active = data.isActive,
                EndDate = data.endDate,
                IconUrl = new Uri(data.iconUrl),
                Id = Guid.Parse(data.id),
                Name = data.name,
                PlaylistIds = new List<Guid>
                {
                    Guid.Parse(data.playlists.First().id)
                },
                StartDate = data.startDate,
            };

            var actual = Mapper().Map<Season>(data);
            Equal(expected, actual);
        }

        private static void Equal(Season expected, Season actual)
        {
            Assert.Equal(expected.Active, actual.Active);
            Assert.Equal(expected.EndDate, actual.EndDate);
            Assert.True(expected.IconUrl.Equals(actual.IconUrl));
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.PlaylistIds.First(), actual.PlaylistIds.First());
        }

        [Fact]
        public void SpartanRank()
        {
            var data = SpartanRankData()[0];
            var expected = new SpartanRank
            {
                Id = data.id,
                StartXp = data.startXp,
            };

            var actual = Mapper().Map<SpartanRank>(data);
            Equal(expected, actual);
        }

        private static void Equal(SpartanRank expected, SpartanRank actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.StartXp, actual.StartXp);
        }

        [Fact]
        public void TeamColor()
        {
            var data = TeamColorData()[0];
            var expected = new TeamColor
            {
                Color = data.color,
                Description = data.description,
                IconUrl = new Uri(data.iconUrl),
                Id = data.id,
                Name = data.name,
            };

            var actual = Mapper().Map<TeamColor>(data);
            Equal(expected, actual);
        }

        private static void Equal(TeamColor expected, TeamColor actual)
        {
            Assert.Equal(expected.Color, actual.Color);
            Assert.Equal(expected.Description, actual.Description);
            Assert.True(expected.IconUrl.Equals(actual.IconUrl));
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
        }

        [Fact]
        public void Vehicle()
        {
            var data = VehicleData()[0];
            var expected = new Vehicle
            {
                Description = data.description,
                Id = data.id,
                LargeIconImageUrl = new Uri(data.largeIconImageUrl),
                Name = data.name,
                SmallIconImageUrl = new Uri(data.smallIconImageUrl),
                UsableByPlayer = data.isUsableByPlayer,
            };

            var actual = Mapper().Map<Vehicle>(data);
            Equal(expected, actual);
        }

        private static void Equal(Vehicle expected, Vehicle actual)
        {
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.Id, actual.Id);
            Assert.True(expected.LargeIconImageUrl.Equals(actual.LargeIconImageUrl));
            Assert.Equal(expected.Name, actual.Name);
            Assert.True(expected.SmallIconImageUrl.Equals(actual.SmallIconImageUrl));
            Assert.Equal(expected.UsableByPlayer, actual.UsableByPlayer);
        }

        [Fact]
        public void Weapon()
        {
            var data = WeaponData()[0];
            var expected = new Weapon
            {
                Description = data.description,
                Id = data.id,
                LargeIconImageUrl = new Uri(data.largeIconImageUrl),
                Name = data.name,
                SmallIconImageUrl = new Uri(data.smallIconImageUrl),
                Type = EnumUtility.Parse<WeaponType>(data.type),
                UsableByPlayer = data.isUsableByPlayer,
            };

            var actual = Mapper().Map<Weapon>(data);
            Equal(expected, actual);
        }

        private static void Equal(Weapon expected, Weapon actual)
        {
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.Id, actual.Id);
            Assert.True(expected.LargeIconImageUrl.Equals(actual.LargeIconImageUrl));
            Assert.Equal(expected.Name, actual.Name);
            Assert.True(expected.SmallIconImageUrl.Equals(actual.SmallIconImageUrl));
            Assert.Equal(expected.Type, actual.Type);
            Assert.Equal(expected.UsableByPlayer, actual.UsableByPlayer);
        }

        #endregion

        #region Metadata Data

        private static IList<ApiMetadata.CsrDesignation> CsrDesignationData()
        {
            return new List<ApiMetadata.CsrDesignation>
            {
                new ApiMetadata.CsrDesignation
                {
                    bannerImageUrl = "http://csrDesignationBannerImageUrl",
                    id = 1,
                    name = "csrDesignationName",
                    tiers = CsrTierData().ToList(),
                },
            };
        }

        private static IList<ApiMetadata.CsrTier> CsrTierData()
        {
            return new List<ApiMetadata.CsrTier>
            {
                new ApiMetadata.CsrTier
                {
                    iconImageUrl = "http://csrTierIconImageUrl",
                    id = 11,
                },
            };
        }

        private static IList<ApiMetadata.FlexibleStat> FlexibleStatData()
        {
            return new List<ApiMetadata.FlexibleStat>
            {
                new ApiMetadata.FlexibleStat
                {
                    id = Guid.NewGuid().ToString(),
                    name = "flexibleStatName",
                    type = FlexibleStatType.Count.ToString(),
                },
            };
        }

        private static IList<ApiMetadata.GameBaseVariant> GameBaseVariantData()
        {
            return new List<ApiMetadata.GameBaseVariant>
            {
                new ApiMetadata.GameBaseVariant
                {
                    iconUrl = "http://gameBaseVariantIconUrl",
                    id = Guid.NewGuid().ToString(),
                    name = "gameBaseVariantName",
                    supportedGameModes = new List<string>
                    {
                        GameMode.Arena.ToString(),
                    },
                },
            };
        }

        private static IList<ApiMetadata.GameVariant> GameVariantData()
        {
            return new List<ApiMetadata.GameVariant>
            {
                new ApiMetadata.GameVariant
                {
                    description = "gameVariantDescription",
                    gameBaseVariantId = Guid.NewGuid().ToString(),
                    iconUrl = "http://gameVariantIconUrl",
                    id = Guid.NewGuid().ToString(),
                    name = "gameVariantName",
                },
            };
        }
        private static IList<ApiMetadata.Impulse> ImpulseData()
        {
            return new List<ApiMetadata.Impulse>
            {
                new ApiMetadata.Impulse
                {
                    id = 21,
                    internalName = "impulseName",
                },
            };
        }

        private static IList<ApiMetadata.Map> MapData()
        {
            return new List<ApiMetadata.Map>
            {
                new ApiMetadata.Map
                {
                    description = "mapDescription",
                    id = Guid.NewGuid().ToString(),
                    imageUrl = "http://mapImageUrl",
                    name = "mapName",
                    supportedGameModes = new List<string>
                    {
                        GameMode.Arena.ToString(),
                    },
                },
            };
        }

        private static IList<ApiMetadata.MapVariant> MapVariantData()
        {
            return new List<ApiMetadata.MapVariant>
            {
                new ApiMetadata.MapVariant
                {
                    description = "mapVariantDescription",
                    id = Guid.NewGuid().ToString(),
                    mapId = Guid.NewGuid().ToString(),
                    mapImageUrl = "http://mapVariantImageUrl",
                    name = "mapVariantName",
                },
            };
        }

        private static IList<ApiMetadata.MedalSpriteLocation> MedalSpriteLocationData()
        {
            return new List<ApiMetadata.MedalSpriteLocation>
            {
                new ApiMetadata.MedalSpriteLocation
                {
                    left = 31,
                    height = 32,
                    spriteHeight = 33,
                    spriteSheetUri = "http://spriteSheetUri",
                    spriteWidth = 34,
                    top = 35,
                    width = 36,
                },
            };
        }

        private static IList<ApiMetadata.Medal> MedalData()
        {
            return new List<ApiMetadata.Medal>
            {
                new ApiMetadata.Medal
                {
                    classification = MedalClassification.Spree.ToString(),
                    description = "medalDescription",
                    difficulty = 41,
                    id = 42,
                    name = "medalName",
                    spriteLocation = MedalSpriteLocationData()[0],
                },
            };
        }

        private static IList<ApiMetadata.Playlist> PlaylistData()
        {
            return new List<ApiMetadata.Playlist>
            {
                new ApiMetadata.Playlist
                {
                    description = "playlistDescription",
                    gameMode = GameMode.Arena.ToString(),
                    id = Guid.NewGuid().ToString(),
                    imageUrl = "http://playlistImageUrl",
                    isActive = true,
                    isRanked = true,
                    name = "playlistName",
                },
            };
        }

        private static IList<ApiMetadata.Season> SeasonData()
        {
            return new List<ApiMetadata.Season>
            {
                new ApiMetadata.Season
                {
                    endDate = new DateTime(2016, 2, 2),
                    iconUrl = "http://seasonIconUrl",
                    id = Guid.NewGuid().ToString(),
                    isActive = true,
                    name = "seasonName",
                    playlists = PlaylistData().ToList(),
                    startDate = new DateTime(2016, 1, 1),
                },
            };
        }

        private static IList<ApiMetadata.SpartanRank> SpartanRankData()
        {
            return new List<ApiMetadata.SpartanRank>
            {
                new ApiMetadata.SpartanRank
                {
                    id = 51,
                    startXp = 52,
                },
            };
        }

        private static IList<ApiMetadata.TeamColor> TeamColorData()
        {
            return new List<ApiMetadata.TeamColor>
            {
                new ApiMetadata.TeamColor
                {
                    color = "teamColor",
                    description = "teamColorDescription",
                    iconUrl = "http://teamColorIconUrl",
                    id = 61,
                    name = "teamColorName",
                },
            };
        }

        private static IList<ApiMetadata.Vehicle> VehicleData()
        {
            return new List<ApiMetadata.Vehicle>
            {
                new ApiMetadata.Vehicle
                {
                    description = "vehicleDescription",
                    id = 71,
                    largeIconImageUrl = "http://vehicleLargeIconImageUrl",
                    name = "vehicleName",
                    smallIconImageUrl = "http://vehicleSmallIconImageUrl",
                    isUsableByPlayer = true,
                },
            };
        }

        private static IList<ApiMetadata.Weapon> WeaponData()
        {
            return new List<ApiMetadata.Weapon>
            {
                new ApiMetadata.Weapon
                {
                    description = "weaponDescription",
                    id = 81,
                    largeIconImageUrl = "http://weaponLargeIconImageUrl",
                    name = "weaponName",
                    smallIconImageUrl = "http://weaponSmallIconImageUrl",
                    isUsableByPlayer = true,
                },
            };
        }

        #endregion

        #region Matches

        [Fact]
        public void Csr()
        {
            var data = MatchCsrData()[0];
            var expected = new CsrDto
            {
                CsrDesignationId = data.DesignationId,
                CsrDesignationTierId = data.Tier,
                PercentToNextTier = data.PercentToNextTier,
                Rank = data.Rank,
                Value = data.Csr,
            };

            var actual = Mapper().Map<CsrDto>(data);
            Equal(expected, actual);
        }

        private static void Equal(CsrDto expected, CsrDto actual)
        {
            Assert.Equal(expected.CsrDesignationId, actual.CsrDesignationId);
            Assert.Equal(expected.CsrDesignationTierId, actual.CsrDesignationTierId);
            Assert.Equal(expected.PercentToNextTier, actual.PercentToNextTier);
            Assert.Equal(expected.Rank, actual.Rank);
            Assert.Equal(expected.Value, actual.Value);
        }

        [Fact]
        public void WeaponStats()
        {
            var data = MatchWeaponData()[0];
            var expected = new WeaponStatsDto
            {
                DamageDealt = data.TotalDamageDealt,
                Headshots = data.TotalHeadshots,
                Kills = data.TotalKills,
                PossessionTime = XmlConvert.ToTimeSpan(data.TotalPossessionTime),
                ShotsFired = data.TotalShotsFired,
                ShotsLanded = data.TotalShotsLanded,
                WeaponId = data.WeaponId.StockId,
            };

            var actual = Mapper().Map<WeaponStatsDto>(data);
            Equal(expected, actual);
        }

        private static void Equal(WeaponStatsDto expected, WeaponStatsDto actual)
        {
            Assert.Equal(expected.DamageDealt, actual.DamageDealt);
            Assert.Equal(expected.Headshots, actual.Headshots);
            Assert.Equal(expected.Kills, actual.Kills);
            Assert.Equal(expected.PossessionTime, actual.PossessionTime);
            Assert.Equal(expected.ShotsFired, actual.ShotsFired);
            Assert.Equal(expected.ShotsLanded, actual.ShotsLanded);
            Assert.Equal(expected.WeaponId, actual.WeaponId);
        }

        [Fact]
        public void Player()
        {
            var data = MatchPlayerStatsData()[0];
            var expected = new PlayerDto
            {
                Assassinations = data.TotalAssassinations,
                Assists = data.TotalAssists,
                AvgLifeTime = XmlConvert.ToTimeSpan(data.AvgLifeTimeOfPlayer),
                CurrentCsr = new CsrDto
                {
                    CsrDesignationId = data.CurrentCsr.DesignationId,
                    CsrDesignationTierId = data.CurrentCsr.Tier,
                    PercentToNextTier = data.CurrentCsr.PercentToNextTier,
                    Rank = data.CurrentCsr.Rank,
                    Value = data.CurrentCsr.Csr,
                },
                Deaths = data.TotalDeaths,
                Dnf = data.DNF,
                GrenadeDamage = data.TotalGrenadeDamage,
                GrenadeKills = data.TotalGrenadeKills,
                GroundPoundDamage = data.TotalGroundPoundDamage,
                GroundPoundKills = data.TotalGroundPoundKills,
                Headshots = data.TotalHeadshots,
                Kills = data.TotalKills,
                MeleeDamage = data.TotalMeleeDamage,
                MeleeKills = data.TotalMeleeKills,
                Name = data.Player.Gamertag,
                PowerWeaponDamage = data.TotalPowerWeaponDamage,
                PowerWeaponGrabs = data.TotalPowerWeaponGrabs,
                PowerWeaponKills = data.TotalPowerWeaponKills,
                PowerWeaponPossessionTime = XmlConvert.ToTimeSpan(data.TotalPowerWeaponPossessionTime),
                PreviousCsr = new CsrDto
                {
                    CsrDesignationId = data.PreviousCsr.DesignationId,
                    CsrDesignationTierId = data.PreviousCsr.Tier,
                    PercentToNextTier = data.PreviousCsr.PercentToNextTier,
                    Rank = data.PreviousCsr.Rank,
                    Value = data.PreviousCsr.Csr,
                },
                Rank = data.Rank,
                ShotsFired = data.TotalShotsFired,
                ShotsLanded = data.TotalShotsLanded,
                ShoulderBashDamage = data.TotalShoulderBashDamage,
                ShoulderBashKills = data.TotalShoulderBashKills,
                Team = data.TeamId,
                WeaponDamage = data.TotalWeaponDamage,
                WeaponsStats = new List<WeaponStatsDto>
                {
                    new WeaponStatsDto
                    {
                        DamageDealt = data.WeaponStats.First().TotalDamageDealt,
                        Headshots = data.WeaponStats.First().TotalHeadshots,
                        Kills = data.WeaponStats.First().TotalKills,
                        PossessionTime = XmlConvert.ToTimeSpan(data.WeaponStats.First().TotalPossessionTime),
                        ShotsFired = data.WeaponStats.First().TotalShotsFired,
                        ShotsLanded = data.WeaponStats.First().TotalShotsLanded,
                        WeaponId = data.WeaponStats.First().WeaponId.StockId,
                    }
                }
            };

            var actual = Mapper().Map<PlayerDto>(data);
            Equal(expected, actual);
        }

        private static void Equal(PlayerDto expected, PlayerDto actual)
        {
            Assert.Equal(expected.Assassinations, actual.Assassinations);
            Assert.Equal(expected.Assists, actual.Assists);
            Assert.Equal(expected.AvgLifeTime, actual.AvgLifeTime);
            Equal(expected.CurrentCsr, actual.CurrentCsr);
            Assert.Equal(expected.Deaths, actual.Deaths);
            Assert.Equal(expected.Dnf, actual.Dnf);
            Assert.Equal(expected.GrenadeDamage, actual.GrenadeDamage);
            Assert.Equal(expected.GrenadeKills, actual.GrenadeKills);
            Assert.Equal(expected.GroundPoundDamage, actual.GroundPoundDamage);
            Assert.Equal(expected.GroundPoundKills, actual.GroundPoundKills);
            Assert.Equal(expected.Headshots, actual.Headshots);
            Assert.Equal(expected.Kills, actual.Kills);
            Assert.Equal(expected.MeleeDamage, actual.MeleeDamage);
            Assert.Equal(expected.MeleeKills, actual.MeleeKills);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.PowerWeaponDamage, actual.PowerWeaponDamage);
            Assert.Equal(expected.PowerWeaponGrabs, actual.PowerWeaponGrabs);
            Assert.Equal(expected.PowerWeaponKills, actual.PowerWeaponKills);
            Assert.Equal(expected.PowerWeaponPossessionTime, actual.PowerWeaponPossessionTime);
            Equal(expected.PreviousCsr, actual.PreviousCsr);
            Assert.Equal(expected.Rank, actual.Rank);
            Assert.Equal(expected.ShotsFired, actual.ShotsFired);
            Assert.Equal(expected.ShotsLanded, actual.ShotsLanded);
            Assert.Equal(expected.ShoulderBashDamage, actual.ShoulderBashDamage);
            Assert.Equal(expected.ShoulderBashKills, actual.ShoulderBashKills);
            Assert.Equal(expected.Team, actual.Team);
            Assert.Equal(expected.WeaponDamage, actual.WeaponDamage);
            Equal(expected.WeaponsStats.First(), actual.WeaponsStats.First());
        }

        [Fact]
        public void Team1()
        {
            var data = MatchTeamStatsData()[0];
            var expected = new TeamDto
            {
                Rank = data.Rank,
                Score = data.Score,
                TeamId = data.TeamId,
            };

            var actual = Mapper().Map<TeamDto>(data);
            Equal(expected, actual);
        }

        [Fact]
        public void Team2()
        {
            var data = MatchTeamStatsData()[1];
            var expected = new TeamDto
            {
                Rank = data.Rank,
                Score = data.Score,
                TeamId = data.TeamId,
            };

            var actual = Mapper().Map<TeamDto>(data);
            Equal(expected, actual);
        }

        private static void Equal(TeamDto expected, TeamDto actual)
        {
            Assert.Equal(expected.Rank, actual.Rank);
            Assert.Equal(expected.Score, actual.Score);
            Assert.Equal(expected.TeamId, actual.TeamId);
        }

        [Fact]
        public void MatchFromPlayerMatch()
        {
            var data = PlayerMatchData()[0];
            var expected = new MatchDto
            {
                Completed = data.MatchCompletedDate.ISO8601Date,
                Duration = XmlConvert.ToTimeSpan(data.MatchDuration),
                GameBaseVariantId = Guid.Parse(data.GameBaseVariantId),
                GameMode = EnumUtility.Parse<GameMode>(data.Id.GameMode),
                GameVariantId = Guid.Parse(data.GameVariant.ResourceId),
                Id = Guid.Parse(data.Id.MatchId),
                MapId = Guid.Parse(data.MapId),
                MapVariantId = Guid.Parse(data.MapVariant.ResourceId),
                PlaylistId = Guid.Parse(data.HopperId),
                SeasonId = Guid.Parse(data.SeasonId),
                TeamGame = data.IsTeamGame,
            };

            var actual = Mapper().Map<MatchDto>(data);
            Assert.Equal(expected.Completed, actual.Completed);
            Assert.Equal(expected.Duration, actual.Duration);
            Assert.Equal(expected.GameBaseVariantId, actual.GameBaseVariantId);
            Assert.Equal(expected.GameMode, actual.GameMode);
            Assert.Equal(expected.GameVariantId, actual.GameVariantId);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.MapId, actual.MapId);
            Assert.Equal(expected.MapVariantId, actual.MapVariantId);
            Assert.Equal(expected.PlaylistId, actual.PlaylistId);
            Assert.Equal(expected.SeasonId, actual.SeasonId);
            Assert.Equal(expected.TeamGame, actual.TeamGame);
        }

        [Fact]
        public void MatchFromMatchReport()
        {
            var data = MatchReportData()[0];
            var expected = new MatchDto
            {
                Players = new List<PlayerDto>
                {
                    new PlayerDto
                    {
                        Assassinations = data.PlayerStats.First().TotalAssassinations,
                        Assists = data.PlayerStats.First().TotalAssists,
                        AvgLifeTime = XmlConvert.ToTimeSpan(data.PlayerStats.First().AvgLifeTimeOfPlayer),
                        CurrentCsr = new CsrDto
                        {
                            CsrDesignationId = data.PlayerStats.First().CurrentCsr.DesignationId,
                            CsrDesignationTierId = data.PlayerStats.First().CurrentCsr.Tier,
                            PercentToNextTier = data.PlayerStats.First().CurrentCsr.PercentToNextTier,
                            Rank = data.PlayerStats.First().CurrentCsr.Rank,
                            Value = data.PlayerStats.First().CurrentCsr.Csr,
                        },
                        Deaths = data.PlayerStats.First().TotalDeaths,
                        Dnf = data.PlayerStats.First().DNF,
                        GrenadeDamage = data.PlayerStats.First().TotalGrenadeDamage,
                        GrenadeKills = data.PlayerStats.First().TotalGrenadeKills,
                        GroundPoundDamage = data.PlayerStats.First().TotalGroundPoundDamage,
                        GroundPoundKills = data.PlayerStats.First().TotalGroundPoundKills,
                        Headshots = data.PlayerStats.First().TotalHeadshots,
                        Kills = data.PlayerStats.First().TotalKills,
                        MeleeDamage = data.PlayerStats.First().TotalMeleeDamage,
                        MeleeKills = data.PlayerStats.First().TotalMeleeKills,
                        Name = data.PlayerStats.First().Player.Gamertag,
                        PowerWeaponDamage = data.PlayerStats.First().TotalPowerWeaponDamage,
                        PowerWeaponGrabs = data.PlayerStats.First().TotalPowerWeaponGrabs,
                        PowerWeaponKills = data.PlayerStats.First().TotalPowerWeaponKills,
                        PowerWeaponPossessionTime = XmlConvert.ToTimeSpan(data.PlayerStats.First().TotalPowerWeaponPossessionTime),
                        PreviousCsr = new CsrDto
                        {
                            CsrDesignationId = data.PlayerStats.First().PreviousCsr.DesignationId,
                            CsrDesignationTierId = data.PlayerStats.First().PreviousCsr.Tier,
                            PercentToNextTier = data.PlayerStats.First().PreviousCsr.PercentToNextTier,
                            Rank = data.PlayerStats.First().PreviousCsr.Rank,
                            Value = data.PlayerStats.First().PreviousCsr.Csr,
                        },
                        Rank = data.PlayerStats.First().Rank,
                        ShotsFired = data.PlayerStats.First().TotalShotsFired,
                        ShotsLanded = data.PlayerStats.First().TotalShotsLanded,
                        ShoulderBashDamage = data.PlayerStats.First().TotalShoulderBashDamage,
                        ShoulderBashKills = data.PlayerStats.First().TotalShoulderBashKills,
                        Team = data.PlayerStats.First().TeamId,
                        WeaponDamage = data.PlayerStats.First().TotalWeaponDamage,
                        WeaponsStats = new List<WeaponStatsDto>
                        {
                            new WeaponStatsDto
                            {
                                DamageDealt = data.PlayerStats.First().WeaponStats.First().TotalDamageDealt,
                                Headshots = data.PlayerStats.First().WeaponStats.First().TotalHeadshots,
                                Kills = data.PlayerStats.First().WeaponStats.First().TotalKills,
                                PossessionTime = XmlConvert.ToTimeSpan(data.PlayerStats.First().WeaponStats.First().TotalPossessionTime),
                                ShotsFired = data.PlayerStats.First().WeaponStats.First().TotalShotsFired,
                                ShotsLanded = data.PlayerStats.First().WeaponStats.First().TotalShotsLanded,
                                WeaponId = data.PlayerStats.First().WeaponStats.First().WeaponId.StockId,
                            }
                        }
                    }
                },
                Teams = new List<TeamDto>
                {
                    new TeamDto
                    {
                        Rank = data.TeamStats[0].Rank,
                        Score = data.TeamStats[0].Score,
                        TeamId = data.TeamStats[0].TeamId,
                    },
                    new TeamDto
                    {
                        Rank = data.TeamStats[1].Rank,
                        Score = data.TeamStats[1].Score,
                        TeamId = data.TeamStats[1].TeamId,
                    },
                },
            };

            var actual = Mapper().Map<MatchDto>(data);
            Equal(expected.Players.First(), actual.Players.First());
            Equal(expected.Teams.First(), actual.Teams.First());
            Equal(expected.Teams.ElementAt(1), actual.Teams.ElementAt(1));
        }

        [Fact]
        public void Match()
        {
            var mapper = Mapper();
            var data1 = PlayerMatchData()[0];
            var data2 = MatchReportData()[0];
            var expected = new MatchDto
            {
                Completed = data1.MatchCompletedDate.ISO8601Date,
                Duration = XmlConvert.ToTimeSpan(data1.MatchDuration),
                GameBaseVariantId = Guid.Parse(data1.GameBaseVariantId),
                GameMode = EnumUtility.Parse<GameMode>(data1.Id.GameMode),
                GameVariantId = Guid.Parse(data1.GameVariant.ResourceId),
                Id = Guid.Parse(data1.Id.MatchId),
                MapId = Guid.Parse(data1.MapId),
                MapVariantId = Guid.Parse(data1.MapVariant.ResourceId),
                PlaylistId = Guid.Parse(data1.HopperId),
                SeasonId = Guid.Parse(data1.SeasonId),
                TeamGame = data1.IsTeamGame,

                Players = new List<PlayerDto>
                {
                    new PlayerDto
                    {
                        Assassinations = data2.PlayerStats.First().TotalAssassinations,
                        Assists = data2.PlayerStats.First().TotalAssists,
                        AvgLifeTime = XmlConvert.ToTimeSpan(data2.PlayerStats.First().AvgLifeTimeOfPlayer),
                        CurrentCsr = new CsrDto
                        {
                            CsrDesignationId = data2.PlayerStats.First().CurrentCsr.DesignationId,
                            CsrDesignationTierId = data2.PlayerStats.First().CurrentCsr.Tier,
                            PercentToNextTier = data2.PlayerStats.First().CurrentCsr.PercentToNextTier,
                            Rank = data2.PlayerStats.First().CurrentCsr.Rank,
                            Value = data2.PlayerStats.First().CurrentCsr.Csr,
                        },
                        Deaths = data2.PlayerStats.First().TotalDeaths,
                        Dnf = data2.PlayerStats.First().DNF,
                        GrenadeDamage = data2.PlayerStats.First().TotalGrenadeDamage,
                        GrenadeKills = data2.PlayerStats.First().TotalGrenadeKills,
                        GroundPoundDamage = data2.PlayerStats.First().TotalGroundPoundDamage,
                        GroundPoundKills = data2.PlayerStats.First().TotalGroundPoundKills,
                        Headshots = data2.PlayerStats.First().TotalHeadshots,
                        Kills = data2.PlayerStats.First().TotalKills,
                        MeleeDamage = data2.PlayerStats.First().TotalMeleeDamage,
                        MeleeKills = data2.PlayerStats.First().TotalMeleeKills,
                        Name = data2.PlayerStats.First().Player.Gamertag,
                        PowerWeaponDamage = data2.PlayerStats.First().TotalPowerWeaponDamage,
                        PowerWeaponGrabs = data2.PlayerStats.First().TotalPowerWeaponGrabs,
                        PowerWeaponKills = data2.PlayerStats.First().TotalPowerWeaponKills,
                        PowerWeaponPossessionTime = XmlConvert.ToTimeSpan(data2.PlayerStats.First().TotalPowerWeaponPossessionTime),
                        PreviousCsr = new CsrDto
                        {
                            CsrDesignationId = data2.PlayerStats.First().PreviousCsr.DesignationId,
                            CsrDesignationTierId = data2.PlayerStats.First().PreviousCsr.Tier,
                            PercentToNextTier = data2.PlayerStats.First().PreviousCsr.PercentToNextTier,
                            Rank = data2.PlayerStats.First().PreviousCsr.Rank,
                            Value = data2.PlayerStats.First().PreviousCsr.Csr,
                        },
                        Rank = data2.PlayerStats.First().Rank,
                        ShotsFired = data2.PlayerStats.First().TotalShotsFired,
                        ShotsLanded = data2.PlayerStats.First().TotalShotsLanded,
                        ShoulderBashDamage = data2.PlayerStats.First().TotalShoulderBashDamage,
                        ShoulderBashKills = data2.PlayerStats.First().TotalShoulderBashKills,
                        Team = data2.PlayerStats.First().TeamId,
                        WeaponDamage = data2.PlayerStats.First().TotalWeaponDamage,
                        WeaponsStats = new List<WeaponStatsDto>
                        {
                            new WeaponStatsDto
                            {
                                DamageDealt = data2.PlayerStats.First().WeaponStats.First().TotalDamageDealt,
                                Headshots = data2.PlayerStats.First().WeaponStats.First().TotalHeadshots,
                                Kills = data2.PlayerStats.First().WeaponStats.First().TotalKills,
                                PossessionTime = XmlConvert.ToTimeSpan(data2.PlayerStats.First().WeaponStats.First().TotalPossessionTime),
                                ShotsFired = data2.PlayerStats.First().WeaponStats.First().TotalShotsFired,
                                ShotsLanded = data2.PlayerStats.First().WeaponStats.First().TotalShotsLanded,
                                WeaponId = data2.PlayerStats.First().WeaponStats.First().WeaponId.StockId,
                            }
                        }
                    },
                },
                Teams = new List<TeamDto>
                {
                    new TeamDto
                    {
                        Rank = data2.TeamStats[0].Rank,
                        Score = data2.TeamStats[0].Score,
                        TeamId = data2.TeamStats[0].TeamId,
                    },
                    new TeamDto
                    {
                        Rank = data2.TeamStats[1].Rank,
                        Score = data2.TeamStats[1].Score,
                        TeamId = data2.TeamStats[1].TeamId,
                    },
                },
            };

            var actual = mapper.Map<MatchDto>(data1)
                               .Map(data2, mapper);
            Equal(expected, actual);
        }

        private static void Equal(MatchDto expected, MatchDto actual)
        {
            Assert.Equal(expected.Completed, actual.Completed);
            Assert.Equal(expected.Duration, actual.Duration);
            Assert.Equal(expected.GameBaseVariantId, actual.GameBaseVariantId);
            Assert.Equal(expected.GameMode, actual.GameMode);
            Assert.Equal(expected.GameVariantId, actual.GameVariantId);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.MapId, actual.MapId);
            Assert.Equal(expected.MapVariantId, actual.MapVariantId);
            Assert.Equal(expected.PlaylistId, actual.PlaylistId);
            Assert.Equal(expected.SeasonId, actual.SeasonId);
            Assert.Equal(expected.TeamGame, actual.TeamGame);

            Equal(expected.Players.First(), actual.Players.First());
            Equal(expected.Teams.First(), actual.Teams.First());
            Equal(expected.Teams.ElementAt(1), actual.Teams.ElementAt(1));
        }

        #endregion

        #region Match Data

        private static IList<ApiModels.MatchCsr> MatchCsrData()
        {
            return new List<ApiModels.MatchCsr>
            {
                new ApiModels.MatchCsr
                {
                    Csr = 30,
                    DesignationId = 31,
                    PercentToNextTier = 32,
                    Rank = 33,
                    Tier = 34,
                },
                new ApiModels.MatchCsr
                {
                    Csr = 35,
                    DesignationId = 36,
                    PercentToNextTier = 37,
                    Rank = 38,
                    Tier = 39,
                },
            };
        }

        private static IList<ApiModels.MatchWeapon> MatchWeaponData()
        {
            return new List<ApiModels.MatchWeapon>
            {
                new ApiModels.MatchWeapon
                {
                    TotalDamageDealt = 40,
                    TotalHeadshots = 41,
                    TotalKills = 42,
                    TotalPossessionTime = "PT43S",
                    TotalShotsFired = 44,
                    TotalShotsLanded = 45,
                    WeaponId = new ApiModels.MatchWeaponId
                    {
                        StockId = 46,
                    },
                },
            };
        }

        private static IList<ApiModels.MatchPlayerStats> MatchPlayerStatsData()
        {
            return new List<ApiModels.MatchPlayerStats>
            {
                new ApiModels.MatchPlayerStats
                {
                    AvgLifeTimeOfPlayer = "PT1M10S",
                    CurrentCsr = MatchCsrData()[0],
                    DNF = true,
                    Player = new ApiModels.MatchReportPlayer
                    {
                        Gamertag = "playerName",
                    },
                    PreviousCsr = MatchCsrData()[1],
                    Rank = 1000,
                    TeamId = 100,
                    TotalAssassinations = 101,
                    TotalAssists = 102,
                    TotalDeaths = 103,
                    TotalGrenadeDamage = 104,
                    TotalGrenadeKills = 105,
                    TotalGroundPoundDamage = 106,
                    TotalGroundPoundKills = 107,
                    TotalHeadshots = 108,
                    TotalKills = 109,
                    TotalMeleeDamage = 110,
                    TotalMeleeKills = 111,
                    TotalPowerWeaponDamage = 112,
                    TotalPowerWeaponGrabs = 113,
                    TotalPowerWeaponKills = 114,
                    TotalPowerWeaponPossessionTime = "PT1M55S",
                    TotalShotsFired = 116,
                    TotalShotsLanded = 117,
                    TotalShoulderBashDamage = 118,
                    TotalShoulderBashKills = 119,
                    TotalWeaponDamage = 120,
                    WeaponStats = MatchWeaponData().ToList(),
                },
            };
        }

        private static IList<ApiModels.MatchTeamStats> MatchTeamStatsData()
        {
            return new List<ApiModels.MatchTeamStats>
            {
                new ApiModels.MatchTeamStats
                {
                    Rank = 2,
                    Score = 40,
                    TeamId = 1,
                },
                new ApiModels.MatchTeamStats
                {
                    Rank = 1,
                    Score = 50,
                    TeamId = 2,
                },
            };
        }

        private static IList<ApiModels.PlayerMatch> PlayerMatchData()
        {
            return new List<ApiModels.PlayerMatch>
            {
                new ApiModels.PlayerMatch
                {
                    GameBaseVariantId = Guid.NewGuid().ToString(),
                    GameVariant = new ApiModels.GameVariantId
                    {
                        ResourceId = Guid.NewGuid().ToString(),
                    },
                    HopperId = Guid.NewGuid().ToString(),
                    Id = new ApiModels.MatchIdClass
                    {
                        GameMode = (int) GameMode.Arena,
                        MatchId = Guid.NewGuid().ToString(),
                    },
                    IsTeamGame = true,
                    MapId = Guid.NewGuid().ToString(),
                    MapVariant = new ApiModels.MapVariantId
                    {
                        ResourceId = Guid.NewGuid().ToString(),
                    },
                    MatchCompletedDate = new ApiModels.Iso8601Date
                    {
                        ISO8601Date = new DateTime(2016, 3, 3),
                    },
                    MatchDuration = "PT1M40S",
                    SeasonId = Guid.NewGuid().ToString(),
                },
            };
        }

        private static IList<ApiModels.MatchReport> MatchReportData()
        {
            return new List<ApiModels.MatchReport>
            {
                new ApiModels.MatchReport
                {
                    PlayerStats = MatchPlayerStatsData().ToList(),
                    TeamStats = MatchTeamStatsData().ToList(),
                },
            };
        }

        #endregion
    }
}
