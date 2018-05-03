using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PanelBatik.Models;
using PanelBatik.Models.HomeModels;
using PanelBatik.Models.OperationClass;

namespace PanelBatik.Controllers
{
    public class DataController : Controller
    {

        public JsonResult GetSiparisTableData()
        {
            if(Session["Email"] != null) {
                using (var db = new DatabaseContext())
                {
                    List<SiparisModel> list = new List<SiparisModel>();

                    foreach (var item in db.Siparisler.OrderByDescending(x => x.Id).Take(100).ToList())
                    {
                        SiparisModel sm = new SiparisModel();
                        sm.Id = item.Id;
                        sm.SiparisKodu = item.SiparisKodu;
                        sm.Statu = item.Statu;
                        sm.MusteriAdSoyad = (item.Musteri.Ad + item.Musteri.Soyad);
                        SiparisDetay sd = db.SiparisDetaylari.FirstOrDefault(x => x.Siparis.Id == item.Id);
                        if (sd != null)
                        {
                            sm.Tarih = sd.Tarih;
                            sm.Adres = sd.Adres.AdresTanim;
                            sm.UrunAd = sd.Urun.Ad;
                            sm.UrunAdet = sd.UrunAdet;
                            list.Add(sm);
                        }
                    }
                    return Json(list, JsonRequestBehavior.AllowGet);
                }
            }
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetUrunListesiData()
        {
            if (Session["Email"] != null)
            {
                using (var db = new DatabaseContext())
                {
                    List<UrunModel> model = new List<UrunModel>();
                    List<Urun> urunList = db.Urunler.ToList();

                    foreach (var urun in urunList)
                    {
                        UrunModel um = new UrunModel();
                        UrunDetay urunDetay = db.UrunDetaylari.First(x => x.Urun.Id == urun.Id);

                        um.Id = urun.Id;
                        um.Ad = urun.Ad;
                        um.Aciklama = urunDetay.Aciklama;
                        um.KategoriAd = urunDetay.Kategori.Ad;
                        um.PicPath = urunDetay.FotografYolu;
                        um.Stok = urunDetay.Stok;
                        um.Fiyat = urunDetay.Fiyat;
                        model.Add(um);
                    }
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
            }
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetUrunData(int id)
        {
            if (Session["Email"] != null)
            {
                using (var db = new DatabaseContext())
                {
                    UrunModel urunModel = new UrunModel();
                    Urun urun = db.Urunler.FirstOrDefault(x => x.Id == id);
                    UrunDetay urunDetay = db.UrunDetaylari.FirstOrDefault(x => x.Urun.Id == urun.Id);
                    urunModel.Id = urun.Id;
                    urunModel.Ad = urun.Ad;
                    urunModel.PicPath = urunDetay.FotografYolu;
                    urunModel.Stok = urunDetay.Stok;
                    return Json(urunModel, JsonRequestBehavior.AllowGet);
                }
            }
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetUrunwGtin(string gtinNo)
        {
            if (Session["Email"] != null)
            {
                using (var db = new DatabaseContext())
                {
                    List<UrunModel> urunModelList = new List<UrunModel>();
                    List<GtinNo> gtinNoList = db.GtinNo.Where(x =>
                        x.Gtin.EndsWith(gtinNo) || x.Gtin.StartsWith(gtinNo) || x.Gtin.Contains(gtinNo) ||
                        x.Gtin == gtinNo).ToList();

                    foreach (var item in gtinNoList)
                    {
                        Urun urun = db.Urunler.FirstOrDefault(x => x.Id == item.Urun.Id);
                        if (urun != null)
                        {
                            UrunDetay urunDetay = db.UrunDetaylari.FirstOrDefault(x => x.Urun.Id == urun.Id);
                            UrunModel urunModel = new UrunModel();
                            urunModel.Id = urun.Id;
                            urunModel.Aciklama = urunDetay.Aciklama;
                            urunModel.Ad = urun.Ad;
                            urunModel.KategoriAd = db.Kategoriler.First(x => x.Id == urunDetay.Kategori.Id).Ad;
                            urunModel.KategoriId = db.Kategoriler.First(x => x.Id == urunDetay.Kategori.Id).Id;
                            urunModel.PicPath = urunDetay.FotografYolu;
                            urunModel.Stok = urunDetay.Stok;
                            urunModelList.Add(urunModel);

                        }
                    }
                    return Json(urunModelList, JsonRequestBehavior.AllowGet);
                }
            }
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetSiparisListesiData()
        {
            if (Session["Email"] != null)
            {
                using (var db = new DatabaseContext())
                {
                    List<SiparisModel> siparisModel = new List<SiparisModel>();

                    foreach (var item in db.SiparisDetaylari.OrderByDescending(x=>x.Tarih).ToList())
                    {
                        Siparis siparis = item.Siparis;

                        SiparisModel model = new SiparisModel();
                        model.Id = siparis.Id;
                        model.Adres = item.Adres.AdresTanim;
                        model.MusteriAdSoyad = (siparis.Musteri.Ad + siparis.Musteri.Soyad);
                        model.SiparisKodu = siparis.SiparisKodu;
                        model.Statu = siparis.Statu;
                        model.Tarih = item.Tarih;
                        model.UrunAd = item.Urun.Ad;
                        model.UrunAdet = item.UrunAdet;
                        siparisModel.Add(model);
                    }

                    return Json(siparisModel, JsonRequestBehavior.AllowGet);
                }
            }
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SiparisBulData(string musteriAdSoyad, string siparisKodu)
        {
            if (Session["Email"] != null)
            {
                using (var db = new DatabaseContext())
                {
                    List<Siparis> siparisList =
                        db.Siparisler.Where(x =>
                            x.Musteri.Ad.StartsWith(musteriAdSoyad) || x.Musteri.Ad.Contains(musteriAdSoyad) ||
                            x.Musteri.Ad.EndsWith(musteriAdSoyad) || x.Musteri.Soyad.StartsWith(musteriAdSoyad) ||
                            x.Musteri.Soyad.Contains(musteriAdSoyad) || x.Musteri.Soyad.EndsWith(musteriAdSoyad) ||
                            x.SiparisKodu == siparisKodu).ToList();
                    List<SiparisModel> siparisModelList = new List<SiparisModel>();
                    foreach (var siparis in siparisList)
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
                    return Json(siparisModelList, JsonRequestBehavior.AllowGet);
                }
            }
            else
                return Json(null, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetKategoriList()
        {
            if (Session["Email"] != null)
            {
                using (var db = new DatabaseContext())
                {
                    List<Kategori> kategoriList = db.Kategoriler.ToList();
                    return Json(kategoriList, JsonRequestBehavior.AllowGet);
                }
            }
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        // Home Başlar

        public JsonResult GetLeftProducts()
        {
            using (var db = new DatabaseContext())
            {
                List<LeftModel> model = new List<LeftModel>();

                foreach (var urunDetay in db.UrunDetaylari.OrderByDescending(x=>x.Id).Take(3).ToList())
                {
                    LeftModel leftModel = new LeftModel();
                    Urun urun = db.Urunler.First(x => x.Id == urunDetay.Urun.Id);
                    leftModel.Ad = urun.Ad;
                    leftModel.Fiyat = urunDetay.Fiyat;
                    leftModel.FotografYolu = urunDetay.FotografYolu;
                    leftModel.Id = urun.Id;
                    model.Add(leftModel);
                }
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetNewProducts()
        {
            using (var db = new DatabaseContext())
            {
                List<NewProducts> model = new List<NewProducts>();
                foreach (var urunDetay in db.UrunDetaylari.OrderByDescending(x=>x.Id).Take(8).ToList())
                {
                    NewProducts newProducts = new NewProducts();
                    Urun urun = db.Urunler.First(x => x.Id == urunDetay.Urun.Id);
                    newProducts.Id = urun.Id;
                    newProducts.ResimYolu = urunDetay.FotografYolu;
                    model.Add(newProducts);
                }
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetRandomProducts()
        {
            using (var db = new DatabaseContext())
            {
                List<RandomProducts> model = new List<RandomProducts>();

                foreach (var item in db.UrunDetaylari.OrderBy(x => Guid.NewGuid()).Take(3).ToList())
                {
                    RandomProducts randomProducts = new RandomProducts();
                    Urun urun = db.Urunler.First(x => x.Id == item.Urun.Id);
                    randomProducts.Id = item.Id;
                    randomProducts.Ad = urun.Ad;
                    randomProducts.Fiyat = item.Fiyat;
                    randomProducts.ResimYolu = item.FotografYolu;
                    model.Add(randomProducts);
                }
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }


    }
}