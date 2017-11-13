using DancingGoat.Models;
using KenticoCloud.Delivery;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DancingGoat.Controllers
{
    public class AccessoriesController : ControllerBase
    {
        public async Task<ActionResult> Index()
        {
            var itemsTask = client.GetItemsAsync<Accessory>(
                new EqualsFilter("system.type", "accessory"),
                new OrderParameter("elements.product_name"),
                new ElementsParameter(Accessory.ImageCodename, Accessory.PriceCodename, Accessory.ProductStatusCodename, Accessory.ProductNameCodename, Accessory.UrlPatternCodename),
                new DepthParameter(0)
            );

            var statusTask = client.GetTaxonomyAsync(Accessory.ProductStatusCodename);

            var model = new AccessoriesViewModel
            {
                Items = (await itemsTask).Items,
                Filter = new AccessoriesFilterViewModel
                {
                    AvailableProductStatuses = GetTaxonomiesAsSelectList(await statusTask)
                }
            };

            return View(model);
        }

        public async Task<ActionResult> Filter(AccessoriesFilterViewModel model)
        {
            var parameters = new List<IQueryParameter> {
                new EqualsFilter("system.type", "accessory"),
                new OrderParameter("elements.product_name"),
                new ElementsParameter(Accessory.ImageCodename, Accessory.PriceCodename, Accessory.ProductStatusCodename, Accessory.ProductNameCodename, Accessory.UrlPatternCodename),
                new DepthParameter(0),
            };

            var filterStatus = model.GetFilteredProductStatuses().ToArray();
            if (filterStatus.Any())
            {
                parameters.Add(new AnyFilter($"elements.{Accessory.ProductStatusCodename}", filterStatus));
            }

            var response = await client.GetItemsAsync<Accessory>(parameters);

            return PartialView("ProductListing", response.Items);
        }

        private IList<SelectListItem> GetTaxonomiesAsSelectList(TaxonomyGroup taxonomyGroup)
        {
            return taxonomyGroup.Terms.Select(x => new SelectListItem { Text = x.Name, Value = x.Codename }).ToList();
        }
    }
}