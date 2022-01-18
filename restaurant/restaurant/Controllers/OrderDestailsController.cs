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
    public class OrderDestailsController : BaseController
    {
        private OrderDestailDAO dao = new OrderDestailDAO();

        // GET: OrderDestails
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

        // GET: OrderDestails/Details/5
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDestail orderDestail = await dao.GetSingleByID((int)id);
            if (orderDestail == null)
            {
                return HttpNotFound();
            }
            return View(orderDestail);
        }

        // GET: OrderDestails/Create
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create()
        {
            
            ViewBag.OrderID = new SelectList(await new OrderDAO().GetAll(), "ID","OrderDate");
            ViewBag.UserID = new SelectList(await new UserDAO().GetAll(), "ID", "UserName");
            ViewBag.ProductID = new SelectList(await new ProductDAO().GetAll(), "ID", "NameProduct");
            ViewBag.Status = GetSelectListStatus(false, false);
            return View();
        }

        // POST: OrderDestails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,OrderID,Destail,Status,UnitPrice")] OrderDestail orderDestail)
        {
            if (ModelState.IsValid)
            {
                orderDestail.Status = false;
                await dao.Add(orderDestail);
                return RedirectToAction("Index");
            }

            ViewBag.OrderID = new SelectList(await new OrderDAO().GetAll(), "ID", "OrderDate", orderDestail.OrderID);
            ViewBag.UserID = new SelectList(await new UserDAO().GetAll(), "ID", "UserName", orderDestail.Order.User.UserName);
            ViewBag.ProductID = new SelectList(await new ProductDAO().GetAll(), "ID", "NameProduct", orderDestail.Order.Product.NameProduct);
            ViewBag.Status = GetSelectListStatus(false, (bool)orderDestail.Status);
            return View(orderDestail);
        }

        // GET: OrderDestails/Edit/5
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDestail orderDestail = await dao.GetSingleByID((int)id);
            if (orderDestail == null)
            {
                return HttpNotFound();
            }

            
            ViewBag.OrderID = new SelectList(await new OrderDAO().GetAll(), "ID", "OrderDate", orderDestail.OrderID);
            ViewBag.UserID = new SelectList(await new UserDAO().GetAll(), "ID", "UserName", orderDestail.Order.User.UserName);
            ViewBag.ProductID = new SelectList(await new ProductDAO().GetAll(), "ID", "NameProduct", orderDestail.Order.Product.NameProduct);
            ViewBag.Status = GetSelectListStatus(false, (bool)orderDestail.Status);
            return View(orderDestail);
        }

        // POST: OrderDestails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,OrderID,Destail,Status,UnitPrice")] OrderDestail orderDestail)
        {
            if (ModelState.IsValid)
            {
                await dao.Update(orderDestail);
                return RedirectToAction("Index");
            }

            
            ViewBag.OrderID = new SelectList(await new OrderDAO().GetAll(), "ID", "OrderDate", orderDestail.OrderID);
            ViewBag.UserID = new SelectList(await new UserDAO().GetAll(), "ID", "UserName", orderDestail.Order.User.UserName);
            ViewBag.ProductID = new SelectList(await new ProductDAO().GetAll(), "ID", "NameProduct", orderDestail.Order.Product.NameProduct);
            ViewBag.Status = GetSelectListStatus(false, (bool)orderDestail.Status);
            return View(orderDestail);
        }

        // GET: OrderDestails/Delete/5
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDestail orderDestail = await dao.GetSingleByID((int)id);
            if (orderDestail == null)
            {
                return HttpNotFound();
            }
            return View(orderDestail);
        }

        // POST: OrderDestails/Delete/5
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

        private SelectList GetSelectListStatus(bool isSelectedValue, bool val)
        {
            List<object> objects = new List<object>()
            {
                new {Name = "In process...", value = true},
                new {Name = "Paid", value = false},
            };

            if (isSelectedValue)
            {
                return new SelectList(objects, "Value", "Name", val);

            }
            return new SelectList(objects, "Value", "Name");

        }
    }
}
