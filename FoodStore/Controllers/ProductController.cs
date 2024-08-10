using FoodStore.Models.ViewModel;
using FoodStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;

        public ProductController(ProductService productService, CategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        // GET: Products
        public IActionResult Index(int page = 1)
        {
            var pageSize = 8; // Number of products per page
            var products = _productService.GetProductsPaginated(page, pageSize);
            var categories = _categoryService.GetCategories();
            var totalProducts = _productService.CountAllProducts();
            var viewModel = new ProductCategoryViewModel
            {
                Products = products,
                Categories = categories,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalProducts / (double)pageSize)
            };
            return View(viewModel);
        }




        [HttpGet]
        public IActionResult Search(string searchTerm, int page = 1)
        {
            var pageSize = 8;
            var products = _productService.SearchProducts(searchTerm);
            var totalProducts = products.Count;
            var pagedProducts = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var categories = _categoryService.GetCategories();
            var viewModel = new ProductCategoryViewModel
            {
                Products = pagedProducts,
                Categories = categories,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalProducts / (double)pageSize)
            };
            return View("Index", viewModel);
        }




        [HttpGet]
        public IActionResult LoadMoreProducts(int currentPage)
        {
            var products = _productService.GetProductsPaginated(currentPage + 1, 3);
            return PartialView("_ProductsPartial", products);
        }


        [HttpGet]
        public IActionResult FilterProducts(string categoryId)
        {
            var products = _productService.GetProductsByCategory(categoryId);
            return Json(products);
        }







        // GET: Products/Details/5
        public IActionResult Detail(string id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

    }
}
