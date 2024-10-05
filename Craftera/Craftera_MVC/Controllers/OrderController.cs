using Microsoft.AspNetCore.Mvc;
using Craftera_MVC.Models;
using System.Linq;

namespace Craftera_MVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly EXE202_CrafteraContext _context;

        public OrderController(EXE202_CrafteraContext context)
        {
            _context = context;
        }

        [HttpGet("/myorders")]
        public IActionResult MyOrders()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Lấy danh sách đơn hàng theo UserId
            var orders = _context.Orders
                .Where(o => o.UserId == userId)
                .Select(o => new
                {
                    o.OrderId,
                    o.OrderDate,
                    o.AcceptedDate,
                    o.DeliveryDate,
                    o.ReceivedDate,
                    o.Status,
                    o.TotalMoney,
                    PaymentName = _context.Payments.FirstOrDefault(p => p.PaymentId == o.PaymentId).PaymentName
                }).ToList();

            return View(orders);
        }
    }
}
