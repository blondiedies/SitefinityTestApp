using Progress.Sitefinity.AspNetCore.Web.Security;

namespace WebApp.Utilities
{
    /// <summary>
    /// Permite utilizar HtmlSanitize en este proyecto.
    /// </summary>
    public class CustomHtmlSanitizer : HtmlSanitizer
    {
        public CustomHtmlSanitizer()
            : base()
        {
            // add the tel scheme
            this.AllowedSchemes.Add("tel");
        }
    }
}
