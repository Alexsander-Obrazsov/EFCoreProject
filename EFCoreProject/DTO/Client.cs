using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreProject.DTO
{
    public partial class Client
    { 
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Patronymic { get; set; } = null!;
        public string Address { get; set; } = null!;
        public decimal PhoneNumber { get; set; }
    }
}
