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
    public class CartController : BaseController
    {
        private OrderDAO dao = new OrderDAO();
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }

        // GET: Cart/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Cart/Create
        public ActionResult AddtoCart()
        {
            Order model = new Order()
            {
                OrderDate = DateTime.Now,
            };
            return View(model);
        }

        // POST: Cart/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddtoCart([Bind(Include = "ID,UserID,ProductID,OrderDate,Status,Quantily,Discount,TotalPrice")] Order order)
        {
            order.OrderDate = DateTime.Now;
            order.Quantily = 1;
            if (ModelState.IsValid)
            {
                await dao.Add(order);
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Cart/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Cart/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Cart/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Cart/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
