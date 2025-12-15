using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Sitefinity.Frontend.Media.Mvc.Models.Image;

namespace SitefinityWebApp.Mvc.ViewModels
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Descripcion { get; set; }
        public string Beneficios { get; set; }
        public ImageViewModel Imagen { get; set; }
    }
}