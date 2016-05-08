using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GHaack.Halo.Domain;

namespace GHaack.Halo.Web.Controllers
{
    public class PlayerStatsController : Controller
    {
        private readonly IHaloDataManager _haloDataManager;

        public PlayerStatsController(IHaloDataManager haloDataManager)
        {
            if (haloDataManager == null)
                throw new ArgumentNullException(nameof(haloDataManager));
            _haloDataManager = haloDataManager;
        }

        // GET: PlayerStats
        public ActionResult Index()
        {
            return View();
        }
    }
}