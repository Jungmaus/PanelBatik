using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PanelBatik.Models.OperationClass
{
    public class KisiselAyarModel
    {

        [StringLength(25, ErrorMessage = "Ad en fazla 25 karakter olabilir."), Required(ErrorMessage = "Ad verisi gereklidir.")]
        public string Ad { get; set; }

        [StringLength(25, ErrorMessage = "Soyad en fazla 25 karakter olabilir."), Required(ErrorMessage = "Soyad verisi gereklidir.")]
        public string Soyad { get; set; }

        [StringLength(50, ErrorMessage = "Email en fazla 50 karakter olabilir."), Required(ErrorMessage = "Email verisi gereklidir.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Geçerli bir Email adresi girmelisiniz.")]
        public string Email { get; set; }

        [StringLength(30, ErrorMessage = "Telefon Numarası en fazla 14 karakter olabilir."), Required(ErrorMessage = "Telefon Numarası verisi gereklidir.")]
        public string TelNo { get; set; }

        [Column(TypeName = "VARCHAR"), StringLength(450)]
        public string FotografYolu { get; set; }


    }
}