using FoodStore.Helpers;
using FoodStore.Models;
using FoodStore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using FoodStore.Models.ViewModel;
using System.Data;

namespace FoodStore.Controllers
{
    public class AccountController : Controller
	{
		private readonly UserService _userService;

		public AccountController(UserService userService)
		{
            _userService = userService;
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

        [HttpPost]
        public IActionResult Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

            try
            {
                if (_userService.UserExistsByUsername(registerVM.Username))
                {
                    ModelState.AddModelError("Username", "Tài khoản đã tồn tại.");
                    return View(registerVM);
                }
                if (_userService.UserExistsByEmail(registerVM.Email))
                {
                    ModelState.AddModelError("Email", "Email đã tồn tại.");
                    return View(registerVM);
                }

                var hashedPassword = HashingHelper.HashPassword(registerVM.Password);

                var user = new User
                {
                    Email = registerVM.Email,
                    Username = registerVM.Username,
                    Password = hashedPassword,
                    Roles = new List<string> { "User" }
                };

                _userService.AddUser(user);

                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi tạo tài khoản.");
                return View(registerVM);
            }
        }


        [HttpGet]
		public IActionResult Login()
		{
			return View();
		}

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            var user = _userService.GetUserByUsername(loginVM.Username);
            if (user != null)
            {
                if (HashingHelper.VerifyPassword(user.Password, loginVM.Password))
                {
                    HttpContext.Session.SetString("UserId", user.Id);
                    HttpContext.Session.SetString("Roles", string.Join(",", user.Roles));
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetString("Email", user.Email);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Sai tài khoản hoặc mật khẩu.");
                return View();
            }
            ModelState.AddModelError(string.Empty, "Có lỗi xảy ra.");
            return View();

        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); 
            return RedirectToAction("Login", "Account");
        }

    }
}
