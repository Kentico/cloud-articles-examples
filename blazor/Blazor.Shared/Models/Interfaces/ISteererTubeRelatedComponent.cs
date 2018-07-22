using System;
using System.Collections.Generic;
using System.Text;

using KenticoCloud.Delivery;

namespace Blazor.Shared.Models
{
    public interface ISteererTubeRelatedComponent
    {
        IEnumerable<TaxonomyTerm> SteererTubeType { get; set; }
        IEnumerable<TaxonomyTerm> SteererTubeDiameter { get; set; }
    }
}
