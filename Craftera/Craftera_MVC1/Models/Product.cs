using System;
using System.Collections.Generic;

namespace Craftera_MVC1.Models
{
    public partial class Product
    {
        public Product()
        {
            ProductDetails = new HashSet<ProductDetail>();
        }

        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductImage { get; set; }
        public int? CategoryId { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
    }
}
