using System;
using System.Collections.Generic;
using System.Text;

using KenticoCloud.Delivery;

namespace Blazor.Shared.Models.Interfaces
{
    public interface IBike
    {
        string Name { get; set; }
        IEnumerable<TaxonomyTerm> BikeKind { get; set; }
        string ShortDescription { get; set; }
        IRichTextContent LongDescription { get; set; }
        IEnumerable<object> Frame { get; set; }
        IEnumerable<object> Fork { get; set; }
        IEnumerable<object> Wheelset { get; set; }
        IEnumerable<object> Brakes { get; set; }
        IEnumerable<object> FrontBrakeRotor { get; set; }
        IEnumerable<object> RearBrakeRotor { get; set; }
        IEnumerable<object> Crankset { get; set; }
        IEnumerable<object> Seatpost { get; set; }
        IEnumerable<object> Shock { get; set; }
        IEnumerable<object> Stem { get; set; }
        IEnumerable<object> Handlebar { get; set; }
        IEnumerable<object> Saddle { get; set; }
        IEnumerable<object> Tires { get; set; }
        IEnumerable<object> FrontDerailleur { get; set; }
        IEnumerable<object> RearDerailleur { get; set; }
        IEnumerable<Asset> Images { get; set; }
    }
}
