using Progress.Sitefinity.Renderer.Designers;
using Progress.Sitefinity.Renderer.Designers.Attributes;
using Progress.Sitefinity.Renderer.Entities.Content;
using SitefinityWebApp.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Libraries.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Modules.Libraries;
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
        /// <summary>
        /// Utiliza metodología vieja para widget designer
        /// </summary>
        #region Widget Designer exposed attributes

        public string SectionTitle { get; set; }

        public int ItemsCount { get ; set; }

        public bool ShowCreateButton { get; set; }

        public CategoryEnum OfficeCategory { get; set; }

        public Guid HeaderImage { get; set; }

        public DateTime? EventDate { get; set; }

        public enum CategoryEnum
        {
            All,
            Headquarters,
            Branch,
            Regional,
            Virtual
        }

        #endregion

        public ActionResult Index()
        {
            var offices = officeModel.GetOfficesViewModel();
            if (OfficeCategory!=CategoryEnum.All) {
                offices=offices.Where(o=>o.Info.Contains(OfficeCategory.ToString())).ToList();
            };

            var viewModel = offices.Take(ItemsCount > 0 ? ItemsCount : int.MaxValue).ToList();

            // Resolve to an Image object
           Image headerImage = null;
            if (HeaderImage != Guid.Empty)
            {
                var manager = LibrariesManager.GetManager();
                headerImage = manager.GetImage(HeaderImage);

            }

            ViewBag.HeaderImage = headerImage;
            ViewBag.SectionTitle = SectionTitle;
            ViewBag.ShowCreateButton = ShowCreateButton;
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