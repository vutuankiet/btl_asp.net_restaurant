using restaurant.Models.DAO;
using restaurant.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace restaurant.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private UserDAO dao = new UserDAO();

        // GET: Account
        public async Task<ActionResult> Index(int page = 1, int pageSize = 10, string keyword = "")
        {
            var model = await dao.GetByPaged(page, pageSize, keyword);

            ViewBag.Keyword = keyword;
            ViewBag.Page = page;
            ViewBag.Pagesize = pageSize;

            return View(model);

            //return View(await dao.GetPagedList(page, pageSize, keyword));
        }

        // GET: Account/Details/5
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
        // GET: Account/Create
        [AllowAnonymous]
        public async Task<ActionResult> Register()
        {
            User model = new User() { ID = SetBId() };
            //ViewBag.ID = SetBId();
            ViewBag.RolesID = new SelectList(await new RoleDAO().GetAll(), "ID", "NameRoles");
            //viewbag.available = getselectliststatus(false, false);
            //GetSelectListRole();
            return View(model);
        }

        // POST: Account/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register([Bind(Include = "ID,UserName,Password,ConfirmPassword,RolesID,Email,PhoneNumber,Status")] User user)
        {
            if (ModelState.IsValid)
            {
                
                //??để thông báo thôi thanhf coong m redirect ve index ma
                using (QL_NhaHangEntities1 db = new QL_NhaHangEntities1())
                {
                    var userRegister = db.Users.Where(u => u.Email == user.Email && u.Password == user.Password).FirstOrDefault();
                    if(dao.CheckEmail(user.Email))
                    {
                        ModelState.AddModelError("", "Email already exists!");
                    }
                    else
                    {
                        user.ID = SetBId();
                        user.RolesID = 1;
                        user.Status = true;
                        await dao.Add(user);
                        ViewBag.SuccessMessage = "Registration Successfull!";
                        var userregisterlog = db.Users.Where(u => u.Email == user.Email && u.Password == user.Password).FirstOrDefault();
                        if(userregisterlog != null)
                        {
                            Session["ID"] = userregisterlog.ID.ToString();
                            Session["RolesID"] = userregisterlog.RolesID.ToString();
                            Session["UserName"] = userregisterlog.UserName.ToString();
                            Session["Email"] = userregisterlog.Email.ToString();
                        }
                        

                        //var Ticket = new FormsAuthenticationTicket(user.Email, true, 3000);
                        //string Encrypt = FormsAuthentication.Encrypt(Ticket);
                        //var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, Encrypt);

                        //var identity = await base.GenerateClamsAsync(ApplicationIdentity user)
                        //identity.AddClaim(new Claim("FullName", user.FullName));
                        //identity.AddClaim(new Claim("Email", user.Email));
                        //await User.AddClaimAsync(user.ID, new Claim("UserName", user.UserName));

                        //cookie.Expires = DateTime.Now.AddHours(3000);
                        //cookie.HttpOnly = true;
                        //Response.Cookies.Add(cookie);

                        //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);

                        //identity.AddClaim(new Claim("DateCreated", user.DateCreated.ToString("MM/dd/yyyy")));
                        //AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
                        if (userregisterlog.RolesID == 2)
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    
                }
                return View();
            }
            ModelState.AddModelError("", "Account already exists!");
            ViewBag.ID = SetBId();//??in id tren view ta co thay m dung dau t xoa bo r:)  chay lai thu xemno invalid cho nao
            //ViewBag.RolesID = new SelectList(await new RoleDAO().GetAll(), "ID", "NameRoles", user.RolesID);
            //ViewBag.Available = GetSelectListStatus(false, (bool)user.Status);
            //ModelState.Clear();
            return View(user);
        }

        // GET: Account/Create
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            using (QL_NhaHangEntities1 db = new QL_NhaHangEntities1())
            {
                //Product product = new Product();
                //var productss = db.Products.Where(p => p.ID == product.ID && p.NameProduct == product.NameProduct).FirstOrDefault();
                //if (productss != null)
                //{
                //    Session["ProductID"] = productss.ID.ToString();
                //    Session["ProductName"] = productss.NameProduct.ToString();
                //    Session["ProductImage"] = productss.Images.ToString();
                //    Session["ProductCategories"] = productss.CategoryID.ToString();
                //    Session["ProductUnitPrice"] = productss.UnitPrice.ToString();
                //}

                var userlogin = db.Users.Where(u => u.Email == user.Email && u.Password == user.Password).FirstOrDefault();
                if (userlogin != null)
                {
                    

                    //var Ticket = new FormsAuthenticationTicket(user.Email, true, 3000);
                    //string Encrypt = FormsAuthentication.Encrypt(Ticket);
                    //var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, Encrypt);

                    //var identity = await base.GenerateClamsAsync(ApplicationIdentity user)
                    //identity.AddClaim(new Claim("FullName", user.FullName));
                    //identity.AddClaim(new Claim("Email", user.Email));
                    //await User.AddClaimAsync(user.ID, new Claim("UserName", user.UserName));
                    
                    //cookie.Expires = DateTime.Now.AddHours(3000);
                    //cookie.HttpOnly = true;
                    //Response.Cookies.Add(cookie);

                    //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);

                    //identity.AddClaim(new Claim("DateCreated", user.DateCreated.ToString("MM/dd/yyyy")));
                    //AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
                    
                    if (userlogin.Status == true)
                    {
                        Session["ID"] = userlogin.ID.ToString();
                        Session["RolesID"] = userlogin.RolesID.ToString();
                        Session["UserName"] = userlogin.UserName.ToString();
                        Session["Email"] = userlogin.Email.ToString();
                        Session["PhoneNumber"] = userlogin.PhoneNumber.ToString();
                        if (userlogin.RolesID == 2)
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    if (userlogin.Status == false)
                    {
                        ViewBag.LockAlert = "Account has been locked";
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Wrong email or password!");
                }
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("Index","Home");
        }

        // GET: Account/Edit/5
        [AllowAnonymous]
        public async Task<ActionResult> Manager(string id)
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

            user.Status = true;
            //ViewBag.RolesID = new SelectList(await new RoleDAO().GetAll(), "ID", "NameRoles", user.RolesID);
            //ViewBag.Available = GetSelectListStatus(false, (bool)user.Status);
            return View(user);
        }

        // POST: Account/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manager([Bind(Include = "ID,UserName,Password,ConfirmPassword,RolesID,Email,PhoneNumber,Status")] User user)
        {
            user.Status = true;
            if (ModelState.IsValid)
            {
                    user.Status = true;
                    await dao.Update(user);
                    ViewBag.SuccessMessage = "Edit Successfull!";
                    Session["UserName"] = user.UserName.ToString();
                    Session["PhoneNumber"] = user.PhoneNumber.ToString();
                    return RedirectToAction("Manager");
            }
            //ViewBag.RolesID = new SelectList(await new RoleDAO().GetAll(), "ID", "NameRoles", user.RolesID);
            //ViewBag.Available = GetSelectListStatus(false, (bool)user.Status);
            return View(user);
        }

        // GET: Account/Delete/5
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

        // POST: Account/Delete/5
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
