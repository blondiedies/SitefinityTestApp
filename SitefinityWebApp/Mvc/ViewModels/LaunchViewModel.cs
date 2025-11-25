using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitefinityWebApp.Mvc.ViewModels
{
    /// <summary>
    /// Modelo de información de los datos del vuelo de SpaceX.
    /// </summary>
    public class LaunchViewModel
    {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }
        [JsonProperty(PropertyName = "flight_number")]
        public string FlightNumber { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}