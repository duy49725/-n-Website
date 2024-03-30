using DoAnWebNangCao.Data.Paging;
using DoAnWebNangCao.Data;
using DoAnWebNangCao.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnWebNangCao.Repositories.Abstraction;

namespace DoAnWebNangCao.Repositories.Implementation
{
    public class AuthorService : IAuthorService
    {
        private readonly ApplicationDbContext _context;
        public AuthorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(Author model)
        {
            try
            {
                _context.Authors.Add(model);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IQueryable<Author> List()
        {
            var data = _context.Authors.AsQueryable();
            return data;
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var data = await _context.Authors.FindAsync(id);
                if (data == null)
                    return false;
                _context.Authors.Remove(data);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Author> GetById(int id)
        {
            return await _context.Authors.FindAsync(id);
        }

        public async Task<AuthorPaging> List(string searchString, string searchButton, string filterButton, string sortOrder, int? page)
        {
            var query = from g in _context.Authors
                        select g;
            if (!string.IsNullOrEmpty(searchButton) && !String.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.AuthorName.Contains(searchString));
            }

            query = sortOrder switch
            {
                "AuthorName_desc" => query.OrderByDescending(s => s.AuthorName),
                _ => query.OrderBy(s => s.AuthorName)
            };
            int pageSize = 8;
            int pageNumber = page ?? 1;
            var authorPaging = new AuthorPaging
            {
                author = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync()
            };
            authorPaging.TotalPages = (int)Math.Ceiling(await query.CountAsync() / (double)pageSize);
            authorPaging.CurrentPage = pageNumber;
            return authorPaging;
        }

        public async Task<bool> Update(Author model)
        {
            try
            {
                _context.Authors.Update(model);
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
