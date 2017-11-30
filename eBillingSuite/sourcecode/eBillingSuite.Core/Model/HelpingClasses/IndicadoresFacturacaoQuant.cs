using eBillingSuite.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.Model.HelpingClasses
{
    public class IndicadoresFacturacaoQuant
    {
        public string label { get; set; }
        public double data { get; set; }
        public string color { get; set; }

        public static List<IndicadoresFacturacaoQuant> GetTopCostumers(string instances)
        {
            List<IndicadoresFacturacaoQuant> top5costumers = new List<IndicadoresFacturacaoQuant>();
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
                    listaGlobal = cicdbdata.OutboundPacket.Select(o => o).ToList();
                }

                //buscar o total
                decimal total = listaGlobal.Sum(o => decimal.Parse(o.QuantiaComIVA));

                //Buscar os recetores
                var recetores = (from p in listaGlobal
                                 group p by p.NomeReceptor into g
                                 select new
                                 {
                                     NomeReceptor = g.Key,
                                 });
                //buscar os valores de cada um dos recetores
                foreach (var item in recetores)
                {
                    decimal totalrecetor = listaGlobal.Where(p => p.NomeReceptor == item.NomeReceptor).Sum(o => decimal.Parse(o.QuantiaComIVA));
                    top5costumers.Add(new IndicadoresFacturacaoQuant
                    {
                        label = item.NomeReceptor,
                        data = Math.Round(double.Parse(totalrecetor.ToString()) / double.Parse(total.ToString()) * 100, 2)
                    });
                }

                top5costumers = top5costumers.OrderByDescending(o => o.data).Take(5).ToList();
                double valortotal5 = Math.Round(top5costumers.Sum(o => o.data), 2);
                top5costumers.Add(new IndicadoresFacturacaoQuant
                {
                    label = "OUTROS",
                    data = Math.Round((100 - valortotal5), 2)
                });
            }

            return top5costumers;
        }

        public static List<IndicadoresFacturacaoQuant> GetTopFornecedores(string instances)
        {
            List<IndicadoresFacturacaoQuant> top5fornecedores = new List<IndicadoresFacturacaoQuant>();
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
                decimal total = listaGlobal.Sum(o => Parse.ToDecimal(o.Quantia, true));

                //Buscar os recetores
                var recetores = (from p in listaGlobal
                                 group p by p.NomeFornec into g
                                 select new
                                 {
                                     NomeFornec = g.Key,
                                 });

                //buscar os valores de cada um dos recetores
                foreach (var item in recetores)
                {
                    decimal totalemissor = listaGlobal.Where(p => p.NomeFornec == item.NomeFornec).Sum(o => decimal.Parse(o.Quantia));
                    top5fornecedores.Add(new IndicadoresFacturacaoQuant
                    {
                        label = item.NomeFornec,
                        data = Math.Round((double.Parse(totalemissor.ToString()) / double.Parse(total.ToString())) * 100, 2)
                    });
                }

                //para mostrar apenas os maiores 5.
                top5fornecedores = top5fornecedores.OrderByDescending(o => o.data).Take(5).ToList();
                //colocar na lista o valor restante para dar 100%
                double valortotal5 = Math.Round(top5fornecedores.Sum(o => o.data), 2);
                top5fornecedores.Add(new IndicadoresFacturacaoQuant
                {
                    label = "OUTROS",
                    data = Math.Round((100 - valortotal5), 2)
                });
            }

            return top5fornecedores;
        }

        public static List<IndicadoresFacturacaoQuant> GetDocsLastYearOUT(string instances)
        {
            List<IndicadoresFacturacaoQuant> DocsLastYear = new List<IndicadoresFacturacaoQuant>();
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
                    listaGlobal = cicdbdata.OutboundPacket.Select(o => o).ToList();
                }

                //buscar valores
                var query = listaGlobal
                            .Where(o => o.CreationDate >= inicio && o.CreationDate <= fim)
                            .GroupBy(p => p.CreationDate.Value.Month)
                            .Select(g => new
                            {
                                Valor = g.Sum(s => decimal.Parse(s.QuantiaComIVA)),
                                Mes = g.Key
                            })
                            .OrderBy(x => x.Mes).ToList();


                if (query.Count > 0)
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        if (query.Any(o => o.Mes == i))
                            DocsLastYear.Add(new IndicadoresFacturacaoQuant
                            {
                                data = Math.Round(double.Parse(query.Where(o => o.Mes == 1).FirstOrDefault().Valor.ToString()), 2),
                                label = query.Where(o => o.Mes == 1).FirstOrDefault().Mes.ToString()
                            });
                        else
                            DocsLastYear.Add(new IndicadoresFacturacaoQuant
                            {
                                data = 0,
                                label = i.ToString()
                            });

                    }
                }

            }
            return DocsLastYear;
        }

        public static List<IndicadoresFacturacaoQuant> GetDocsLastYearIN(string instances)
        {
            List<IndicadoresFacturacaoQuant> DocsLastYear = new List<IndicadoresFacturacaoQuant>();
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

                //buscar valores
                var query = listaGlobal
                            .Where(o => o.ReceptionDate >= inicio && o.ReceptionDate <= fim)
                            .GroupBy(p => p.ReceptionDate.Value.Month)
                            .Select(g => new
                            {
                                Valor = g.Sum(s => decimal.Parse(s.Quantia)),
                                Mes = g.Key
                            })
                            .OrderBy(x => x.Mes).ToList();

                if (query.Count > 0)
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        if (query.Any(o => o.Mes == i))
                            DocsLastYear.Add(new IndicadoresFacturacaoQuant
                            {
                                data = Math.Round(double.Parse(query.Where(o => o.Mes == 1).FirstOrDefault().Valor.ToString()), 2),
                                label = query.Where(o => o.Mes == 1).FirstOrDefault().Mes.ToString()
                            });
                        else
                            DocsLastYear.Add(new IndicadoresFacturacaoQuant
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
            decimal ftoutvalor = 0; decimal ncoutvalor = 0;
            decimal ndoutvalor = 0; decimal rcoutvalor = 0;
            decimal gtoutvalor = 0; decimal groutvalor = 0;
            decimal gdoutvalor = 0; decimal gaoutvalor = 0;
            decimal sopdfoutvalor = 0;

            //variaveis valores in
            decimal ftinvalor = 0; decimal ncinvalor = 0;
            decimal ndinvalor = 0; decimal rcinvalor = 0;
            decimal gtinvalor = 0; decimal grinvalor = 0;
            decimal gdinvalor = 0; decimal gainvalor = 0;
            decimal sopdfinvalor = 0;

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

                #region Outbound
                //buscar tipos de documentos outbound
                var query = (from p in listaGlobalOUT
                             group p by p.TipoDocumento.ToLower() into g
                             select new
                             {
                                 //Valor = g.Count(),
                                 TipoDocumento = g.Key.ToLower()

                             });
                //.OrderByDescending(x => x.Valor);

                //para cada tipo de documento fazer o somatório
                decimal totaltipodocumento = 0;
                foreach (var item in query)
                {
                    totaltipodocumento = 0;
                    //BUSCAR OS DADOS PARA CADA UM DOS MESES
                    foreach (string value in (from op in listaGlobalOUT
                                            .Where(p => p.TipoDocumento.ToLower().Equals(item.TipoDocumento))
                                              select op.QuantiaComIVA)
                         )
                        totaltipodocumento = totaltipodocumento + Parse.ToDecimal(value, true);


                    IndicadoresFacturacaoQuant ii = new IndicadoresFacturacaoQuant();
                    if (item.TipoDocumento == null)
                    {
                        sopdfoutvalor = sopdfoutvalor + totaltipodocumento;
                        //dados[8] = item.Valor.ToString();
                        ii.label = "Só PDF";
                    }
                    else
                    {
                        if (item.TipoDocumento.ToLower().Equals("fact") || item.TipoDocumento.ToLower().Equals("factura") || item.TipoDocumento.ToLower().Equals("ft") || item.TipoDocumento.ToLower().Equals("fatura") || item.TipoDocumento.ToLower().Equals("fc") || item.TipoDocumento.ToLower().Equals("invoice") || item.TipoDocumento.ToLower().Equals("fs"))
                        {
                            //dados[0] = item.Valor.ToString();
                            ftoutvalor = ftoutvalor + totaltipodocumento;
                            ii.label = "FT";
                        }
                        else if (item.TipoDocumento.ToLower().Equals("ncred") || item.TipoDocumento.ToLower().Equals("cred") || item.TipoDocumento.ToLower().Equals("nota credito") || item.TipoDocumento.ToLower().Equals("nota de credito") || item.TipoDocumento.ToLower().Equals("nota de crédito") || item.TipoDocumento.ToLower().Equals("nc"))
                        {
                            //dados[1] = item.Valor.ToString();
                            ncoutvalor = ncoutvalor + totaltipodocumento;
                            ii.label = "NC";
                        }
                        else if (item.TipoDocumento.ToLower().Equals("ndebt") || item.TipoDocumento.ToLower().Equals("debt") || item.TipoDocumento.ToLower().Equals("nota debito") || item.TipoDocumento.ToLower().Equals("nota de debito") || item.TipoDocumento.ToLower().Equals("nota de débito") || item.TipoDocumento.ToLower().Equals("nd") || item.TipoDocumento.ToLower().Equals("debit note"))
                        {
                            //dados[2] = item.Valor.ToString();
                            ndoutvalor = ndoutvalor + totaltipodocumento;
                            ii.label = "ND";
                        }
                        else if (item.TipoDocumento.ToLower().Equals("receipt") || item.TipoDocumento.ToLower().Equals("recibo") || item.TipoDocumento.ToLower().Equals("rc"))
                        {
                            //dados[3] = item.Valor.ToString();
                            rcoutvalor = rcoutvalor + totaltipodocumento;
                            ii.label = "RC";
                        }
                        else if (item.TipoDocumento.ToLower().Equals("guia transporte") || item.TipoDocumento.ToLower().Equals("guia de transporte") || item.TipoDocumento.ToLower().Equals("gt"))
                        {
                            //dados[4] = item.Valor.ToString();
                            gtoutvalor = gtoutvalor + totaltipodocumento;
                            ii.label = "GT";
                        }
                        else if (item.TipoDocumento.ToLower().Equals("guia remessa") || item.TipoDocumento.ToLower().Equals("guia de remessa") || item.TipoDocumento.ToLower().Equals("gr"))
                        {
                            //dados[5] = item.Valor.ToString();
                            groutvalor = groutvalor + totaltipodocumento;
                            ii.label = "GR";
                        }
                        else if (item.TipoDocumento.ToLower().Equals("guia devolucao") || item.TipoDocumento.ToLower().Equals("guia de devolucao") || item.TipoDocumento.ToLower().Equals("gd") || item.TipoDocumento.ToLower().Equals("guia de devolução"))
                        {
                            //dados[6] = item.Valor.ToString();
                            gdoutvalor = gdoutvalor + totaltipodocumento;
                            ii.label = "GD";
                        }
                        else if (item.TipoDocumento.ToLower().Equals("guia anulacao") || item.TipoDocumento.ToLower().Equals("guia de anulacao") || item.TipoDocumento.ToLower().Equals("ga") || item.TipoDocumento.ToLower().Equals("guia de anulacao"))
                        {
                            //dados[7] = item.Valor.ToString();
                            gaoutvalor = gaoutvalor + totaltipodocumento;
                            ii.label = "GA";
                        }
                    }

                }

                #endregion

                #region Inbound
                query = null;
                //buscar tipos de documentos Inbound
                query = (from p in listaGlobalIN
                         group p by p.TipoDoc.ToLower() into g
                         select new
                         {
                             //Valor = g.Count(),
                             TipoDocumento = g.Key.ToLower()

                         });


                foreach (var item in query)
                {
                    totaltipodocumento = 0;
                    //BUSCAR OS DADOS PARA CADA UM DOS MESES
                    foreach (string value in (from op in listaGlobalIN
                                            .Where(p => p.TipoDoc.ToLower().Equals(item.TipoDocumento))
                                              select op.Quantia)
                         )
                        totaltipodocumento = totaltipodocumento + Parse.ToDecimal(value, true);

                    IndicadoresFacturacaoQuant ii = new IndicadoresFacturacaoQuant();
                    if (item.TipoDocumento == null)
                    {
                        sopdfinvalor = sopdfinvalor + totaltipodocumento;
                        //dados[8] = dados[8].ToString() + " : " + item.Valor.ToString();
                        ii.label = "Só PDF";
                    }
                    else
                    {
                        if (item.TipoDocumento.ToLower().Equals("fact") || item.TipoDocumento.ToLower().Equals("factura") || item.TipoDocumento.ToLower().Equals("ft") || item.TipoDocumento.ToLower().Equals("fatura") || item.TipoDocumento.ToLower().Equals("fc") || item.TipoDocumento.ToLower().Equals("invoice") || item.TipoDocumento.ToLower().Equals("fs"))
                        {
                            //dados[0] = dados[0].ToString() + " : " + item.Valor.ToString();
                            ftinvalor = ftinvalor + totaltipodocumento;
                            ii.label = "FT";
                        }
                        else if (item.TipoDocumento.ToLower().Equals("ncred") || item.TipoDocumento.ToLower().Equals("cred") || item.TipoDocumento.ToLower().Equals("nota credito") || item.TipoDocumento.ToLower().Equals("nota de credito") || item.TipoDocumento.ToLower().Equals("nota de crédito") || item.TipoDocumento.ToLower().Equals("nc"))
                        {
                            //dados[1] = dados[1].ToString() + " : " + item.Valor.ToString();
                            ncinvalor = ncinvalor + totaltipodocumento;
                            ii.label = "NC";
                        }
                        else if (item.TipoDocumento.ToLower().Equals("ndebt") || item.TipoDocumento.ToLower().Equals("debt") || item.TipoDocumento.ToLower().Equals("nota debito") || item.TipoDocumento.ToLower().Equals("nota de debito") || item.TipoDocumento.ToLower().Equals("nota de débito") || item.TipoDocumento.ToLower().Equals("nd") || item.TipoDocumento.ToLower().Equals("debit note"))
                        {
                            //dados[2] = dados[2].ToString() + " : " + item.Valor.ToString();
                            ndinvalor = ndinvalor + totaltipodocumento;
                            ii.label = "ND";
                        }
                        else if (item.TipoDocumento.ToLower().Equals("receipt") || item.TipoDocumento.ToLower().Equals("recibo") || item.TipoDocumento.ToLower().Equals("rc"))
                        {
                            //dados[3] = dados[3].ToString() + " : " + item.Valor.ToString();
                            rcinvalor = rcinvalor + totaltipodocumento;
                            ii.label = "RC";
                        }
                        else if (item.TipoDocumento.ToLower().Equals("guia transporte") || item.TipoDocumento.ToLower().Equals("guia de transporte") || item.TipoDocumento.ToLower().Equals("gt"))
                        {
                            //dados[4] = dados[4].ToString() + " : " + item.Valor.ToString();
                            gtinvalor = gtinvalor + totaltipodocumento;
                            ii.label = "GT";
                        }
                        else if (item.TipoDocumento.ToLower().Equals("guia remessa") || item.TipoDocumento.ToLower().Equals("guia de remessa") || item.TipoDocumento.ToLower().Equals("gr"))
                        {
                            //dados[5] = dados[5].ToString() + " : " + item.Valor.ToString();
                            grinvalor = grinvalor + totaltipodocumento;
                            ii.label = "GR";
                        }
                        else if (item.TipoDocumento.ToLower().Equals("guia devolucao") || item.TipoDocumento.ToLower().Equals("guia de devolucao") || item.TipoDocumento.ToLower().Equals("gd") || item.TipoDocumento.ToLower().Equals("guia de devolução"))
                        {
                            //dados[6] = dados[6].ToString() + " : " + item.Valor.ToString();
                            gdinvalor = gdinvalor + totaltipodocumento;
                            ii.label = "GD";
                        }
                        else if (item.TipoDocumento.ToLower().Equals("guia anulacao") || item.TipoDocumento.ToLower().Equals("guia de anulacao") || item.TipoDocumento.ToLower().Equals("ga") || item.TipoDocumento.ToLower().Equals("guia de anulacao"))
                        {
                            //dados[7] = dados[7].ToString() + " : " + item.Valor.ToString();
                            gainvalor = gainvalor + totaltipodocumento;
                            ii.label = "GA";
                        }
                    }
                }

                //    dados[0] = ftoutvalor.ToString() + " : " + ftinvalor.ToString();
                //    dados[1] = ncoutvalor.ToString() + " : " + ncinvalor.ToString();
                //    dados[2] = ndoutvalor.ToString() + " : " + ndinvalor.ToString();
                //    dados[3] = rcoutvalor.ToString() + " : " + rcinvalor.ToString();
                //    dados[4] = gtoutvalor.ToString() + " : " + gtinvalor.ToString();
                //    dados[5] = groutvalor.ToString() + " : " + grinvalor.ToString();
                //    dados[6] = gdoutvalor.ToString() + " : " + gdinvalor.ToString();
                //    dados[7] = gaoutvalor.ToString() + " : " + gainvalor.ToString();
                //    dados[8] = sopdfoutvalor.ToString() + " : " + sopdfinvalor.ToString();
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
                #endregion
            }

            return dados;
        }
    }
}
