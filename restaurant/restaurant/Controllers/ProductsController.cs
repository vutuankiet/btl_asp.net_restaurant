using restaurant.Models.DAO;
using restaurant.Models.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace restaurant.Controllers
{
    public class ProductsController : BaseController
    {
        private ProductDAO dao = new ProductDAO();

        // GET: Products
        public async Task<ActionResult> Index(int page = 1, int pageSize = 10, string keyword = "")
        {
            var model = await dao.GetByPaged(page, pageSize, keyword);

            ViewBag.Keyword = keyword;
            ViewBag.Page = page;
            ViewBag.Pagesize = pageSize;

            return View(model);

            //return View(await dao.GetPagedList(page, pageSize, keyword));
        }

        // GET: Products/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await dao.GetSingleByID((int)id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public async Task<ActionResult> Create()
        {
            Product model = new Product()
            {
                Date = DateTime.Now,
            };

            ViewBag.CategoryID = new SelectList(await new CategoryDAO().GetAll(), "ID", "Name");
            return View(model);
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,CategoryID,NameProduct,Descriptions,Images,Date,UnitPrice")] Product product, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                // Create avatar on server
                var filename = Path.GetFileName(product.Images);
                var extension = Path.GetExtension(product.Images);
                filename = filename + "-" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-tt") + extension + ".png";
                var path = Path.Combine(Server.MapPath("~/images/"), filename);
                file.SaveAs(path);
                // Add avatar reference to model and save
                product.Images = string.Concat("images/", filename);

                if (await dao.Add(product))
                {
                    return RedirectToAction("Index");
                }
                //await dao.Add(product);
                //return RedirectToAction("Index");
                using (QL_NhaHangEntities1 db = new QL_NhaHangEntities1())
                {
                    var productss = db.Products.Where(p => p.ID == product.ID && p.NameProduct == product.NameProduct).FirstOrDefault();
                    if (productss != null)
                    {
                        Session["ProductID"] = productss.ID.ToString();
                        Session["ProductName"] = productss.NameProduct.ToString();
                        Session["ProductImage"] = productss.Images.ToString();
                        Session["ProductCategories"] = productss.CategoryID.ToString();
                        Session["ProductUnitPrice"] = productss.UnitPrice.ToString();
                    }
                }
            }
            ViewBag.CategoryID = new SelectList(await new CategoryDAO().GetAll(), "ID", "Name", product.CategoryID);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await dao.GetSingleByID((int)id);
            if (product == null)
            {
                return HttpNotFound();
            }

            ViewBag.CategoryID = new SelectList(await new CategoryDAO().GetAll(), "ID", "Name", product.CategoryID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,CategoryID,NameProduct,Descriptions,Images,Date,UnitPrice")] Product product, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                Session["ID"] = product.ID.ToString();
                // Create avatar on server
                var filename = Path.GetFileName(product.NameProduct);
                var extension = Path.GetExtension(product.Images);
                filename = filename + "-" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-tt") + extension + ".png";
                var path = Path.Combine(Server.MapPath("~/images/"), filename);
                if(file != null)
                {
                    file.SaveAs(path);
                }
                
                // Add avatar reference to model and save
                product.Images = string.Concat("images/", filename);

                await dao.Update(product);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(await new CategoryDAO().GetAll(), "ID", "Name", product.CategoryID);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await dao.GetSingleByID((int)id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
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

        //private FileResult UpLoadImageProduct (Product Image)
        //{
        //    string FileName = Path.GetFileNameWithoutExtension(Image.Images);
        //    string Extension = Path.GetExtension(Image.Images);
        //    FileName = FileName + DateTime.Now.ToString("dd-MM-yyyy") + Extension;
        //    Image.Images = "../images/" + FileName;
        //    FileName = Path.Combine(Server.MapPath("../images/"), FileName);
        //    Image.Images.SaveAs(FileName);
        //    ProductDAO productDAO = new ProductDAO();
        //}
    }
}
