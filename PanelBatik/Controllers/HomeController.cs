using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PanelBatik.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult AnaSayfa()
        {
            return View();
        }

        public ActionResult UrunDetay(int Id)
        {
            return View();
        }

    }
}