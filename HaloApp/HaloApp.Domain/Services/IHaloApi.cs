using HaloApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaloApp.Domain.Services
{
    public interface IHaloApi
    {
        Task<IList<Map>> GetMapsAsync();
        Task<IList<Match>> GetMatchesAsync(string player, int start, int count);
    }
}
