using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DancingGoat.Models
{
    public class AccessoriesFilterViewModel
    {
        public IList<SelectListItem> AvailableProductStatuses { get; set; } = new List<SelectListItem>();

        public IEnumerable<string> GetFilteredProductStatuses()
            => AvailableProductStatuses.Where(x => x.Selected).Select(x => x.Value);
    }
}