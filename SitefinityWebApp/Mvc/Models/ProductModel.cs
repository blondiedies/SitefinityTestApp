using Progress.Sitefinity.Renderer.Entities.Content;
using SitefinityWebApp.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Data.Linq.Dynamic;
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
    public class ProductModel
    {
        #region Config de Sitefinity
        public Type ProductType => TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.Productos.Producto");

        public string ProviderName
        {
            get; set;
        }
        public ProductModel()
        {
            var dynType = ModuleBuilderManager.GetActiveTypes().FirstOrDefault(t => t.FullTypeName == ProductType.FullName);
            ProviderName = DynamicModuleManager.GetDefaultProviderName(dynType.ModuleName);
        }
        protected DynamicModuleManager GetManager()
        {
            return DynamicModuleManager.GetManager(ProviderName);
        }
        #endregion
        public List<ProductViewModel> GetProductsViewModel()
        {
            var Products = GetManager().GetDataItems(ProductType).Where(o => o.Status == Telerik.Sitefinity.GenericContent.Model.ContentLifecycleStatus.Live && o.Visible);

            //optimiza la llamada de datos de BD
            Products.SetRelatedDataSourceContext();

            return Products.Select(o => ToViewModel(o)).OrderBy(i => i.Title).ToList();

        }
        public List<ProductViewModel> GetProductsViewModel(MixedContentContext Products)
        {
            return ManagerBase.GetItems(Products, ProductType.FullName).OfType<DynamicContent>().Select(o => ToViewModel(o)).ToList();
        }

        private ProductViewModel ToViewModel(DynamicContent Product) =>

            new ProductViewModel
            {
                Id = Product.Id,
                Title = Product.GetString("Title").Value,
                Descripcion = Product.GetString("Descripcion").Value,
                Beneficios = Product.GetString("Beneficios").Value,
                Imagen = GetImageViewModel(Product.GetRelatedItems<Image>("Imagen").ToList())

            };

        private ImageViewModel GetImageViewModel(List<Image> relatedImages)
        {
            var image = new ImageViewModel();
            if (relatedImages.Any())
            {
                var relatedImage = relatedImages.First();
                image.Title = relatedImage.Title;
                image.AlternativeText = relatedImage.AlternativeText;
                image.ThumbnailUrl = relatedImage.ThumbnailUrl;
                image.LinkedContentUrl = relatedImage.Url;
            }
            return image;
        }

    }
}