using HaloApp.Domain.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HaloApp.Domain.Services
{
    public interface IHaloRepository
    {
        Task ReplaceMetadataAsync<TMetadata>(IEnumerable<TMetadata> metadata);
        Task<IEnumerable<TMetadata>> GetMetadataAsync<TMetadata>();
        Task AddMatchesAsync(IEnumerable<MatchDto> matches);
        Task<IEnumerable<MatchDto>> GetMatchesAsync(string player);
    }
}
