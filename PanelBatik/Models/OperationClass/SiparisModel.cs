using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PanelBatik.Models.OperationClass
{
    public class SiparisModel
    {
        public int Id { get; set; }
        public string SiparisKodu { get; set; }
        public int Statu { get; set; }
        public string MusteriAdSoyad { get; set; }

        public DateTime Tarih { get; set; }
        public string Adres { get; set; }
        public string UrunAd { get; set; }
        public int UrunAdet { get; set; }


    }
}