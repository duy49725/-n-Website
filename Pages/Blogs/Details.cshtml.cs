using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DoAnWebNangCao.Data;
using DoAnWebNangCao.Models;

namespace DoAnWebNangCao.Pages.Blogs
{
    public class DetailsModel : PageModel
    {
        private readonly DoAnWebNangCao.Data.ApplicationDbContext _context;

        public DetailsModel(DoAnWebNangCao.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public Blog Blog { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }
            else 
            {
                Blog = blog;
            }
            return Page();
        }
    }
}
