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
    public class ContactDAO: BaseDAO, IContactDAO
    {
        public async Task<bool> Add(Contact entity)
        {
            try
            {
                _context.Contacts.Add(entity);

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
                _context.Contacts.Remove(ef);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<List<Contact>> GetAll()
        {
            return await _context.Contacts.ToListAsync();
        }

        public async Task<List<Contact>> GetByKeyword(string keyword)
        {
            return await _context.Contacts
                .Where(t => t.LastName.Contains(keyword))
                .OrderByDescending(t => t.ID)
                .ToListAsync();
        }

        public async Task<IPagedList<Contact>> GetByPaged(int page, int pageSize, string keyword)
        {
            var contacts = await GetByKeyword(keyword);

            return contacts.ToPagedList(page, pageSize);
        }

        public async Task<Contact> GetSingleByID(int id)
        {
            return await _context.Contacts.FindAsync(id);
        }

        public async Task<bool> Update(Contact entity)
        {
            try
            {
                var ef = await _context.Contacts.FindAsync(entity.ID);

                if (ef != null)
                {
                    ef.FirstName = entity.FirstName;
                    ef.LastName = entity.LastName;
                    ef.UserID = entity.UserID;
                    ef.Messenger = entity.Messenger;

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