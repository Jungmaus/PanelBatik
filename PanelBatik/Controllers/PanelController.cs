using PanelBatik.Models;
using PanelBatik.Models.OperationClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using OfficeOpenXml.Style;


namespace PanelBatik.Controllers
{
    public class PanelController : Controller
    {
        // GET: Panel
        public ActionResult Login()
        {
            if (Session["Email"] != null)
                return RedirectToAction("GostergePaneli");
            LoginModel lm = new LoginModel();
            using (var db = new DatabaseContext())
            {
                if (HttpContext.Response.Cookies["Lrmb"] != null)
                {

                    string cocoMail = Request.Cookies["Lrmb"].Value;
                    Admin admin = db.Adminler.FirstOrDefault(x => x.Email == cocoMail);
                    if (admin != null)
                    {
                        lm.Email = admin.Email;
                        lm.Sifre = admin.Sifre;
                    }
                }
            }
            return View(lm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel data)
        {
            if (ModelState.IsValid)
            {
                using (var db = new DatabaseContext())
                {
                    Admin admin = db.Adminler.FirstOrDefault(x => x.Email == data.Email && x.Sifre == data.Sifre);
                    if (admin != null)
                    {
                        Session["Email"] = admin.Email;
                        admin.SonGiris = DateTime.Now;
                        db.Entry(admin).State = System.Data.Entity.EntityState.Modified;
                        if (data.Remember == true)
                        {
                            HttpCookie coco = new HttpCookie("Lrmb");
                            if (HttpContext.Response.Cookies["Lrmb"] == null)
                            {
                                coco.Value = admin.Email;
                                coco.Expires = DateTime.Now.AddDays(3);
                                Response.Cookies.Add(coco);
                            }
                            else
                            {
                                Response.Cookies.Remove("Lrmb");
                                coco.Value = admin.Email;
                                coco.Expires = DateTime.Now.AddDays(3);
                                Response.Cookies.Add(coco);
                            }
                        }
                        db.SaveChanges();
                        return RedirectToAction("GostergePaneli");
                    }
                    else
                    {
                        ViewBag.msgError = "Email adresiniz ve/veya Şifreniz yanlıştır. Lütfen tekrar deneyiniz.";
                        return View(data);
                    }
                }
            }
            return View(data);
        }

        public ActionResult SifreYenile()
        {
            if (Session["Email"] != null)
                return RedirectToAction("GostergePaneli");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SifreYenile(SifreYenileModel data)
        {
            if (ModelState.IsValid)
            {
                using (var db = new DatabaseContext())
                {
                    Admin admin = db.Adminler.FirstOrDefault(x => x.Email == data.Email);
                    if (admin != null)
                    {
                        using (var service = new MailServis.MailClient())
                        {
                            PrivateUrl controlUri = db.PrivateUrls.FirstOrDefault(x => x.Admin.Id == admin.Id);
                            if (controlUri == null)
                            {
                                var uri = service.GetPassword();
                                PrivateUrl privateUri = new PrivateUrl();
                                privateUri.Admin = admin;
                                privateUri.Url = uri;
                                db.PrivateUrls.Add(privateUri);
                                db.SaveChanges();
                                service.SendMail("http://localhost58701/Panel/PrivateUri?code=" + privateUri.Url,
                                    admin.Email);
                                ViewBag.basari = "Yeni bilgileriniz mail olarak gönderilmiştir.";
                            }
                            else
                            {
                                ViewBag.msgError = "Bir referans kodunuz hala mevcut.";
                            }
                        }
                    }
                    else
                    {
                        ViewBag.msgError = "Böyle bir Email adresi bulunamamıştır.";
                    }
                }
            }
            return View(data);
        }

        public ActionResult PrivateUri(string code)
        {
            using (var db = new DatabaseContext())
            {
                PrivateUrl privateUrl = db.PrivateUrls.FirstOrDefault(x => x.Url == code);
                if (privateUrl != null)
                {
                    Admin admin = db.Adminler.First(x => x.Id == privateUrl.Admin.Id);
                    Session["ADMINID"] = admin.Id;
                    Session["PRIVATEURID"] = privateUrl.Id;
                    SifreDegistirModel sd = new SifreDegistirModel();
                    sd.EskiSifre = admin.Sifre;
                    return View(sd);
                }
                else
                    return RedirectToAction("Login");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PrivateUri(SifreDegistirModel sd)
        {
            if (ModelState.IsValid)
            {
                if (sd.YeniSifre == sd.YeniSifreTekrar)
                {
                    using (var db = new DatabaseContext())
                    {
                        int adminId = Convert.ToInt32(Session["ADMINID"]);
                        int uriId = Convert.ToInt32(Session["PRIVATEURID"]);
                        Admin admin = db.Adminler.First(x => x.Id == adminId);
                        admin.Sifre = sd.YeniSifre;
                        db.Entry(admin).State = EntityState.Modified;
                        PrivateUrl uri = db.PrivateUrls.First(x => x.Id == uriId);
                        db.PrivateUrls.Remove(uri);
                        db.SaveChanges();
                        return RedirectToAction("Login");
                    }
                }
                else
                {
                    ViewBag.msg = "Girdiğiniz şifreler aynı değil.";
                    return View();
                }
            }
            return View(sd);
        }

        public ActionResult GostergePaneli()
        {
            return View();
        }

        public ActionResult Ayarlar()
        {
            return View();
        }


        // --- URUN YONETIMI

        public ActionResult UrunListesi()
        {
            return View();
        }



        public ActionResult UrunEdit(int Id)
        {
            using (var db = new DatabaseContext())
            {
                UrunModel urunModel = new UrunModel();

                Urun urun = db.Urunler.FirstOrDefault(x => x.Id == Id);
                if (urun != null)
                {
                    UrunDetay urunDetay = db.UrunDetaylari.First(x => x.Urun.Id == urun.Id);
                    List<SelectListItem> kategoriler = new List<SelectListItem>();
                    foreach (var kategori in db.Kategoriler.ToList())
                    {
                        SelectListItem item = new SelectListItem();

                        if (kategori.Id == urunDetay.Kategori.Id)
                        {
                            item.Selected = true;
                            urunModel.KategoriId = kategori.Id;
                        }

                        item.Text = kategori.Ad;
                        item.Value = kategori.Id.ToString();
                        kategoriler.Add(item);
                    }
                    ViewData["kategoriler"] = kategoriler;

                    urunModel.Id = urun.Id;
                    urunModel.Ad = urun.Ad;
                    urunModel.KategoriAd = urunDetay.Kategori.Ad;
                    urunModel.PicPath = urunDetay.FotografYolu;
                    urunModel.Stok = urunDetay.Stok;
                    urunModel.Aciklama = urunDetay.Aciklama;
                    urunModel.Fiyat = Convert.ToDecimal(urunDetay.Fiyat);
                    Session["URUNEDITID"] = urun.Id;
                    return View(urunModel);
                }
                else return RedirectToAction("UrunListesi", "Panel");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UrunEdit(UrunModel um)
        {
            using (var db = new DatabaseContext())
            {
                if (ModelState.IsValid)
                {

                    UrunDetay urunDetay = db.UrunDetaylari.First(x => x.Id == um.Id);
                    Urun urun = urunDetay.Urun;

                    urun.Ad = um.Ad;
                    urunDetay.Aciklama = um.Aciklama;
                    urunDetay.Kategori = db.Kategoriler.First(x => x.Id == um.kategoriler);
                    urunDetay.Stok = um.Stok;
                    urunDetay.Fiyat = um.Fiyat;
                    db.Entry(urunDetay).State = EntityState.Modified;
                    db.Entry(urun).State = EntityState.Modified;
                    db.SaveChanges();

                    ViewBag.msg = "Ürün başarılı bir şekilde güncelleştirilmiştir.";
                }

                List<SelectListItem> kategoriler = new List<SelectListItem>();
                foreach (var kategori in db.Kategoriler.ToList())
                {
                    SelectListItem item = new SelectListItem();

                    if (kategori.Id == um.KategoriId)
                    {
                        item.Selected = true;
                    }

                    item.Text = kategori.Ad;
                    item.Value = kategori.Id.ToString();
                    kategoriler.Add(item);
                }

                ViewData["kategoriler"] = kategoriler;
                return View(um);
            }
        }

        public ActionResult YeniUrun()
        {
            using (var db = new DatabaseContext())
            {
                List<SelectListItem> kategoriler = new List<SelectListItem>();

                foreach (var kategori in db.Kategoriler.ToList())
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = kategori.Ad;
                    item.Value = kategori.Id.ToString();
                    kategoriler.Add(item);
                }

                ViewData["kategoriler"] = kategoriler;

                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult YeniUrun(UrunEkleModel um, HttpPostedFileBase Resim)
        {
            if (ModelState.IsValid && Resim != null)
            {
                string path = Server.MapPath("~/DatabaseFiles/ProductFiles/" + Resim.FileName);
                Resim.SaveAs(path);

                string yol = "/DatabaseFiles/ProductFiles/" + Resim.FileName;

                using (var db = new DatabaseContext())
                {
                    Urun urun = new Urun();
                    UrunDetay urunDetay = new UrunDetay();

                    urun.Ad = um.Ad;
                    urunDetay.Aciklama = um.Aciklama;
                    urunDetay.FotografYolu = yol;
                    urunDetay.Kategori = db.Kategoriler.First(x => x.Id == um.kategoriler);
                    urunDetay.Stok = um.Stok;
                    urunDetay.Fiyat = Convert.ToDecimal(um.Fiyat);
                    db.Urunler.Add(urun);
                    urunDetay.Urun = urun;
                    db.UrunDetaylari.Add(urunDetay);
                    db.SaveChanges();


                    Random rndm = new Random();
                    string gtin = "";
                    string[] harfList = { "a", "A", "x", "Z", "X", "z", "U", "y", "Y", "u", "v", "V", "B", "b" };

                    for (int i = 0; i < 10; i++)
                    {
                        if (i % 2 == 0)
                        {
                            gtin += harfList[i];
                        }
                        else
                        {
                            gtin += rndm.Next(0, 9);
                        }
                    }

                    GtinNo gtinNo = new GtinNo();
                    gtinNo.Urun = urun;
                    gtinNo.Gtin = gtin;
                    db.GtinNo.Add(gtinNo);
                    db.SaveChanges();

                    ViewBag.msg = "Ürün başarılı bir şekilde eklenmiştir.";
                }

            }

            using (var db = new DatabaseContext())
            {
                List<SelectListItem> kategoriler = new List<SelectListItem>();

                foreach (var kategori in db.Kategoriler.ToList())
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = kategori.Ad;
                    item.Value = kategori.Id.ToString();
                    kategoriler.Add(item);
                }

                ViewData["kategoriler"] = kategoriler;
            }
            return View(um);
        }

        public ActionResult GtinSorgula()
        {
            return View();
        }

        // URUN YONETIMI ---


        // Kategori Yonetimi

        public ActionResult KategoriListesi()
        {
            return View();
        }

        public ActionResult KategoriEkle()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KategoriEkle(Kategori k)
        {
            if (ModelState.IsValid)
            {
                using (var db = new DatabaseContext())
                {
                    Kategori kategori = db.Kategoriler.FirstOrDefault(x => x.Ad == k.Ad);
                    if (kategori == null)
                    {
                        db.Kategoriler.Add(k);
                        db.SaveChanges();
                        ViewBag.msg = "Kategori başarılı bir şekilde eklendi.";
                    }
                    else
                        ViewBag.ms = "Böyle bir kategori mevcut.";
                }
            }
            return View(k);
        }

        public ActionResult KategoriyeAitUrunler(int Id)
        {
            using (var db = new DatabaseContext())
            {
                Kategori kategori = db.Kategoriler.First(x => x.Id == Id);
                ViewBag.kategoriAd = kategori.Ad;
                List<UrunModel> urunModelList = new List<UrunModel>();
                foreach (var item in db.UrunDetaylari.Where(x=>x.Kategori.Id == kategori.Id).ToList())
                {
                    Urun urun = db.Urunler.First(x => x.Id == item.Urun.Id);
                    UrunModel urunModel = new UrunModel();
                    urunModel.Ad = urun.Ad;
                    urunModel.Aciklama = item.Aciklama;
                    urunModel.Stok = item.Stok;
                    urunModel.Id = item.Id;
                    urunModel.PicPath = item.FotografYolu;
                    urunModelList.Add(urunModel);
                }
                return View(urunModelList);
            }
        }


        // Kategori Yonetimi ---





        public ActionResult SiparisListesi()
        {
            return View();
        }

        public ActionResult SiparisBul()
        {
            return View();
        }


        // YARARLI LINKLER

        public ActionResult UrunListesiExcell()
        {
            using (var db = new DatabaseContext())
            {
                ViewBag.urunCount = db.Urunler.Count();
            }

            return View();
        }

        public ActionResult TopluUrunEkleExcell()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TopluUrunEkleExcell(HttpPostedFileBase excellFile)
        {
            if (excellFile != null)
            {
                if (excellFile.FileName.EndsWith("xls") || excellFile.FileName.EndsWith("xlsx"))
                {
                    string path = "~/DatabaseFiles/ExcellFiles/" + excellFile.FileName;

                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);

                    excellFile.SaveAs(Server.MapPath(path));

                    path = Server.MapPath("~/DatabaseFiles/ExcellFiles/" + excellFile.FileName);
                    try
                    {
                        string errorText = "";
                        List<Urun> urunList = new List<Urun>();
                        List<UrunDetay> urunDetayList = new List<UrunDetay>();
                        Microsoft.Office.Interop.Excel.Application application =
                            new Microsoft.Office.Interop.Excel.Application();
                        Microsoft.Office.Interop.Excel.Workbook workbook = application.Workbooks.Open(path);
                        Microsoft.Office.Interop.Excel.Worksheet worksheet = workbook.ActiveSheet;
                        Microsoft.Office.Interop.Excel.Range range = worksheet.UsedRange;
                        using (var db = new DatabaseContext())
                        {
                            for (int row = 2; row < range.Rows.Count; row++)
                            {
                                string urunAd = ((Excel.Range)range.Cells[row, 1]).Text;
                                Urun urun = db.Urunler.FirstOrDefault(x => x.Ad == urunAd);
                                if (urun == null)
                                {
                                    urun.Ad = urunAd;
                                    UrunDetay urunDetay = new UrunDetay();
                                    urunDetay.Aciklama = ((Excel.Range) range.Cells[row, 2]).Text;
                                    urunDetay.FotografYolu = ((Excel.Range) range.Cells[row, 3]).Text;
                                    string ktgAd = ((Excel.Range) range.Cells[row, 4]).Text;
                                    Kategori kategori = db.Kategoriler.FirstOrDefault(x => x.Ad == ktgAd);
                                    if (kategori == null)
                                    {
                                        kategori = new Kategori();
                                        kategori.Ad = ktgAd;
                                        db.Kategoriler.Add(kategori);
                                        db.SaveChanges();
                                    }

                                    urunDetay.Kategori = db.Kategoriler.First(x => x.Ad == ktgAd);
                                    urunDetay.Stok = int.Parse(((Excel.Range) range.Cells[row, 5]).Text);
                                    urunDetay.Fiyat = Convert.ToDecimal(((Excel.Range) range.Cells[row, 6]).Text);
                                    urunList.Add(urun);
                                    urunDetay.Urun = urun;
                                    urunDetayList.Add(urunDetay);
                                }
                                else
                                {
                                    errorText += "- '" + urunAd + "' adına sahip bir ürün bulunmaktadır. \n";
                                }
                            }

                            foreach (var urun in urunList)
                            {
                                db.Urunler.Add(urun);
                            }

                            foreach (var urunDetay in urunDetayList)
                            {
                                db.UrunDetaylari.Add(urunDetay);
                            }

                            db.SaveChanges();
                            application.Workbooks.Close();
                            if (errorText != "")
                                ViewBag.errorList = errorText;
                        }
                    }
                    catch(Exception ex)
                    {
                        //ViewBag.msg =
                        //    "Bir hata oluştu. Lütfen EXCELL dosyanızın sistemimize uyumluluğunu kontrol edin ve tekrar deneyin.";
                        ViewBag.msg = ex.ToString();
                    }
                }
                else
                {
                    ViewBag.msg = "Lütfen Excell dosyası yükleyiniz.";
                }


            }
            else
                ViewBag.msg = "Lütfen EXCELL dosyanızı seçiniz.";
            return View();
        }

        public ActionResult SiparisListesiExcell()
        {
            using (var db = new DatabaseContext())
            {
                ViewBag.siparisCount = db.Siparisler.Count();
            }
            return View();
        }


        // YARARLI LINKLER ---

    }
}