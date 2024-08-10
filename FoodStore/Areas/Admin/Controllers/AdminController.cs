using FoodStore.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodStore.Areas.Admin.Controllers
{
	[Area("Admin")]
    [AuthorizeRole("Admin")]
    public class AdminController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
