using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace SitefinityWebApp.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "BNCNavBar", Title = "Barra de navegación", SectionName = "Widgets BNC - Figma")]
    public class BNCNavBarController : Controller
    {
        // GET: BNCNavBar
        public ActionResult Index()
        {
            return View();
        }
    }
}