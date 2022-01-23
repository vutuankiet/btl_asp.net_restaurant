using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _404ErrorHandler.Controllers
{
    public class ErrorController : Controller
    {
        //Get:Error
        public ActionResult NotFound()
        {
            return View();
        }
    }
}