using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Sitefinity.Modules.News;
using Telerik.Sitefinity.Mvc;
using Telerik.Sitefinity.News.Model;

namespace SitefinityWebApp.Mvc.Controllers
{
    /// <summary>
    /// Controlador que usa Sitefinity Native API (NO un servicio Odata).
    /// </summary>
    [ControllerToolboxItem(Name="Articles Widget",Title ="Articles", SectionName ="Custom Widgets")]
    public class ArticlesController : Controller
    {
        // GET: Articles
        public ActionResult Index()
        {
            return View("ArticlesList", GetNewsItems());
        }

        private List<NewsItem> GetNewsItems()
        {
            var newsManager = NewsManager.GetManager();
            return newsManager.GetNewsItems()
                .Where(n=>n.Status == Telerik.Sitefinity.GenericContent.Model.ContentLifecycleStatus.Live && n.Visible).OrderBy(n=>n.PublicationDate).Take(3).ToList();
        }
    }
}