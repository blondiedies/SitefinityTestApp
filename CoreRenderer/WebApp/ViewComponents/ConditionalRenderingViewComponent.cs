using Microsoft.AspNetCore.Mvc;
using Progress.Sitefinity.AspNetCore.ViewComponents;

namespace WebApp.ViewComponents
{
    /// <summary>
    /// ViewComponent para renderizado condicional en el editor  NET Core.
    /// </summary>
    [SitefinityWidget(Title = "Conditional Rendering Widget", Category = WidgetCategory.Content)]
    public class ConditionalRenderingViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IViewComponentContext context)
        {
            return View(context);
        }
    }
}