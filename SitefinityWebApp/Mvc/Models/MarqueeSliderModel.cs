using Newtonsoft.Json;
using SitefinityWebApp.Configuration;
using SitefinityWebApp.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Telerik.Sitefinity.Configuration;

namespace SitefinityWebApp.Mvc.Models
{

    public class MarqueeSliderResponse
    {
        [JsonProperty("@odata.context")]
        public string Context { get; set; }

        [JsonProperty("value")]
        public MarqueeSliderModel Value { get; set; }
    }
    public class MarqueeSliderModel
    {
        private readonly TasasConfig config;
        public MarqueeSliderModel() => config = Config.Get<TasasConfig>();

        public MarqueeSliderViewModel GetViewModel() => Task.Run(() => this.GetLaunchAsync()).Result;
        /// <summary>
        /// Realiza la llamada al API y obtiene los datos.
        /// </summary>
        /// <returns></returns>
        private async Task<MarqueeSliderViewModel> GetLaunchAsync()
        {
            if (config.IsActive)
            {
                string jsonString = "http://localhost:1969/api/featured-articles/newsitems/featured-article-and-image"; //placeholder 

                using (var client = new HttpClient()) //chequea q endpoint activo
                {
                    var response = await client.GetAsync(config.Endpoint);
                    if (response != null)
                    {
                        response.EnsureSuccessStatusCode();
                        jsonString = await response.Content.ReadAsStringAsync();

                    }
                    try
                    {
                        var wrapper = JsonConvert.DeserializeObject<MarqueeSliderViewModel>(jsonString);
                        return wrapper;
                    }
                    catch { return null; }
                }
            }
            else
            {
                var model = new MarqueeSliderViewModel();
                model.PetroCotizacion=config.DefaultValue;
                model.MesaCambioVenta = config.DefaultValue;
                model.MenudeoVenta = config.DefaultValue;
                model.MesaCambioCompra=config.DefaultValue;
                model.MenudeoCompra = config.DefaultValue;
                model.MenudeoFecha = DateTime.Now;
                model.MesaCambioFecha = DateTime.Now;
                return model;
            }
        }
    }
}