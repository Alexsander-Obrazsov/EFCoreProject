using System;
using System.Collections.Generic;

namespace EFCoreProject.Entities
{
    public partial class UnitMeasurement
    {
        public UnitMeasurement()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
    }
}
