using System.Collections.Generic;
using System.Threading.Tasks;
using GHaack.Halo.Domain.Models;

namespace GHaack.Halo.Domain
{
    public interface IHaloDataManager
    {
        PlayerStats GetPlayerStats(IEnumerable<Match> matches, string player);
        Task ReplaceAllMetadataAsync();
        Task<IEnumerable<Match>> RetrieveStoredMatchesAsync(string player);
        Task StoreMatchesAsync(string player, int start = 0, int quantity = 25);
    }
}