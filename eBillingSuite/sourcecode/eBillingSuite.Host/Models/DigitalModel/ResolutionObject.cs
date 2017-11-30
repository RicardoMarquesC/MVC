using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBillingSuite.Models.DigitalModel
{
    public class ResolutionObject
    {
        private int dpiResolution;

        public int DpiResolution
        {
            get { return dpiResolution; }
            set { dpiResolution = value; }
        }
        private int width;

        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        private int height;

        public int Height
        {
            get { return height; }
            set { height = value; }
        }
    }
}