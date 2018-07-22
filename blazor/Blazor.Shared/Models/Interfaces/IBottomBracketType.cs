using System;
using System.Collections.Generic;
using System.Text;

using KenticoCloud.Delivery;

namespace Blazor.Shared.Models
{
    public interface IBottomBracketType
    {
        IEnumerable<TaxonomyTerm> BottomBracketType { get; set; }
    }
}
