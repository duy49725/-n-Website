using DoAnWebNangCao.Data.Paging;
using DoAnWebNangCao.Data;
using DoAnWebNangCao.Models;
using Microsoft.EntityFrameworkCore;
using DoAnWebNangCao.Repositories.Abstraction;

namespace DoAnWebNangCao.Repositories.Implementation
{
    public class PublisherService : IPublisherService
    {
        private readonly ApplicationDbContext _context;
        public PublisherService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(Publisher model)
        {
            try
            {
                _context.Publishers.Add(model);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var data = await _context.Publishers.FindAsync(id);
                if (data == null)
                    return false;
                _context.Publishers.Remove(data);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Publisher> GetById(int id)
        {
            return await _context.Publishers.FindAsync(id);
        }
        public IQueryable<Publisher> List()
        {
            var data = _context.Publishers.AsQueryable();
            return data;
        }

        public async Task<PublisherPaging> List(string searchString, string searchButton, string filterButton, string sortOrder, int? page)
        {
            var query = from g in _context.Publishers
                        select g;
            if (!string.IsNullOrEmpty(searchButton) && !String.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.PublisherName.Contains(searchString));
            }

            query = sortOrder switch
            {
                "PublisherName_desc" => query.OrderByDescending(s => s.PublisherName),
                _ => query.OrderBy(s => s.PublisherName)
            };
            int pageSize = 8;
            int pageNumber = page ?? 1;
            var publisherPaging = new PublisherPaging
            {
                Publishers = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync()
            };
            publisherPaging.TotalPages = (int)Math.Ceiling(await query.CountAsync() / (double)pageSize);
            publisherPaging.CurrentPage = pageNumber;
            return publisherPaging;
        }

        public async Task<bool> Update(Publisher model)
        {
            try
            {
                _context.Publishers.Update(model);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
