using System;
using System.Web.Mvc;
using GHaack.Halo.Domain;

namespace GHaack.Halo.Web.Controllers
{
    public class HomeController : HaloControllerBase 
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
            return View();
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