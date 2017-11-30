using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBillingSuite.Models
{
    public class IndicadoresQtd
    {
        public List<ChartData> TopCostumers { get; set; }
        public List<ChartData> TopFornecedores { get; set; }
        public List<FlotData> DocsLastYearOUT { get; set; }
        public List<FlotData> DocsLastYearIN { get; set; }
        public string[] DocsByType { get; set; }
    }

    public class ChartData
    {
        public string label { get; set; }
        public double data { get; set; }
    }

    public class FlotData
    {
        public double data { get; set; }
        public int mes { get; set; }
    }
}