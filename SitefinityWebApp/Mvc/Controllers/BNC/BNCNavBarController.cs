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
                var sectionItems = allSections.Where(s => selectedIds.Contains(s.Id)).ToList();

                foreach (var section in sectionItems)
                {
                    try
                    {
                        var title = SafeGetString(section, "Title") ?? section.Id.ToString();
                        var urlName = SafeGetString(section, "UrlName");
                        var key = !string.IsNullOrWhiteSpace(urlName) ? urlName : title;

                        var sectionVm = new BNCNavBarViewModel
                        {
                            Key = key,
                            Title = title,
                            Listado = new List<NavBarMenuViewModel>()
                        };

                        // 6) Obtener subsecciones relacionadas por la relación SubsectionLinks
                        List<DynamicContent> related = null;
                        try
                        {
                            var relObjs = TryGetRelatedItems(section, "SubsectionLinks");
                            if (relObjs != null)
                                related = relObjs.Where(o => o is DynamicContent).Cast<DynamicContent>().ToList();
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"TryGetRelatedItems threw: {ex}");
                            related = new List<DynamicContent>();
                        }

                        // 7) Mapear enlaces desde el campo URL de cada subsección
                        foreach (var sub in related ?? Enumerable.Empty<DynamicContent>())
                        {
                            try
                            {
                                var urlJson = SafeGetString(sub, "URL");
                                if (string.IsNullOrWhiteSpace(urlJson)) continue;

                                LinkItemModel[] links = null;
                                try { links = JsonConvert.DeserializeObject<LinkItemModel[]>(urlJson); }
                                catch (Exception jex)
                                {
                                    System.Diagnostics.Debug.WriteLine($"JSON parse error for subsection {sub.Id}: {jex}");
                                    continue;
                                }
                                if (links == null) continue;

                                var subDescription = SafeGetString(sub, "Description") ?? string.Empty;
                                foreach (var link in links)
                                {
                                    sectionVm.Listado.Add(new NavBarMenuViewModel
                                    {
                                        LinkTitle = link.Text,
                                        Description = subDescription,
                                        Url = link.Href,
                                        IconClass = string.Empty,
                                        Target = link.Target
                                    });
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"Error mapping subsection {sub.Id}: {ex}");
                            }
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
        private IEnumerable<object> TryGetRelatedItems(DynamicContent item, string relationName)
        {
            if (item == null) return null;

            try
            {
                var mi = item.GetType().GetMethod("GetRelatedItems", new[] { typeof(string) });
                if (mi != null)
                {
                    try
                    {
                        var result = mi.Invoke(item, new object[] { relationName });
                        if (result is System.Collections.IEnumerable enumRes) return enumRes.Cast<object>();
                        if (result != null) return new[] { result };
                    }
                    catch (TargetInvocationException tie)
                    {
                        System.Diagnostics.Debug.WriteLine($"GetRelatedItems invocation error: {tie.InnerException ?? tie}");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"GetRelatedItems error: {ex}");
                    }
                }

                var prop = item.GetType().GetProperty("RelatedItems") ?? item.GetType().GetProperty("Relations");
                if (prop != null)
                {
                    try
                    {
                        var val = prop.GetValue(item) as System.Collections.IEnumerable;
                        if (val != null) return val.Cast<object>();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"RelatedItems/Relations property read error: {ex}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"TryGetRelatedItems outer error: {ex}");
            }

            return null;
        }

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
            try { return item.GetString(fieldName); }
            catch
            {
                try { var v = item.GetValue(fieldName); return v?.ToString(); }
                catch { return null; }
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
    }
}