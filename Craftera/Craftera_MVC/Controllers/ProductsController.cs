using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Craftera_MVC.Models;
using Craftera_MVC.ViewModels;

namespace Craftera_MVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly EXE202_CrafteraContext _context;

        public ProductsController(EXE202_CrafteraContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Details(int? id)
        {
            var productDetails = _context.ProductDetails
                .Include(pd => pd.Size)
                .Include(pd => pd.Material)
                .Include(pd => pd.Product)
                .Where(pd => pd.ProductId == id)
                .ToList();

            var categories = _context.Categories.ToList();
            
            var sizes = productDetails.Select(pd => new
            {
                SizeId = pd.Size.SizeId,
                SizeName = pd.Size.SizeName,

            }).Distinct().ToList();
            var materials = productDetails.Select(pd => new
            {
                MaterialId = pd.Material.MaterialId,
                MaterialName = pd.Material.MaterialName,
            }).Distinct().ToList();

            ProductViewModel model = new ProductViewModel()
            {
                Product = productDetails.First().Product,
                ProductDetails = productDetails,
                Categories = categories,
            };
            ViewBag.Sizes = sizes;
            ViewBag.Materials = materials;
            return View(model);
        }

        public async Task<IActionResult> List()
        {
            var products = await _context.Products.Include(p => p.ProductDetails).Include(p => p.Category).ToListAsync();
            var categories = _context.Categories.ToList();
            ProductListModel model = new ProductListModel()
            {
                Products = products,
                Categories = categories
            };
            return View(model);
        }


        

        [HttpPost]
        public async Task<IActionResult> FilterProduct(string? search, List<int>? categoryId, int sortType)
        {
            if (string.IsNullOrEmpty(search))
            {
                search = "";
            }
            List<Product> products = _context.Products.Include(p => p.ProductDetails)
                .Where(p => categoryId == null || categoryId.Count <= 0 || categoryId.Contains((int)p.CategoryId))
                .Where(p => p.ProductName.Contains(search))
                .ToList();

            if (sortType == 0)
            {
                products = products.OrderBy(p => p.ProductName.ToLower()).ToList();
            }
            else
            {
                products = products.OrderByDescending(p => p.ProductName.ToLower()).ToList();
            }

            return Json(new { success = true, data = products});

        }


    }
        

        

        

        

        
}
