using System.Configuration;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Localization;

namespace SitefinityWebApp.Configuration
{
    public class IntegrationConfig : ConfigSection
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
        [ObjectInfo(Description = "Determines if the endpoint is active or not", Title = "Is Active")]
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

    }
}