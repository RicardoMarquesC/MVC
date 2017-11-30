using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.Model.HelpingClasses
{
    public class IndicadoresFacturacaoQtd
    {
        public string label { get; set; }
        public double data { get; set; }
        public string color { get; set; }
        public int mes { get; set; }

        public static List<IndicadoresFacturacaoQtd> GetTopCostumers(string instances)
        {
            List<IndicadoresFacturacaoQtd> top5costumers = new List<IndicadoresFacturacaoQtd>();
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

                //buscar o total
                int totalglobal = listaGlobal.Count();

                //buscar os valores
                var query = (from p in listaGlobal
                             group p by p.NomeReceptor into g
                             select new
                             {
                                 NomeReceptor = g.Key,
                                 count = g.Count()
                             }).OrderByDescending(x => x.count).Take(5);


                foreach (var item in query)
                {
                    top5costumers.Add(new IndicadoresFacturacaoQtd
                    {
                        label = item.NomeReceptor,
                        data = Math.Round((double.Parse(item.count.ToString()) / double.Parse(totalglobal.ToString())) * 100, 2)
                    });
                }

                int total5 = query.Sum(o => o.count);
                top5costumers.Add(new IndicadoresFacturacaoQtd
                {
                    label = "Outros",
                    data = Math.Round((double.Parse((totalglobal - total5).ToString()) / double.Parse(totalglobal.ToString())) * 100, 2)
                });

            }

            return top5costumers;
        }

        public static List<IndicadoresFacturacaoQtd> GetTopFornecedores(string instances)
        {
            List<IndicadoresFacturacaoQtd> topFornecedores = new List<IndicadoresFacturacaoQtd>();

            List<CIC_DB.InboundPacket> listaGlobal = new List<CIC_DB.InboundPacket>();
            List<string> listInstances = instances.Split(';').ToList();

            using (var cicdbdata = new CIC_DB.CIC_DB())
            {
                if (instances != "*")
                {
                    listaGlobal = cicdbdata.InboundPacket.Where(o => listInstances.Contains(o.NIF)).ToList();
                }
                else
                {
                    listaGlobal = cicdbdata.InboundPacket.ToList();
                }
                //buscar o total
                int totalglobal = listaGlobal.Count;

                //buscar os valores
                var query = (from p in listaGlobal
                             group p by p.NomeFornec into g
                             select new
                             {
                                 NomeFornec = g.Key,
                                 count = g.Count()
                             }).OrderByDescending(x => x.count).Take(5);

                foreach (var item in query)
                {
                    topFornecedores.Add(new IndicadoresFacturacaoQtd
                    {
                        label = item.NomeFornec,
                        data = Math.Round((double.Parse(item.count.ToString()) / double.Parse(totalglobal.ToString())) * 100, 2)
                    });
                }

                int total5 = query.Sum(o => o.count);
                topFornecedores.Add(new IndicadoresFacturacaoQtd
                {
                    label = "OUTROS",
                    data = Math.Round((double.Parse((totalglobal - total5).ToString()) / double.Parse(totalglobal.ToString())) * 100, 2)
                });
            }


            return topFornecedores;
        }

        public static List<IndicadoresFacturacaoQtd> GetDocsLastYearOUT(string instances)
        {
            List<IndicadoresFacturacaoQtd> DocsLastYear = new List<IndicadoresFacturacaoQtd>();

            List<CIC_DB.OutboundPacket> listaGlobal = new List<CIC_DB.OutboundPacket>();
            List<string> listInstances = instances.Split(';').ToList();

            using (var cicdbdata = new CIC_DB.CIC_DB())
            {
                DateTime inicio = new DateTime(DateTime.Now.AddYears(-1).Year, 1, 1);
                DateTime fim = new DateTime(DateTime.Now.AddYears(-1).Year, 12, 31, 23, 59, 59);

                if (instances != "*")
                {
                    listaGlobal = cicdbdata.OutboundPacket.Where(o => listInstances.Contains(o.NIFEmissor)).ToList();
                }
                else
                {
                    listaGlobal = cicdbdata.OutboundPacket.ToList();
                }

                //buscar o total
                var query = (from p in listaGlobal
                                   .Where(p => p.CreationDate >= inicio)
                                   .Where(p => p.CreationDate <= fim)
                             group p by p.CreationDate.Value.Month into g
                             select new
                             {
                                 Valor = g.Count(),
                                 Mes = g.Key

                             }).OrderBy(x => x.Mes).ToList();


                if (query.Count > 0)
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        if (query.Any(o => o.Mes == i))
                            DocsLastYear.Add(new IndicadoresFacturacaoQtd
                            {
                                data = Math.Round(double.Parse(query.Where(o => o.Mes == 1).FirstOrDefault().Valor.ToString()), 2),
                                label = query.Where(o => o.Mes == 1).FirstOrDefault().Mes.ToString()
                            });
                        else
                            DocsLastYear.Add(new IndicadoresFacturacaoQtd
                            {
                                data = 0,
                                label = i.ToString()
                            });

                    }
                }
            }
            return DocsLastYear.OrderBy(x => x.mes).ToList();
        }

        public static List<IndicadoresFacturacaoQtd> GetDocsLastYearIN(string instances)
        {
            List<IndicadoresFacturacaoQtd> DocsLastYear = new List<IndicadoresFacturacaoQtd>();
            List<CIC_DB.InboundPacket> listaGlobal = new List<CIC_DB.InboundPacket>();
            List<string> listInstances = instances.Split(';').ToList();

            using (var cicdbdata = new CIC_DB.CIC_DB())
            {
                DateTime inicio = new DateTime(DateTime.Now.AddYears(-1).Year, 1, 1);
                DateTime fim = new DateTime(DateTime.Now.AddYears(-1).Year, 12, 31, 23, 59, 59);

                if (instances != "*")
                {
                    listaGlobal = cicdbdata.InboundPacket.Where(o => listInstances.Contains(o.NIF)).ToList();
                }
                else
                {
                    listaGlobal = cicdbdata.InboundPacket.ToList();
                }

                //buscar o total
                var query = (from p in listaGlobal
                                   .Where(p => p.ReceptionDate >= inicio)
                                   .Where(p => p.ReceptionDate <= fim)
                             group p by p.ReceptionDate.Value.Month into g
                             select new
                             {
                                 Valor = g.Count(),
                                 Mes = g.Key

                             }).OrderBy(x => x.Mes).ToList();

                if (query.Count > 0)
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        if (query.Any(o => o.Mes == i))
                            DocsLastYear.Add(new IndicadoresFacturacaoQtd
                            {
                                data = Math.Round(double.Parse(query.Where(o => o.Mes == 1).FirstOrDefault().Valor.ToString()), 2),
                                label = query.Where(o => o.Mes == 1).FirstOrDefault().Mes.ToString()
                            });
                        else
                            DocsLastYear.Add(new IndicadoresFacturacaoQtd
                            {
                                data = 0,
                                label = i.ToString()
                            });

                    }
                }
            }
            return DocsLastYear;
        }

        public static string[] GetDocsByType(string instances)
        {
            string[] dados = new string[18];

            //variaveis valores out
            int ftoutvalor = 0; int ncoutvalor = 0;
            int ndoutvalor = 0; int rcoutvalor = 0;
            int gtoutvalor = 0; int groutvalor = 0;
            int gdoutvalor = 0; int gaoutvalor = 0;
            int sopdfoutvalor = 0;

            //variaveis valores in
            int ftinvalor = 0; int ncinvalor = 0;
            int ndinvalor = 0; int rcinvalor = 0;
            int gtinvalor = 0; int grinvalor = 0;
            int gdinvalor = 0; int gainvalor = 0;
            int sopdfinvalor = 0;

            List<CIC_DB.InboundPacket> listaGlobalIN = new List<CIC_DB.InboundPacket>();
            List<CIC_DB.OutboundPacket> listaGlobalOUT = new List<CIC_DB.OutboundPacket>();
            List<string> listInstances = instances.Split(';').ToList();

            using (var cicdbdata = new CIC_DB.CIC_DB())
            {
                if (instances != "*")
                {
                    listaGlobalIN = cicdbdata.InboundPacket.Where(o => listInstances.Contains(o.NIF)).ToList();
                    listaGlobalOUT = cicdbdata.OutboundPacket.Where(o => listInstances.Contains(o.NIFEmissor)).ToList();
                }
                else
                {
                    listaGlobalOUT = cicdbdata.OutboundPacket.ToList();
                    listaGlobalIN = cicdbdata.InboundPacket.ToList();
                }


                //buscar outbound
                var query = (from p in listaGlobalOUT
                             group p by p.TipoDocumento.ToLower() into g
                             select new
                             {
                                 Valor = g.Count(),
                                 TipoDocumento = g.Key.ToLower()

                             }).OrderByDescending(x => x.Valor);

                var queryIn = (from p in listaGlobalIN
                               group p by p.TipoDoc.ToLower() into g
                               select new
                               {
                                   Valor = g.Count(),
                                   TipoDocumento = g.Key.ToLower()

                               }).OrderByDescending(x => x.Valor);

                //Outbound
                foreach (var item in query)
                {
                    IndicadoresFacturacaoQtd ii = new IndicadoresFacturacaoQtd();
                    if (item.TipoDocumento == null)
                    {
                        sopdfoutvalor = sopdfoutvalor + item.Valor;
                        //dados[8] = item.Valor.ToString();
                        ii.label = "Só PDF";
                    }
                    else
                    {
                        if (item.TipoDocumento.ToLower().Equals("fact") || item.TipoDocumento.ToLower().Equals("factura") || item.TipoDocumento.ToLower().Equals("ft") || item.TipoDocumento.ToLower().Equals("fatura") || item.TipoDocumento.ToLower().Equals("fc") || item.TipoDocumento.ToLower().Equals("invoice") || item.TipoDocumento.ToLower().Equals("fs"))
                        {
                            //dados[0] = item.Valor.ToString();
                            ftoutvalor = ftoutvalor + item.Valor;
                            ii.label = "FT";
                        }
                        else if (item.TipoDocumento.ToLower().Equals("ncred") || item.TipoDocumento.ToLower().Equals("cred") || item.TipoDocumento.ToLower().Equals("nota credito") || item.TipoDocumento.ToLower().Equals("nota de credito") || item.TipoDocumento.ToLower().Equals("nota de crédito") || item.TipoDocumento.ToLower().Equals("nc"))
                        {
                            //dados[1] = item.Valor.ToString();
                            ncoutvalor = ncoutvalor + item.Valor;
                            ii.label = "NC";
                        }
                        else if (item.TipoDocumento.ToLower().Equals("ndebt") || item.TipoDocumento.ToLower().Equals("debt") || item.TipoDocumento.ToLower().Equals("nota debito") || item.TipoDocumento.ToLower().Equals("nota de debito") || item.TipoDocumento.ToLower().Equals("nota de débito") || item.TipoDocumento.ToLower().Equals("nd") || item.TipoDocumento.ToLower().Equals("debit note"))
                        {
                            //dados[2] = item.Valor.ToString();
                            ndoutvalor = ndoutvalor + item.Valor;
                            ii.label = "ND";
                        }
                        else if (item.TipoDocumento.ToLower().Equals("receipt") || item.TipoDocumento.ToLower().Equals("recibo") || item.TipoDocumento.ToLower().Equals("rc"))
                        {
                            //dados[3] = item.Valor.ToString();
                            rcoutvalor = rcoutvalor + item.Valor;
                            ii.label = "RC";
                        }
                        else if (item.TipoDocumento.ToLower().Equals("guia transporte") || item.TipoDocumento.ToLower().Equals("guia de transporte") || item.TipoDocumento.ToLower().Equals("gt"))
                        {
                            //dados[4] = item.Valor.ToString();
                            gtoutvalor = gtoutvalor + item.Valor;
                            ii.label = "GT";
                        }
                        else if (item.TipoDocumento.ToLower().Equals("guia remessa") || item.TipoDocumento.ToLower().Equals("guia de remessa") || item.TipoDocumento.ToLower().Equals("gr"))
                        {
                            //dados[5] = item.Valor.ToString();
                            groutvalor = groutvalor + item.Valor;
                            ii.label = "GR";
                        }
                        else if (item.TipoDocumento.ToLower().Equals("guia devolucao") || item.TipoDocumento.ToLower().Equals("guia de devolucao") || item.TipoDocumento.ToLower().Equals("gd") || item.TipoDocumento.ToLower().Equals("guia de devolução"))
                        {
                            //dados[6] = item.Valor.ToString();
                            gdoutvalor = gdoutvalor + item.Valor;
                            ii.label = "GD";
                        }
                        else if (item.TipoDocumento.ToLower().Equals("guia anulacao") || item.TipoDocumento.ToLower().Equals("guia de anulacao") || item.TipoDocumento.ToLower().Equals("ga") || item.TipoDocumento.ToLower().Equals("guia de anulacao"))
                        {
                            //dados[7] = item.Valor.ToString();
                            gaoutvalor = gaoutvalor + item.Valor;
                            ii.label = "GA";
                        }
                    }
                }

                int i = 0;
                foreach (string s in dados)
                {
                    if (String.IsNullOrEmpty(s))
                        dados[i] = "0";

                    i++;
                }

                //INBOUND
                foreach (var item in queryIn)
                {
                    IndicadoresFacturacaoQtd ii = new IndicadoresFacturacaoQtd();
                    if (item.TipoDocumento == null)
                    {
                        sopdfinvalor = sopdfinvalor + item.Valor;
                        //dados[8] = dados[8].ToString() + " : " + item.Valor.ToString();
                        ii.label = "Só PDF";
                    }
                    else
                    {
                        if (item.TipoDocumento.ToLower().Equals("fact") || item.TipoDocumento.ToLower().Equals("factura") || item.TipoDocumento.ToLower().Equals("ft") || item.TipoDocumento.ToLower().Equals("fatura") || item.TipoDocumento.ToLower().Equals("fc") || item.TipoDocumento.ToLower().Equals("invoice") || item.TipoDocumento.ToLower().Equals("fs"))
                        {
                            //dados[0] = dados[0].ToString() + " : " + item.Valor.ToString();
                            ftinvalor = ftinvalor + item.Valor;
                            ii.label = "FT";
                        }
                        else if (item.TipoDocumento.ToLower().Equals("ncred") || item.TipoDocumento.ToLower().Equals("cred") || item.TipoDocumento.ToLower().Equals("nota credito") || item.TipoDocumento.ToLower().Equals("nota de credito") || item.TipoDocumento.ToLower().Equals("nota de crédito") || item.TipoDocumento.ToLower().Equals("nc"))
                        {
                            //dados[1] = dados[1].ToString() + " : " + item.Valor.ToString();
                            ncinvalor = ncinvalor + item.Valor;
                            ii.label = "NC";
                        }
                        else if (item.TipoDocumento.ToLower().Equals("ndebt") || item.TipoDocumento.ToLower().Equals("debt") || item.TipoDocumento.ToLower().Equals("nota debito") || item.TipoDocumento.ToLower().Equals("nota de debito") || item.TipoDocumento.ToLower().Equals("nota de débito") || item.TipoDocumento.ToLower().Equals("nd") || item.TipoDocumento.ToLower().Equals("debit note"))
                        {
                            //dados[2] = dados[2].ToString() + " : " + item.Valor.ToString();
                            ndinvalor = ndinvalor + item.Valor;
                            ii.label = "ND";
                        }
                        else if (item.TipoDocumento.ToLower().Equals("receipt") || item.TipoDocumento.ToLower().Equals("recibo") || item.TipoDocumento.ToLower().Equals("rc"))
                        {
                            //dados[3] = dados[3].ToString() + " : " + item.Valor.ToString();
                            rcinvalor = rcinvalor + item.Valor;
                            ii.label = "RC";
                        }
                        else if (item.TipoDocumento.ToLower().Equals("guia transporte") || item.TipoDocumento.ToLower().Equals("guia de transporte") || item.TipoDocumento.ToLower().Equals("gt"))
                        {
                            //dados[4] = dados[4].ToString() + " : " + item.Valor.ToString();
                            gtinvalor = gtinvalor + item.Valor;
                            ii.label = "GT";
                        }
                        else if (item.TipoDocumento.ToLower().Equals("guia remessa") || item.TipoDocumento.ToLower().Equals("guia de remessa") || item.TipoDocumento.ToLower().Equals("gr"))
                        {
                            //dados[5] = dados[5].ToString() + " : " + item.Valor.ToString();
                            grinvalor = grinvalor + item.Valor;
                            ii.label = "GR";
                        }
                        else if (item.TipoDocumento.ToLower().Equals("guia devolucao") || item.TipoDocumento.ToLower().Equals("guia de devolucao") || item.TipoDocumento.ToLower().Equals("gd") || item.TipoDocumento.ToLower().Equals("guia de devolução"))
                        {
                            //dados[6] = dados[6].ToString() + " : " + item.Valor.ToString();
                            gdinvalor = gdinvalor + item.Valor;
                            ii.label = "GD";
                        }
                        else if (item.TipoDocumento.ToLower().Equals("guia anulacao") || item.TipoDocumento.ToLower().Equals("guia de anulacao") || item.TipoDocumento.ToLower().Equals("ga") || item.TipoDocumento.ToLower().Equals("guia de anulacao"))
                        {
                            //dados[7] = dados[7].ToString() + " : " + item.Valor.ToString();
                            gainvalor = gainvalor + item.Valor;
                            ii.label = "GA";
                        }
                    }
                }


                dados[0] = ftoutvalor.ToString();
                dados[1] = ncoutvalor.ToString();
                dados[2] = ndoutvalor.ToString();
                dados[3] = rcoutvalor.ToString();
                dados[4] = gtoutvalor.ToString();
                dados[5] = groutvalor.ToString();
                dados[6] = gdoutvalor.ToString();
                dados[7] = gaoutvalor.ToString();
                dados[8] = sopdfoutvalor.ToString();
                dados[9] = ftinvalor.ToString();
                dados[10] = ncinvalor.ToString();
                dados[11] = ndinvalor.ToString();
                dados[12] = rcinvalor.ToString();
                dados[13] = gtinvalor.ToString();
                dados[14] = grinvalor.ToString();
                dados[15] = gdinvalor.ToString();
                dados[16] = gainvalor.ToString();
                dados[17] = sopdfinvalor.ToString();

            }

            return dados;
        }

    }
}
