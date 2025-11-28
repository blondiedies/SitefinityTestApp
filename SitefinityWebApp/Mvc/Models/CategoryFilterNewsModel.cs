using DDay.iCal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.OpenAccess;
using Telerik.Sitefinity.Frontend.News.Mvc.Models;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.News.Model;
using Telerik.Sitefinity.Web;

namespace SitefinityWebApp.Mvc.Models
{
    /// <summary>
    /// Modificación de News widget para incluir filtro por categoría
    /// </summary>
    public class CategoryFilterNewsModel : NewsModel
    {
        protected override IQueryable<IDataItem> GetItemsQuery()
        {
            var baseNewsQuery = base.GetItemsQuery();
            var categoryIds = GetCurrentPageCategoryID();
            if (categoryIds.Any())
            {
                var category = categoryIds.First();
                baseNewsQuery = baseNewsQuery.Cast<NewsItem>()
                    .Where(n=>n.GetValue<TrackedList<Guid>>("Category").Contains(category));

            }
            return baseNewsQuery;
        }

        private TrackedList<Guid> GetCurrentPageCategoryID()
        {
            var currentNode=SiteMapBase.GetActualCurrentNode();
            var res = new TrackedList<Guid>();
            if (currentNode != null)
            {
                res.AddRange(currentNode.GetCustomFieldValue("Category") as TrackedList<Guid>);
            }
            return res;
        }
    }
}