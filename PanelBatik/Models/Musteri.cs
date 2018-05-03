using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PanelBatik.Models
{
    [Table("Musteriler")]
    public class Musteri
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(25, ErrorMessage = "Ad en fazla 25 karakter olabilir."), Required(ErrorMessage = "Ad verisi gereklidir.")]
        public string Ad { get; set; }

        [StringLength(25, ErrorMessage = "Soyad en fazla 25 karakter olabilir."), Required(ErrorMessage = "Soyad verisi gereklidir.")]
        public string Soyad { get; set; }

        [StringLength(30, ErrorMessage = "Telefon Numarası en fazla 14 karakter olabilir."), Required(ErrorMessage = "Telefon Numarası verisi gereklidir."), DataType(DataType.PhoneNumber)]
        public string TelNo { get; set; }

        public virtual List<Siparis> Siparisler { get; set; }
        public virtual List<Adres> Adresler { get; set; }
        public virtual List<BankaHesabi> BankaHesaplari { get; set; }

    }
}