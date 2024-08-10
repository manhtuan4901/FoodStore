using FoodStore.Filters;
using FoodStore.Models;
using FoodStore.Models.ViewModel;
using FoodStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodStore.Areas.Admin.Controllers
{
	[Area("Admin")]
    [AuthorizeRole("Admin")]
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var ordersWithDetails = await _orderService.GetAllOrdersWithUserDetailsAsync();
            if (ordersWithDetails == null || !ordersWithDetails.Any())
            {
                return NotFound();
            }
            return View(ordersWithDetails);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var orderDetails = await _orderService.GetOrderDetailsAsync(id);
            if (orderDetails == null)
            {
                return NotFound();
            }
            return View(orderDetails);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, OrderWithDetails model)
        //{
        //    if (id != model.Order.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        await _orderService.UpdateOrder(model);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(model);
        //}

        public async Task<IActionResult> Detail(string id)
        {
            var orderDetails = await _orderService.GetOrderDetailsAsync(id);
            if (orderDetails == null)
            {
                return NotFound();
            }
            return View(orderDetails);
        }





        [HttpPost]  // Change from HttpGet to HttpPost
        public async Task<IActionResult> UpdateStatus(string orderId, OrderStatus newStatus)
        {
            await _orderService.UpdateOrderStatus(orderId, newStatus);
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentStatus(string orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order != null)
            {
                return Json(new { status = order.OrderStatus.ToString() });
            }
            else
            {
                return NotFound();
            }
        }






        public async Task<IActionResult> Delete(string id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _orderService.DeleteOrder(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
