using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GHaack.Halo.Domain;

namespace GHaack.Halo.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHaloDataManager _haloDataManager;

        public HomeController(IHaloDataManager haloDataManager)
        {
            if (haloDataManager == null)
                throw new ArgumentNullException(nameof(haloDataManager));
            _haloDataManager = haloDataManager;
        }

        public ActionResult Index()
        {
            return RedirectToAction("Index", "PlayerStats");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}