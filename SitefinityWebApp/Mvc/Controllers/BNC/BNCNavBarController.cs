using Newtonsoft.Json;
using Progress.Sitefinity.Renderer.Designers.Attributes;
using Progress.Sitefinity.Renderer.Entities.Content;
using SitefinityWebApp.Mvc.Models.BNC.NavBar;
using SitefinityWebApp.Mvc.ViewModels.BNC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Mvc;
using Telerik.Sitefinity.RelatedData;
using Telerik.Sitefinity.Utilities.TypeConverters;
using Telerik.Sitefinity.Web.UI.Fields.Model;

namespace SitefinityWebApp.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "BNCNavBar", Title = "Barra de navegación", SectionName = "Widgets BNC - Figma")]
    public class BNCNavBarController : Controller
    {

        // Propiedad del controlador (tipo que espera el diseñador)
        [Content(Type = "Telerik.Sitefinity.DynamicTypes.Model.Menusuperior.Section", LiveData = true)]
        [DisplayName("Secciones del menú")]
        public MixedContentContext Sections { get; set; }

        // Index completo
        public ActionResult Index()
        {
            var model = new BNCNavBarModel { MenuList = new List<BNCNavBarViewModel>() };

            try
            {
                System.Diagnostics.Debug.WriteLine("=== BNCNavBarController.Index start ===");

                if (Sections == null)
                {
                    System.Diagnostics.Debug.WriteLine("Sections is null.");
                    return View("Index", model);
                }

                // 1) Extraer IDs seleccionados
                var selectedIds = GetSelectedIdsFromMixedContext(Sections).ToList();
                System.Diagnostics.Debug.WriteLine("Selected IDs: " + string.Join(",", selectedIds));
                if (!selectedIds.Any()) return View("Index", model);

                // 2) Resolver tipos dinámicos
                var sectionType = TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.Menusuperior.Section");
                var subsectionType = TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.Menusuperior.Subsection");
                if (sectionType == null) return View("Index", model);

                // 3) Detectar provider que contiene los IDs seleccionados
                DynamicModuleManager dynamicManager = null;
                string chosenProvider = null;
                var providerCandidates = new[] { "demobncmenusuperior", String.Empty, "OpenAccessProvider" }; // añade más si hace falta

                foreach (var provider in providerCandidates)
                {
                    try
                    {
                        var mgr = DynamicModuleManager.GetManager(provider);
                        var all = mgr.GetDataItems(sectionType).ToList();
                        if (all.Any(a => selectedIds.Contains(a.Id)))
                        {
                            dynamicManager = mgr;
                            chosenProvider = provider;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Provider '{provider}' init error: {ex.Message}");
                    }
                }

                if (dynamicManager == null)
                {
                    System.Diagnostics.Debug.WriteLine("No se encontró ningún provider con los IDs seleccionados.");
                    return View("Index", model);
                }

                System.Diagnostics.Debug.WriteLine("Usando provider: " + (chosenProvider ?? "(default)"));

                // 4) Cargar secciones y subsecciones (Master + Live)
                var allSections = dynamicManager.GetDataItems(sectionType)
                    .Where(d => d.Status == ContentLifecycleStatus.Master || d.Status == ContentLifecycleStatus.Live)
                    .ToList();

                var allSubsections = subsectionType != null
                    ? dynamicManager.GetDataItems(subsectionType)
                        .Where(d => d.Status == ContentLifecycleStatus.Master || d.Status == ContentLifecycleStatus.Live)
                        .ToList()
                    : new List<DynamicContent>();

                // 5) Filtrar por los IDs seleccionados y mapear
                var unorderedSections = allSections.Where(s => selectedIds.Contains(s.Id)).ToList();

                // Re-ordenar basado en la posición exacta dentro de selectedIds (el orden del diseñador)
                var sectionItems = unorderedSections
                    .OrderBy(s => selectedIds.IndexOf(s.Id))
                    .ToList();

                foreach (var section in sectionItems)
                {
                    try
                    {
                        var title = SafeGetString(section, "Title") ?? section.Id.ToString();
                        var enlace = JsonConvert.DeserializeObject<List<UrlJsonModel>>(SafeGetString(section, "Enlace")) ?? null;
                        var urlName = SafeGetString(section, "UrlName");
                        var key = !string.IsNullOrWhiteSpace(urlName) ? urlName : title;

                        var firstLink = enlace?.FirstOrDefault() ?? null;

                        string url = "", target = "";

                        if (firstLink != null)
                        {
                            url = firstLink.href;
                            target = firstLink.target;
                        }


                        var sectionVm = new BNCNavBarViewModel
                        {
                            Key = key,
                            Title = title,
                            Listado = new List<NavBarMenuViewModel>(),
                            Url = url,
                            Target= target
                        };

                        var subsectionLinks= section.GetRelatedItems("SubsectionLinks");

                        List<DynamicContent> subsectionItems = new List<DynamicContent>();
                        List<NavBarMenuViewModel> subsectionModeledItems = new List<NavBarMenuViewModel>();
                        foreach (var subsection in subsectionLinks) //add each individual url to the list
                        {
                            //get id
                            //get item from all subsections that matches id
                            subsectionItems.AddRange(allSubsections.Where(s => s.Id == subsection.Id).ToList());
                            //get info from this item and parse to navbarmenuviewmodel
                            var subsectionObject = subsectionItems.Where(s => s.Id == subsection.Id).First();

                            var urlList = JsonConvert.DeserializeObject<List<UrlJsonModel>>(SafeGetString(subsectionObject, "URL"));
                            var urlJson = urlList?.FirstOrDefault();

                            //parse to navbarmenuviewmodel
                            var subsectionVm = new NavBarMenuViewModel
                            {
                                LinkTitle= SafeGetString(subsectionObject, "Title"),
                                Description= SafeGetString(subsectionObject, "Description"),
                                Url= urlJson.href,
                                Target=urlJson.target
                            };

                            //add to listado
                            sectionVm.Listado.Add(subsectionVm);

                        }


                        model.MenuList.Add(sectionVm);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error mapping section {section.Id}: {ex}");
                    }
                }

                System.Diagnostics.Debug.WriteLine("Final model.MenuList count: " + model.MenuList.Count);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Unhandled error in Index: " + ex);
            }
            finally
            {
                System.Diagnostics.Debug.WriteLine("=== BNCNavBarController.Index end ===");
            }

            return View("Index", model);
        }

        // --- Helpers ---

        private IEnumerable<Guid> GetSelectedIdsFromMixedContext(object mixedContext)
        {
            if (mixedContext == null) return Enumerable.Empty<Guid>();
            var ids = new List<Guid>();
            var ctxType = mixedContext.GetType();

            var contentProp = ctxType.GetProperty("Content", BindingFlags.Public | BindingFlags.Instance);
            if (contentProp != null)
            {
                var contentArr = contentProp.GetValue(mixedContext) as System.Collections.IEnumerable;
                if (contentArr != null)
                {
                    foreach (var contentElem in contentArr)
                    {
                        if (contentElem == null) continue;
                        var elemType = contentElem.GetType();

                        var itemIdsProp = elemType.GetProperty("ItemIdsOrdered", BindingFlags.Public | BindingFlags.Instance);
                        if (itemIdsProp != null)
                        {
                            var arr = itemIdsProp.GetValue(contentElem) as IEnumerable<string>;
                            if (arr != null) foreach (var s in arr) if (Guid.TryParse(s, out Guid g)) ids.Add(g);
                        }

                        var filterProp = elemType.GetProperty("Filter", BindingFlags.Public | BindingFlags.Instance);
                        if (filterProp != null)
                        {
                            var filterVal = filterProp.GetValue(contentElem);
                            if (filterVal != null)
                            {
                                var kvType = filterVal.GetType();
                                var keyProp = kvType.GetProperty("Key");
                                var valueProp = kvType.GetProperty("Value");
                                if (keyProp != null && valueProp != null)
                                {
                                    var key = keyProp.GetValue(filterVal)?.ToString();
                                    var value = valueProp.GetValue(filterVal)?.ToString();
                                    if (!string.IsNullOrWhiteSpace(key) && key.Equals("Ids", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(value))
                                    {
                                        foreach (var part in value.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                                            if (Guid.TryParse(part.Trim(), out Guid g)) ids.Add(g);
                                    }
                                }
                            }
                        }

                        var manualProp = elemType.GetProperty("ManualSelectionItems", BindingFlags.Public | BindingFlags.Instance);
                        if (manualProp != null)
                        {
                            var manualColl = manualProp.GetValue(contentElem) as System.Collections.IEnumerable;
                            if (manualColl != null)
                            {
                                foreach (var mi in manualColl)
                                {
                                    if (mi == null) continue;
                                    var miType = mi.GetType();
                                    var idProp = miType.GetProperty("Id") ?? miType.GetProperty("ItemId") ?? miType.GetProperty("ContentId");
                                    if (idProp != null)
                                    {
                                        var idVal = idProp.GetValue(mi);
                                        if (idVal is Guid g) ids.Add(g);
                                        else if (Guid.TryParse(idVal?.ToString(), out Guid parsed)) ids.Add(parsed);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Top-level fallbacks
            if (!ids.Any())
            {
                var topItemIds = ctxType.GetProperty("ItemIdsOrdered", BindingFlags.Public | BindingFlags.Instance);
                if (topItemIds != null)
                {
                    var arr = topItemIds.GetValue(mixedContext) as IEnumerable<string>;
                    if (arr != null) foreach (var s in arr) if (Guid.TryParse(s, out Guid g)) ids.Add(g);
                }

                var topFilter = ctxType.GetProperty("Filter", BindingFlags.Public | BindingFlags.Instance);
                if (topFilter != null)
                {
                    var fv = topFilter.GetValue(mixedContext);
                    if (fv != null)
                    {
                        var kvType = fv.GetType();
                        var keyProp = kvType.GetProperty("Key");
                        var valueProp = kvType.GetProperty("Value");
                        if (keyProp != null && valueProp != null)
                        {
                            var key = keyProp.GetValue(fv)?.ToString();
                            var value = valueProp.GetValue(fv)?.ToString();
                            if (!string.IsNullOrWhiteSpace(key) && key.Equals("Ids", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(value))
                            {
                                foreach (var part in value.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                                    if (Guid.TryParse(part.Trim(), out Guid g)) ids.Add(g);
                            }
                        }
                    }
                }
            }

            return ids.Distinct().ToList();
        }

        private string SafeGetString(DynamicContent item, string fieldName)
        {
            if (item == null || string.IsNullOrWhiteSpace(fieldName)) return null;

            // Prefer GetValue first — avoids triggering Lstring.Value NRE inside GetString
            try
            {
                var val = item.GetValue(fieldName);
                if (val != null)
                {
                    // Handle Sitefinity Lstring safely
                    if (val is Lstring lstr)
                    {
                        try
                        {
                            // Guard access to Value (it can be null internally)
                            var raw = lstr.Value;
                            if (!string.IsNullOrEmpty(raw)) return raw;
                            // Fallback to ToString() (safe)
                            return lstr.ToString();
                        }
                        catch
                        {
                            // ignore and fallback below
                        }
                    }

                    // For plain strings or other types
                    return val.ToString();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SafeGetString: GetValue failed for '{fieldName}': {ex.Message}");
            }

            // Last resort: call GetString but catch any exceptions (previous NRE happened here)
            try
            {
                return item.GetString(fieldName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SafeGetString: GetString failed for '{fieldName}': {ex.Message}");
                return null;
            }
        }

        private class LinkItemModel
        {
            public Guid Id { get; set; }
            public string Type { get; set; }
            public string Text { get; set; }
            public string Target { get; set; }
            public string Href { get; set; }
        }

        private class UrlJsonModel
        {
            public string id { get; set; }
            public string href { get; set; }
            public string sfref { get; set; }
            public string target { get; set; }
            public string queryParams { get; set; }
            public string anchor { get; set; }
            public string tooltip { get; set; }
            public string type { get; set; }
            public string[] classList { get; set; }
            public string attributes { get; set; }
        }
    }
}