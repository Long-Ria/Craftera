using Craftera_MVC.Models;
using Craftera_MVC.Services;
using Craftera_MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;

namespace Craftera_MVC.Controllers
{
    public class CartsController : Controller
    {
        private readonly EXE202_CrafteraContext _context;
        private readonly IVnPayService _vnPayservice;

        public CartsController(EXE202_CrafteraContext context, IVnPayService vnPayservice)
        {
            _context = context;
            _vnPayservice = vnPayservice;
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
                        }
                        else
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

        [HttpPost]
        public IActionResult CheckoutWithVnPay()
        {
            // Prepare VNPay request model
            var vnPayModel = new VnPaymentRequestModel
            {
                Amount = (double)(_context.Items.Where(i => i.CartId == 2).Sum(i => i.Price) ?? 0),  // Replace with actual cart user ID or session
                CreatedDate = DateTime.Now,
                Description = "Payment for Craftera accessories",
                FullName = "User Name",  // You can get the name from logged-in user context
                OrderId = new Random().Next(1000, 100000)  // Generate random order ID or use your logic
            };

            // Redirect to VNPay URL
            return Redirect(_vnPayservice.CreatePaymentUrl(HttpContext, vnPayModel));
        }


        [HttpGet]
        public IActionResult VnPayPaymentCallback()
        {
            // Process the response from VNPay
            var response = _vnPayservice.PaymentExecute(Request.Query);

            if (response == null || response.VnPayResponseCode != "00")
            {
                TempData["Message"] = $"Lỗi thanh toán VNPay: {response?.VnPayResponseCode ?? "Không xác định"}";
                return RedirectToAction("PaymentFail");
            }

            // Retrieve the user's cart (user id should be dynamic)
            var cart = _context.Carts.Include(c => c.Items).FirstOrDefault(c => c.UserId == 2);

            if (cart != null)
            {
                // Fetch the VNPay payment method (assuming PaymentName = "VNPay")
                var paymentMethod = _context.Payments.FirstOrDefault(p => p.PaymentName == "VNPay");
                if (paymentMethod == null)
                {
                    TempData["Message"] = "Phương thức thanh toán không tồn tại!";
                    return RedirectToAction("PaymentFail");
                }

                // Create and save the order in the database
                var order = new Order
                {
                    UserId = cart.UserId,
                    OrderDate = DateTime.Now,
                    TotalMoney = cart.TotalMoney,
                    Payment = paymentMethod,  // Assign the payment method here
                    Status = 2 // Assuming "2" is the status for "Paid"
                };

                _context.Orders.Add(order);
                _context.SaveChanges();

                // Save order details (items in the cart)
                foreach (var item in cart.Items)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.OrderId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price
                    };

                    _context.OrderDetails.Add(orderDetail);
                }
                _context.SaveChanges();

                // Clear the cart after successful payment
                _context.Items.RemoveRange(cart.Items);
                cart.TotalMoney = 0;
                _context.SaveChanges();

                TempData["Message"] = "Thanh toán VNPay thành công!";
                return RedirectToAction("PaymentSuccess");
            }

            return RedirectToAction("PaymentFail");
        }

    }
}