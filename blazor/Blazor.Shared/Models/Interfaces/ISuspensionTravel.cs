using System;
using System.Collections.Generic;
using System.Text;

using KenticoCloud.Delivery;

namespace Blazor.Shared.Models
{
    public interface ISuspensionTravel
    {
        IEnumerable<TaxonomyTerm> SuspensionTravel { get; set; }
    }
}