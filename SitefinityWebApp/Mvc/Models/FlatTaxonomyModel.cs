using ServiceStack;
using SitefinityWebApp.Mvc.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using Telerik.Sitefinity.Taxonomies;
using Telerik.Sitefinity.Taxonomies.Model;

namespace SitefinityWebApp.Mvc.Models
{
    /// <summary>
    /// Modelo para widget creado from scratch.
    /// </summary>
    public class FlatTaxonomyModel
    {
        private readonly TaxonomyManager taxonomyManager;
        private List<TaxaCount> taxaCounts = new List<TaxaCount>();

        public FlatTaxonomyModel()
        {
            taxonomyManager = TaxonomyManager.GetManager();
            taxaCounts = taxonomyManager.GetTaxa<FlatTaxon>().GroupBy(t => t.TaxonomyId).Select(g => new TaxaCount() { TaxonomyId = g.Key, Count = g.Count() }).ToList();

        }

        public List<FlatTaxonomyViewModel> Taxonomies
        {
            private set; get;
        }

        /// <summary>
        /// Al trabajar con taxonomies, usamos el taxonomymanager
        /// </summary>
        public void Populate()
        {
            //Instanciamos
            Taxonomies = taxonomyManager.GetTaxonomies<FlatTaxonomy>().Select(t => ToViewModel(t)).ToList();
        }
        private FlatTaxonomyViewModel ToViewModel(FlatTaxonomy t)
        {
            var taxaCount = taxaCounts.FirstOrDefault(tax => tax.TaxonomyId == t.Id);
            //Title.Value es específico para cada cultura.
            return new FlatTaxonomyViewModel() { Id = t.Id, Name = t.Title.Value, TaxaCount = taxaCount != null ? taxaCount.Count : 0 };
        }
    }
}