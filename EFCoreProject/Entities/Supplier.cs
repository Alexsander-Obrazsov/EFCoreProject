using System;
using System.Collections.Generic;

namespace EFCoreProject.Entities
{
    public partial class Supplier
    {
        public Supplier()
        {
            Deliveries = new HashSet<Delivery>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public decimal PhoneNumber { get; set; }

        public virtual ICollection<Delivery> Deliveries { get; set; }
    }
}
