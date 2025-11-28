using SitefinityWebApp.Configuration;
using SitefinityWebApp.Mvc.Models;
using System;
using Telerik.Microsoft.Practices.Unity;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Frontend;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Routing;
using Telerik.Sitefinity.Frontend.News.Mvc.Models;

namespace SitefinityWebApp
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            Bootstrapper.Initialized += Bootstrapper_Initialized;
        }

        private void Bootstrapper_Initialized(object sender, ExecutedEventArgs e)
        {
            if (e.CommandName == "Bootstrapped")
            {
                //Configuraciones de endpoints custom
                Config.RegisterSection<IntegrationConfig>();
                Config.RegisterSection<FeaturedNewsConfig>();
                //Config de modificaciones al widget News
                FrontendModule.Current.DependencyResolver.Rebind<INewsModel>().To<CategoryFilterNewsModel>();
               
            }
        }
        protected void Bootstrapper_Bootstrapped(object sender, ExecutedEventArgs e)
        {

        }

    }
}