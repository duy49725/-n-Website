using DoAnWebNangCao.Data;
using DoAnWebNangCao.Data.Paging;
using DoAnWebNangCao.Models;
using DoAnWebNangCao.Repositories.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace DoAnWebNangCao.Repositories.Implementation
{
    public class GenreService : IGenreService
    {
        private readonly ApplicationDbContext _context;
        public GenreService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(Genre model)
        {
            try
            {
                _context.Genres.Add(model);
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
                var data = await _context.Genres.FindAsync(id);
                if (data == null)
                    return false;
                _context.Genres.Remove(data);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IQueryable<Genre> List()
        {
            var data = _context.Genres.AsQueryable();
            return data;
        }

        public async Task<Genre> GetById(int id)
        {
            return await _context.Genres.FindAsync(id);
        }

        public async Task<GenrePaging> List(string searchString, string searchButton, string filterButton, string sortOrder, int? page)
        {
            var query = from g in _context.Genres
                        select g;
            if (!string.IsNullOrEmpty(searchButton) && !String.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.GenreName.Contains(searchString));
            }

            query = sortOrder switch
            {
                "GenreName_desc" => query.OrderByDescending(s => s.GenreName),
                _ => query.OrderBy(s => s.GenreName)
            };
            int pageSize = 2;
            int pageNumber = page ?? 1;
            var genrePaging = new GenrePaging 
            { 
                genres = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync()
            };
            genrePaging.TotalPages = (int)Math.Ceiling(await query.CountAsync() / (double)pageSize);
            genrePaging.CurrentPage = pageNumber;
            return genrePaging;
        }

        public async Task<bool> Update([Bind("Id, GenreName")]Genre model)
        {
            try
            {
                 _context.Genres.Update(model);
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
