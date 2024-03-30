using DoAnWebNangCao.Models;
using DoAnWebNangCao.Repositories.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace DoAnWebNangCao.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponRepository _couponRepository;
        public CouponController(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Coupon model)
        {
            if(!ModelState.IsValid) 
                return View(model);
            var result = await _couponRepository.Add(model);
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
            var data = await _couponRepository.GetById(id);
            return View(data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var data = await _couponRepository.GetById(id);
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Coupon model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await _couponRepository.Update(model);
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
            var data = await _couponRepository.List(searchString, searchButton, filterButton, sortOrder, page);
            ViewBag.TotalPages = data.TotalPages;
            ViewBag.CurrentPage = data.CurrentPage;
            return View(data);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _couponRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
