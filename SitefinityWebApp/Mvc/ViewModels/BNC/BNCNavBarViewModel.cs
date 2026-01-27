using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SitefinityWebApp.Mvc.ViewModels.BNC
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class BNCNavBarViewModel
    {
        public BNCNavBarViewModel() { Listado = new List<NavBarMenuViewModel>(); }
        
        [DisplayName("Section key")]
        [Required(ErrorMessage = "Section key obligatoria")]
        [RegularExpression(@"^[\w\-]+$", ErrorMessage = "La clave solo puede contener letras, números, guiones bajos y guiones. Sin espacios.")]
        [Description("Identificación de la sección del menú en URL. Sin espacios.")]
        public string Key { get; set; }
        [Required(ErrorMessage = "Título de sección obligatorio")]

        [DisplayName("Título de sección")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Enlaces obligatorios")]

        [DisplayName("Listado de enlaces")]
        public List<NavBarMenuViewModel> Listado { get; set; }
        [DisplayName("Enlace")]
        [Description("Enlace individual. Sólo seleccionar si Listado de enlaces será vacío.")]
        public string Url { get; set; }

        public string Target { get; set; }
    }
}