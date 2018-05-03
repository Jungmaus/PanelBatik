using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PanelBatik.Models
{
    [Table("Urunler")]
    public class Urun
    {

        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(30,ErrorMessage ="Urun Ad verisi en fazla 30 karakter olabilir."),Required(ErrorMessage ="Urun Ad verisi gereklidir.")]
        public string Ad { get; set; }

    }
}