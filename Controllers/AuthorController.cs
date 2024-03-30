using DoAnWebNangCao.Models;
using DoAnWebNangCao.Repositories.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace DoAnWebNangCao.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IAuthorService _authorService;
        private readonly IFileService _fileService;
        public AuthorController(IAuthorService authorService, IFileService fileService)
        {
            _authorService = authorService;
            _fileService = fileService;
        }

        public async Task<IActionResult> Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Author model)
        {
			if (!ModelState.IsValid)
				return View(model);
			if (model.ImageFile != null)
			{
				var fileResult = this._fileService.SaveImage(model.ImageFile);
				if (fileResult.Item1 == 0)
				{
					TempData["msg"] = "File could not saved";
					return View(model);
				}
				var imageName = fileResult.Item2;
				model.Image = imageName;
			}
			var result = await _authorService.Add(model);
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
            var data = await _authorService.GetById(id);
            return View(data);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var data = await _authorService.GetById(id);
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Author model)
        {
			if (!ModelState.IsValid)
				return View(model);
			if (model.ImageFile != null)
			{
				var fileResult = this._fileService.SaveImage(model.ImageFile);
				if (fileResult.Item1 == 0)
				{
					TempData["msg"] = "File could not saved";
					return View(model);
				}
				var imageName = fileResult.Item2;
				model.Image = imageName;
			}
			var result = await _authorService.Update(model);
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
            var data = await _authorService.List(searchString, searchButton, filterButton, sortOrder, page);

            ViewBag.TotalPages = data.TotalPages;
            ViewBag.CurrentPage = data.CurrentPage;

            return View(data);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _authorService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
