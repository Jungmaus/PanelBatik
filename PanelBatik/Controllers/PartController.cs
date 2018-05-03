using PanelBatik.Models;
using PanelBatik.Models.OperationClass;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PanelBatik.Controllers
{
    public class PartController : Controller
    {
        [ChildActionOnly]
        public ActionResult GetCountPart()
        {
            using (var db = new DatabaseContext())
            {
                CountModel model = new CountModel();
                model.MusteriCount = db.Musteriler.Count();
                model.SiparisCount = db.Siparisler.Count();
                model.UrunCount = db.Urunler.Count();
                return View(model);
            }
        }

        [ChildActionOnly]
        public ActionResult GetSiparisTable()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult KisiselBilgilerPart()
        {
            if (Session["Email"] != null)
            {
                string adminMail = Session["Email"].ToString();
                using (var db = new DatabaseContext())
                {
                    Admin admin = db.Adminler.First(x => x.Email == adminMail);
                    KisiselAyarModel ka = new KisiselAyarModel();
                    ka.Ad = admin.Ad;
                    ka.Email = admin.Email;
                    ka.Soyad = admin.Soyad;
                    ka.TelNo = admin.TelNo;
                    ka.FotografYolu = admin.FotografYolu;
                    return View(ka);
                }
            }
            else
            return RedirectToAction("Login", "Panel");
        }

        [ChildActionOnly]
        public ActionResult SifreDegistirPart()
        {
            return View();
        }


    }
}