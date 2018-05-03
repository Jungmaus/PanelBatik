using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PanelBatik.Models
{
    [Table("BankaHesaplari")]
    public class BankaHesabi
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity),Key]
        public int Id { get; set; }

        [StringLength(30),Required]
        public string HesapAdi { get; set; }

        public virtual Musteri Musteri { get; set; }


    }
}