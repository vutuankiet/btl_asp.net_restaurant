using restaurant.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace restaurant.Models.DAO
{
    public interface IOrderDAO : IBaseDAO<Order, int>, IPagedListDAO<Order>
    {
    }
}