using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.Model.HelpingClasses
{
    public class IndicadoresPerf
    {
        public string TempoMedioResposta { get; set; }
        public List<ChartDataPerf> TopCostumers { get; set; }
        public List<ChartDataPerf> LastCostumers { get; set; }
        public List<ChartDataPerf> TemposMediosCliente { get; set; }
        public List<ChartDataPerf> TemposMediosClienteAno { get; set; }

        //public List<FlotData> DocsLastYearOUT { get; set; }
        //public List<FlotData> DocsLastYearIN { get; set; }
        //public string[] DocsByType { get; set; }
    }

    public class ChartDataPerf
    {
        public string label { get; set; }
        public double data { get; set; }
        public float TempoMedio { get; set; }
    }

    public class FlotDataPerf
    {
        public double data { get; set; }
    }   
}