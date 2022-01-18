using restaurant.Models.EF;
using restaurant.Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace restaurant.Models.DAO
{
    public interface IUserDAO: IBaseDAO<User, string>, IPagedListDAO<User>
    {
    }
}