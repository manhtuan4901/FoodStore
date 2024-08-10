using FoodStore.Areas.Admin.Models;
using FoodStore.Filters;
using FoodStore.Models;
using FoodStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodStore.Areas.Admin.Controllers
{
	[Area("Admin")]
    [AuthorizeRole("Admin")]
    public class CategoryController : Controller
	{
		private readonly CategoryService _categoryService;

		public CategoryController(CategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		// GET: Category
		public IActionResult Index()
		{
			var category = _categoryService.GetCategories();
			return View(category);
		}

		// GET: Category/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Category/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create([Bind("Name")] CategoryVM categoryVM)
		{
			if (!ModelState.IsValid)
			{
				return View(categoryVM);
			}
			var category = new Category
			{
				Name = categoryVM.Name,

			};
			_categoryService.AddCategory(category);
			return RedirectToAction(nameof(Index));
		}


		// GET: Category/Delete/5
		public IActionResult Delete(string id)
		{
			var category = _categoryService.GetCategoryById(id);
			if (category == null)
			{
				return NotFound();
			}
			return View(category);
		}

		// POST: Category/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public IActionResult DeleteConfirmed(string id)
		{
			_categoryService.DeleteCategory(id);
			return RedirectToAction(nameof(Index));
		}
	}
}
