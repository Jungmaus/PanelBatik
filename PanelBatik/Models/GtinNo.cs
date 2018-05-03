using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PanelBatik.Models
{
    [Table("GtinNo")]
    public class GtinNo
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(25,ErrorMessage ="Gtin Numarası verisi en fazla 25 karakter olabilir."),Required(ErrorMessage ="Gtin Numarası gereklidir.")]
        public string Gtin { get; set; }

        public virtual Urun Urun { get; set; }

    }
}