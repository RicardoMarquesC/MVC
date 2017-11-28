using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OCR.Controllers
{
    public class ImageWorkController : Controller
    {
        // GET: ImageWork
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Duplex(FormCollection form)
        {
            List<string> cenas = new List<string>();
            foreach (string des in form.Keys) {
                cenas.Add(form[des]);
            }
            string x = form.Get("Value");
            return View();
        }

    }
}