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
    public class RoleDAO : BaseDAO, IRoleDAO
    {
        public async Task<bool> Add(Role entity)
        {
            try
            {
                _context.Roles.Add(entity);

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
                _context.Roles.Remove(ef);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<List<Role>> GetAll()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<List<Role>> GetByKeyword(string keyword)
        {
            return await _context.Roles
                .Where(t => t.NameRoles.Contains(keyword))
                .OrderByDescending(t => t.ID)
                .ToListAsync();
        }

        public async Task<IPagedList<Role>> GetByPaged(int page, int pageSize, string keyword)
        {
            var roles = await GetByKeyword(keyword);

            return roles.ToPagedList(page, pageSize);
        }

        public async Task<Role> GetSingleByID(int id)
        {
            return await _context.Roles.FindAsync(id);
        }

        public async Task<bool> Update(Role entity)
        {
            try
            {
                var ef = await _context.Roles.FindAsync(entity.ID);

                if (ef != null)
                {
                    ef.NameRoles = entity.NameRoles;

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