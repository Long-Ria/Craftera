using System;
using System.Collections.Generic;

namespace BackUp.Models
{
    public partial class OrderDetail
    {
        public int? ProductId { get; set; }
        public int? SizeId { get; set; }
        public int? MaterialId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public int? OrderId { get; set; }

        public virtual Order? Order { get; set; }
        public virtual ProductDetail? ProductDetail { get; set; }
    }
}
