using restaurant.Models.DAO;
using restaurant.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace restaurant.Controllers
{
    //[Authorize]
    public class AdminController : Controller
    {
        // GET: Admin
        //[Authorize(Roles = "Admin")]
        private OrderDAO orderdao = new OrderDAO();

        public async Task<ActionResult> Index(int page = 1, int pageSize = 4, string keyword = "")
        {
            var model = await orderdao.GetByPaged(page, pageSize, keyword);

            ViewBag.Keyword = keyword;
            ViewBag.Page = page;
            ViewBag.Pagesize = pageSize;
            using (QL_NhaHangEntities1 db = new QL_NhaHangEntities1())
            {
                ViewBag.user = db.Users.Count();
                ViewBag.product = db.Products.Count();
                ViewBag.order = db.Orders.Count();
                ViewBag.haveship = db.Orders.Where(s => s.Status == true).Count();
                ViewBag.contact = db.Contacts.Count();
            }
                return View(model);
        }
        public ActionResult GamePlay()
        {
            return View();
        }
    }
}