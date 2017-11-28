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
        private string syspath = @"C: \Users\x4v1\Documents\OCR\OCR\App_Data";
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

            string filepath = Path.Combine(syspath, file.FileName);
            file.SaveAs(filepath);

            Image image = Image.FromFile(filepath);            

            Bitmap bitmap = new Bitmap(image);
            string yolo = form.AllKeys.Where(k => k.StartsWith("coords")).FirstOrDefault().ToString();

            string[] coordenadas = form[yolo].ToString().Split(';');

            string x = form.Get("Value");
            return null;
        }

    }
}