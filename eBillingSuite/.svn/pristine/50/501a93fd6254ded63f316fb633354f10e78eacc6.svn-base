using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBillingSuite.Models
{
    public class IndicadoresQuant
    {
        public List<ChartDataQuant> TopCostumers { get; set; }
        public List<ChartDataQuant> TopFornecedores { get; set; }
        public List<FlotDataQuant> DocsLastYearOUT { get; set; }
        public List<FlotDataQuant> DocsLastYearIN { get; set; }
        public string[] DocsByType { get; set; }
    }

    public class ChartDataQuant
    {
        public string label { get; set; }
        public double data { get; set; }
    }

    public class FlotDataQuant
    {
        public double data { get; set; }
        public int mes { get; set; }
    }
}