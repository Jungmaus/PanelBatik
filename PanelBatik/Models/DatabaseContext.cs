using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using FakeData;

namespace PanelBatik.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Admin> Adminler { get; set; }
        public DbSet<Adres> Adresler { get; set; }
        public DbSet<GtinNo> GtinNo { get; set; }
        public DbSet<Kategori> Kategoriler { get; set; }
        public DbSet<Musteri> Musteriler { get; set; }
        public DbSet<Siparis> Siparisler { get; set; }
        public DbSet<SiparisDetay> SiparisDetaylari { get; set; }
        public DbSet<Urun> Urunler { get; set; }
        public DbSet<UrunDetay> UrunDetaylari { get; set; }
        public DbSet<BankaHesabi> BankaHesaplari { get; set; }
        public DbSet<BankaHesapDetay> BankaHesapDetaylari { get; set; }
        public DbSet<PrivateUrl> PrivateUrls { get; set; }


        public DatabaseContext()
        {
            Database.SetInitializer(new CreateDatabase());
        }

        public class CreateDatabase : CreateDatabaseIfNotExists<DatabaseContext>
        {
            public override void InitializeDatabase(DatabaseContext context) // Database oluşurken InitializeDatabase
            {
                base.InitializeDatabase(context);
            }

            protected override void Seed(DatabaseContext context) // Database oluştuktan sonra Seed
            {
                Admin admin = new Admin();
                admin.Ad = "Abdulsamet";
                admin.Soyad = "Şentürk";
                admin.TelNo = "905350285566";
                admin.Email = "sametsenturkkk@outlook.com";
                admin.Sifre = "sametbaba";
                admin.FotografYolu = "/DatabaseFiles/AdminPictures/ben.jpg";
                context.Adminler.Add(admin);

                context.SaveChanges();

                for (int i = 0; i < 50; i++)
                {
                    Musteri musteri = new Musteri();
                    musteri.Ad = FakeData.NameData.GetFirstName();
                    musteri.Soyad = FakeData.NameData.GetSurname();
                    musteri.TelNo = FakeData.PhoneNumberData.GetPhoneNumber();
                    context.Musteriler.Add(musteri);
                }

                context.SaveChanges();

                Kategori kategori = new Kategori();
                kategori.Ad = "T-shirt";
                context.Kategoriler.Add(kategori);
                kategori = new Kategori();
                kategori.Ad = "Gömlek";
                context.Kategoriler.Add(kategori);
                kategori = new Kategori();
                kategori.Ad = "Pantalon";
                context.Kategoriler.Add(kategori);

                context.SaveChanges();


                foreach (var item in context.Musteriler.ToList())
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Adres adres = new Adres();
                        adres.AdresTanim = FakeData.PlaceData.GetAddress();
                        adres.Musteri = item;
                        context.Adresler.Add(adres);
                    }
                }
                context.SaveChanges();
                for (int i = 0; i < 100; i++)
                {
                    Urun urun = new Urun();
                    urun.Ad = FakeData.NameData.GetCompanyName();
                    context.Urunler.Add(urun);
                }

                context.SaveChanges();

                foreach (var item in context.Urunler.ToList())
                {
                    Models.GtinNo gtinNo = new Models.GtinNo();
                    gtinNo.Gtin = "AB" + FakeData.NumberData.GetNumber(100, 10000) + "BA";
                    gtinNo.Urun = item;
                    context.GtinNo.Add(gtinNo);
                }

                context.SaveChanges();
                Random rndm = new Random();
                foreach (var item in context.Urunler.ToList())
                {
                    UrunDetay urunDetay = new UrunDetay();
                    urunDetay.FotografYolu = "/DatabaseFiles/ProductFiles/aaa.png";
                    int ctgId = rndm.Next(1, context.Kategoriler.Count() + 1);
                    Kategori ctg = context.Kategoriler.Find(ctgId);
                    urunDetay.Aciklama = "Uzun kollu t-shirt, siyah-gri renkleri mevcut.";
                    urunDetay.Kategori = ctg;
                    urunDetay.Stok = 100;
                    urunDetay.Fiyat = Convert.ToDecimal(10.10);
                    urunDetay.Urun = item;
                    context.UrunDetaylari.Add(urunDetay);
                }

                context.SaveChanges();

                foreach (var item in context.Musteriler.ToList())
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Siparis siparis = new Siparis();
                        siparis.SiparisKodu = FakeData.NumberData.GetNumber(100, 10000) + "A";
                        siparis.Musteri = item;
                        siparis.Statu = 0;
                        context.Siparisler.Add(siparis);
                    }
                }
                context.SaveChanges();

                int countInt;
                string[] bankaAdlari = new string[5];
                bankaAdlari[0] = "Garanti Bankası";
                bankaAdlari[1] = "Akbank Bankası";
                bankaAdlari[2] = "Ziraat Bankası";
                bankaAdlari[3] = "İş Bankası";
                bankaAdlari[4] = "Yapı Kredi Bankası";
                foreach (var item in context.Musteriler.ToList())
                {
                    for (int i = 0; i < 5; i++)
                    {
                        BankaHesabi bankaHesabi = new BankaHesabi();
                        BankaHesapDetay bankaHesapDetay = new BankaHesapDetay();

                        countInt = rndm.Next(0, 4);

                        bankaHesabi.HesapAdi = bankaAdlari[countInt];
                        bankaHesabi.Musteri = item;

                        bankaHesapDetay.IlgiliBanka = bankaAdlari[countInt];
                        countInt = rndm.Next(11111111, 899999999);
                        bankaHesapDetay.Iban = "TR" + countInt;
                        bankaHesapDetay.BankaHesabi = bankaHesabi;

                        context.BankaHesaplari.Add(bankaHesabi);
                        context.BankaHesapDetaylari.Add(bankaHesapDetay);

                    }
                }

                context.SaveChanges();

                foreach (var item in context.Siparisler.ToList())
                {
                    SiparisDetay siparisDetay = new SiparisDetay();
                    siparisDetay.Adres = context.Adresler.First(x => x.Musteri.Id == item.Musteri.Id );
                    siparisDetay.Tarih = FakeData.DateTimeData.GetDatetime();
                    siparisDetay.Siparis = item;
                    siparisDetay.Urun = context.Urunler.First();
                    context.SiparisDetaylari.Add(siparisDetay);
                }

                context.SaveChanges();

                base.Seed(context);
            }


        }


    }
}