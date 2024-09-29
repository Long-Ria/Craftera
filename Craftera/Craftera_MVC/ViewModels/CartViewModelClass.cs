using Craftera_MVC.Models;

namespace Craftera_MVC.ViewModels
{
    public class CartViewModelClass
    {
        public Cart Cart { get; set; }

        public List<Item> Items { get; set; }

        public List<ProductDetail> ItemsInfo { get; set; }
    }
}
