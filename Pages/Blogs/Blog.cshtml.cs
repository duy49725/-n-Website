using DoAnWebNangCao.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DoAnWebNangCao.Pages.Blogs
{
    public class BlogModel : PageModel
    {
        private readonly DoAnWebNangCao.Data.ApplicationDbContext _context;

        public BlogModel(DoAnWebNangCao.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        public IList<Blog> Blog { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Blogs != null)
            {
                Blog = await _context.Blogs.ToListAsync();
            }
        }
    }
}
