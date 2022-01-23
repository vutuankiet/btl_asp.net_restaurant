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
    public class OrderDAO : BaseDAO, IOrderDAO
    {
        public async Task<bool> Add(Order entity)
        {
            try
            {
                _context.Orders.Add(entity);

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
                _context.Orders.Remove(ef);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<List<Order>> GetAll()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<List<Order>> GetByKeyword(string keyword)
        {
            return await _context.Orders
                .Where(t => t.User.UserName.Contains(keyword))
                .OrderByDescending(t => t.ID)
                .ToListAsync();
        }

        public async Task<IPagedList<Order>> GetByPaged(int page, int pageSize, string keyword)
        {
            var orders = await GetByKeyword(keyword);

            return orders.ToPagedList(page, pageSize);
        }

        public async Task<Order> GetSingleByID(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<bool> Update(Order entity)
        {
            try
            {
                var ef = await _context.Orders.FindAsync(entity.ID);

                if (ef != null)
                {
                    //ef.UserID = entity.UserID;
                    //ef.ProductID = entity.ProductID;
                    //ef.OrderDate = entity.OrderDate;
                    ef.Status = entity.Status;
                    //ef.Quantily = entity.Quantily;
                    //ef.Discount = entity.Discount;
                    //ef.TotalPrice = entity.TotalPrice;

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