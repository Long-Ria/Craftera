using Microsoft.AspNetCore.Mvc;
using Craftera_MVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;


public class AccountController : Controller
{
    private readonly EXE202_CrafteraContext _context; 

    public AccountController(EXE202_CrafteraContext context)
    {
        _context = context;
    }


    // GET: Account/Login
    [HttpGet("/login")]
    public IActionResult Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            
            var user = _context.Users
                .FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.Username); 
                HttpContext.Session.SetInt32("UserId", user.UserId);
                return RedirectToAction("List", "Products"); 
            }

            ModelState.AddModelError("", "Invalid username or password.");
        }

        
        return View(model);

    }
    // GET: Account/Logout
    [HttpGet("/logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear(); 
        return RedirectToAction("List", "Products"); 
    }

    // GET: Account/Register
    [HttpGet("/register")]
    public IActionResult Register()
    {
        return View(new RegisterViewModel()); 
    }

    // POST: Account/Register
    [HttpPost("/register")]
    [AllowAnonymous]
    public IActionResult Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Username == model.Username);
            if (existingUser != null)
            {
                ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại. Vui lòng chọn tên khác.");
                return View(model);
            }

            var user = new User
            {
                Username = model.Username,
                Password = model.Password, 
                RoleId = model.RoleId,
            };


            _context.Users.Add(user);
            _context.SaveChanges(); 

            var userId = user.UserId;

            var userDetails = new UserDetail
            {
                UserId = userId, 
                FullName = null,
                Email = null,
                PhoneNumber = null,
                Avatar = null,
                Gender = null
            };

            _context.UserDetails.Add(userDetails);
            _context.SaveChanges(); 

            return RedirectToAction("Login"); 
        }

        return View(model); 
    }

}
