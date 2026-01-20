using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SitefinityWebApp.Mvc.ViewModels.BNC
{
    // Models/MenuViewModel.cs
    [TypeConverter(typeof(ExpandableObjectConverter))]

    public class NavBarMenuViewModel
    {
        [DisplayName("Título del enlace")]
        [Required(ErrorMessage = "El título del enlace es obligatorio.")]
        public string LinkTitle { get; set; }
        [DisplayName("Descripción corta")]
        [Required(ErrorMessage = "La descripción corta es obligatoria.")]
        public string Description { get; set; }
        [DisplayName("Enlace")]
        [Required(ErrorMessage = "El enlace (URL) es obligatorio.")]
        public string Url { get; set; }
        [DisplayName("Clase CSS del icono (opcional)")]
        public string IconClass { get; set; } // opcional
        [DisplayName("Objetivo (_blank, #, etc)")]
        public string Target { get; set; } // opcional ej: "_blank"
    }
}