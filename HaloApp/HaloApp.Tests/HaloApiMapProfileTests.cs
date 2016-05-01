using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using HaloApp.ApiClient;
using Xunit;
using System.Threading.Tasks;
using MongoDB.Driver;
using HaloApp.Domain.Services;
using HaloApp.Data;
using AutoMapper;
using ApiModels = HaloApp.ApiClient.Models;
using ApiMetadata = HaloApp.ApiClient.Models.Metadata;
using DomainMetadata = HaloApp.Domain.Models.Metadata;
using DomainModels = HaloApp.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using HaloApp.Domain.Enums;

namespace HaloApp.Tests
{
    [ExcludeFromCodeCoverage]
    public class HaloApiMapProfileTests
    {
        public HaloApiMapProfileTests()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<HaloApiMapProfile>();
            });
        }

        [Fact]
        public void ValidConfiguration()
        {
            Mapper.AssertConfigurationIsValid();
        }

        #region Type Conversion

        [Fact]
        public void UriTypeConversion()
        {
            string source = "http://uri";
            var uri = Mapper.Map<Uri>(source);

            Assert.NotNull(uri);
            Assert.IsType<Uri>(uri);
            Assert.True(uri.Equals(new Uri(source)));
        }

        [Fact]
        public void UriTypeConversion_Null()
        {
            string source = null;
            var uri = Mapper.Map<Uri>(source);

            Assert.Null(uri);
        }

        [Fact]
        public void GuidTypeConversion()
        {
            var source = Guid.NewGuid();
            var guid = Mapper.Map<Guid>(source.ToString());

            Assert.NotNull(guid);
            Assert.IsType<Guid>(guid);
            Assert.Equal(source, guid);
        }

        [Fact]
        public void GuidTypeConversion_Null()
        {
            string source = null;
            var guid = Mapper.Map<Guid>(source);

            Assert.Equal(Guid.Empty, guid);
        }

        [Fact]
        public void TimeSpanTypeConversion()
        {
            string source = "PT1M30S";
            var timeSpan = Mapper.Map<TimeSpan>(source);

            Assert.NotNull(timeSpan);
            Assert.IsType<TimeSpan>(timeSpan);
            Assert.Equal(TimeSpan.FromSeconds(90), timeSpan);
        }

        [Fact]
        public void TimeSpanTypeConversion_Null()
        {
            string source = null;
            var timeSpan = Mapper.Map<TimeSpan>(source);

            Assert.Equal(TimeSpan.Zero, timeSpan);
        }

        #endregion

        #region Metatdata

        [Fact]
        public void CsrTier()
        {
            var csrTier = Mapper.Map<DomainMetadata.CsrTier>(CsrTierData()[0]);

            Assert.Equal(new Uri("http://csrTierIconImageUrl"), csrTier.IconImageUrl);
            Assert.Equal(2, csrTier.Id);
        }

        [Fact]
        public void CsrDesignation()
        {
            var csrDesignation = Mapper.Map<DomainMetadata.CsrDesignation>(CsrDesignationData()[0]);

            Assert.Equal(new Uri("http://csrDesignationBannerImageUrl"), csrDesignation.BannerImageUrl);
            Assert.Equal(1, csrDesignation.Id);
            Assert.Equal("csrDesignationName", csrDesignation.Name);
            var csrTier = csrDesignation.Tiers[0];
            Assert.Equal(new Uri("http://csrTierIconImageUrl"), csrTier.IconImageUrl);
            Assert.Equal(2, csrTier.Id);
        }

        [Fact]
        public void FlexibleStat()
        {
            var flexibleStat = Mapper.Map<DomainMetadata.FlexibleStat>(FlexibleStatData()[0]);

            Assert.True(flexibleStat.Id.Equals(FlexibleStatGuid));
            Assert.Equal("flexibleStatName", flexibleStat.Name);
            Assert.Equal(FlexibleStatType.Count, flexibleStat.Type);
        }

        [Fact]
        public void GameBaseVariant()
        {
            var gameBaseVariant = Mapper.Map<DomainMetadata.GameBaseVariant>(GameBaseVariantData()[0]);

            Assert.True(gameBaseVariant.IconUrl.Equals(new Uri("http://gameBaseVariantIconUrl")));
            Assert.True(gameBaseVariant.Id.Equals(GameBaseVariantGuid));
            Assert.Equal("gameBaseVariantName", gameBaseVariant.Name);
            Assert.Equal(1, gameBaseVariant.SupportedGameModes.Count);
            Assert.Equal(GameMode.Arena, gameBaseVariant.SupportedGameModes[0]);
        }

        [Fact]
        public void GameVariant()
        {
            var gameVariant = Mapper.Map<DomainMetadata.GameVariant>(GameVariantData()[0]);

            Assert.Equal("gameVariantDescription", gameVariant.Description);
            Assert.True(gameVariant.IconUrl.Equals(new Uri("http://gameVariantIconUrl")));
            Assert.True(gameVariant.GameBaseVariantId.Equals(GameBaseVariantGuid));
            Assert.True(gameVariant.Id.Equals(GameVariantGuid));
            Assert.Equal("gameVariantName", gameVariant.Name);
        }

        [Fact]
        public void Map()
        {
            var map = Mapper.Map<DomainMetadata.Map>(MapData()[0]);

            Assert.Equal("mapDescription", map.Description);
            Assert.True(map.ImageUrl.Equals(new Uri("http://mapImageUrl")));
            Assert.True(map.Id.Equals(MapGuid));
            Assert.Equal("mapName", map.Name);
            Assert.Equal(1, map.SupportedGameModes.Count);
            Assert.Equal(GameMode.Arena, map.SupportedGameModes[0]);
        }

        [Fact]
        public void MapVariant()
        {
            var mapVariant = Mapper.Map<DomainMetadata.MapVariant>(MapVariantData()[0]);

            Assert.Equal("mapVariantDescription", mapVariant.Description);
            Assert.True(mapVariant.Id.Equals(MapVariantGuid));
            Assert.True(mapVariant.MapId.Equals(MapGuid));
            Assert.True(mapVariant.MapImageUrl.Equals(new Uri("http://mapVariantImageUrl")));
            Assert.Equal("mapVariantName", mapVariant.Name);
        }

        [Fact]
        public void MedalSpriteLocation()
        {
            var medalSpriteLocation = Mapper.Map<DomainMetadata.MedalSpriteLocation>(MedalSpriteLocationData()[0]);

            Assert.Equal(3, medalSpriteLocation.Left);
            Assert.Equal(4, medalSpriteLocation.Height);
            Assert.Equal(5, medalSpriteLocation.SpriteHeight);
            Assert.True(medalSpriteLocation.SpriteSheetUri.Equals(new Uri("http://spriteSheetUri")));
            Assert.Equal(6, medalSpriteLocation.SpriteWidth);
            Assert.Equal(7, medalSpriteLocation.Top);
            Assert.Equal(8, medalSpriteLocation.Width);
        }

        [Fact]
        public void Medal()
        {
            var medal = Mapper.Map<DomainMetadata.Medal>(MedalData()[0]);

            Assert.Equal(MedalClassification.Spree, medal.Classification);
            Assert.Equal("medalDescription", medal.Description);
            Assert.Equal(9, medal.Difficulty);
            Assert.Equal(10, medal.Id);
            Assert.Equal("medalName", medal.Name);

            var medalSpriteLocation = medal.SpriteLocation;
            Assert.Equal(3, medalSpriteLocation.Left);
            Assert.Equal(4, medalSpriteLocation.Height);
            Assert.Equal(5, medalSpriteLocation.SpriteHeight);
            Assert.True(medalSpriteLocation.SpriteSheetUri.Equals(new Uri("http://spriteSheetUri")));
            Assert.Equal(6, medalSpriteLocation.SpriteWidth);
            Assert.Equal(7, medalSpriteLocation.Top);
            Assert.Equal(8, medalSpriteLocation.Width);
        }

        [Fact]
        public void Playlist()
        {
            var playlist = Mapper.Map<DomainMetadata.Playlist>(PlaylistData()[0]);

            Assert.Equal("playlistDescription", playlist.Description);
            Assert.Equal(GameMode.Arena, playlist.GameMode);
            Assert.True(playlist.Id.Equals(PlaylistGuid));
            Assert.True(playlist.ImageUrl.Equals(new Uri("http://playlistImageUrl")));
            Assert.True(playlist.Active);
            Assert.True(playlist.Ranked);
            Assert.Equal("playlistName", playlist.Name);
        }

        [Fact]
        public void Season()
        {
            var season = Mapper.Map<DomainMetadata.Season>(SeasonData()[0]);

            Assert.True(season.EndDate.Equals(new DateTime(2016, 2, 2)));
            Assert.True(season.IconUrl.Equals(new Uri("http://seasonIconUrl")));
            Assert.True(season.Id.Equals(SeasonGuid));
            Assert.True(season.Active);
            Assert.Equal("seasonName", season.Name);
            Assert.True(season.StartDate.Equals(new DateTime(2016, 1, 1)));

            var playlist = season.PlaylistIds[0];
            Assert.True(playlist.Equals(PlaylistGuid));
        }

        [Fact]
        public void SpartanRank()
        {
            var spartanRank = Mapper.Map<DomainMetadata.SpartanRank>(SpartanRankData()[0]);

            Assert.Equal(11, spartanRank.Id);
            Assert.Equal(12, spartanRank.StartXp);
        }

        [Fact]
        public void TeamColor()
        {
            var teamColor = Mapper.Map<DomainMetadata.TeamColor>(TeamColorData()[0]);

            Assert.Equal("teamColor", teamColor.Color);
            Assert.Equal("teamColorDescription", teamColor.Description);
            Assert.True(teamColor.IconUrl.Equals(new Uri("http://teamColorIconUrl")));
            Assert.Equal(13, teamColor.Id);
            Assert.Equal("teamColorName", teamColor.Name);
        }

        [Fact]
        public void Vehicle()
        {
            var vehicle = Mapper.Map<DomainMetadata.Vehicle>(VehicleData()[0]);

            Assert.Equal("vehicleDescription", vehicle.Description);
            Assert.Equal(13, vehicle.Id);
            Assert.True(vehicle.LargeIconImageUrl.Equals(new Uri("http://vehicleLargeIconImageUrl")));
            Assert.Equal("vehicleName", vehicle.Name);
            Assert.True(vehicle.SmallIconImageUrl.Equals(new Uri("http://vehicleSmallIconImageUrl")));
            Assert.True(vehicle.UsableByPlayer);
        }

        [Fact]
        public void Weapon()
        {
            var weapon = Mapper.Map<DomainMetadata.Weapon>(WeaponData()[0]);

            Assert.Equal("weaponDescription", weapon.Description);
            Assert.Equal(14, weapon.Id);
            Assert.True(weapon.LargeIconImageUrl.Equals(new Uri("http://weaponLargeIconImageUrl")));
            Assert.Equal("weaponName", weapon.Name);
            Assert.True(weapon.SmallIconImageUrl.Equals(new Uri("http://weaponSmallIconImageUrl")));
            Assert.True(weapon.UsableByPlayer);
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
                    id = 2,
                },
            };
        }

        private static readonly Guid FlexibleStatGuid = Guid.NewGuid();
        private static IList<ApiMetadata.FlexibleStat> FlexibleStatData()
        {
            return new List<ApiMetadata.FlexibleStat>
            {
                new ApiMetadata.FlexibleStat
                {
                    id = FlexibleStatGuid.ToString(),
                    name = "flexibleStatName",
                    type = FlexibleStatType.Count.ToString(),
                },
            };
        }

        private static readonly Guid GameBaseVariantGuid = Guid.NewGuid();
        private static IList<ApiMetadata.GameBaseVariant> GameBaseVariantData()
        {
            return new List<ApiMetadata.GameBaseVariant>
            {
                new ApiMetadata.GameBaseVariant
                {
                    iconUrl = "http://gameBaseVariantIconUrl",
                    id = GameBaseVariantGuid.ToString(),
                    name = "gameBaseVariantName",
                    supportedGameModes = new List<string>
                    {
                        GameMode.Arena.ToString(),
                    },
                },
            };
        }

        private static readonly Guid GameVariantGuid = Guid.NewGuid();
        private static IList<ApiMetadata.GameVariant> GameVariantData()
        {
            return new List<ApiMetadata.GameVariant>
            {
                new ApiMetadata.GameVariant
                {
                    description = "gameVariantDescription",
                    gameBaseVariantId = GameBaseVariantGuid.ToString(),
                    iconUrl = "http://gameVariantIconUrl",
                    id = GameVariantGuid.ToString(),
                    name = "gameVariantName",
                },
            };
        }

        private static readonly Guid MapGuid = Guid.NewGuid();
        private static IList<ApiMetadata.Map> MapData()
        {
            return new List<ApiMetadata.Map>
            {
                new ApiMetadata.Map
                {
                    description = "mapDescription",
                    id = MapGuid.ToString(),
                    imageUrl = "http://mapImageUrl",
                    name = "mapName",
                    supportedGameModes = new List<string>
                    {
                        GameMode.Arena.ToString(),
                    },
                },
            };
        }

        private static readonly Guid MapVariantGuid = Guid.NewGuid();
        private static IList<ApiMetadata.MapVariant> MapVariantData()
        {
            return new List<ApiMetadata.MapVariant>
            {
                new ApiMetadata.MapVariant
                {
                    description = "mapVariantDescription",
                    id = MapVariantGuid.ToString(),
                    mapId = MapGuid.ToString(),
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
                    left = 3,
                    height = 4,
                    spriteHeight = 5,
                    spriteSheetUri = "http://spriteSheetUri",
                    spriteWidth = 6,
                    top = 7,
                    width = 8,
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
                    difficulty = 9,
                    id = 10,
                    name = "medalName",
                    spriteLocation = MedalSpriteLocationData()[0],
                },
            };
        }

        private static readonly Guid PlaylistGuid = Guid.NewGuid();
        private static IList<ApiMetadata.Playlist> PlaylistData()
        {
            return new List<ApiMetadata.Playlist>
            {
                new ApiMetadata.Playlist
                {
                    description = "playlistDescription",
                    gameMode = GameMode.Arena.ToString(),
                    id = PlaylistGuid.ToString(),
                    imageUrl = "http://playlistImageUrl",
                    isActive = true,
                    isRanked = true,
                    name = "playlistName",
                },
            };
        }

        private static readonly Guid SeasonGuid = Guid.NewGuid();
        private static IList<ApiMetadata.Season> SeasonData()
        {
            return new List<ApiMetadata.Season>
            {
                new ApiMetadata.Season
                {
                    endDate = new DateTime(2016, 2, 2),
                    iconUrl = "http://seasonIconUrl",
                    id = SeasonGuid.ToString(),
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
                    id = 11,
                    startXp = 12,
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
                    id = 13,
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
                    id = 13,
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
                    id = 14,
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
            var csr = Mapper.Map<DomainModels.Csr>(MatchCsrData()[0]);

            Assert.Equal(31, csr.CsrDesignationId);
            Assert.Equal(34, csr.CsrDesignationTierId);
            Assert.Equal(32, csr.PercentToNextTier);
            Assert.Equal(33, csr.Rank);
            Assert.Equal(30, csr.Value);
        }

        [Fact]
        public void WeaponStats()
        {
            var weaponStats = Mapper.Map<DomainModels.WeaponStats>(MatchWeaponData()[0]);

            Assert.Equal(40, weaponStats.DamageDealt);
            Assert.Equal(41, weaponStats.Headshots);
            Assert.Equal(42, weaponStats.Kills);
            Assert.True(TimeSpan.FromSeconds(43).Equals(weaponStats.PossessionTime));
            Assert.Equal(44, weaponStats.ShotsFired);
            Assert.Equal(45, weaponStats.ShotsLanded);
            Assert.Equal(46, weaponStats.Weapon.Id);
        }

        [Fact]
        public void MatchPlayer()
        {
            var matchPlayer = Mapper.Map<DomainModels.MatchPlayer>(MatchPlayerStatsData()[0]);

            Assert.True(TimeSpan.FromSeconds(70).Equals(matchPlayer.AvgLifeTime));
            Assert.True(matchPlayer.Dnf);
            Assert.Equal("matchPlayerName", matchPlayer.Name);
            Assert.Equal(1000, matchPlayer.Rank);
            Assert.Equal(100, matchPlayer.Team);
            Assert.Equal(101, matchPlayer.Assassinations);
            Assert.Equal(102, matchPlayer.Assists);
            Assert.Equal(103, matchPlayer.Deaths);
            Assert.Equal(104, matchPlayer.GrenadeDamage);
            Assert.Equal(105, matchPlayer.GrenadeKills);
            Assert.Equal(106, matchPlayer.GroundPoundDamage);
            Assert.Equal(107, matchPlayer.GroundPoundKills);
            Assert.Equal(108, matchPlayer.Headshots);
            Assert.Equal(109, matchPlayer.Kills);
            Assert.Equal(110, matchPlayer.MeleeDamage);
            Assert.Equal(111, matchPlayer.MeleeKills);
            Assert.Equal(112, matchPlayer.PowerWeaponDamage);
            Assert.Equal(113, matchPlayer.PowerWeaponGrabs);
            Assert.Equal(114, matchPlayer.PowerWeaponKills);
            Assert.True(TimeSpan.FromSeconds(115).Equals(matchPlayer.PowerWeaponPossessionTime));
            Assert.Equal(116, matchPlayer.ShotsFired);
            Assert.Equal(117, matchPlayer.ShotsLanded);
            Assert.Equal(118, matchPlayer.ShoulderBashDamage);
            Assert.Equal(119, matchPlayer.ShoulderBashKills);
            Assert.Equal(120, matchPlayer.WeaponDamage);

            var currentCsr = matchPlayer.CurrentCsr;
            Assert.Equal(31, currentCsr.CsrDesignationId);
            Assert.Equal(34, currentCsr.CsrDesignationTierId);
            Assert.Equal(32, currentCsr.PercentToNextTier);
            Assert.Equal(33, currentCsr.Rank);
            Assert.Equal(30, currentCsr.Value);

            var previousCsr = matchPlayer.PreviousCsr;
            Assert.Equal(36, previousCsr.CsrDesignationId);
            Assert.Equal(39, previousCsr.CsrDesignationTierId);
            Assert.Equal(37, previousCsr.PercentToNextTier);
            Assert.Equal(38, previousCsr.Rank);
            Assert.Equal(35, previousCsr.Value);

            var weaponsStats = matchPlayer.WeaponsStats;
            Assert.Equal(1, weaponsStats.Count);
            var weaponStats = weaponsStats[0];
            Assert.Equal(40, weaponStats.DamageDealt);
            Assert.Equal(41, weaponStats.Headshots);
            Assert.Equal(42, weaponStats.Kills);
            Assert.True(TimeSpan.FromSeconds(43).Equals(weaponStats.PossessionTime));
            Assert.Equal(44, weaponStats.ShotsFired);
            Assert.Equal(45, weaponStats.ShotsLanded);
            Assert.Equal(46, weaponStats.Weapon.Id);
        }

        [Fact]
        public void MatchFromPlayerMatch()
        {
            var match = Mapper.Map<DomainModels.Match>(PlayerMatchData()[0]);

            Assert.True(match.Completed.Equals(new DateTime(2016, 3, 3)));
            Assert.True(TimeSpan.FromSeconds(100).Equals(match.Duration));
            Assert.True(match.GameBaseVariantId.Equals(GameBaseVariantGuid));
            Assert.Equal(GameMode.Arena, match.GameMode);
            Assert.True(match.GameVariantId.Equals(GameVariantGuid));
            Assert.True(match.Id.Equals(MatchGuid));
            Assert.True(match.MapId.Equals(MapGuid));
            Assert.True(match.MapVariantId.Equals(MapVariantGuid));
            Assert.True(match.SeasonId.Equals(SeasonGuid));
            Assert.True(match.TeamGame);
        }

        [Fact]
        public void MatchFromMatchPlayerStats()
        {
            var match = Mapper.Map<DomainModels.Match>(MatchPlayerStatsData()[0]);

            Assert.True(match.PlaylistId.Equals(PlaylistGuid));
        }

        [Fact]
        public void MatchFromMatchReport()
        {
            var match = Mapper.Map<DomainModels.Match>(MatchReportData()[0]);

            var matchPlayer = match.Players[0];
            Assert.True(TimeSpan.FromSeconds(70).Equals(matchPlayer.AvgLifeTime));
            Assert.True(matchPlayer.Dnf);
            Assert.Equal("matchPlayerName", matchPlayer.Name);
            Assert.Equal(1000, matchPlayer.Rank);
            Assert.Equal(100, matchPlayer.Team);
            Assert.Equal(101, matchPlayer.Assassinations);
            Assert.Equal(102, matchPlayer.Assists);
            Assert.Equal(103, matchPlayer.Deaths);
            Assert.Equal(104, matchPlayer.GrenadeDamage);
            Assert.Equal(105, matchPlayer.GrenadeKills);
            Assert.Equal(106, matchPlayer.GroundPoundDamage);
            Assert.Equal(107, matchPlayer.GroundPoundKills);
            Assert.Equal(108, matchPlayer.Headshots);
            Assert.Equal(109, matchPlayer.Kills);
            Assert.Equal(110, matchPlayer.MeleeDamage);
            Assert.Equal(111, matchPlayer.MeleeKills);
            Assert.Equal(112, matchPlayer.PowerWeaponDamage);
            Assert.Equal(113, matchPlayer.PowerWeaponGrabs);
            Assert.Equal(114, matchPlayer.PowerWeaponKills);
            Assert.True(TimeSpan.FromSeconds(115).Equals(matchPlayer.PowerWeaponPossessionTime));
            Assert.Equal(116, matchPlayer.ShotsFired);
            Assert.Equal(117, matchPlayer.ShotsLanded);
            Assert.Equal(118, matchPlayer.ShoulderBashDamage);
            Assert.Equal(119, matchPlayer.ShoulderBashKills);
            Assert.Equal(120, matchPlayer.WeaponDamage);

            var currentCsr = matchPlayer.CurrentCsr;
            Assert.Equal(31, currentCsr.CsrDesignationId);
            Assert.Equal(34, currentCsr.CsrDesignationTierId);
            Assert.Equal(32, currentCsr.PercentToNextTier);
            Assert.Equal(33, currentCsr.Rank);
            Assert.Equal(30, currentCsr.Value);

            var previousCsr = matchPlayer.PreviousCsr;
            Assert.Equal(36, previousCsr.CsrDesignationId);
            Assert.Equal(39, previousCsr.CsrDesignationTierId);
            Assert.Equal(37, previousCsr.PercentToNextTier);
            Assert.Equal(38, previousCsr.Rank);
            Assert.Equal(35, previousCsr.Value);

            var weaponsStats = matchPlayer.WeaponsStats;
            Assert.Equal(1, weaponsStats.Count);
            var weaponStats = weaponsStats[0];
            Assert.Equal(40, weaponStats.DamageDealt);
            Assert.Equal(41, weaponStats.Headshots);
            Assert.Equal(42, weaponStats.Kills);
            Assert.True(TimeSpan.FromSeconds(43).Equals(weaponStats.PossessionTime));
            Assert.Equal(44, weaponStats.ShotsFired);
            Assert.Equal(45, weaponStats.ShotsLanded);
            Assert.Equal(46, weaponStats.Weapon.Id);
        }

        [Fact]
        public void Match()
        {
            var match = Mapper.Map<DomainModels.Match>(PlayerMatchData()[0])
                              .Map(MatchPlayerStatsData()[0])
                              .Map(MatchReportData()[0]);

            Assert.True(match.Completed.Equals(new DateTime(2016, 3, 3)));
            Assert.True(TimeSpan.FromSeconds(100).Equals(match.Duration));
            Assert.True(match.GameBaseVariantId.Equals(GameBaseVariantGuid));
            Assert.Equal(GameMode.Arena, match.GameMode);
            Assert.True(match.GameVariantId.Equals(GameVariantGuid));
            Assert.True(match.Id.Equals(MatchGuid));
            Assert.True(match.MapId.Equals(MapGuid));
            Assert.True(match.MapVariantId.Equals(MapVariantGuid));
            Assert.True(match.SeasonId.Equals(SeasonGuid));
            Assert.True(match.TeamGame);
            Assert.True(match.PlaylistId.Equals(PlaylistGuid));

            var matchPlayer = match.Players[0];
            Assert.True(TimeSpan.FromSeconds(70).Equals(matchPlayer.AvgLifeTime));
            Assert.True(matchPlayer.Dnf);
            Assert.Equal("matchPlayerName", matchPlayer.Name);
            Assert.Equal(1000, matchPlayer.Rank);
            Assert.Equal(100, matchPlayer.Team);
            Assert.Equal(101, matchPlayer.Assassinations);
            Assert.Equal(102, matchPlayer.Assists);
            Assert.Equal(103, matchPlayer.Deaths);
            Assert.Equal(104, matchPlayer.GrenadeDamage);
            Assert.Equal(105, matchPlayer.GrenadeKills);
            Assert.Equal(106, matchPlayer.GroundPoundDamage);
            Assert.Equal(107, matchPlayer.GroundPoundKills);
            Assert.Equal(108, matchPlayer.Headshots);
            Assert.Equal(109, matchPlayer.Kills);
            Assert.Equal(110, matchPlayer.MeleeDamage);
            Assert.Equal(111, matchPlayer.MeleeKills);
            Assert.Equal(112, matchPlayer.PowerWeaponDamage);
            Assert.Equal(113, matchPlayer.PowerWeaponGrabs);
            Assert.Equal(114, matchPlayer.PowerWeaponKills);
            Assert.True(TimeSpan.FromSeconds(115).Equals(matchPlayer.PowerWeaponPossessionTime));
            Assert.Equal(116, matchPlayer.ShotsFired);
            Assert.Equal(117, matchPlayer.ShotsLanded);
            Assert.Equal(118, matchPlayer.ShoulderBashDamage);
            Assert.Equal(119, matchPlayer.ShoulderBashKills);
            Assert.Equal(120, matchPlayer.WeaponDamage);

            var currentCsr = matchPlayer.CurrentCsr;
            Assert.Equal(31, currentCsr.CsrDesignationId);
            Assert.Equal(34, currentCsr.CsrDesignationTierId);
            Assert.Equal(32, currentCsr.PercentToNextTier);
            Assert.Equal(33, currentCsr.Rank);
            Assert.Equal(30, currentCsr.Value);

            var previousCsr = matchPlayer.PreviousCsr;
            Assert.Equal(36, previousCsr.CsrDesignationId);
            Assert.Equal(39, previousCsr.CsrDesignationTierId);
            Assert.Equal(37, previousCsr.PercentToNextTier);
            Assert.Equal(38, previousCsr.Rank);
            Assert.Equal(35, previousCsr.Value);

            var weaponsStats = matchPlayer.WeaponsStats;
            Assert.Equal(1, weaponsStats.Count);
            var weaponStats = weaponsStats[0];
            Assert.Equal(40, weaponStats.DamageDealt);
            Assert.Equal(41, weaponStats.Headshots);
            Assert.Equal(42, weaponStats.Kills);
            Assert.True(TimeSpan.FromSeconds(43).Equals(weaponStats.PossessionTime));
            Assert.Equal(44, weaponStats.ShotsFired);
            Assert.Equal(45, weaponStats.ShotsLanded);
            Assert.Equal(46, weaponStats.Weapon.Id);
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
                        Gamertag = "matchPlayerName",
                    },
                    PlaylistId = PlaylistGuid.ToString(),
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

        private static readonly Guid HopperGuid = Guid.NewGuid();
        private static readonly Guid MatchGuid = Guid.NewGuid();
        private static IList<ApiModels.PlayerMatch> PlayerMatchData()
        {
            return new List<ApiModels.PlayerMatch>
            {
                new ApiModels.PlayerMatch
                {
                    GameBaseVariantId = GameBaseVariantGuid.ToString(),
                    GameVariant = new ApiModels.GameVariantId
                    {
                        ResourceId = GameVariantGuid.ToString(),
                    },
                    Id = new ApiModels.MatchIdClass
                    {
                        GameMode = (int) GameMode.Arena,
                        MatchId = MatchGuid.ToString(),
                    },
                    IsTeamGame = true,
                    MapId = MapGuid.ToString(),
                    MapVariant = new ApiModels.MapVariantId
                    {
                        ResourceId = MapVariantGuid.ToString(),
                    },
                    MatchCompletedDate = new ApiModels.Iso8601Date
                    {
                        ISO8601Date = new DateTime(2016, 3, 3),
                    },
                    MatchDuration = "PT1M40S",
                    SeasonId = SeasonGuid.ToString(),
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
                },
            };
        }

        #endregion
    }
}
