using SitefinityWebApp.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace SitefinityWebApp.Mvc.Controllers
{
    /// <summary>
    /// Controlador que consume Sitefinity Native API para un content type custom (Offices)
    /// </summary>
    /// 
    [ControllerToolboxItem(Name = "Our Offices Widget", Title = "Our Offices", SectionName = "Custom Widgets")]
    public class OurOfficesController : Controller
    {
        private readonly OfficeModel officeModel;

        public OurOfficesController()
        {
            officeModel = new OfficeModel();
        }

        public ActionResult Index()
        {
            var viewModel = officeModel.GetOfficesViewModel();
            return View("Index", viewModel);
        }

        /// <summary>
        /// Crea una Oficina mediante Native API.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateOffice() {
            ViewBag.Result=officeModel.CreateOffice();
            return View("OfficeResult");
        }
    }
}