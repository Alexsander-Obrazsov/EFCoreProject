using System;
using System.Collections.Generic;

namespace EFCoreProject.DTO
{
    public partial class Order
    {
        public int Id { get; set; }
        public DateTime DateOrder { get; set; }
        public int Count { get; set; }
        public string PaymentType { get; set; } = null!;
        public string? Remark { get; set; }
        public int ClientId { get; set; }
    }
}
