using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PanelBatik.Models.OperationClass
{
    public class SifreYenileModel
    {
        [Required(ErrorMessage = "Email verisi gereklidir.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Geçerli bir Email adresi girmelisiniz.")]
        [MaxLength(50, ErrorMessage = "En fazla 50 karakter girebilirsiniz.")]
        public string Email { get; set; }
    }
}