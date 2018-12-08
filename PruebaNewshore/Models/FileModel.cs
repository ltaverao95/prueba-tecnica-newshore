using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PruebaNewshore.Models
{
    public class FileModel
    {
        [Required(ErrorMessage = "Por favor selecciona un archivo.")]
        [Display(Name = "Seleccionar archivo")]
        public HttpPostedFileBase[] files { get; set; }
    }
}