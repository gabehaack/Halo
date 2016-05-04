using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoMapper;
using HaloApp.Domain;
using HaloApp.Domain.Enums;
using HaloApp.Domain.Models;
using HaloApp.Domain.Models.Dto;
using Xunit;

namespace HaloApp.Tests
{
    [ExcludeFromCodeCoverage]
    public class HaloDomainMapProfileTests
    {
        public HaloDomainMapProfileTests()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<HaloDomainMapProfile>();
            });
        }

        #region Tests

        [Fact]
        public void ValidConfiguration()
        {
            Mapper.AssertConfigurationIsValid();
        }

        [Fact]
        public void Csr()
        {
            var csr = Mapper.Map<Csr>(CsrDtoData()[0]);
            ValidateCsrMapping(csr, CsrDtoData()[0]);
        }

        [Fact]
        public void WeaponStats()
        {
            var weaponStats = Mapper.Map<WeaponStats>(WeaponStatsDtoData()[0]);
            ValidateWeaponStatsMapping(weaponStats, WeaponStatsDtoData()[0]);
        }

        [Fact]
        public void Player()
        {
            var player = Mapper.Map<Player>(PlayerDtoData()[0]);
            ValidatePlayerMapping(player, PlayerDtoData()[0]);
            ValidateCsrMapping(player.CurrentCsr, CsrDtoData()[0]);
            ValidateCsrMapping(player.PreviousCsr, CsrDtoData()[1]);
            ValidateWeaponStatsMapping(player.WeaponsStats.First(), WeaponStatsDtoData()[0]);
            ValidateWeaponStatsMapping(player.WeaponsStats.ElementAt(1), WeaponStatsDtoData()[1]);
        }

        [Fact]
        public void Team()
        {
            var team = Mapper.Map<Team>(TeamDtoData()[0]);
            ValidateTeamMapping(team, TeamDtoData()[0]);
        }

        [Fact]
        public void Match()
        {
            var match = Mapper.Map<Match>(MatchDtoData()[0]);

            var player = match.Players.First();
            ValidatePlayerMapping(player, PlayerDtoData()[0]);
            ValidateCsrMapping(player.CurrentCsr, CsrDtoData()[0]);
            ValidateCsrMapping(player.PreviousCsr, CsrDtoData()[1]);
            ValidateWeaponStatsMapping(player.WeaponsStats.First(), WeaponStatsDtoData()[0]);
            ValidateWeaponStatsMapping(player.WeaponsStats.ElementAt(1), WeaponStatsDtoData()[1]);
            ValidateTeamMapping(match.Teams.First(), TeamDtoData()[0]);
            ValidateTeamMapping(match.Teams.ElementAt(1), TeamDtoData()[1]);
        }

        private static void ValidateCsrMapping(Csr csr, CsrDto csrDto)
        {
            Assert.Equal(csrDto.PercentToNextTier, csr.PercentToNextTier);
            Assert.Equal(csrDto.Rank, csr.Rank);
            Assert.Equal(csrDto.Value, csr.Value);

            var csrDesignation = csr.Designation;
            Assert.NotNull(csrDesignation);
            Assert.Equal(csrDto.CsrDesignationId, csrDesignation.Id);

            var csrTier = csr.Tier;
            Assert.NotNull(csrTier);
            Assert.Equal(csrDto.CsrDesignationTierId, csrTier.Id);
        }

        private static void ValidateWeaponStatsMapping(WeaponStats weaponStats, WeaponStatsDto weaponStatsDto)
        {
            Assert.Equal(weaponStatsDto.DamageDealt, weaponStats.DamageDealt);
            Assert.Equal(weaponStatsDto.Headshots, weaponStats.Headshots);
            Assert.Equal(weaponStatsDto.Kills, weaponStats.Kills);
            Assert.True(weaponStatsDto.PossessionTime.Equals(weaponStats.PossessionTime));
            Assert.Equal(weaponStatsDto.ShotsFired, weaponStats.ShotsFired);
            Assert.Equal(weaponStatsDto.ShotsLanded, weaponStats.ShotsLanded);

            var weapon = weaponStats.Weapon;
            Assert.NotNull(weapon);
            Assert.Equal(weaponStatsDto.WeaponId, weapon.Id);
        }

        private static void ValidatePlayerMapping(Player player, PlayerDto playerDto)
        {
            Assert.Equal(playerDto.Assassinations, player.Assassinations);
            Assert.Equal(playerDto.Assists, player.Assists);
            Assert.Equal(playerDto.Deaths, player.Deaths);
            Assert.Equal(playerDto.Dnf, player.Dnf);
            Assert.Equal(playerDto.GrenadeDamage, player.GrenadeDamage);
            Assert.Equal(playerDto.GrenadeKills, player.GrenadeKills);
            Assert.Equal(playerDto.GroundPoundDamage, player.GroundPoundDamage);
            Assert.Equal(playerDto.GroundPoundKills, player.GroundPoundKills);
            Assert.Equal(playerDto.Headshots, player.Headshots);
            Assert.Equal(playerDto.Kills, player.Kills);
            Assert.Equal(playerDto.MeleeDamage, player.MeleeDamage);
            Assert.Equal(playerDto.MeleeKills, player.MeleeKills);
            Assert.Equal(playerDto.Name, player.Name);
            Assert.Equal(playerDto.PowerWeaponDamage, player.PowerWeaponDamage);
            Assert.Equal(playerDto.PowerWeaponGrabs, player.PowerWeaponGrabs);
            Assert.Equal(playerDto.PowerWeaponKills, player.PowerWeaponKills);
            Assert.True(playerDto.PowerWeaponPossessionTime.Equals(player.PowerWeaponPossessionTime));
            Assert.Equal(playerDto.Rank, player.Rank);
            Assert.Equal(playerDto.ShotsFired, player.ShotsFired);
            Assert.Equal(playerDto.ShotsLanded, player.ShotsLanded);
            Assert.Equal(playerDto.ShoulderBashDamage, player.ShoulderBashDamage);
            Assert.Equal(playerDto.ShoulderBashKills, player.ShoulderBashKills);
            Assert.Equal(playerDto.Team, player.Team);
            Assert.Equal(playerDto.WeaponDamage, player.WeaponDamage);
        }

        private static void ValidateTeamMapping(Team team, TeamDto teamDto)
        {
            Assert.Equal(teamDto.Rank, team.Rank);
            Assert.Equal(teamDto.Score, team.Score);
            Assert.Equal(teamDto.TeamId, team.TeamId);
        }

        private static void ValidateMatchMapping(Match match, MatchDto matchDto)
        {
            Assert.True(matchDto.Completed.Equals(match.Completed));
            Assert.True(matchDto.Duration.Equals(match.Duration));
            Assert.Equal(matchDto.Id, match.Id);
            Assert.Equal(matchDto.TeamGame, match.TeamGame);

            var gameBaseVariant = match.GameBaseVariant;
            Assert.NotNull(gameBaseVariant);
            Assert.Equal(matchDto.GameBaseVariantId, gameBaseVariant.Id);

            var gameVariant = match.GameVariant;
            Assert.NotNull(gameVariant);
            Assert.Equal(matchDto.GameVariantId, gameVariant.Id);

            var map = match.Map;
            Assert.NotNull(map);
            Assert.Equal(matchDto.MapId, map.Id);

            var mapVariant = match.MapVariant;
            Assert.NotNull(mapVariant);
            Assert.Equal(matchDto.MapVariantId, mapVariant.Id);

            var playlist = match.Playlist;
            Assert.NotNull(playlist);
            Assert.Equal(matchDto.PlaylistId, playlist.Id);

            var season = match.Season;
            Assert.NotNull(season);
            Assert.Equal(matchDto.SeasonId, season.Id);
        }

        #endregion

        #region Data

        private static Guid CsrDesignationGuid = Guid.NewGuid();
        private static IList<CsrDto> CsrDtoData()
        {
            return new List<CsrDto>
            {
                new CsrDto
                {
                    CsrDesignationId = 1,
                    CsrDesignationTierId = 2,
                    PercentToNextTier = 3,
                    Rank = null,
                    Value = 0,
                },
                new CsrDto
                {
                    CsrDesignationId = 4,
                    CsrDesignationTierId = 5,
                    PercentToNextTier = 6,
                    Rank = 7,
                    Value = 8,
                }
            };
        }

        private static IList<TeamDto> TeamDtoData()
        {
            return new List<TeamDto>
            {
                new TeamDto
                {
                    Rank = 61,
                    Score = 62,
                    TeamId = 63,
                },
                new TeamDto
                {
                    Rank = 64,
                    Score = 65,
                    TeamId = 66,
                },
            };
        }

        private static IList<WeaponStatsDto> WeaponStatsDtoData()
        {
            return new List<WeaponStatsDto>
            {
                new WeaponStatsDto
                {
                    DamageDealt = 11,
                    Headshots = 12,
                    Kills = 13,
                    PossessionTime = TimeSpan.FromSeconds(14),
                    ShotsFired = 15,
                    ShotsLanded = 16,
                    WeaponId = 17,
                },
                new WeaponStatsDto
                {
                    DamageDealt = 51,
                    Headshots = 52,
                    Kills = 53,
                    PossessionTime = TimeSpan.FromSeconds(54),
                    ShotsFired = 55,
                    ShotsLanded = 56,
                    WeaponId = 57,
                },
            };
        }

        private static IList<PlayerDto> PlayerDtoData()
        {
            return new List<PlayerDto>
            {
                new PlayerDto
                {
                    Assassinations = 20,
                    Assists = 21,
                    CurrentCsr = CsrDtoData()[0],
                    Deaths = 22,
                    Dnf = true,
                    GrenadeDamage = 23,
                    GrenadeKills = 24,
                    GroundPoundDamage = 25,
                    GroundPoundKills = 26,
                    Headshots = 27,
                    Kills = 28,
                    MeleeDamage = 29,
                    MeleeKills = 30,
                    Name = "playerName",
                    PowerWeaponDamage = 31,
                    PowerWeaponGrabs = 32,
                    PowerWeaponKills = 33,
                    PowerWeaponPossessionTime = TimeSpan.FromSeconds(34),
                    PreviousCsr = CsrDtoData()[1],
                    Rank = 35,
                    ShotsFired = 36,
                    ShotsLanded = 37,
                    ShoulderBashDamage = 38,
                    ShoulderBashKills = 39,
                    Team = 40,
                    WeaponDamage = 41,
                    WeaponsStats = WeaponStatsDtoData(),
                }
            };
        }

        private static Guid GameBaseVariantGuid = Guid.NewGuid();
        private static Guid GameVariantGuid = Guid.NewGuid();
        private static Guid MatchGuid = Guid.NewGuid();
        private static Guid MapGuid = Guid.NewGuid();
        private static Guid MapVariantGuid = Guid.NewGuid();
        private static Guid PlaylistGuid = Guid.NewGuid();
        private static Guid SeasonGuid = Guid.NewGuid();
        private static IList<MatchDto> MatchDtoData()
        {
            return new List<MatchDto>
            {
                new MatchDto
                {
                    Completed = new DateTime(2016, 1, 1),
                    Duration = TimeSpan.FromSeconds(71),
                    GameBaseVariantId = GameBaseVariantGuid,
                    GameMode = GameMode.Arena,
                    GameVariantId = GameVariantGuid,
                    Id = MatchGuid,
                    MapId = MapGuid,
                    MapVariantId = MapVariantGuid,
                    Players = PlayerDtoData(),
                    PlaylistId = PlaylistGuid,
                    SeasonId = SeasonGuid,
                    TeamGame = true,
                    Teams = TeamDtoData(),
                }
            };
        }

        #endregion
    }
}
