using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace Sitefinity.Training.Web.UI.Mvc.Controllers
{
    /// <summary>
    /// Controller externo para demostrar funcionalidad de patrón de diseño por capas
    /// </summary>
    [ControllerToolboxItem(Name ="SystemInfo", Title ="System Info", SectionName ="Widgets Externos")]
    public class SystemInfoController : Controller
    {
        [DisplayName("Mensaje del widget")]
        public string Message {  get; set; }

        public ActionResult Index()
        {
            ViewBag.OS=Environment.OSVersion;
            ViewBag.Message=Message;
            return View("Index");

        }
        /// <summary>
        /// Otra forma de hacer APIs en Sitefinity de manera rápida.
        /// Message no cargará a través de Route, ya que depende del estado del widget.
        /// </summary>
        /// <returns></returns>
        [Route("~/json/system-info")]
        public JsonResult ShortInfo()
        {
            var dynamic = new { OS = Environment.OSVersion.ToString(), Message };
            return Json(dynamic, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Acceso a la misma información mediante Widget de sitefinity.
        /// Aquí cargará Message.
        /// Accesible mediante http://localhost:1969/designer-test/Info, es decir, la url donde está el widget+el nombre del método
        /// </summary>
        /// <returns></returns>
        public JsonResult Info()
        {
            var dynamic = new { OS = Environment.OSVersion.ToString(), Message };
            return Json(dynamic, JsonRequestBehavior.AllowGet);
        }

        //Permite que el widget se mantenga visible ante acciones impredecibles
        //Ejemplo: cargar una noticia en la misma página, no en una nueva
        protected override void HandleUnknownAction(string actionName)
        {
            ActionInvoker.InvokeAction(ControllerContext, "Index");
        }
    }
}
