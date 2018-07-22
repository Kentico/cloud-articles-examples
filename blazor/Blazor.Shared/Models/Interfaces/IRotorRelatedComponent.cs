using System;
using System.Collections.Generic;
using System.Text;

using KenticoCloud.Delivery;

namespace Blazor.Shared.Models
{
    public interface IRotorRelatedComponent
    {
        decimal? MinimumRotorDiameterMm { get; set; }
        decimal? MaximumRotorDiameterMm { get; set; }
    }
}
