using PagedList;
using restaurant.Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace restaurant.Models.DAO
{
    public class OrderDestailDAO : BaseDAO, IOrderDestailDAO
    {
        public async Task<bool> Add(OrderDestail entity)
        {
            try
            {
                _context.OrderDestails.Add(entity);

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
                _context.OrderDestails.Remove(ef);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<List<OrderDestail>> GetAll()
        {
            return await _context.OrderDestails.ToListAsync();
        }

        public async Task<List<OrderDestail>> GetByKeyword(string keyword)
        {
            return await _context.OrderDestails
                .Where(t => t.Order.User.UserName.Contains(keyword))
                .OrderByDescending(t => t.ID)
                .ToListAsync();
        }

        public async Task<IPagedList<OrderDestail>> GetByPaged(int page, int pageSize, string keyword)
        {
            var orderDestails = await GetByKeyword(keyword);

            return orderDestails.ToPagedList(page, pageSize);
        }

        public async Task<OrderDestail> GetSingleByID(int id)
        {
            return await _context.OrderDestails.FindAsync(id);
        }

        public async Task<bool> Update(OrderDestail entity)
        {
            try
            {
                var ef = await _context.OrderDestails.FindAsync(entity.ID);

                if (ef != null)
                {
                    ef.OrderID = entity.OrderID;
                    ef.Destail = entity.Destail;
                    ef.Status = entity.Status;
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