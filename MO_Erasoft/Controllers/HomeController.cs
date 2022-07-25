using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MO_Erasoft.Controllers;

namespace MO_Erasoft.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            new ValuesController().ProsesMutasiStok();
            return View();
        }
    }
}
