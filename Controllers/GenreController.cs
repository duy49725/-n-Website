using DoAnWebNangCao.Models;
using DoAnWebNangCao.Repositories.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace DoAnWebNangCao.Controllers
{
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;
        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        public async Task<IActionResult> Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Genre model)
        {
            if(!ModelState.IsValid) 
                return View(model);
            var result = await _genreService.Add(model);
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
            var data = await _genreService.GetById(id);
            return View(data);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var data = await _genreService.GetById(id);
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Genre model) 
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await _genreService.Update(model);
            if (result)
            {
                TempData["msg"] = "Added Successfully";
                return RedirectToAction(nameof(GenreList));
            }
            else
            {
                TempData["msg"] = "Error on server side";
                return View(model);
            }
        }
        public async Task<IActionResult> GenreList(string searchString, string searchButton, string filterButton, string sortOrder, int? page)
        { 
            var data = await _genreService.List( searchString,  searchButton,  filterButton,  sortOrder, page);

            ViewBag.TotalPages = data.TotalPages;
            ViewBag.CurrentPage = data.CurrentPage;

            return View(data);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _genreService.Delete(id);
            return RedirectToAction(nameof(GenreList));
        }
    }
}
