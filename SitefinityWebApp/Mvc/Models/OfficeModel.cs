

using SitefinityWebApp.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.DynamicModules.Builder;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Frontend.Media.Mvc.Models.Image;
using Telerik.Sitefinity.Libraries.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.RelatedData;
using Telerik.Sitefinity.Utilities.TypeConverters;

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
    }
}