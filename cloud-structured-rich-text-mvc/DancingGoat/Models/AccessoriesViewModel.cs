using System.Collections.Generic;

namespace DancingGoat.Models
{
    public class AccessoriesViewModel
    {
        public IEnumerable<Accessory> Items { get; set; } = new List<Accessory>();
        public AccessoriesFilterViewModel Filter { get; set; }
    }
}