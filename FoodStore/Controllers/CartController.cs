using FoodStore.Models;
using FoodStore.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FoodStore.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService _cartService;
        private readonly ProductService _productService;

        public CartController(CartService cartService, ProductService productService)
        {
            _cartService = cartService;
            _productService = productService;
        }

        public IActionResult Index()
        {
            var cart = _cartService.GetCart();
            decimal totalAmount = cart.Sum(item => item.TotalPrice);

            ViewBag.TotalAmount = totalAmount;
            return View(cart);
        }

        public IActionResult AddToCart(string productId, int quantity)
        {
            var product = _productService.GetProductById(productId);
            if (product != null)
            {
                _cartService.AddToCart(product, quantity);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateCart(int[] quantities)
        {
            var cart = _cartService.GetCart();
            Console.WriteLine("Before update: " + JsonConvert.SerializeObject(cart.Select(x => x.Quantity).ToList()));
            for (int i = 0; i < cart.Count; i++)
            {
                cart[i].Quantity = quantities[i];
            }
            _cartService.SaveCart(cart);

            decimal totalAmount = cart.Sum(item => item.TotalPrice);
            var individualTotals = cart.Select(item => item.TotalPrice).ToList();

            Console.WriteLine("After update: " + JsonConvert.SerializeObject(cart.Select(x => x.Quantity).ToList()));

            return Json(new { totalAmount = totalAmount, individualTotals = individualTotals });
        }


        public IActionResult RemoveFromCart(string productId)
        {
            _cartService.RemoveFromCart(productId);
            return RedirectToAction("Index");
        }

        public IActionResult ClearCart()
        {
            _cartService.ClearCart();
            return RedirectToAction("Index");
        }
    }
}
