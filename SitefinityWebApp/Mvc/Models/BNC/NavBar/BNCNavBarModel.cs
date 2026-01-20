using SitefinityWebApp.Mvc.ViewModels;
using SitefinityWebApp.Mvc.ViewModels.BNC;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Telerik.Sitefinity.Modules.Pages.PropertyPersisters;

namespace SitefinityWebApp.Mvc.Models.BNC.NavBar
{
    public class BNCNavBarModel :IValidatableObject
    {
        [PropertyPersistence(PersistAsJson = true)]
        public IList<BNCNavBarViewModel> MenuList { get; set; } = new List<BNCNavBarViewModel>();
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MenuList == null) yield break;

            // Find duplicate keys
            var duplicates = MenuList
                .Where(m => !string.IsNullOrWhiteSpace(m.Key))
                .GroupBy(m => m.Key.Trim().ToLowerInvariant())
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicates.Any())
            {
                // Error message
                var dupList = string.Join(", ", duplicates);
                yield return new ValidationResult(
                    $"Section key ya existe: {dupList}. Por favor seleccione otro.",
                    new[] { nameof(MenuList) }
                );
            }

            // Validaciones de requireds
            for (int s = 0; s < MenuList.Count; s++)
            {
                var section = MenuList[s];
                if (string.IsNullOrWhiteSpace(section.Key))
                {
                    yield return new ValidationResult($"La sección #{s + 1} tiene la clave vacía.", new[] { nameof(MenuList) });
                }
                if (string.IsNullOrWhiteSpace(section.Title))
                {
                    yield return new ValidationResult($"La sección #{s + 1} tiene el título vacío.", new[] { nameof(MenuList) });
                }
                if (section.Listado == null || section.Listado.Count == 0)
                {
                    yield return new ValidationResult($"La sección '{section.Title ?? section.Key}' debe contener al menos un enlace.", new[] { nameof(MenuList) });
                }
                else
                {
                    for (int i = 0; i < section.Listado.Count; i++)
                    {
                        var item = section.Listado[i];
                        if (string.IsNullOrWhiteSpace(item.LinkTitle))
                            yield return new ValidationResult($"Sección '{section.Title}': el enlace #{i + 1} necesita un título.", new[] { nameof(MenuList) });
                        if (string.IsNullOrWhiteSpace(item.Description))
                            yield return new ValidationResult($"Sección '{section.Title}': el enlace '{item.LinkTitle ?? ("#" + (i + 1))}' necesita una descripción corta.", new[] { nameof(MenuList) });
                        if (string.IsNullOrWhiteSpace(item.Url))
                            yield return new ValidationResult($"Sección '{section.Title}': el enlace '{item.LinkTitle ?? ("#" + (i + 1))}' necesita una URL.", new[] { nameof(MenuList) });
                    }
                }
            }

        }
        
        public BNCNavBarModel GetViewModel()
        {
            return this;
        }
    }

}