using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PanelBatik.Models.OperationClass
{
    public class UrunModel
    {
        public int Id { get; set; }

        [StringLength(30, ErrorMessage = "Urun Ad verisi en fazla 30 karakter olabilir."), Required(ErrorMessage = "Urun Ad verisi gereklidir.")]
        public string Ad { get; set; }

        [StringLength(250,ErrorMessage = "Aciklama verisi en fazla 250 karakter olabilir.")]
        public string Aciklama { get; set; }

        public int Stok { get; set; }

        public string PicPath { get; set; }

        public string KategoriAd { get; set; }

        [Required]
        public decimal Fiyat { get; set; }

        public int KategoriId { get; set; }

        public int kategoriler { get; set; }

    }
}