using DoAnWebNangCao.Models;
using DoAnWebNangCao.Repositories.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace DoAnWebNangCao.Controllers
{
    public class OrdersStatusController : Controller
    {
        private readonly IOrdersStatusService _ordersStatusService;
        public OrdersStatusController(IOrdersStatusService ordersStatusService)
        {
            _ordersStatusService = ordersStatusService;
        }

        public async Task<IActionResult> Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(OrderStatus model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await _ordersStatusService.Add(model);
            if (result)
            {
                TempData["msg"] = "Added Successfully";
                return RedirectToAction(nameof(Add));
            }
            else
            {
                TempData["msg"] = "Error on server side";
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var data = await _ordersStatusService.GetById(id);
            return View(data);
        }
        public async Task<IActionResult> Details(int id)
        {
            var data = await _ordersStatusService.GetById(id);
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OrderStatus model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await _ordersStatusService.Update(model);
            if (result)
            {
                TempData["msg"] = "Added Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["msg"] = "Error on server side";
                return View(model);
            }
        }
        public async Task<IActionResult> Index(string searchString, string searchButton, string filterButton, string sortOrder, int? page)
        {
            var data = await _ordersStatusService.List(searchString, searchButton, filterButton, sortOrder, page);

            ViewBag.TotalPages = data.TotalPages;
            ViewBag.CurrentPage = data.CurrentPage;

            return View(data);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _ordersStatusService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
