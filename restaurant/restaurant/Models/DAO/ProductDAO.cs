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
    public class ProductDAO : BaseDAO, IProductDAO
    {
        //public List<Product>GetAll()
        //{
        //    return _context.Products.ToList();
        //}
        public async Task<bool> Add(Product entity)
        {
            try
            {
                _context.Products.Add(entity);

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
                _context.Products.Remove(ef);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<List<Product>> GetAll()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<List<Product>> GetByKeyword(string keyword)
        {
            return await _context.Products
                .Where(t => t.NameProduct.Contains(keyword))
                .OrderByDescending(t => t.ID)
                .ToListAsync();
        }

        public async Task<IPagedList<Product>> GetByPaged(int page, int pageSize, string keyword)
        {
            var products = await GetByKeyword(keyword);

            return products.ToPagedList(page, pageSize);
        }

        public async Task<Product> GetSingleByID(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<bool> Update(Product entity)
        {
            try
            {
                var ef = await _context.Products.FindAsync(entity.ID);

                if (ef != null)
                {
                    ef.CategoryID = entity.CategoryID;
                    ef.NameProduct = entity.NameProduct;
                    ef.Descriptions = entity.Descriptions;
                    ef.Images = entity.Images;
                    ef.Date = entity.Date;
                    ef.UnitPrice = entity.UnitPrice;

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