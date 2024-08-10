using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FoodStore.Services;
using FoodStore.Models;
using FoodStore.Areas.Admin.Models;
using FoodStore.Filters;

namespace FoodStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthorizeRole("Admin")]
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            var users = _userService.GetAllUsers();
            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Email = model.Email,
                    Fullname = model.Fullname,
                    Phone = model.Phone,
                    Address = model.Address,
                    Username = model.Username,
                    Password = model.Password,
                    Roles = new List<string> { "User" }
                };

                _userService.AddUser(user);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public IActionResult Delete(string id)
        {
            var product = _userService.GetUserById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(User user)
        {
            _userService.DeleteUser(user);
            return RedirectToAction(nameof(Index));
        }
    }
}
