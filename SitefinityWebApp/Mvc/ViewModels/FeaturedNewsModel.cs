using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitefinityWebApp.Mvc.ViewModels
{

    public class FeaturedNewsResponse
    {
        [JsonProperty("@odata.context")]
        public string Context { get; set; }

        [JsonProperty("value")]
        public List<FeaturedNewsModel> Value { get; set; }
    }
    /// <summary>
    /// Modelo de información de la API de Sitefinity para noticias destacadas.
    /// </summary>
    public class FeaturedNewsModel
    {
        public Guid Id { get; set; }          // Maps to "Id"
        public string Title { get; set; }     // Maps to "Title"
        public string Content { get; set; }   // Maps to "Content"
        [JsonProperty(PropertyName = "ItemDefaultUrl")]
        public string Url { get; set; } //Maps URL
        public string Summary { get; set; } //Maps Summary

        // Collection because NewsImage is an array in JSON
        [JsonProperty("NewsImage")]
        public List<NewsImageViewModel> NewsImages { get; set; } = new List<NewsImageViewModel>();
    }
    /// <summary>
    /// Modelo para imágenes de noticias destacadas.
    /// </summary>
    public class NewsImageViewModel
    {
        public Guid Id { get; set; }              // Maps to "NewsImage.Id"
        public string Url { get; set; }           // Maps to "NewsImage.Url"
        public string ThumbnailUrl { get; set; }  // Maps to "NewsImage.ThumbnailUrl"
    }

}