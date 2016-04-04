using HaloApp.Domain.Models;
using HaloApp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaloApp.Domain
{
    public class HaloDataManager
    {
        private readonly IHaloApi _haloApi;
        private IList<Map> _maps;

        public HaloDataManager(IHaloApi haloApi)
        {
            if (haloApi == null)
                throw new ArgumentNullException(nameof(haloApi));
            _haloApi = haloApi;
        }

        public async Task GetMaps()
        {
            _maps = await _haloApi.GetMapsAsync();
        }
    }
}
