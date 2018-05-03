using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PanelBatik.Models
{
    [Table("UrunDetaylari")]
    public class UrunDetay
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(250, ErrorMessage = "Aciklama verisi en fazla 250 karakter olabilir.")]
        public string Aciklama { get; set; }

        [Required]
        public decimal Fiyat { get; set; }

        public int Stok { get; set; }

        [Column(TypeName = "VARCHAR"), StringLength(450)]
        public string FotografYolu { get; set; }


        public virtual Kategori Kategori { get; set; }

        public virtual Urun Urun { get; set; }

    }
}