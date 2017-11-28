using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OCR.Models
{
    public class FieldData
    {

        //public int PageID { get; set; }

        public string Field { get; set; }

        public string Coordinates { get; set; }

        public string File { set; get; }

        
    }
}