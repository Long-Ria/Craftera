using Microsoft.AspNetCore.Mvc;
using Craftera_MVC.Models;
using System.Linq;
using System.IO;

namespace Craftera_MVC.Controllers
{
    public class ProfileController : Controller
    {
        private readonly EXE202_CrafteraContext _context;

        public ProfileController(EXE202_CrafteraContext context)
        {
            _context = context;
        }

        [HttpGet("/myprofile")]
        public IActionResult MyProfile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var userDetail = _context.UserDetails.FirstOrDefault(u => u.UserId == userId);
            if (userDetail == null)
            {
                return NotFound();
            }

            return View(userDetail);
        }

        [HttpPost("/updateprofile")]
        public IActionResult UpdateProfile(UserDetail updatedUser)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var existUser = _context.UserDetails.FirstOrDefault(u => u.UserId == userId);
            if (existUser == null)
            {
                return NotFound();
            }

            existUser.FullName = updatedUser.FullName;
            existUser.Email = updatedUser.Email;
            existUser.PhoneNumber = updatedUser.PhoneNumber;
            existUser.Avatar = updatedUser.Avatar;
            existUser.Gender = updatedUser.Gender;

            _context.SaveChanges();

            return RedirectToAction("MyProfile");
        }




    }
}
