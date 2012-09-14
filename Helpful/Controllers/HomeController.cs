using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Helpful.Models;

namespace Helpful.Controllers
{
    public class HomeController : Controller
    {
        // Methods
        public ActionResult About(string Url)
        {
            return base.View();
        }

        public ActionResult Index()
        {
            return base.View();
        }
    }
}
