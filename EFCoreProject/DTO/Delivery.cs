using System;
using System.Collections.Generic;

namespace EFCoreProject.DTO
{
    public partial class Delivery
    {
        public int Id { get; set; }
        public DateTime DateDelivery { get; set; }
        public string? Remark { get; set; }
        public int SupplierId { get; set; }
    }
}
