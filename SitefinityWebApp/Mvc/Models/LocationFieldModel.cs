using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Models.Fields;
using Telerik.Sitefinity.Metadata.Model;
using Telerik.Sitefinity.Modules.Forms.Web.UI;

namespace SitefinityWebApp.Mvc.Models
{
    /// <summary>
    /// Modelo para widget de ubicación de un form
    /// </summary>
    public class LocationFieldModel : FormFieldModel, IHideable
    {

        public ZoomLevel Zoom {  get; set; }

        public bool Hidden { get; set; }

        public override object GetViewModel(object value, IMetaField metaField)
        {
            Value = value as string ?? MetaField.DefaultValue ?? string.Empty;
            return this;
        }
    }
}