using System;
using System.Collections.Generic;

namespace EFCoreProject.Entities
{
    public partial class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Price { get; set; }
        public int UnitMeasurementId { get; set; }
        public string? Description { get; set; }
        public int ProductGroupId { get; set; }

        public virtual ProductGroup ProductGroup { get; set; } = null!;
        public virtual UnitMeasurement UnitMeasurement { get; set; } = null!;
    }
}
