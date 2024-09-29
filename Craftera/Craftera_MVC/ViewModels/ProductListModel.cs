using Craftera_MVC.Models;

namespace Craftera_MVC.ViewModels
{
    public class ProductListModel
    {
        public string? SearchKeyword { get; set; }
        public List<int>? CategoryIds { get; set; }

        public int SortType { get; set; }

        public List<Product> Products { get; set; }

        public List<Category> Categories { get; set; }


        public int CategoryId { get; set; }


    }
}
