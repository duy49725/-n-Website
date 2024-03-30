using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DoAnWebNangCao.Data;
using DoAnWebNangCao.Models;
using DoAnWebNangCao.Repositories.Abstraction;

namespace DoAnWebNangCao.Pages.Blogs
{
    public class CreateModel : PageModel
    {
        private readonly DoAnWebNangCao.Data.ApplicationDbContext _context;
        private readonly IFileService _fileService;

        public CreateModel(DoAnWebNangCao.Data.ApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Blog Blog { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Blogs == null || Blog == null)
            {
                return Page();
            }
            _context.Blogs.Add(Blog);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
