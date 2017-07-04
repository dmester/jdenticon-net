using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jdenticon.WebApi.Sample.Controllers
{
    public class MvcController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Icon(string name, int size)
        {
            // The fully qualified name is used to clarify the difference between the MVC IdenticonResult 
            // and the WebApi IdenticonResult.

            return Jdenticon.AspNet.Mvc.IdenticonResult.FromValue(name, size);
        }
    }
}
