using System;
using System.Collections.Generic;

namespace BackUp.Models
{
    public partial class Material
    {
        public Material()
        {
            ProductDetails = new HashSet<ProductDetail>();
        }

        public int MaterialId { get; set; }
        public string? MaterialName { get; set; }

        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
    }
}
