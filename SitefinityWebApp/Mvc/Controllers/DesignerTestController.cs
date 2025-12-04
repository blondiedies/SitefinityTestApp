using Progress.Sitefinity.Renderer.Designers.Attributes;
using Progress.Sitefinity.Renderer.Entities.Content;
using SitefinityWebApp.Mvc.Models;
using System.ComponentModel;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace SitefinityWebApp.Mvc.Controllers
{
/// <summary>
/// Widget utilizando metodología built-in de sitefinity para widget designer
/// </summary>
    [ControllerToolboxItem(Name = "Designer Test Widget", Title = "Designer Test", SectionName = "Custom Widgets")]
    public class DesignerTestController : Controller
    {
        [Content(Type="Telerik.Sitefinity.DynamicTypes.Model.Meettheteam.Office", LiveData = true)]
        [DisplayName("Select offices")]
        public MixedContentContext Offices { get; set; }
        // GET: DesignerTest
        public ActionResult Index()
        {
            var model = new OfficeModel();
            return View(model.GetOfficesViewModel(Offices));
        }
    }
}