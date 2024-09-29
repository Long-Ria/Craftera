using System;
using System.Collections.Generic;

namespace BackUp.Models
{
    public partial class ProductDetail
    {
        public ProductDetail()
        {
            Items = new HashSet<Item>();
        }

        public int ProductId { get; set; }
        public int SizeId { get; set; }
        public int MaterialId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }

        public virtual Material Material { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
        public virtual Size Size { get; set; } = null!;
        public virtual ICollection<Item> Items { get; set; }
    }
}
