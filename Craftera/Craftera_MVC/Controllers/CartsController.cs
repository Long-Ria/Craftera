using Craftera_MVC.Models;
using Craftera_MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;

namespace Craftera_MVC.Controllers
{
    public class CartsController : Controller
    {
        private readonly EXE202_CrafteraContext _context;

        public CartsController(EXE202_CrafteraContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            Cart cart = _context.Carts.Include(c => c.Items).ThenInclude(i => i.ProductDetail).FirstOrDefault(c => c.UserId == 2);
            List<Item> items = cart.Items.ToList();
            List<ProductDetail> itemsInfo = new List<ProductDetail>();
            foreach (Item item in items)
            {
                ProductDetail productDetail = _context.ProductDetails
                    .Include(pd => pd.Product)
                    .Include(pd => pd.Material)
                    .Include(pd => pd.Size)
                    .Where(pd => pd.ProductId == item.ProductId
                    && pd.MaterialId == item.MaterialId
                    && pd.SizeId == item.SizeId).FirstOrDefault();
                itemsInfo.Add(productDetail);
            }
            CartViewModelClass cartViewModelClass = new CartViewModelClass()
            {
                Cart = cart,
                Items = items,
                ItemsInfo = itemsInfo
            };
            return View(cartViewModelClass);
        }


        [HttpPost]
        public async Task<IActionResult> AddToCartDetails(int? productId, int? materialId, int? sizeId, int? quantity)
        {
            if (productId == null || materialId == null || sizeId == null || quantity == null)
            {
                return Json(new { success = false });
            }
            else
            {
                var product = _context.ProductDetails.FirstOrDefault(p => p.ProductId == productId
                && p.MaterialId == materialId
                && p.SizeId == sizeId);
                if (product == null)
                {
                    return Json(new { success = false });
                }
                else
                {
                    Cart cart = _context.Carts.FirstOrDefault(c => c.UserId == 2);
                    if (cart == null)
                    {

                        cart = new Cart()
                        {
                            UserId = 2,
                            TotalMoney = 0,
                        };

                        _context.Carts.Add(cart);
                        await _context.SaveChangesAsync();

                        cart = _context.Carts.FirstOrDefault(c => c.UserId == 2);
                        Item item = new Item()
                        {
                            CartId = cart.CartId,
                            ProductId = productId,
                            SizeId = product.SizeId,
                            MaterialId = product.MaterialId,
                            Quantity = quantity,
                            Price = product.Price,
                        };
                        cart.TotalMoney += item.Price;
                        _context.Items.Add(item);
                        _context.Carts.Update(cart);
                        await _context.SaveChangesAsync();
                        return Json(new { success = true });
                    }
                    else
                    {
                        cart = _context.Carts.FirstOrDefault(c => c.UserId == 2);
                        Item item = _context.Items.FirstOrDefault(i => i.CartId == cart.CartId
                        && i.ProductId == productId
                        && i.SizeId == product.SizeId
                        && i.MaterialId == product.MaterialId);

                        if (item == null)
                        {
                            item = new Item()
                            {
                                CartId = cart.CartId,
                                ProductId = productId,
                                SizeId = product.SizeId,
                                MaterialId = product.MaterialId,
                                Quantity = quantity,
                                Price = product.Price,
                            };

                            _context.Items.Add(item);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            item.Quantity += quantity;
                            item.Price = product.Price * item.Quantity;
                            _context.Items.Update(item);
                            await _context.SaveChangesAsync();
                        }


                        cart.TotalMoney = _context.Items.Where(i => i.CartId == cart.CartId).ToList().Sum(i => i.Price);

                        _context.Carts.Update(cart);
                        await _context.SaveChangesAsync();
                        return Json(new { success = true });
                    }
                }
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddToCart(int? productId)
        {
            if (productId == null)
            {
                return Json(new { success = false });
            }
            else
            {
                var product = _context.Products.Include(p => p.ProductDetails).FirstOrDefault(p => p.ProductId == productId);
                if (product == null)
                {
                    return Json(new { success = false });
                }
                else
                {
                    Cart cart = _context.Carts.FirstOrDefault(c => c.UserId == 2);
                    if (cart == null)
                    {

                        cart = new Cart()
                        {
                            UserId = 2,
                            TotalMoney = 0,
                        };

                        _context.Carts.Add(cart);
                        await _context.SaveChangesAsync();

                        cart = _context.Carts.FirstOrDefault(c => c.UserId == 2);
                        Item item = new Item()
                        {
                            CartId = cart.CartId,
                            ProductId = productId,
                            SizeId = product.ProductDetails.First().SizeId,
                            MaterialId = product.ProductDetails.First().MaterialId,
                            Quantity = 1,
                            Price = product.ProductDetails.First().Price,
                        };
                        cart.TotalMoney += item.Price;
                        _context.Items.Add(item);
                        _context.Carts.Update(cart);
                        await _context.SaveChangesAsync();
                        return Json(new { success = true });
                    }
                    else
                    {
                        cart = _context.Carts.FirstOrDefault(c => c.UserId == 2);
                        Item item = _context.Items.FirstOrDefault(i => i.CartId == cart.CartId
                        && i.ProductId == productId
                        && i.SizeId == product.ProductDetails.First().SizeId
                        && i.MaterialId == product.ProductDetails.First().MaterialId);

                        if (item == null)
                        {
                            item = new Item()
                            {
                                CartId = cart.CartId,
                                ProductId = productId,
                                SizeId = product.ProductDetails.First().SizeId,
                                MaterialId = product.ProductDetails.First().MaterialId,
                                Quantity = 1,
                                Price = product.ProductDetails.First().Price,
                            };
                            
                            _context.Items.Add(item);
                            await _context.SaveChangesAsync();
                        } else
                        {
                            item.Quantity += 1;
                            item.Price = product.ProductDetails.First().Price * item.Quantity;
                            _context.Items.Update(item);
                            await _context.SaveChangesAsync();
                        }

                        
                        cart.TotalMoney = _context.Items.Where(i => i.CartId == cart.CartId).ToList().Sum(i => i.Price);
                        
                        _context.Carts.Update(cart);
                        await _context.SaveChangesAsync();
                        return Json(new { success = true });
                    }
                }
            }
        }

    }
}
