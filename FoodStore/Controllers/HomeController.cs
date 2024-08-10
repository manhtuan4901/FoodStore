using FoodStore.Models;
using FoodStore.Models.ViewModel;
using FoodStore.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FoodStore.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ProductService _productService;
		private readonly CategoryService _categoryService;
		public HomeController(ILogger<HomeController> logger, ProductService productService, CategoryService categoryService)
		{
			_logger = logger;
			_productService = productService;
			_categoryService = categoryService;
		}

		public IActionResult Index()
		{
			var products = _productService.GetProductsPaginated(1, 6);
			var categories = _categoryService.GetCategories();
			var viewModel = new ProductCategoryViewModel
			{
				Products = products,
				Categories = categories
			};
			return View(viewModel);
		}

		public IActionResult Contact()
		{
			return View();
		}

		public IActionResult About()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
