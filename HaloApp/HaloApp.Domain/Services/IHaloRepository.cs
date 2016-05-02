using HaloApp.Domain.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HaloApp.Domain.Services
{
    public interface IHaloRepository
    {
        Task ReplaceMetadataAsync<TMetadata>(IList<TMetadata> metadata);
        Task<IList<TMetadata>> GetMetadataAsync<TMetadata>();
        Task AddMatchesAsync(IList<MatchDto> matches);
        Task<IList<MatchDto>> GetMatchesAsync(string player);
    }
}
