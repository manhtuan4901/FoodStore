using Microsoft.AspNetCore.Mvc;
using FoodStore.Services;
using FoodStore.Models;
using FoodStore.Models.ViewModel;

namespace FoodStore.Controllers
{
	public class OrderController : Controller
	{
		private readonly CartService _cartService;
		private readonly OrderService _orderService;
        private readonly UserService _userService;

        public OrderController(CartService cartService, OrderService orderService, UserService userService)
		{
			_cartService = cartService;
			_orderService = orderService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                ViewBag.Message = "Bạn chưa có đơn hàng.";
                return View();
            }

            try
            {
                var ordersWithDetails = await _orderService.GetOrdersWithUserDetailsAsync(userId);
                if (ordersWithDetails == null || !ordersWithDetails.Any())
                {
                    ViewBag.Message = "Bạn chưa có đơn hàng.";
                }
                return View(ordersWithDetails);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }


        public async Task<IActionResult> Detail(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return View("Error");
            }

            var orderDetails = await _orderService.GetOrderDetailsAsync(Id);
            if (orderDetails == null)
            {
                return View("Error");
            }

            return View(orderDetails);
        }



        [HttpGet]
        public IActionResult Checkout()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = _cartService.GetCart();
            if (!cart.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            var user = _userService.GetUserById(userId);

            var viewModel = new CheckoutVM
            {
                UserInfo = user,
                CartItems = cart,
                TotalAmount = cart.Sum(item => item.TotalPrice)
            };

            return View(viewModel);
        }



        public static string GetStatusDisplayName(OrderStatus status)
        {
            switch (status)
            {
                case OrderStatus.Pending:
                    return "Pending";
                case OrderStatus.Delivered:
                    return "Delivered";
                case OrderStatus.Cancelled:
                    return "Cancelled";
                default:
                    return "Unknown";
            }
        }




        [HttpPost]
        public IActionResult Checkout(CheckoutVM model)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = _cartService.GetCart();
            if (!cart.Any())
            {
                ModelState.AddModelError("", "Không có sản phẩm trong giỏ hàng.");
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userToUpdate = _userService.GetUserById(userId);
            if (userToUpdate != null)
            {
                userToUpdate.Fullname = model.UserInfo.Fullname;
                userToUpdate.Phone = model.UserInfo.Phone;
                userToUpdate.Address = model.UserInfo.Address;
                _userService.UpdateUser(userToUpdate);
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = cart.Sum(item => item.TotalPrice),

            };

            try
            {
                _orderService.CreateOrder(order, cart);
                _cartService.ClearCart();
                return RedirectToAction("Index", "Cart");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi thanh toán: ");
                return View(model);
            }
        }


    }
}
