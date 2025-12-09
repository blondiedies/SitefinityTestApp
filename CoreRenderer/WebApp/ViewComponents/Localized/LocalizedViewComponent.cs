using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Progress.Sitefinity.AspNetCore.ViewComponents;

namespace WebApp.ViewComponents.Localized
{
    /// <summary>
    /// Test widget with different kind of restrictions for its properties.
    /// </summary>
    [SitefinityWidget(Title = "Localized Widget", Category = WidgetCategory.Content)]
    public class LocalizedViewComponent : ViewComponent
    {
        private IStringLocalizer<LocalizedViewComponent> localizer;

        public LocalizedViewComponent(IStringLocalizer<LocalizedViewComponent> localizer)
        {
            this.localizer = localizer;
        }

        /// <summary>
        /// Invokes the view.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public IViewComponentResult Invoke()
        {
            // when a Sitefinity page is executed the CultureInfo.CurrentUICulture is automatically populated
            // with the culture the page is translated on, so the string here would be resolved automatically
            var localizedString = localizer.GetString("Hello World!");
            return View("Default", localizedString.Value);
        }
    }
}
