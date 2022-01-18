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
    public class UsersController : BaseController
    {
        private UserDAO dao = new UserDAO();

        // GET: Users
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

        // GET: Users/Details/5
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await dao.GetSingleByID((string)id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        private string SetBId()
        {
            return BaseDAO.RandomString(10);
        }
        // GET: Users/Create
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create()
        {
            User model = new User() { ID = SetBId()};
            //ViewBag.ID = SetBId();
            ViewBag.RolesID = new SelectList(await new RoleDAO().GetAll(), "ID", "NameRoles");
            //ViewBag.Status = GetSelectListStatus(false, false);
            //GetSelectListRole();
            return View(model);
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,UserName,Password,ConfirmPassword,RolesID,Email,PhoneNumber,Status")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Status = false;
                await dao.Add(user);
                return RedirectToAction("Index");
            }
            ViewBag.ID = SetBId();
            ViewBag.RolesID = new SelectList(await new RoleDAO().GetAll(), "ID", "NameRoles", user.RolesID);
            //ViewBag.Status = GetSelectListStatus(false, (bool)user.Status);
            return View(user);
        }

        // GET: Users/Edit/5
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await dao.GetSingleByID((string)id);
            if (user == null)
            {
                return HttpNotFound();
            }
            user.Status = false;
            ViewBag.RolesID = new SelectList(await new RoleDAO().GetAll(), "ID", "NameRoles", user.RolesID);
            ViewBag.Status = GetSelectListStatus(false, (bool)user.Status);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,UserName,Password,ConfirmPassword,RolesID,Email,PhoneNumber,Status")] User user)
        {
            user.Status = false;
            if (ModelState.IsValid)
            {
                user.Status = false;
                await dao.Update(user);
                return RedirectToAction("Index");
            }

            ViewBag.RolesID = new SelectList(await new RoleDAO().GetAll(), "ID", "NameRoles", user.RolesID);
            ViewBag.Status = GetSelectListStatus(false, (bool)user.Status);
            return View(user);
        }

        // GET: Users/Delete/5
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await dao.GetSingleByID((string)id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        //[Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
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
                new {Name = "On", value = true},
                new {Name = "Off", value = false},
            };

            if (isSelectedValue)
            {
                return new SelectList(objects, "Value", "Name", val);

            }
            return new SelectList(objects, "Value", "Name");

        }

        //private SelectList GetSelectListRole(int? selectedId = null)
        //{
        //    var dao = new RoleDAO();
        //    ViewBag.RolesID = new SelectList((System.Collections.IEnumerable)dao.GetAll(), "ID", "NameRoles", selectedId);
        //}
    }
}
