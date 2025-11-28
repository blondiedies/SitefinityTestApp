using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Sitefinity.Frontend.Media.Mvc.Models.Image;

namespace SitefinityWebApp.Mvc.ViewModels
{
    public class OfficeViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Info { get; set; }
        public ImageViewModel Picture { get; set; }
        public List<ImageViewModel> Gallery { get; set; }
    }
}