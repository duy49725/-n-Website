using DoAnWebNangCao.Models;
using DoAnWebNangCao.Repositories.Abstraction;
using Microsoft.AspNetCore.Mvc;


namespace DoAnWebNangCao.Controllers
{
    public class PublisherController : Controller
    {
        private readonly IPublisherService _publisherService;
        public PublisherController(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        public async Task<IActionResult> Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Publisher model)
        {
           
            ModelState.Remove("Books");
            if (!ModelState.IsValid)
                return View(model);
            var result = await _publisherService.Add(model);
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
            var data = await _publisherService.GetById(id);
            return View(data);
        }
        public async Task<IActionResult> Details(int id)
        {
            var data = await _publisherService.GetById(id);
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Publisher model)
        {
            ModelState.Remove("Books");
            if (!ModelState.IsValid)
                return View(model);
            var result = await _publisherService.Update(model);
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
            var data = await _publisherService.List(searchString, searchButton, filterButton, sortOrder, page);

            ViewBag.TotalPages = data.TotalPages;
            ViewBag.CurrentPage = data.CurrentPage;

            return View(data);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _publisherService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
