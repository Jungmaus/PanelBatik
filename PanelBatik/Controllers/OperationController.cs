using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using PanelBatik.Models;
using PanelBatik.Models.OperationClass;

namespace PanelBatik.Controllers
{
    public class OperationController : Controller
    {
        [HttpPost]
        public void SiparisOnayla(int id)
        {
            if (Session["Email"] != null)
            {
                using (var db = new DatabaseContext())
                {
                    Siparis siparis = db.Siparisler.FirstOrDefault(x => x.Id == id);
                    if (siparis != null)
                    {
                        if (siparis.Statu != 1)
                        {
                            siparis.Statu = 1;
                            db.Entry(siparis).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }
            }
        }

        [HttpPost]
        public void SiparisKargola(int id)
        {
            using (var db = new DatabaseContext())
            {
                Siparis siparis = db.Siparisler.FirstOrDefault(x => x.Id == id);
                if (siparis != null)
                {
                    if (siparis.Statu != 2)
                    {
                        siparis.Statu = 2;
                        db.Entry(siparis).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
        }

        [HttpPost]
        public void SiparisIptal(int id)
        {
            using (var db = new DatabaseContext())
            {
                Siparis siparis = db.Siparisler.FirstOrDefault(x => x.Id == id);
                if (siparis != null)
                {
                    siparis.Statu = 3;
                    db.Entry(siparis).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void KisiselBilgilerUpdate(KisiselAyarModel kam)
        {
            if (ModelState.IsValid)
            {
                using (var db = new DatabaseContext())
                {
                    string adminMail = Session["Email"].ToString();
                    Admin admin = db.Adminler.First(x => x.Email == adminMail);
                    admin.Ad = kam.Ad;
                    admin.Email = kam.Email;
                    admin.Soyad = kam.Soyad;
                    admin.TelNo = kam.TelNo;
                    db.Entry(admin).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void SifreUpdate(SifreDegistirModel sd)
        {
            if (ModelState.IsValid)
            {
                using (var db = new DatabaseContext())
                {
                    string adminMail = Session["Email"].ToString();
                    Admin admin = db.Adminler.First(x => x.Email == adminMail);
                    if (sd.YeniSifre == sd.YeniSifreTekrar && sd.EskiSifre == admin.Sifre)
                    {
                        admin.Sifre = sd.YeniSifre;
                        db.Entry(admin).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        string a = "!";
                        int b = int.Parse(a);
                    }
                }
            }
        }

        [HttpPost]
        public void UrunSil(int Id)
        {
            using (var db = new DatabaseContext())
            {
                UrunDetay urunDetay = db.UrunDetaylari.First(x => x.Urun.Id == Id);
                Urun urun = db.Urunler.First(x => x.Id == Id);
                GtinNo gtinNo = db.GtinNo.FirstOrDefault(x => x.Urun.Id == urunDetay.Urun.Id);
                List<SiparisDetay> siparisDetayList = db.SiparisDetaylari.Where(x => x.Urun.Id == urun.Id).ToList();
                foreach (var item in siparisDetayList)
                    db.SiparisDetaylari.Remove(item);
                if (gtinNo != null)
                    db.GtinNo.Remove(gtinNo);
                db.UrunDetaylari.Remove(urunDetay);
                db.Urunler.Remove(urun);
                db.SaveChanges();
            }
        }



        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Panel");
        }

        public void UrunGridExportToExcel()
        {
            using (var db = new DatabaseContext())
            {
                DataTable dt = new DataTable();
                List<UrunModel> urunModelList = new List<UrunModel>();
                foreach (var urun in db.Urunler.ToList())
                {
                    UrunDetay urunDetay = db.UrunDetaylari.First(x => x.Urun.Id == urun.Id);
                    UrunModel urunModel = new UrunModel();

                    urunModel.Id = urun.Id;
                    urunModel.Ad = urun.Ad;
                    urunModel.Aciklama = urunDetay.Aciklama;
                    urunModel.Stok = urunDetay.Stok;
                    urunModel.KategoriAd = urunDetay.Kategori.Ad;
                    urunModel.PicPath = urunDetay.FotografYolu;
                    urunModel.Fiyat = urunDetay.Fiyat;
                    urunModelList.Add(urunModel);
                }

                dt.Columns.AddRange(new DataColumn[]
                {
                    new DataColumn("Ürün Ad"),
                    new DataColumn("Açıklama"),
                    new DataColumn("Resim Yolu"), 
                    new DataColumn("Kategori Adı"),
                    new DataColumn("Stok Adeti"),
                    new DataColumn("Ürün Fiyatı"), 
                });

                foreach (var item in urunModelList)
                {
                    dt.Rows.Add(item.Ad, item.Aciklama, item.PicPath, item.KategoriAd, item.Stok, item.Fiyat);
                }


                string dosyaAdi = "UrunListesi";
                var grid = new GridView();
                grid.DataSource = dt;
                grid.DataBind();

                Response.ClearContent();
                Response.Charset = "utf-8";
                Response.AddHeader("content-disposition", "attachment; filename=" + dosyaAdi + ".xls");

                Response.ContentType = "application/vnd.ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);

                Response.Write(sw.ToString());
                Response.End();
            }
        }

        public void SiparisGridExportToExcel()
        {
            using (var db = new DatabaseContext())
            {
                DataTable dt = new DataTable();
                List<SiparisModel> siparisModelList = new List<SiparisModel>();
                foreach (var siparis in db.Siparisler.ToList())
                {
                    SiparisDetay siparisDetay = db.SiparisDetaylari.First(x => x.Siparis.Id == siparis.Id);
                    SiparisModel siparisModel = new SiparisModel();

                    siparisModel.Id = siparis.Id;
                    siparisModel.Adres = siparisDetay.Adres.AdresTanim;
                    siparisModel.MusteriAdSoyad = (siparis.Musteri.Ad + siparis.Musteri.Soyad);
                    siparisModel.SiparisKodu = siparis.SiparisKodu;
                    siparisModel.Statu = siparis.Statu;
                    siparisModel.Tarih = siparisDetay.Tarih;
                    siparisModel.UrunAd = siparisDetay.Urun.Ad;
                    siparisModel.UrunAdet = siparisDetay.UrunAdet;
                    siparisModelList.Add(siparisModel);
                }

                dt.Columns.AddRange(new DataColumn[]
                {
                    new DataColumn("Id"),
                    new DataColumn("Sipariş Kodu"),
                    new DataColumn("Urun Adı"),
                    new DataColumn("Ürün Adeti"), 
                    new DataColumn("Müşteri Ad-Soyad"), 
                    new DataColumn("Tarih"), 
                    new DataColumn("Durum"), 
                });

                foreach (var item in siparisModelList)
                {
                    dt.Rows.Add(item.Id, item.SiparisKodu, item.UrunAd, item.UrunAdet, item.MusteriAdSoyad, item.Tarih,
                        item.Statu);
                }


                string dosyaAdi = "SiparisListesi";
                var grid = new GridView();
                grid.DataSource = dt;
                grid.DataBind();

                Response.ClearContent();
                Response.Charset = "utf-8";
                Response.AddHeader("content-disposition", "attachment; filename=" + dosyaAdi + ".xls");

                Response.ContentType = "application/vnd.ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);

                Response.Write(sw.ToString());
                Response.End();
            }
        }

        [HttpPost]
        public void KategoriSil(int Id)
        {
            using (var db = new DatabaseContext())
            {
                Kategori kategori = db.Kategoriler.FirstOrDefault(x => x.Id == Id);
                if (kategori != null)
                {
                    List<UrunDetay> urunDetayList = db.UrunDetaylari.Where(x => x.Kategori.Id == kategori.Id).ToList();
                    foreach (var urunDetay in urunDetayList)
                    {
                        Urun urun = urunDetay.Urun;
                        GtinNo gNo = db.GtinNo.First(x => x.Urun.Id == urun.Id);
                        List<SiparisDetay> siparisDetay = db.SiparisDetaylari.Where(x => x.Urun.Id == urun.Id).ToList();
                        foreach (var detay in siparisDetay)
                        {
                            Siparis siparis = db.Siparisler.First(x => x.Id == detay.Siparis.Id);
                            db.SiparisDetaylari.Remove(detay);
                            db.Siparisler.Remove(siparis);
                        }
                        db.GtinNo.Remove(gNo);
                        db.UrunDetaylari.Remove(urunDetay);
                        db.Urunler.Remove(urun);
                    }

                    db.Kategoriler.Remove(kategori);
                    db.SaveChanges();
                }
            }
        }


    }
}