using restaurant.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace restaurant.Models.DAO
{
    public interface IProductDAO : IBaseDAO<Product, int>, IPagedListDAO<Product>
    {
        //Task<User> GetSingleByID(string id);
    }
}