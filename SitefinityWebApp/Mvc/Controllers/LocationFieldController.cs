using SitefinityWebApp.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Sitefinity.Data.Metadata;
using Telerik.Sitefinity.Forms.Model;
using Telerik.Sitefinity.Frontend.Forms;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Controllers.Base;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Models.Fields.TextField;
using Telerik.Sitefinity.Modules.Forms.Web.UI.Fields;
using Telerik.Sitefinity.Modules.Pages.Web.Services;
using Telerik.Sitefinity.Mvc;
using Telerik.Sitefinity.Web.UI.Fields.Enums;

namespace SitefinityWebApp.Mvc.Controllers
{
    /// <summary>
    /// Widget de dirección para Forms
    /// </summary>
    [ControllerToolboxItem(Name ="LocationField",Title ="Location Field", Toolbox =FormsConstants.FormControlsToolboxName, SectionName =FormsConstants.CommonSectionName)]
    [DatabaseMapping(Telerik.Sitefinity.Model.UserFriendlyDataType.ShortText)] //tipo de dato en BD
    public class LocationFieldController : FormFieldControllerBase<LocationFieldModel>, ISupportRules, ITextField
    {
        private LocationFieldModel model;

        public LocationFieldController()
        {
            DisplayMode=FieldDisplayMode.Write;
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        [ReflectInheritedProperties]
        public override LocationFieldModel Model {
            get
            {
                if (model == null)
                {
                    model= new LocationFieldModel();
                }
                model.Zoom=ZoomLevel.City;
                return model;
            }
        }


        public TextType InputType
        {
            get 
            {return TextType.Text; 
                
            }
        }

        public string Title
        {
            get
            {
                return "Location Field";
            }
        }

        IDictionary<ConditionOperator, string> ISupportRules.Operators 
        {
            get
            {
                return new Dictionary<ConditionOperator, string>
                {
                    [ConditionOperator.Equal]="equal",
                    [ConditionOperator.NotEqual]="not equal"
                };
            }
        }

    }
}