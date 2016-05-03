using HaloApp.Domain.Models.Dto;
using HaloApp.Domain.Models.Metadata;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HaloApp.Domain.Services
{
    public interface IHaloApi
    {
        // Metadata
        Task<IEnumerable<CsrDesignation>> GetCsrDesignationMetadataAsync();
        Task<IEnumerable<FlexibleStat>> GetFlexibleStatMetadataAsync();
        Task<IEnumerable<GameBaseVariant>> GetGameBaseVariantMetadataAsync();
        Task<GameVariant> GetGameVariantMetadatumAsync(Guid gameVariantId);
        Task<IEnumerable<Impulse>> GetImpulseMetadataAsync();
        Task<IEnumerable<Map>> GetMapMetadataAsync();
        Task<MapVariant> GetMapVariantMetadatumAsync(Guid mapVariantId);
        Task<IEnumerable<Medal>> GetMedalMetadataAsync();
        Task<IEnumerable<Playlist>> GetPlaylistMetadataAsync();
        Task<IEnumerable<Season>> GetSeasonMetadataAsync();
        Task<IEnumerable<SpartanRank>> GetSpartanRankMetadataAsync();
        Task<IEnumerable<TeamColor>> GetTeamColorMetadataAsync();
        Task<IEnumerable<Vehicle>> GetVehicleMetadataAsync();
        Task<IEnumerable<Weapon>> GetWeaponMetadataAsync();

        Task<IEnumerable<MatchDto>> GetMatchesAsync(string player, int start = 0, int quantity = 25);
        Task<IEnumerable<PlayerDto>> GetMatchStatsAsync(Guid matchId);
    }
}
