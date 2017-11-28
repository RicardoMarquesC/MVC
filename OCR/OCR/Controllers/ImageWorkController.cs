using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

        [HttpPost]
        public ActionResult Duplex(FormCollection form, HttpPostedFileBase file)
        {
            List<string> cenas = new List<string>();
            foreach (string des in form.Keys) {
                cenas.Add(form[des]);
            }

            string filepath = Path.Combine(@"C:\Users\x4v1\Documents\MVC\OCR\OCR\App_Data\" , file.FileName);
            file.SaveAs(filepath);
            

            string x = form.Get("Value");
            return null;
        }

    }
}