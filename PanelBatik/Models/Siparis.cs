using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PanelBatik.Models
{
    [Table("Siparisler")]
    public class Siparis
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(30, ErrorMessage = "Sipariş Kodu en fazla 30 karakter olabilir."), Required]
        public string SiparisKodu { get; set; }

        public virtual Musteri Musteri { get; set; }

        public int Statu { get; set; }

    }
}