using Progress.Sitefinity.Renderer.Designers.Attributes;
using Progress.Sitefinity.Renderer.Entities.Content;
using SitefinityWebApp.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace SitefinityWebApp.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "productTabCard", Title = "Descripción de Producto con pestañas", SectionName = "BNC Widgets")]
    public class ProductTabCardController : Controller
    {
        // GET: ProductTabCard
        [Content(Type = "Telerik.Sitefinity.DynamicTypes.Model.Productos.Producto", LiveData = true, AllowMultipleItemsSelection = true)]
        [DisplayName("Selecciona productos a mostrar")]
        public MixedContentContext Product { get; set; }
        [DisplayName("Información de los productos")]
        public string Information { get; set; }
        public ActionResult Index()
        {
            var model = new ProductModel();
            var products = model.GetProductsViewModel(Product);
            ViewBag.Info = Information;
            return View("Index", products);
        }
    }
}