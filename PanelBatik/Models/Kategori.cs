using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PanelBatik.Models
{
    [Table("Kategoriler")]
    public class Kategori
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(30,ErrorMessage ="Kategori Adı verisi en fazla 30 karakter olabilir."),Required(ErrorMessage ="Kategori Adı verisi gereklidir.")]
        public string Ad { get; set; }

    }
}