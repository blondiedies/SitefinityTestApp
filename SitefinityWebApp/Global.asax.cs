using SitefinityWebApp.Configuration;
using SitefinityWebApp.Mvc.Models;
using System;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.DynamicModules.Events;
using Telerik.Sitefinity.Frontend;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Routing;
using Telerik.Sitefinity.Frontend.News.Mvc.Models;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Services;

namespace SitefinityWebApp
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            Bootstrapper.Initialized += Bootstrapper_Initialized;
            Bootstrapper.Bootstrapped += Bootstrapper_Bootstrapped;
        }

        private void Bootstrapper_Bootstrapped(object sender, EventArgs e)
        {
            EventHub.Subscribe<IDynamicContentCreatingEvent>(eventInfo => IDynamicContentCreatingEvent(eventInfo)); 
        }
/// <summary>
/// Evento que reemplaza el texto info de una noticia al ser creada.
/// </summary>
/// <param name="eventInfo"></param>
        private void IDynamicContentCreatingEvent(IDynamicContentCreatingEvent eventInfo)
        {
            var userId = eventInfo.UserId;
            var dynamicContentItem = eventInfo.Item;
            var officeModel = new OfficeModel();
            if (dynamicContentItem.GetType().Equals(officeModel.OfficeType))
            {
                dynamicContentItem.SetString("Info", "Replacement Test");
            }
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

    }
}