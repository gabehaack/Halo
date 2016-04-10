using HaloApp.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HaloApp.Domain.Services
{
    public interface IHaloRepository
    {
        Task ReplaceMetadataAsync<TMetadata>(IList<TMetadata> metadata);
        Task AddMatchesAsync(IList<Match> matches);
        Task<IEnumerable<Match>> GetMatchesAsync(string player);
    }
}
