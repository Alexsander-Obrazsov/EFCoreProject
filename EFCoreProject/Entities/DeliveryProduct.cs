using System;
using System.Collections.Generic;

namespace EFCoreProject.Entities
{
    public partial class DeliveryProduct
    {
        public int? DeliveryId { get; set; }
        public int? ProductId { get; set; }

        public virtual Delivery Delivery { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
