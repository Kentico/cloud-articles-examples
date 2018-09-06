using System;
using System.Collections.Generic;
using System.Text;

using KenticoCloud.Delivery;

namespace Blazor.Shared.Models
{
    public interface IBikeComponent
    {
        string Name { get; set; }
        IEnumerable<TaxonomyTerm> BikeKind { get; set; }
        string ShortDescription { get; set; }
        IRichTextContent LongDescription { get; set; }
        IEnumerable<Asset> Images { get; set; }
        decimal? WeightGrams { get; set; }
    }
}
