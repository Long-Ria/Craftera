using Craftera_MVC.Models;

namespace Craftera_MVC.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductImage { get; set; }
        public decimal? Price { get; set; }

        public List<ProductDetail> ProductDetails { get; set; }

        public List<Category> Categories { get; set; }

        public List<Size> Sizes { get; set; }
        public List<Material> Materials { get; set; }
    }
}
