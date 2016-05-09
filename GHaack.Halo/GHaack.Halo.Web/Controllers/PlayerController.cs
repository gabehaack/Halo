using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GHaack.Halo.Domain;
using GHaack.Halo.Web.Models;

namespace GHaack.Halo.Web.Controllers
{
    public class PlayerController : Controller
    {
        private readonly IHaloDataManager _haloDataManager;

        public PlayerController(IHaloDataManager haloDataManager)
        {
            if (haloDataManager == null)
                throw new ArgumentNullException(nameof(haloDataManager));
            _haloDataManager = haloDataManager;
        }

        public async Task<ActionResult> Index(string player)
        {
            var emblemUri = await _haloDataManager.GetEmblemImageUriAsync(player);
            if (emblemUri == null)
            {
                return View(new PlayerViewModel
                {
                    Player = player,
                });
            }

            var matches = await _haloDataManager.RetrieveStoredMatchesAsync(player);
            if (!matches.Any())
            {
                await _haloDataManager.StoreMatchesAsync(player);
                matches = await _haloDataManager.RetrieveStoredMatchesAsync(player);
            }

            var stats = _haloDataManager.GetPlayerStats(matches, player);
            var viewModel = new PlayerViewModel
            {
                Player = player,
                Stats = stats,
            };
            return View(viewModel);
        }
    }
}