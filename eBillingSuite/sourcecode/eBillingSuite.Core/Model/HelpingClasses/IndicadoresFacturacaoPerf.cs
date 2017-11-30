using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.Model.HelpingClasses
{
    public class IndicadoresFacturacaoPerf
    {
        public string label { get; set; }
        public double data { get; set; }
        public string Mes { get; set; }
        public int Ano { get; set; }
        public float DataTempoMedio { get; set; }
        public float ClienteTempoMedio { get; set; }
        public string cliente { get; set; }
        public decimal tempomedio { get; set; }
        public int counter { get; set; }

        public static string GetTempoMedioResposta(string instances)
        {
            string tempomedio = "";
            List<CIC_DB.OutboundPacket> listaGlobal = new List<CIC_DB.OutboundPacket>();
            List<string> listInstances = instances.Split(';').ToList();

            using (var cicdbdata = new CIC_DB.CIC_DB())
            {
                if (instances != "*")
                {
                    listaGlobal = cicdbdata.OutboundPacket.Where(o => listInstances.Contains(o.NIFEmissor)).ToList();
                }
                else
                {
                    listaGlobal = cicdbdata.OutboundPacket.ToList();
                }



                //buscar os valores
                var query = (from p in cicdbdata.OutboundPacket
                             .Where(g => g.CurrentEBCState.Equals("DELIVERED"))
                             select new
                             {
                                 CreationDate = p.CreationDate,
                                 LastUpdate = p.LastUpdate
                             }).OrderByDescending(x => x.CreationDate);

                List<int> valoresTemp = new List<int>();
                foreach (var item in query)
                {
                    TimeSpan valortemp = item.LastUpdate.Value - item.CreationDate.Value;
                    valoresTemp.Add(valortemp.Minutes);
                }

                //13-02-2014
                if (valoresTemp.Count > 0)
                    tempomedio = Math.Round(decimal.Parse(valoresTemp.Average().ToString()), 2) + " minutos";
                else
                    tempomedio = "0 minutos";
            }

            return tempomedio;
        }

        public static List<IndicadoresFacturacaoPerf> GetTopCostumers(string instances)
        {
            List<IndicadoresFacturacaoPerf> topcostumers = new List<IndicadoresFacturacaoPerf>();
            List<CIC_DB.OutboundPacket> listaGlobal = new List<CIC_DB.OutboundPacket>();
            List<string> listInstances = instances.Split(';').ToList();

            using (var cicdbdata = new CIC_DB.CIC_DB())
            {
                if (instances != "*")
                {
                    listaGlobal = cicdbdata.OutboundPacket.Where(o => listInstances.Contains(o.NIFEmissor)).ToList();
                }
                else
                {
                    listaGlobal = cicdbdata.OutboundPacket.ToList();
                }


                //Buscar os recetores
                var recetores = (from p in listaGlobal
                                 group p by p.NomeReceptor into g
                                 select new
                                 {
                                     NomeReceptor = g.Key,
                                 }).ToList();

                //Para cada um dos recetores buscar os dados; calcular a diferença; guardar na lista.
                foreach (var item in recetores)
                {
                    //buscar os valores
                    var values = listaGlobal
                                .Where(g => g.CurrentEBCState.Equals("DELIVERED") && g.NomeReceptor.Equals(item.NomeReceptor))
                                .Select(p => new
                                {
                                    CreationDate = p.CreationDate,
                                    LastUpdate = p.LastUpdate
                                }).OrderByDescending(x => x.CreationDate)
                                .ToList();
                   

                    List<int> valoresTemp = new List<int>();
                    foreach (var itemvalues in values)
                    {
                        TimeSpan valortemp = itemvalues.LastUpdate.Value - itemvalues.CreationDate.Value;
                        valoresTemp.Add(valortemp.Minutes);
                    }

                    IndicadoresFacturacaoPerf ifp = new IndicadoresFacturacaoPerf();
                    string[] valor = item.NomeReceptor.Split(' ');

                    if (valor.Count() > 2)
                        ifp.label = valor[0] + ' ' + valor[valor.Count() - 1];
                    else
                        ifp.label = item.NomeReceptor;

                    //13-02-2014
                    if (valoresTemp.Count > 0)
                        ifp.data = double.Parse(Math.Round(decimal.Parse(valoresTemp.Average().ToString()), 2).ToString());
                    //else
                    //    ifp.data = 0;

                    topcostumers.Add(ifp);
                }

                topcostumers = topcostumers.Where(o=>o.data>0).OrderBy(x => x.data).Take(5).ToList();
            }

            return topcostumers;
        }

        public static List<IndicadoresFacturacaoPerf> GetLastCostumers(string instances)
        {
            List<IndicadoresFacturacaoPerf> lastCostumers = new List<IndicadoresFacturacaoPerf>();
            List<CIC_DB.OutboundPacket> listaGlobal = new List<CIC_DB.OutboundPacket>();
            List<string> listInstances = instances.Split(';').ToList();

            using (var cicdbdata = new CIC_DB.CIC_DB())
            {
                if (instances != "*")
                {
                    listaGlobal = cicdbdata.OutboundPacket.Where(o => listInstances.Contains(o.NIFEmissor)).ToList();
                }
                else
                {
                    listaGlobal = cicdbdata.OutboundPacket.ToList();
                }


                //Buscar os recetores
                var recetores = (from p in listaGlobal
                                 group p by p.NomeReceptor into g
                                 select new
                                 {
                                     NomeReceptor = g.Key,
                                 });

                //Para cada um dos Fornecedores buscar os dados; calcular a diferença; guardar na lista.
                foreach (var item in recetores)
                {
                    //buscar os valores
                    var values = (from p in listaGlobal
                                 .Where(g => g.CurrentEBCState.Equals("DELIVERED"))
                                 .Where(g => g.NomeReceptor.Equals(item.NomeReceptor))
                                  select new
                                  {
                                      CreationDate = p.CreationDate,
                                      LastUpdate = p.LastUpdate
                                  }).OrderByDescending(x => x.CreationDate);

                    List<int> valoresTemp = new List<int>();
                    foreach (var itemvalues in values)
                    {
                        TimeSpan valortemp = itemvalues.LastUpdate.Value - itemvalues.CreationDate.Value;
                        valoresTemp.Add(valortemp.Minutes);
                    }

                    IndicadoresFacturacaoPerf ifp = new IndicadoresFacturacaoPerf();
                    //Reduzir o tamanho do nome
                    string[] valor = item.NomeReceptor.Split(' ');

                    if (valor.Count() > 2)
                        ifp.label = valor[0] + ' ' + valor[valor.Count() - 1];
                    else
                        ifp.label = item.NomeReceptor;

                    //13-02-2014
                    if (valoresTemp.Count > 0)
                        ifp.data = double.Parse(Math.Round(decimal.Parse(valoresTemp.Average().ToString()), 2).ToString());
                    else
                        ifp.data = 0;

                    lastCostumers.Add(ifp);
                }

                lastCostumers = lastCostumers.OrderByDescending(x => x.data).Take(5).ToList();
            }

            return lastCostumers;
        }

        public static List<IndicadoresFacturacaoPerf> GetTemposMediosCliente(string instances)
        {
            List<IndicadoresFacturacaoPerf> lastCostumers = new List<IndicadoresFacturacaoPerf>();
            List<CIC_DB.OutboundPacket> listaGlobal = new List<CIC_DB.OutboundPacket>();
            List<string> listInstances = instances.Split(';').ToList();

            using (var cicdbdata = new CIC_DB.CIC_DB())
            {
                if (instances != "*")
                {
                    listaGlobal = cicdbdata.OutboundPacket.Where(o => listInstances.Contains(o.NIFEmissor)).ToList();
                }
                else
                {
                    listaGlobal = cicdbdata.OutboundPacket.ToList();
                }

                //Buscar os recetores
                var recetores = (from p in listaGlobal
                                 group p by p.NomeReceptor into g
                                 select new
                                 {
                                     NomeReceptor = g.Key,
                                 });

                int counter = 0;
                //Para cada um dos Fornecedores buscar os dados; calcular a diferença; guardar na lista.
                foreach (var item in recetores)
                {
                    //buscar os valores
                    var values = (from p in listaGlobal
                                 .Where(g => g.CurrentEBCState.Equals("DELIVERED"))
                                 .Where(g => g.NomeReceptor.Equals(item.NomeReceptor))
                                  select new
                                  {
                                      CreationDate = p.CreationDate,
                                      LastUpdate = p.LastUpdate
                                  }).OrderByDescending(x => x.CreationDate);

                    List<int> valoresTemp = new List<int>();
                    foreach (var itemvalues in values)
                    {
                        TimeSpan valortemp = itemvalues.LastUpdate.Value - itemvalues.CreationDate.Value;
                        valoresTemp.Add(valortemp.Minutes);
                    }

                    IndicadoresFacturacaoPerf ifp = new IndicadoresFacturacaoPerf();
                    ifp.counter = counter;
                    ifp.cliente = item.NomeReceptor;

                    if (valoresTemp.Count > 0)
                        ifp.ClienteTempoMedio = float.Parse(Math.Round(decimal.Parse(valoresTemp.Average().ToString()), 2).ToString());
                    else
                        ifp.ClienteTempoMedio = 0;

                    lastCostumers.Add(ifp);
                    counter++;
                }
                lastCostumers = lastCostumers.OrderBy(x => x.tempomedio).ToList();
            }

            return lastCostumers;
        }

        public static List<IndicadoresFacturacaoPerf> GetTemposMediosAnoMes(string instances)
        {
            List<IndicadoresFacturacaoPerf> lastCostumers = new List<IndicadoresFacturacaoPerf>();
            List<CIC_DB.OutboundPacket> listaGlobal = new List<CIC_DB.OutboundPacket>();
            List<string> listInstances = instances.Split(';').ToList();

            using (var cicdbdata = new CIC_DB.CIC_DB())
            {
                if (instances != "*")
                {
                    listaGlobal = cicdbdata.OutboundPacket.Where(o => listInstances.Contains(o.NIFEmissor)).ToList();
                }
                else
                {
                    listaGlobal = cicdbdata.OutboundPacket.ToList();
                }
                int counter = 0;
                //Buscar os Anos
                var Anos = (from p in listaGlobal
                            group p by p.CreationDate.Value.Year into g
                            select new
                            {
                                Ano = g.Key,
                            });

                foreach (var year in Anos)
                {
                    //buscar os meses
                    var Meses = (from p in listaGlobal
                                 group p by p.CreationDate.Value.Month into g
                                 select new
                                 {
                                     Mes = g.Key,
                                 });

                    foreach (var months in Meses)
                    {
                        //buscar os dados
                        var values = (from p in listaGlobal
                                    .Where(g => g.CurrentEBCState.Equals("DELIVERED"))
                                    .Where(g => g.CreationDate.Value.Month.Equals(months.Mes))
                                    .Where(g => g.CreationDate.Value.Year.Equals(year.Ano))
                                      select new
                                      {
                                          CreationDate = p.CreationDate,
                                          LastUpdate = p.LastUpdate
                                      }).OrderByDescending(x => x.CreationDate); ;

                        if (values.Count() > 0)
                        {
                            List<int> valoresTemp = new List<int>();
                            foreach (var itemvalues in values)
                            {
                                TimeSpan valortemp = itemvalues.LastUpdate.Value - itemvalues.CreationDate.Value;
                                valoresTemp.Add(valortemp.Minutes);
                            }

                            IndicadoresFacturacaoPerf ifp = new IndicadoresFacturacaoPerf();
                            ifp.counter = counter;
                            if (months.Mes < 10)
                                ifp.Mes = "0" + months.Mes.ToString();
                            else
                                ifp.Mes = months.Mes.ToString();
                            ifp.Ano = year.Ano;

                            //13-02-2014
                            if (valoresTemp.Count > 0)
                                ifp.DataTempoMedio = float.Parse(Math.Round(decimal.Parse(valoresTemp.Average().ToString()), 2).ToString());
                            else
                                ifp.DataTempoMedio = 0;

                            lastCostumers.Add(ifp);
                            counter++;
                        }

                    }
                }
                lastCostumers = lastCostumers.OrderByDescending(x => x.Ano).ThenByDescending(x => x.Mes).ToList();
            }

            return lastCostumers;
        }
    }
}
