using System;
using System.Collections.Generic;

namespace Craftera_MVC.Models
{
    public partial class Cart
    {
        public int CartId { get; set; }
        public int? UserId { get; set; }
        public decimal? TotalMoney { get; set; }

        public virtual User? User { get; set; }
    }
}
