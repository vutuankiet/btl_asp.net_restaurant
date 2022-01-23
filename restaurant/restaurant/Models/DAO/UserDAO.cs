using PagedList;
using restaurant.Models.EF;
using restaurant.Models.DAO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace restaurant.Models.DAO
{
    public class UserDAO : BaseDAO , IUserDAO
    {
        public async Task<bool> Add(User entity)
        {
            try
            {
                _context.Users.Add(entity);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            return true;
        }

        public async Task<bool> Delete(string id)
        {
            var ef = await GetSingleByID(id);

            try
            {
                _context.Users.Remove(ef);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<List<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<List<User>> GetByKeyword(string keyword)
        {
            return await _context.Users
                .Where(t => t.UserName.Contains(keyword))
                .OrderByDescending(t => t.ID)
                .ToListAsync();
        }

        public async Task<IPagedList<User>> GetByPaged(int page, int pageSize, string keyword)
        {
            var users = await GetByKeyword(keyword);

            return users.ToPagedList(page, pageSize);
        }

        public async Task<User> GetSingleByID(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<bool> Update(User entity)
        {
            try
            {
                var ef = await _context.Users.FindAsync(entity.ID);

                if (ef != null)
                {
                    ef.UserName = entity.UserName;
                    ef.Password = entity.Password;
                    ef.ConfirmPassword = entity.ConfirmPassword;
                    ef.RolesID = entity.RolesID;
                    ef.Email = entity.Email;
                    ef.PhoneNumber = entity.PhoneNumber;
                    ef.Status = entity.Status;

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
        public bool CheckEmail(string email)
        {
            using (QL_NhaHangEntities1 db = new QL_NhaHangEntities1())
            {
                return db.Users.Count(x => x.Email == email) > 0;
            }
        }
    }
}