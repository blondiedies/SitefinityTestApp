

using SitefinityWebApp.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Telerik.Sitefinity;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.DynamicModules.Builder;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Frontend.Media.Mvc.Models.Image;
using Telerik.Sitefinity.GeoLocations.Model;
using Telerik.Sitefinity.Libraries.Model;
using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Locations;
using Telerik.Sitefinity.Locations.Configuration;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Modules.Libraries;
using Telerik.Sitefinity.RelatedData;
using Telerik.Sitefinity.Security;
using Telerik.Sitefinity.Utilities.TypeConverters;
using Telerik.Sitefinity.Versioning;

using Telerik.Sitefinity.Data.Linq.Dynamic;
using System.Activities.Statements;
using Elastic.Clients.Elasticsearch;

namespace SitefinityWebApp.Mvc.Models
{
    public class OfficeModel
    {
        public Type OfficeType => TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.Meettheteam.Office");

        private string ProviderName
        {
            get
                ; set;
        }
        public OfficeModel()
        {
            var dynType = ModuleBuilderManager.GetActiveTypes().FirstOrDefault(t => t.FullTypeName == OfficeType.FullName);
            ProviderName = DynamicModuleManager.GetDefaultProviderName(dynType.ModuleName);
        }

        protected DynamicModuleManager GetManager()
        {
            return DynamicModuleManager.GetManager(ProviderName);
        }

        public List<OfficeViewModel> GetOfficesViewModel()
        {
            var offices = GetManager().GetDataItems(OfficeType).Where(o=>o.Status == Telerik.Sitefinity.GenericContent.Model.ContentLifecycleStatus.Live && o.Visible);

            //optimiza la llamada de datos de BD
            offices.SetRelatedDataSourceContext();

            return offices.Select(o=>ToViewModel(o)).OrderBy(i=>i.Title).ToList();

        }

        private OfficeViewModel ToViewModel(DynamicContent office)  =>
        
            new OfficeViewModel
            {
                Id = office.Id,
                Title = office.GetString("Title").Value,
                Info = office.GetString("Info").Value,
                Picture= GetImageViewModel(office.GetRelatedItems<Image>("Picture").ToList())

            };

        private ImageViewModel GetImageViewModel(List<Image> relatedImages)
        {
            var image = new ImageViewModel();
            if (relatedImages.Any())
            {
                var relatedImage= relatedImages.First();
                image.Title=relatedImage.Title;
                    image.AlternativeText=relatedImage.AlternativeText;
                    image.ThumbnailUrl=relatedImage.ThumbnailUrl;
                    image.LinkedContentUrl=relatedImage.Url;
            }
            return image;
        }

        public string CreateOffice()
        {
            try { 
            //Código autogenerado por sitefinity:
            // Set a transaction name and get the version manager
            var transactionName = "someTransactionName";
            var versionManager = VersionManager.GetManager(null, transactionName);

            DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(ProviderName, transactionName);
            DynamicContent officeItem = dynamicModuleManager.CreateDataItem(OfficeType);

            // Set the culture name for the item fields
            var cultureName = "en";
            var culture = new CultureInfo(cultureName);

                // Wrap the following methods in a using statement using the culture you want to assign to the item
                using (new CultureRegion(culture))
                {
                    // This is how values for the properties are set
                    officeItem.SetString("Title", "Some Title");
                    officeItem.SetString("Info", "Some Info");

                    Address address = new Address();
                    CountryElement addressCountry = Config.Get<LocationsConfig>().Countries.Values.First(x => x.Name == "United States");
                    address.CountryCode = addressCountry.IsoCode;
                    address.StateCode = addressCountry.StatesProvinces.Values.First().Abbreviation;
                    address.City = "Some City";
                    address.Street = "Some Street";
                    address.Zip = "12345";
                    officeItem.SetValue("Address", address);

                    // Get related item manager
                    LibrariesManager pictureManager = LibrariesManager.GetManager();
                    var pictureItem = pictureManager.GetImages().FirstOrDefault(i => i.Status == Telerik.Sitefinity.GenericContent.Model.ContentLifecycleStatus.Master); //primera img en la librería
                    if (pictureItem != null)
                    {
                        // asociación de la img con la oficina
                        officeItem.CreateRelation(pictureItem, "Picture");
                    }

                    // Get related item manager
                    LibrariesManager galleryManager = LibrariesManager.GetManager();
                    var galleryItem = galleryManager.GetImages().FirstOrDefault(i => i.Status == Telerik.Sitefinity.GenericContent.Model.ContentLifecycleStatus.Master);
                    if (galleryItem != null)
                    {
                        // This is how we relate an item
                        officeItem.CreateRelation(galleryItem, "Gallery");
                    }
                    officeItem.SetString("UrlName", "SomeUrlName");
                    officeItem.SetValue("Owner", SecurityManager.GetCurrentUserId());
                    officeItem.SetValue("PublicationDate", DateTime.UtcNow);
                    officeItem.SetWorkflowStatus(dynamicModuleManager.Provider.ApplicationName, "Draft"); //se guarda como borrador, debe publicarse en sitefinity

                    // Create a version and commit the transaction in order changes to be persisted to data store
                    versionManager.CreateVersion(officeItem, false);
                    TransactionManager.CommitTransaction(transactionName);

                    // Use lifecycle so that LanguageData and other Multilingual related values are correctly created
                    DynamicContent checkOutOfficeItem = dynamicModuleManager.Lifecycle.CheckOut(officeItem) as DynamicContent;
                    DynamicContent checkInOfficeItem = dynamicModuleManager.Lifecycle.CheckIn(checkOutOfficeItem) as DynamicContent;
                    versionManager.CreateVersion(checkInOfficeItem, false);
                    TransactionManager.CommitTransaction(transactionName); }
            
            }
                catch(Exception e){
                return e.Message.ToString();

            }
            return "Office created";
        }
    }
}