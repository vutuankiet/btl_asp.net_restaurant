using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using restaurant.Models.EF;
using restaurant.Models.DAO;

namespace restaurant.Models.DAO
{
    public class CategoryDAO : BaseDAO, ICategoryDAO
    {
        public async Task<bool> Add(Category entity)
        {
            try
            {
                _context.Categories.Add(entity);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var ef = await GetSingleByID(id);

            try
            {
                _context.Categories.Remove(ef);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<List<Category>> GetAll()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<List<Category>> GetByKeyword(string keyword)
        {
            return await _context.Categories
                .Where(t => t.Name.Contains(keyword))
                .OrderByDescending(t => t.ID)
                .ToListAsync();
        }

        public async Task<IPagedList<Category>> GetByPaged(int page, int pageSize, string keyword)
        {
            var categories = await GetByKeyword(keyword);

            return categories.ToPagedList(page, pageSize);
        }

        public async Task<Category> GetSingleByID(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<bool> Update(Category entity)
        {
            try
            {
                var ef = await _context.Categories.FindAsync(entity.ID);

                if (ef != null)
                {
                    ef.Name = entity.Name;

                    await _context.SaveChangesAsync();
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

            return true;
        }
    }
}