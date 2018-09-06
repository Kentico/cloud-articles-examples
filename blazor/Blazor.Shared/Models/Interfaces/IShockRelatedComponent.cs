using System;
using System.Collections.Generic;
using System.Text;

using KenticoCloud.Delivery;

namespace Blazor.Shared.Models
{
    public interface IShockRelatedComponent
    {
        IEnumerable<TaxonomyTerm> ShockSize { get; set; }
        IEnumerable<TaxonomyTerm> ShockBushingSizes { get; set; }
    }
}
