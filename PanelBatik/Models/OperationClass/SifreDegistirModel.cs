using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PanelBatik.Models.OperationClass
{
    public class SifreDegistirModel
    {

        [StringLength(30, ErrorMessage = "Şifre en fazla 30 karakter olabilir."), Required(ErrorMessage = "Şifre verisi gereklidir.")]
        public string EskiSifre { get; set; }

        [StringLength(30, ErrorMessage = "Şifre en fazla 30 karakter olabilir."), Required(ErrorMessage = "Şifre verisi gereklidir.")]
        public string YeniSifre { get; set; }

        [StringLength(30, ErrorMessage = "Şifre en fazla 30 karakter olabilir."), Required(ErrorMessage = "Şifre verisi gereklidir.")]
        public string YeniSifreTekrar { get; set; }

    }
}