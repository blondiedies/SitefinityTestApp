using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitefinityWebApp.Mvc.ViewModels
{
    /// <summary>
    /// Viewmodel para widget hecho from scratch.
    /// </summary>
    public class FlatTaxonomyViewModel
    {
        /// <summary>
        /// En Sitefinity, toda ID debe ser tipo Guid.
        /// </summary>
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int TaxaCount { get; set; }

    }
}