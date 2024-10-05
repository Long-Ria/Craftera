using System;
using System.Collections.Generic;

namespace Craftera_MVC1.Models
{
    public partial class Size
    {
        public Size()
        {
            ProductDetails = new HashSet<ProductDetail>();
        }

        public int SizeId { get; set; }
        public string? SizeName { get; set; }

        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
    }
}
