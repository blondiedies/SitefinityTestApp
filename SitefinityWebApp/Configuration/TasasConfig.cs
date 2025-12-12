using System.Configuration;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Localization;

namespace SitefinityWebApp.Configuration
{
    public class TasasConfig : ConfigSection
    {
        [ConfigurationProperty("endpoint", IsRequired = true)]
        public string Endpoint
        {
            get
            {
                return (string)this["endpoint"];
            }
            set
            {
                this["endpoint"] = value;
            }
        }
        [ConfigurationProperty("isActive", DefaultValue = true)]
        [ObjectInfo(Description = "Determina si el endpoint se encuentra activo o si se utilizan los valores predeterminados", Title = "Is Active")]
        public bool IsActive
        {
            get
            {
                return (bool)this["isActive"];
            }
            set
            {
                this["isActive"] = value;
            }
        }
        [ConfigurationProperty("defaultValue", DefaultValue = 123.4f)]
        [ObjectInfo(Description = "Valor predeterminado para pruebas", Title = "Default")]
        public float DefaultValue
        {
            get
            {
                return (float)this["defaultValue"];
            }
            set
            {
                this["defaultValue"] = value;
            }
        }

    }
}