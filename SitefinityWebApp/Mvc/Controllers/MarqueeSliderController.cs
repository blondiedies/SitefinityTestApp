using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;
using SitefinityWebApp.Mvc.Models;

namespace SitefinityWebApp.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "marqueeSlider", Title = "Marquesina", SectionName = "BNC Widgets")]
    public class MarqueeSliderController : Controller
    {
        /*
         <div class="js-marquee" style="margin-right: 0px; float: left;">
<span class="ItemSpace f_black"><span class="B">B</span><span class="N">N</span><span class="C">C</span>, soluciones financieras justo a tu medida.</span><span class="ItemSpace f_black"><span class="USD bg-orange" style="margin-right: 10px;padding-bottom: 0.4rem;"><img alt="" src="/images/default-source/misc/32x32-icon-secure.png" width="24px" heigth="24px"></span>Utiliza plataformas electrónicas seguras y autorizadas.</span>|<span class="ItemSpace"></span><span class="ItemSpaceShort f-bold">Menudeo</span><span class="glyphicon glyphicon-chevron-right ItemSpaceShort f-s-8"></span>Fecha valor: <span class="ValueDate ItemSpace">12/12/2025</span><span class="ItemSpace"><span class="USD"> USD $ </span>Compra Bs: 267,7499 / Venta Bs: 270,4274</span><span class="ItemSpaceShort f-bold">Mesa de Cambio</span><span class="glyphicon glyphicon-chevron-right ItemSpaceShort f-s-8"></span>Fecha valor: <span class="ValueDate ItemSpace">05/12/2025</span><span class="ItemSpace"><span class="USD"> USD $ </span>TC Compra: 255,7108 / TC Venta: 259,6491</span><span class="ItemSpaceShort f-bold">Cotización del Petro</span><span class="glyphicon glyphicon-chevron-right ItemSpaceShort f-s-8"></span><span>1 Petro / 2.159,68</span>
</div>
         */
        // GET: MarqueeSlider
        public ActionResult Index()
        {
            var model = new MarqueeSliderModel();
            var viewmodel = model.GetViewModel();

            return View("Index",viewmodel);
        }
    }
}