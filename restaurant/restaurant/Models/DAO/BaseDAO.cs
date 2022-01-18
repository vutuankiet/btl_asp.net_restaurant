using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using restaurant.Models.EF;

namespace restaurant.Models.DAO
{
    public class BaseDAO
    {
        protected QL_NhaHangEntities1 _context;

        public BaseDAO()
        {
            _context = new QL_NhaHangEntities1();
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}