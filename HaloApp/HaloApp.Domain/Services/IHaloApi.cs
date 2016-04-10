using HaloApp.Domain.Models;
using HaloApp.Domain.Models.Metadata;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HaloApp.Domain.Services
{
    public interface IHaloApi
    {
        Task<IEnumerable<CsrDesignation>> GetCsrDesignationMetadataAsync();
        Task<IEnumerable<FlexibleStat>> GetFlexibleStatMetadataAsync();
        Task<IEnumerable<GameBaseVariant>> GetGameBaseVariantMetadataAsync();
        Task<GameVariant> GetGameVariantMetadataAsync(Guid gameVariantId);
        Task<IEnumerable<Impulse>> GetImpulseMetadataAsync();
        Task<IEnumerable<Map>> GetMapMetadataAsync();
        Task<MapVariant> GetMapVariantMetadataAsync(Guid mapVariantId);
        Task<IEnumerable<Match>> GetMatchesAsync(string player, int start = 0, int count = 25);
        Task<IEnumerable<Medal>> GetMedalMetadataAsync();
        Task<IEnumerable<Playlist>> GetPlaylistMetadataAsync();
        Task<IEnumerable<Season>> GetSeasonMetadataAsync();
        Task<IEnumerable<SpartanRank>> GetSpartanRankMetadataAsync();
        Task<IEnumerable<TeamColor>> GetTeamColorMetadataAsync();
        Task<IEnumerable<Vehicle>> GetVehicleMetadataAsync();
        Task<IEnumerable<Weapon>> GetWeaponMetadataAsync();
    }
}
