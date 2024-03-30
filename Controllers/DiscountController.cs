using DoAnWebNangCao.Models;
using DoAnWebNangCao.Repositories.Abstraction;
using DoAnWebNangCao.Repositories.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace DoAnWebNangCao.Controllers
{
    public class DiscountController : Controller
    {
        private readonly IDiscountService _discountService;
        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        public async Task<IActionResult> Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Discount model)
        {
            ModelState.Remove("Books");
            if (!ModelState.IsValid)
                return View(model);
            var result = await _discountService.Add(model);
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
            var data = await _discountService.GetById(id);
            return View(data);
        }
        public async Task<IActionResult> Details(int id)
        {
            var data = await _discountService.GetById(id);
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Discount model)
        {
            ModelState.Remove("Books");
            if (!ModelState.IsValid)
                return View(model);
            var result = await _discountService.Update(model);
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
            var data = await _discountService.List(searchString, searchButton, filterButton, sortOrder, page);

            ViewBag.TotalPages = data.TotalPages;
            ViewBag.CurrentPage = data.CurrentPage;

            return View(data);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _discountService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
