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
    public class ContactsController : BaseController
    {
        private ContactDAO dao = new ContactDAO();

        // GET: Contacts
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

        // GET: Contacts/Details/5
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = await dao.GetSingleByID((int)id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // GET: Contacts/Create
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create()
        {
            ViewBag.UserEmail = new SelectList(await new UserDAO().GetAll(), "ID", "Email");
            ViewBag.UserPhone = new SelectList(await new UserDAO().GetAll(), "ID", "PhoneNumber");
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,UserID,FirstName,LastName,Messenger")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                await dao.Add(contact);
                return RedirectToAction("Index");
            }

            ViewBag.UserEmail = new SelectList(await new UserDAO().GetAll(), "ID", "Email", contact.UserID);
            ViewBag.UserPhone = new SelectList(await new UserDAO().GetAll(), "ID", "PhoneNumber", contact.UserID);
            return View(contact);
        }

        // GET: Contacts/Edit/5
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = await dao.GetSingleByID((int)id);
            if (contact == null)
            {
                return HttpNotFound();
            }

            ViewBag.UserEmail = new SelectList(await new UserDAO().GetAll(), "ID", "Email", contact.UserID);
            ViewBag.UserPhone = new SelectList(await new UserDAO().GetAll(), "ID", "PhoneNumber", contact.UserID);
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,UserID,FirstName,LastName,Messenger")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                await dao.Update(contact);
                return RedirectToAction("Index");
            }
            ViewBag.UserEmail = new SelectList(await new UserDAO().GetAll(), "ID", "Email", contact.UserID);
            ViewBag.UserPhone = new SelectList(await new UserDAO().GetAll(), "ID", "PhoneNumber", contact.UserID);
            return View(contact);
        }

        // GET: Contacts/Delete/5
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = await dao.GetSingleByID((int)id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Delete/5
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
