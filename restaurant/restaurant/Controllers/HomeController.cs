using restaurant.Models.DAO;
using restaurant.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace restaurant.Controllers
{
    public class HomeController : Controller
    {
        private ProductDAO dao = new ProductDAO();
        private OrderDAO orderdao = new OrderDAO();
        //QL_NhaHangEntities1 objModel = new QL_NhaHangEntities1();
        public async Task<ActionResult> Index()
        {
            //var lstCategory = objModel.Categories.ToList();
            //var lstProduct = objModel.Products.ToList();

            //Product_Category objProduct_Category = new Product_Category();

            //objProduct_Category.ListCategory = lstCategory;
            //objProduct_Category.ListProduct = lstProduct;
            //return View(objProduct_Category);

            //var ListProdust = productDAO.GetAll();
            //return View(ListProdust);
            var model = await dao.GetAll();
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Contact([Bind(Include = "ID,UserID,FirstName,LastName,Messenger")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                using (QL_NhaHangEntities1 db = new QL_NhaHangEntities1())
                {
                    if (Session["ID"] != null)
                    {
                        contact.UserID = Session["ID"].ToString();
                        db.Contacts.Add(contact);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Alert = "Fall!";
                    }
                }
            }

            return View(contact);
        }

        public async Task<ActionResult> Payment(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Menu", "Home");
            }
            Product product = await dao.GetSingleByID((int)id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost]
        public async Task<ActionResult> AddToCart([Bind(Include = "ID,UserID,ProductID,OrderDate,Status,Quantily,Discount,TotalPrice")]Order order,int? id)
        {
            if (ModelState.IsValid)
            {
                using (QL_NhaHangEntities1 db = new QL_NhaHangEntities1())
                {
                    Product product = await dao.GetSingleByID((int)id);

                    var Order = db.Orders.Where(o => o.ID == order.ID).FirstOrDefault();
                    if (Session["ID"] != null /*&& productOrder != null*/)
                    {
                        order.UserID = Session["ID"].ToString();
                        order.ProductID = product.ID;
                        order.OrderDate = DateTime.Now;
                        order.Status = false;
                        order.Discount = (int?)(product.Discount * 1);
                        order.TotalPrice = order.Quantily * product.UnitPrice * 1;

                        db.Orders.Add(order);
                        db.SaveChanges();

                        ViewBag.Alert = "Order Successfuly!";
                        return RedirectToAction("Menu", "Home");
                    }
                    else
                    {
                        ViewBag.Alert = "Fail!";
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Wrong Product!");
            }
            var model = await dao.GetAll();
            return View(model);
        }

        public ActionResult Event()
        {
            return View();
        }

        public async Task<ActionResult> Menu(int page = 1, int pageSize = 30, string keyword = "")
        {
            var model = await dao.GetByPaged(page, pageSize, keyword);

            ViewBag.Keyword = keyword;
            ViewBag.Page = page;
            ViewBag.Pagesize = pageSize;

            return View(model);

        }

        public async Task<ActionResult> HistoryOrder(int page = 1, int pageSize = 30, string keyword = "")
        {
            var model = await orderdao.GetByPaged(page, pageSize, keyword);

            ViewBag.Keyword = keyword;
            ViewBag.Page = page;
            ViewBag.Pagesize = pageSize;

            return View(model);
        }

        //// POST: Cart/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Menu([Bind(Include = "ID,UserID,ProductID,OrderDate,Status,Quantily,Discount,TotalPrice")] Order order1)
        //{
        //    OrderDAO dao1 = new OrderDAO();
        //    order1.UserID = Session["ID"].ToString();
        //    order1.ProductID = Session["ProductID"].ToString();
        //    order1.OrderDate = DateTime.Now;
        //    order1.Quantily = 1;
        //    if (ModelState.IsValid)
        //    {
        //        await dao1.Add(order1);
        //        return RedirectToAction("Index");
        //    }
        //    return View(order1);
        //}

        //[HttpPost]
        //public async Task<ActionResult> Menu(Order order, int? id)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        using (QL_NhaHangEntities1 db = new QL_NhaHangEntities1())
        //        {
        //            Product product = await dao.GetSingleByID((int)id);

        //            var Order = db.Orders.Where(o => o.ID == order.ID).FirstOrDefault();
        //            if (Session["ID"] != null /*&& productOrder != null*/)
        //            {
        //                order.UserID = Session["ID"].ToString();
        //                order.ProductID = product.ID;
        //                order.OrderDate = DateTime.Now;
        //                order.Quantily = 1;
        //                order.TotalPrice = product.UnitPrice;

        //                db.Orders.Add(order);
        //                db.SaveChanges();

        //                ViewBag.Alert = "Order Successfuly!";
        //                return RedirectToAction("Payment", "Home");
        //            }
        //            else
        //            {
        //                ViewBag.Alert = "Fall!";
        //            }
        //        }
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "Wrong Product!");
        //    }
        //    var model = await dao.GetAll();
        //    return View(model);

        //}



    }
}