using FoodStore.Areas.Admin.Models;
using FoodStore.Services;
using Microsoft.AspNetCore.Mvc;
using FoodStore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using FoodStore.Filters;

namespace FoodStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthorizeRole("Admin")]
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
        public IActionResult Index()
        {
            var products = _productService.GetProducts();
            return View(products);
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

        // GET: Products/Create
        public IActionResult Create()
        {
            var categories = _categoryService.GetCategories();
            ViewBag.CategoryList = new SelectList(categories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Description,Price,Quantity,CategoryId,Images")] ProductVM productVM)
        {
            if (!ModelState.IsValid)
            {
                return View(productVM);
            }
            var product = new Product
            {
                Name = productVM.Name,
                Description = productVM.Description,
                Price = productVM.Price,
                Quantity = productVM.Quantity,
                CategoryId = productVM.CategoryId,
                Images = productVM.Images,
            };
            _productService.AddProduct(product);
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Edit/5
        public IActionResult Edit(string id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            var categories = _categoryService.GetCategories();
            ViewBag.CategoryList = new SelectList(categories, "Id", "Name");
            var productVM = new ProductVM
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity,
                Images = product.Images,
                CategoryId = product.CategoryId
            };
            return View(productVM);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind("Id,Name,Description,Price,Quantity,Images,CategoryId")] ProductVM productVM)
        {
            if (!ModelState.IsValid)
            {
                return View(productVM);
            }
            var product = new Product
            {
                Id = id,
                Name = productVM.Name,
                Description = productVM.Description,
                Price = productVM.Price,
                Quantity = productVM.Quantity,
                Images = productVM.Images,
                CategoryId = productVM.CategoryId
            };
            _productService.UpdateProduct(id, product);
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Delete/5
        public IActionResult Delete(string id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            _productService.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
