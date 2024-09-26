using System;
using System.Collections.Generic;

namespace Craftera_MVC.Models
{
    public partial class UserDetail
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Avatar { get; set; }
        public bool? Gender { get; set; }
        public int? UserId { get; set; }

        public virtual User? User { get; set; }
    }
}
