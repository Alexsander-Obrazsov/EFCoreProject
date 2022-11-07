using System;
using System.Collections.Generic;

namespace EFCoreProject.DTO
{
    public partial class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public decimal PhoneNumber { get; set; }
    }
}
