using System;
using System.Collections.Generic;

namespace Craftera_MVC1.Models
{
    public partial class Cart
    {
        public Cart()
        {
            Items = new HashSet<Item>();
        }

        public int CartId { get; set; }
        public int? UserId { get; set; }
        public decimal? TotalMoney { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}
