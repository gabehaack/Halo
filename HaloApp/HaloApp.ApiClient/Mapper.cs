using HaloApp.ApiClient.Models;
using HaloApp.ApiClient.Models.Metadata;
using System.Linq;
using DomainMetadata = HaloApp.Domain.Models.Metadata;
using GHaack.Utilities;
using System;
using System.Xml;

namespace HaloApp.ApiClient
{
    public static class Mapper
    {
        #region Metadata

        public static DomainMetadata.CsrDesignation CsrDesignation(CsrDesignation csrDesignation)
        {
            return new DomainMetadata.CsrDesignation
            {
                BannerImageUrl = csrDesignation.bannerImageUrl,
                Id = csrDesignation.id,
                Name = csrDesignation.name,
                Tiers = csrDesignation.tiers.Select(t => CsrTier(t)).ToList(),
            };
        }

        public static DomainMetadata.CsrTier CsrTier(CsrTier csrTier)
        {
            return new DomainMetadata.CsrTier
            {
                IconImageUrl = Uri(csrTier.iconImageUrl),
                Id = csrTier.id,
            };
        }

        public static DomainMetadata.FlexibleStat FlexibleStat(FlexibleStat flexibleStat)
        {
            return new DomainMetadata.FlexibleStat
            {
                Id = Guid(flexibleStat.id),
                Name = flexibleStat.name,
                Type = EnumUtility.Parse<Domain.Enums.FlexibleStatType>(flexibleStat.type),
            };
        }

        public static DomainMetadata.GameBaseVariant GameBaseVariant(GameBaseVariant gameBaseVariant)
        {
            return new DomainMetadata.GameBaseVariant
            {
                IconUrl = Uri(gameBaseVariant.iconUrl),
                Id = Guid(gameBaseVariant.id),
                Name = gameBaseVariant.name,
                SupportedGameModes = gameBaseVariant.supportedGameModes.Select(m => EnumUtility.Parse<Domain.Enums.GameMode>(m)).ToList(),
            };
        }

        public static DomainMetadata.GameVariant GameVariant(GameVariant gameVariant)
        {
            return new DomainMetadata.GameVariant
            {
                Description = gameVariant.description,
                GameBaseVariantId = Guid(gameVariant.gameBaseVariantId),
                IconUrl = Uri(gameVariant.iconUrl),
                Id = Guid(gameVariant.id),
                Name = gameVariant.name,
            };
        }

        //public static DomainMetadata.Impulse Impulse(Impulse impulse)
        //{
        //    return new DomainMetadata.Impulse
        //    {
        //        Id = impulse.id,
        //    };
        //}

        public static DomainMetadata.Map Map(Map map)
        {
            return new DomainMetadata.Map
            {
                Description = map.description,
                Id = Guid(map.id),
                ImageUrl = Uri(map.imageUrl),
                Name = map.name,
                SupportedGameModes = map.supportedGameModes.Select(m => EnumUtility.Parse<Domain.Enums.GameMode>(m)).ToList(),
            };
        }

        public static DomainMetadata.MapVariant MapVariant(MapVariant mapVariant)
        {
            return new DomainMetadata.MapVariant
            {
                Description = mapVariant.description,
                Id = Guid(mapVariant.id),
                MapId = Guid(mapVariant.mapId),
                MapImageUrl = Uri(mapVariant.mapImageUrl),
                Name = mapVariant.name,
            };
        }

        public static DomainMetadata.Medal Medal(Medal medal)
        {
            return new DomainMetadata.Medal
            {
                Classification = EnumUtility.Parse<Domain.Enums.MedalClassification>(medal.classification),
                Description = medal.description,
                Difficulty = medal.difficulty,
                Id = medal.id,
                Name = medal.name,
                SpriteLocation = MedalSpriteLocation(medal.spriteLocation),
            };
        }

        public static DomainMetadata.MedalSpriteLocation MedalSpriteLocation(MedalSpriteLocation medalSpriteLocation)
        {
            return new DomainMetadata.MedalSpriteLocation
            {
                Height = medalSpriteLocation.height,
                Left = medalSpriteLocation.left,
                SpriteHeight = medalSpriteLocation.spriteHeight,
                SpriteSheetUri = Uri(medalSpriteLocation.spriteSheetUri),
                SpriteWidth = medalSpriteLocation.spriteWidth,
                Top = medalSpriteLocation.top,
                Width = medalSpriteLocation.width,
            };
        }

        public static DomainMetadata.Playlist Playlist(Playlist playlist)
        {
            return new DomainMetadata.Playlist
            {
                Active = playlist.isActive,
                Description = playlist.description,
                GameMode = EnumUtility.Parse<Domain.Enums.GameMode>(playlist.gameMode),
                Id = Guid(playlist.id),
                ImageUrl = Uri(playlist.imageUrl),
                Name = playlist.name,
                Ranked = playlist.isRanked,
            };
        }

        public static DomainMetadata.Season Season(Season season)
        {
            return new DomainMetadata.Season
            {
                Active = season.isActive,
                EndDate = season.endDate,
                IconUrl = Uri(season.iconUrl),
                Id = Guid(season.id),
                Name = season.name,
                PlaylistIds = season.playlists.Select(p => Guid(p.id)).ToList(),
                StartDate = season.startDate,
            };
        }

        public static DomainMetadata.SpartanRank SpartanRank(SpartanRank spartanRank)
        {
            return new DomainMetadata.SpartanRank
            {
                Id = spartanRank.id,
                StartXp = spartanRank.startXp,
            };
        }

        public static DomainMetadata.TeamColor TeamColor(TeamColor teamColor)
        {
            return new DomainMetadata.TeamColor
            {
                Color = teamColor.color,
                Description = teamColor.description,
                IconUrl = Uri(teamColor.iconUrl),
                Id = teamColor.id,
                Name = teamColor.name,
            };
        }

        public static DomainMetadata.Vehicle Vehicle(Vehicle vehicle)
        {
            return new DomainMetadata.Vehicle
            {
                Description = vehicle.description,
                Id = vehicle.id,
                LargeIconImageUrl = Uri(vehicle.largeIconImageUrl),
                Name = vehicle.name,
                SmallIconImageUrl = Uri(vehicle.smallIconImageUrl),
                UsableByPlayer = vehicle.isUsableByPlayer,
            };
        }

        public static DomainMetadata.Weapon Weapon(Weapon weapon)
        {
            return new DomainMetadata.Weapon
            {
                Description = weapon.description,
                Id = weapon.id,
                LargeIconImageUrl = Uri(weapon.largeIconImageUrl),
                Name = weapon.name,
                SmallIconImageUrl = Uri(weapon.smallIconImageUrl),
                Type = EnumUtility.Parse<Domain.Enums.WeaponType>(weapon.type),
                UsableByPlayer = weapon.isUsableByPlayer,
            };
        }

        #endregion

        #region Match Stats

        public static Domain.Models.Csr Csr(MatchCsr csr)
        {
            if (csr == null) return null;
            return new Domain.Models.Csr
            {
                CsrDesignationId = csr.DesignationId,
                CsrDesignationTierId = csr.Tier,
                PercentToNextTier = csr.PercentToNextTier,
                Rank = csr.Rank,
                Value = csr.Csr,
            };
        }

        public static Domain.Models.Match Match(PlayerMatch playerMatch, MatchReport matchReport)
        {
            var playerStat = matchReport.PlayerStats.First();
            return new Domain.Models.Match
            {
                Completed = playerMatch.MatchCompletedDate.ISO8601Date,
                Duration = Duration(playerMatch.MatchDuration),
                GameMode = EnumUtility.Parse<Domain.Enums.GameMode>(playerMatch.Id.GameMode),
                Id = Guid(playerMatch.Id.MatchId),
                MapId = Guid(playerStat.MapId),
                MapVariant = Guid(playerMatch.MapVariant.ResourceId),
                GameBaseVariantId = Guid(playerMatch.GameBaseVariantId),
                GameVariantId = Guid(playerMatch.GameVariant.ResourceId),
                PlaylistId = Guid(playerStat.PlaylistId),
                Players = matchReport.PlayerStats.Select(p => MatchPlayer(p)).ToList(),
                SeasonId = Guid(playerStat.SeasonId),
                TeamGame = playerMatch.IsTeamGame,
            };
        }

        public static Domain.Models.MatchPlayer MatchPlayer(MatchPlayerStats matchPlayer)
        {
            return new Domain.Models.MatchPlayer
            {
                AverageLifeTime = Duration(matchPlayer.AvgLifeTimeOfPlayer),
                CurrentCsr = Csr(matchPlayer.CurrentCsr),
                Dnf = matchPlayer.DNF,
                Name = matchPlayer.Player.Gamertag,
                PreviousCsr = Csr(matchPlayer.PreviousCsr),
                Rank = matchPlayer.Rank,
                Team = matchPlayer.TeamId,
                TotalAssassinations = matchPlayer.TotalAssassinations,
                TotalAssists = matchPlayer.TotalAssists,
                TotalDeaths = matchPlayer.TotalDeaths,
                TotalGrenadeDamage = matchPlayer.TotalGrenadeDamage,
                TotalGrenadeKills = matchPlayer.TotalGrenadeKills,
                TotalGroundPoundDamage = matchPlayer.TotalGroundPoundDamage,
                TotalGroundPoundKills = matchPlayer.TotalGroundPoundKills,
                TotalHeadshots = matchPlayer.TotalHeadshots,
                TotalKills = matchPlayer.TotalKills,
                TotalMeleeDamage = matchPlayer.TotalMeleeDamage,
                TotalMeleeKills = matchPlayer.TotalMeleeKills,
                TotalPowerWeaponDamage = matchPlayer.TotalPowerWeaponDamage,
                TotalPowerWeaponGrabs = matchPlayer.TotalPowerWeaponGrabs,
                TotalPowerWeaponKills = matchPlayer.TotalPowerWeaponKills,
                TotalShotsFired = matchPlayer.TotalShotsFired,
                TotalShotsLanded = matchPlayer.TotalShotsLanded,
                TotalShoulderBashDamage = matchPlayer.TotalShoulderBashDamage,
                TotalShoulderBashKills = matchPlayer.TotalShoulderBashKills,
                TotalWeaponDamage = matchPlayer.TotalWeaponDamage,
                WeaponsStats = matchPlayer.WeaponStats.Select(w => WeaponStats(w)).ToList(),
            };
        }

        public static Domain.Models.WeaponStats WeaponStats(MatchWeapon matchWeapon)
        {
            return new Domain.Models.WeaponStats
            {
                DamageDealt = matchWeapon.TotalDamageDealt,
                Headshots = matchWeapon.TotalHeadshots,
                Id = matchWeapon.WeaponId.StockId,
                Kills = matchWeapon.TotalKills,
                PossessionTime = Duration(matchWeapon.TotalPossessionTime),
                ShotsFired = matchWeapon.TotalShotsFired,
                ShotsLanded = matchWeapon.TotalShotsLanded,
            };
        }

        #endregion

        #region Helpers

        private static Uri Uri(string uriString)
        {
            Uri uri;
            return System.Uri.TryCreate(uriString, UriKind.Absolute, out uri)
                ? uri
                : null;
        }

        private static Guid Guid(string guidString)
        {
            Guid guid;
            return System.Guid.TryParse(guidString, out guid)
                ? guid
                : System.Guid.Empty;
        }

        private static TimeSpan Duration(string duration)
        {
            return XmlConvert.ToTimeSpan(duration);
        }

        #endregion
    }
}
