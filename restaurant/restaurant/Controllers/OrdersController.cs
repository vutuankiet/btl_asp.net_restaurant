using restaurant.Models.DAO;
using restaurant.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace restaurant.Controllers
{
    //[Authorize]
    public class OrdersController : BaseController
    {
        private OrderDAO dao = new OrderDAO();

        // GET: Orders
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index(int page = 1, int pageSize = 10, string keyword = "")
        {
            var model = await dao.GetByPaged(page, pageSize, keyword);

            ViewBag.Keyword = keyword;
            ViewBag.Page = page;
            ViewBag.Pagesize = pageSize;

            return View(model);

            //return View(await dao.GetPagedList(page, pageSize, keyword));
        }

        // GET: Orders/Details/5
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await dao.GetSingleByID((int)id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create()
        {
            Order model = new Order()
            {
                OrderDate = DateTime.Now,
            };

            ViewBag.UserID = new SelectList(await new UserDAO().GetAll(), "ID", "UserName", "RolesID", "Email", "PhoneNumber");
            ViewBag.ProductID = new SelectList(await new ProductDAO().GetAll(), "ID", "NameProduct");
            return View(model);
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,UserID,ProductID,OrderDate,Status,Quantily,Discount,TotalPrice")] Order order)
        {
            if (ModelState.IsValid)
            {
                await dao.Add(order);
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(await new RoleDAO().GetAll(), "ID", "UserName", "RolesID", "Email", "PhoneNumber", order.UserID);
            ViewBag.ProductID = new SelectList(await new ProductDAO().GetAll(), "ID", "NameProduct", order.Product.NameProduct);
            return View(order);
        }

        // GET: Orders/Edit/5
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await dao.GetSingleByID((int)id);
            if (order == null)
            {
                return HttpNotFound();
            }

            ViewBag.UserID = new SelectList(await new RoleDAO().GetAll(), "ID", "UserName", "RolesID", "Email", "PhoneNumber", order.UserID);
            ViewBag.ProductID = new SelectList(await new ProductDAO().GetAll(), "ID", "NameProduct", order.Product.NameProduct);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,UserID,ProductID,OrderDate,Status,Quantily,Discount,TotalPrice")] Order order)
        {
            if (ModelState.IsValid)
            {
                await dao.Update(order);
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(await new RoleDAO().GetAll(), "ID", "UserName", "RolesID", "Email", "PhoneNumber", order.UserID);
            ViewBag.ProductID = new SelectList(await new ProductDAO().GetAll(), "ID", "NameProduct", order.Product.NameProduct);
            return View(order);
        }

        // GET: Orders/Delete/5
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await dao.GetSingleByID((int)id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Categories/Delete/5
        //[Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await dao.Delete(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
