using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PanelBatik.Models
{
    [Table("PrivateUrls")]
    public class PrivateUrl
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity),Key]
        public int Id { get; set; }

        [StringLength(50),Required]
        public string Url { get; set; }

        public virtual Admin Admin { get; set; }

    }
}