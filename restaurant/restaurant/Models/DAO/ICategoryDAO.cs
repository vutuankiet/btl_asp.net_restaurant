using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using restaurant.Models.EF;
using restaurant.Models.DAO;

namespace restaurant.Models.DAO
{
    //xac dinh interface cac chuc nang cho ICategoryDAO
    //sau đó các bạn vào các interface của các đối tượng và thêm int vàotrừ customer là string
    public interface ICategoryDAO : IBaseDAO<Category, int>, IPagedListDAO<Category>
    {
    }
}