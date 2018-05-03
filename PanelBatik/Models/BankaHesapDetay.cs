using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PanelBatik.Models
{
    [Table("BankaHesapDetaylari")]
    public class BankaHesapDetay
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity),Key]   
        public int Id { get; set; }

        [StringLength(30),Required]
        public string IlgiliBanka { get; set; }

        [StringLength(26),Required]
        public string Iban { get; set; }

        public virtual BankaHesabi BankaHesabi { get; set; }

    }
}