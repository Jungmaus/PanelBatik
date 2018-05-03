using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PanelBatik.Models
{
    [Table("SiparisDetaylari")]
    public class SiparisDetay
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DataType(DataType.DateTime),Required]
        public DateTime Tarih { get; set; }

        public virtual Siparis Siparis { get; set; }

        public virtual Adres Adres { get; set; }

        public virtual Urun Urun { get; set; }

        public int UrunAdet { get; set; }

    }
}