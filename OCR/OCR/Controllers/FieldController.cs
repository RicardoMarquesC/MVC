using OCR.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OCR.Controllers
{
    public class FieldController : Controller
    {
        // GET: Field
        public ActionResult Index()
        {
            DirectoryInfo folder = new DirectoryInfo(@"C: \Users\x4v1\Documents\OCR\OCR\App_Data");

            List<FileInfo> fileList= folder.GetFiles("*").ToList();

            string InstanceID = fileList.Select(o => o.Name).FirstOrDefault();
            List<SelectListItem> AvailableInstances = fileList
                            .Select(v => new SelectListItem
                            {
                                Text = v.Name,
                                Value = v.Name.ToString(),
                                Selected = v.Name == InstanceID
                            })
                            .ToList();
            return View(AvailableInstances);
        }

        public ActionResult GetValues(FieldData data)
        {
            return Json("OK");
        }
    }
}