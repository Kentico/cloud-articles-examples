using System;
using System.Collections.Generic;
using System.Text;

using KenticoCloud.Delivery;

namespace Blazor.Shared.Models
{
    public interface IShockBushingSizes
    {
        IEnumerable<TaxonomyTerm> ShockBushingSizes { get; set; }
    }
}