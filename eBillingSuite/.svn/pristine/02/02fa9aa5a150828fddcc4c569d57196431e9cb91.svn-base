using eBillingSuite.Model.EBC_DB;
using eBillingSuite.Model.HelpingClasses;
using eBillingSuite.Models;
using eBillingSuite.Repositories;
using eBillingSuite.Repositories.Interfaces;
using eBillingSuite.ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Shortcut.PixelAdmin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eBillingSuite.Controllers
{
    public class StatsController : Controller
    {
        private IPixelAdminPageContext _pixelAdminPageContext;
        private IEBC_PackageRepository _ebc_packageRepository;
        private IECCListRepositories _eCConfigRepositories;
        private IOutboundInboundRepository _outboundInboundRepository;
        private IComATPacketsRepository _comATPacketRepository;
        private IComATPacketsGuiasRepository _comATPacketGuiasRepository;
        private IeBillingSuiteRequestContext _context;
        private IDocsErrorsRepository docsErrorsRepository;
        private List<InboundPackets> lIP;
        private List<OutboundPackets> lOP;



        public StatsController(IPixelAdminPageContext pixelAdminPageContext,
            IECCListRepositories eCConfigRepositories,
            IeBillingSuiteRequestContext context,
            IOutboundInboundRepository outboundInboundRepository,
            IDocsErrorsRepository docsErrorsRepository,
            IComATPacketsRepository comATPacketRepository,
            IEBC_PackageRepository ebc_packageRepository)
        {
            _pixelAdminPageContext = pixelAdminPageContext;
            _eCConfigRepositories = eCConfigRepositories;
            _context = context;
            _outboundInboundRepository = outboundInboundRepository;
            _comATPacketRepository = comATPacketRepository;
            _ebc_packageRepository = ebc_packageRepository;
        }

        // GET: Stats
        public ActionResult Index(JQueryDataTableParamModel param)
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            if (!Request.IsAjaxRequest())
            {
                var modelVM = new EBCStats(_eCConfigRepositories, _context.UserIdentity.Instances);


                return View(modelVM);
            }

            var data = _eCConfigRepositories.outboundInboundRepository.GetAllPackets(_context.UserIdentity).ToList();

            var r = data.Select(o => new
            {
                Sentido = o.Sentido,
                NomeEmiss = o.NomeEmiss,
                NifEmissor = o.NifEmiss,
                NomeReceptor = o.NomeRecept,
                NifReceptor = o.NifRecept,
                NumDoc = o.NumDoc,
                Quantia = o.Quantia,
                DataFatura = String.Join("-", o.Ano, o.Mes, o.Dia),
                DataCriacao = o.DataCriacao,
                TipoDoc = o.TipoDoc,
                Estado = o.Estado,
                Id = o.ID
            });

            if (param.iDisplayLength != 0)
            {
                var recordCount = r.Count();
                var records = r
                    .Skip(param.iDisplayStart)
                    .Take(param.iDisplayLength)
                    .ToList();

                var reply = new JsonNetResult();
                reply.Data = new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = recordCount,
                    iTotalDisplayRecords = param.iDisplayLength,
                    aaData = records
                };

                return reply;
            }
            else
            {
                var records = r.Take(10).ToList();
                var reply = new JsonNetResult();
                reply.Data = new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = r.Count(),
                    iTotalDisplayRecords = records.Count,
                    aaData = r
                };

                return reply;
            }
        }

        public ActionResult GetData(JQueryDataTableParamModel param)
        {
            // Initialization.   
            JsonResult result = new JsonResult();
            try
            {
                var dataFromDB = _eCConfigRepositories.outboundInboundRepository.GetAllPackets(_context.UserIdentity).ToList();

                var data = dataFromDB.Select(o => new OutInPresentation
                {
                    Sentido = o.Sentido,
                    NomeEmissor = o.NomeEmiss,
                    NifEmissor = o.NifEmiss,
                    NomeReceptor = o.NomeRecept,
                    NifReceptor = o.NifRecept,
                    NumDoc = o.NumDoc,
                    Quantia = o.Quantia.ToString(),
                    DataFatura = String.Join("-", o.Ano, o.Mes, o.Dia),
                    DataCriacao = o.DataCriacao,
                    TipoDoc = o.TipoDoc,
                    Estado = o.Estado,
                    Id = o.ID
                }).ToList();


                // Initialization.   
                var searchParam = new SearchParameters
                {
                    estado = Request.Form.GetValues("columns[11][search][value]")[0],
                    numDoc = Request.Form.GetValues("columns[6][search][value]")[0],
                    nifRec = Request.Form.GetValues("columns[5][search][value]")[0],
                    nifEmi = Request.Form.GetValues("columns[3][search][value]")[0],
                    dataFinal = Request.Form.GetValues("columns[2][search][value]")[0],
                    dataInicial = Request.Form.GetValues("columns[1][search][value]")[0]
                };

                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
                // Loading.   
                // Total record count.   
                int totalRecords = data.Count;
                // Verification.   
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search   
                    data = data.Where(p => p.Sentido.ToString().ToLower().Contains(search.ToLower()) ||
                        p.NomeEmissor.ToLower().Contains(search.ToLower()) ||
                        p.NifEmissor.ToString().ToLower().Contains(search.ToLower()) ||
                        p.NomeReceptor.ToLower().Contains(search.ToLower()) ||
                        p.NifReceptor.ToLower().Contains(search.ToLower()) ||
                        p.NumDoc.ToString().ToLower().Contains(search.ToLower()) ||
                        p.Quantia.ToString().ToLower().Contains(search.ToLower()) ||
                        p.DataCriacao.ToString().ToLower().Contains(search.ToLower()) ||
                        p.TipoDoc.ToString().ToLower().Contains(search.ToLower()) ||
                        p.Estado.ToString().ToLower().Contains(search.ToLower())).ToList();
                }
                // Sorting.   
                data = this.SortByColumnWithOrder(order, orderDir, data, searchParam);
                // Filter record count.   
                int recFilter = data.Count;
                // Apply pagination.   
                data = data.Skip(startRec).Take(pageSize).ToList();
                // Loading drop down lists.   
                result = this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = totalRecords,
                    recordsFiltered = recFilter,
                    data = data
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Info   
                Console.Write(ex);
            }
            // Return info.   
            return result;

        }

        

        public ActionResult Details(int? id, string direction)
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            if (!id.HasValue)
                RedirectToAction("Index");

            string label = direction == "Out" ? "Outbound" : "Inbound";
            label += " " + id.ToString();

            try
            {
                var packetInfo = _outboundInboundRepository.GetPacketById(id.Value, direction);

                PacketDetailVM k = (PacketDetailVM)packetInfo;

                //if (Request.IsAjaxRequest())
                //    return Json(this.PanelContentReply(packetInfo), JsonRequestBehavior.AllowGet);
                //else
                //return View(k);

                if (Request.IsAjaxRequest())
                    return Json(this.ModalContentReply(k), JsonRequestBehavior.AllowGet);
                else
                    return PartialView(k);

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return RedirectToAction("Index");
            }
        }

        public ActionResult FaturasOutros(JQueryDataTableParamModel param)
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            if (!Request.IsAjaxRequest())
            {
                return View("FaturasOutros");
            }
            var data = _eCConfigRepositories.ComATPacketRepository.GetComATPackets(_context.UserIdentity.Instances).ToList();

            var r = data.Select(o => new
            {
                NifRecetor = o.NIFReceptor,
                NDocumento = o.NumeroDocumento,
                DataDocumento = o.DataDocumento.ToShortDateString(),
                TotalIVA = o.TotalComIva,
                Estado = o.EstadoAT,
                MsgRetornoAT = o.ObsRetornoAT,
                UltimoEnvio = o.LastSentDate
            });

            if (param.iDisplayLength != 0)
            {
                var recordCount = r.Count();
                var records = r
                    .Skip(param.iDisplayStart)
                    .Take(param.iDisplayLength)
                    .ToList();

                var reply = new JsonNetResult();
                reply.Data = new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = recordCount,
                    iTotalDisplayRecords = param.iDisplayLength,
                    aaData = records
                };

                return reply;
            }
            else
            {
                var records = r.Take(10).ToList();
                var reply = new JsonNetResult();
                reply.Data = new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = r.Count(),
                    iTotalDisplayRecords = records.Count,
                    aaData = r
                };

                return reply;
            }
        }

        public ActionResult DocTransporte(JQueryDataTableParamModel param)
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);
            if (!Request.IsAjaxRequest())
            {
                return View("DocTransporte");
            }
            var data = _eCConfigRepositories.ComATPacketsGuiasRepository.GetComATPacketsGuias(_context.UserIdentity.Instances).ToList();

            var r = data.Select(o => new
            {
                NDocumento = o.NumeroDocumento,
                CodRetorno = o.ATDocCodeID,
                Estado = o.EstadoAT,
                MsgRetornoAT = o.ObsRetornoAT,
                UltimoEnvio = o.LastSentDate.ToString()
            });

            if (param.iDisplayLength != 0)
            {
                var recordCount = r.Count();
                var records = r
                    .Skip(param.iDisplayStart)
                    .Take(param.iDisplayLength)
                    .ToList();

                var reply = new JsonNetResult();
                reply.Data = new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = recordCount,
                    iTotalDisplayRecords = param.iDisplayLength,
                    aaData = records
                };

                return reply;
            }
            else
            {
                var records = r.Take(10).ToList();
                var reply = new JsonNetResult();
                reply.Data = new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = r.Count(),
                    iTotalDisplayRecords = records.Count,
                    aaData = r
                };

                return reply;
            }
        }

        public ActionResult IndicadoresQuantidades()
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);
            var data = new IndicadoresQtd();
            data.TopCostumers = eBillingSuite.Model.HelpingClasses.IndicadoresFacturacaoQtd
                .GetTopCostumers(_context.UserIdentity.Instances)
                .Select(c => new ChartData
                {
                    label = c.label,
                    data = c.data
                })
                .ToList();

            data.TopFornecedores = eBillingSuite.Model.HelpingClasses.IndicadoresFacturacaoQtd
               .GetTopFornecedores(_context.UserIdentity.Instances)
               .Select(c => new ChartData
               {
                   label = c.label,
                   data = c.data
               })
               .ToList();

            data.DocsLastYearOUT = eBillingSuite.Model.HelpingClasses.IndicadoresFacturacaoQtd
               .GetDocsLastYearOUT(_context.UserIdentity.Instances)
               .Select(c => new FlotData
               {
                   data = c.data,
                   mes = c.mes
               })
               .ToList();

            data.DocsLastYearIN = eBillingSuite.Model.HelpingClasses.IndicadoresFacturacaoQtd
               .GetDocsLastYearIN(_context.UserIdentity.Instances)
               .Select(c => new FlotData
               {
                   data = c.data,
                   mes = c.mes
               })
               .ToList();

            data.DocsByType = eBillingSuite.Model.HelpingClasses.IndicadoresFacturacaoQtd
               .GetDocsByType(_context.UserIdentity.Instances);

            // v4.5
            // porque na vista, o xAxis tem de começar em 1 (senão aparece o mês 0), acrescentamos, no indice 0 da lista um elemento sem nada,
            // para ficar consistente e aparecer o 1º mes no grafico
            //data.DocsLastYearOUT.Insert(0, new FlotData { data = 0.0 });
            //data.DocsLastYearIN.Insert(0, new FlotData { data = 0.0 });

            if (Request.IsAjaxRequest())
                return Json(data, JsonRequestBehavior.AllowGet);

            return View(data);
        }

        public ActionResult IndicadoresQuantias()
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);
            var data = new IndicadoresQuant();
            data.TopCostumers = IndicadoresFacturacaoQuant
               .GetTopCostumers(_context.UserIdentity.Instances)
                .Select(c => new ChartDataQuant
                {
                    label = c.label,
                    data = c.data
                })
               .ToList();

            data.TopFornecedores = IndicadoresFacturacaoQuant
               .GetTopFornecedores(_context.UserIdentity.Instances)
                .Select(c => new ChartDataQuant
                {
                    label = c.label,
                    data = c.data
                })
               .ToList();

            data.DocsLastYearOUT = IndicadoresFacturacaoQuant
               .GetDocsLastYearOUT(_context.UserIdentity.Instances)
               .Select(c => new FlotDataQuant
               {
                   data = c.data,
                   mes = int.Parse(c.label)
               })
               .ToList();

            data.DocsLastYearIN = IndicadoresFacturacaoQuant
               .GetDocsLastYearIN(_context.UserIdentity.Instances)
               .Select(c => new FlotDataQuant
               {
                   mes = int.Parse(c.label),
                   data = c.data
               })
               .ToList();

            data.DocsByType = IndicadoresFacturacaoQuant
               .GetDocsByType(_context.UserIdentity.Instances);

            // v4.5
            // porque na vista, o xAxis tem de começar em 1 (senão aparece o mês 0), acrescentamos, no indice 0 da lista um elemento sem nada,
            // para ficar consistente e aparecer o 1º mes no grafico

            if (Request.IsAjaxRequest())
                return Json(data, JsonRequestBehavior.AllowGet);

            //SetBreadcrumb(
            //    new BreadcrumbStep("Estatísticas"),
            //    new BreadcrumbStep(Url, "Indicadores - Quantia", "IndicadoresFacturacaoQuant"));

            return View(data);
            //return Json(data);


        }

        public ActionResult IndicadoresPerformance()
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);
            var data = new IndicadoresPerf();
            data.TempoMedioResposta = eBillingSuite.Model.HelpingClasses.IndicadoresFacturacaoPerf
               .GetTempoMedioResposta(_context.UserIdentity.Instances);

            data.TopCostumers = eBillingSuite.Model.HelpingClasses.IndicadoresFacturacaoPerf
                            .GetTopCostumers(_context.UserIdentity.Instances)
                            .Select(c => new ChartDataPerf
                            {
                                label = c.label,
                                data = c.data
                            })
                            .ToList();

            data.LastCostumers = eBillingSuite.Model.HelpingClasses.IndicadoresFacturacaoPerf
                .GetLastCostumers(_context.UserIdentity.Instances)
                .Select(c => new ChartDataPerf
                {
                    label = c.label,
                    data = c.data
                })
                .ToList();

            data.TemposMediosCliente = eBillingSuite.Model.HelpingClasses.IndicadoresFacturacaoPerf
                .GetTemposMediosCliente(_context.UserIdentity.Instances)
                .Select(c => new ChartDataPerf
                {
                    label = c.cliente,
                    data = Math.Round(c.ClienteTempoMedio, 2)
                }).ToList();

            data.TemposMediosClienteAno = eBillingSuite.Model.HelpingClasses.IndicadoresFacturacaoPerf
                .GetTemposMediosAnoMes(_context.UserIdentity.Instances)
                .Select(c => new ChartDataPerf
                {
                    data = c.Ano,
                    label = c.Mes,
                    TempoMedio = c.DataTempoMedio
                }).ToList();

            if (Request.IsAjaxRequest())
                return Json(data, JsonRequestBehavior.AllowGet);

            return View(data);
        }

        public ActionResult IntegratedFiles(JQueryDataTableParamModel param)
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            if (!Request.IsAjaxRequest())
            {
                return View("IntegratedFiles");
            }

            var data = eBillingSuite.Model.HelpingClasses.IntegratedFiles.
                GetSubmissionFilesData(_context.UserIdentity.Instances)
                .ToList();

            var r = data.Select(o => new
            {
                NumDoc = o.NumDoc,
                SubmissionF = o.SubmissionFile,
                SubmissionD = o.SubmissionDate
            });

            if (param.iDisplayLength != 0)
            {
                var recordCount = r.Count();
                var records = r
                    .Skip(param.iDisplayStart)
                    .Take(param.iDisplayLength)
                    .ToList();

                var reply = new JsonNetResult();
                reply.Data = new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = recordCount,
                    iTotalDisplayRecords = param.iDisplayLength,
                    aaData = records
                };

                return reply;
            }
            else
            {
                var records = r.Take(10).ToList();
                var reply = new JsonNetResult();
                reply.Data = new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = r.Count(),
                    iTotalDisplayRecords = records.Count,
                    aaData = r
                };

                return reply;
            }
        }

        public ActionResult DocumentosErros(JQueryDataTableParamModel param)
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            if (!Request.IsAjaxRequest())
            {
                return View("DocumentosErros");
            }

            //SetBreadcrumb(new BreadcrumbStep(Url, "Documentos Erros", ""));

            IEnumerable<eBillingSuite.Model.EBC_DB.DocumentosErros> filteredFacts;
            IEnumerable<eBillingSuite.Model.EBC_DB.DocumentosErros> displayedFacts;

            docsErrorsRepository = new DocsErrorsRepository();


            var model = docsErrorsRepository.GetAllErrorDocumentos(_context.UserIdentity.Instances);

            if (!string.IsNullOrWhiteSpace(param.sSearch))
                filteredFacts = docsErrorsRepository.GetAllDocumentosErros(param.sSearch);
            else
                filteredFacts = model;

            //para a ordenação
            var isNumeroDocumentoSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isNIFEmissorSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isNIFReceptorSortable = Convert.ToBoolean(Request["bSortable_3"]);
            var isDataDocumentoSortable = Convert.ToBoolean(Request["bSortable_4"]);
            var isTipoErroSortable = Convert.ToBoolean(Request["bSortable_5"]);
            var isDetalheErroSortable = Convert.ToBoolean(Request["bSortable_6"]);
            var isFicheiroSortable = Convert.ToBoolean(Request["bSortable_7"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);

            Func<DocumentosErros, string> orderingFunction = (c => sortColumnIndex == 0 && isNumeroDocumentoSortable ? c.NumDoc :
                sortColumnIndex == 1 && isNIFEmissorSortable ? c.NifEmissor :
                sortColumnIndex == 2 && isNIFReceptorSortable ? c.NifRecetor :
                sortColumnIndex == 3 && isDataDocumentoSortable ? c.DataCriacao.ToString("YYYY-MM-dd") :
                sortColumnIndex == 4 && isTipoErroSortable ? c.TipoErro :
                sortColumnIndex == 5 && isDetalheErroSortable ? c.DetalheErro :
                sortColumnIndex == 6 && isFicheiroSortable ? c.Ficheiro :
                "");

            if (param.iDisplayLength != 0)
            {
                displayedFacts = filteredFacts.Skip(param.iDisplayStart).Take(param.iDisplayLength);
                var result = from c in displayedFacts
                             select new[] { c.NumDoc, c.NifEmissor, c.NifRecetor,
                    c.DataCriacao.ToString("YYYY-MM-dd"), c.TipoErro, c.DetalheErro, c.Ficheiro};

                var reply = new JsonNetResult();
                reply.Data = new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = model.Count(),
                    iTotalDisplayRecords = filteredFacts.Count(),
                    aaData = result
                };

                return reply;
            }
            else
            {
                displayedFacts = filteredFacts.Skip(0).Take(10);
                model = displayedFacts.ToList();

                var reply = new JsonNetResult();
                reply.Data = new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = model.Count(),
                    iTotalDisplayRecords = filteredFacts.Count(),
                    aaData = model
                };

                return reply;
            }
        }

        public ActionResult UpdateOutboundReprocessed(int? id, string direction, string obs)
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            //if (!Request.IsAjaxRequest())
            //{
            //    return View("Index");
            //}

            var data = _eCConfigRepositories.outboundInboundRepository.GetAllPackets(_context.UserIdentity).ToList();

            try
            {
                if (String.IsNullOrWhiteSpace(id.Value.ToString()))
                    throw new Exception("Tem de selecionar pelo menos um registo.");

                int[] finalOutIds = new int[1];
                int[] finalInIds = new int[1];


                switch (direction)
                {
                    case "In":
                        finalInIds[0] = id.Value;
                        break;
                    case "Out":
                        finalOutIds[0] = id.Value;
                        break;
                }

                _outboundInboundRepository.UpdatePackets(finalOutIds, finalInIds);
                if (direction == "Out")
                {
                    var packetInfo = _outboundInboundRepository.GetPacketById(id.Value, direction);

                    _ebc_packageRepository.InserEventsByIDwObs(packetInfo.outboundDetail.documentDetails.EbcPackageId, 7, 2, obs);
                }
            }
            catch (Exception e)
            {
                if (Request.IsAjaxRequest())
                {
                    //data.error = "Erro ao completar acção: " + e.Message;
                    return View("Index");
                }
            }
            if (Request.IsAjaxRequest())
                return View("Index");

            return View("Index");
        }


        public ActionResult Reprocessamento(int? id, string direction)
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            if (!id.HasValue)
                RedirectToAction("Index");

            string label = direction == "Out" ? "Outbound" : "Inbound";
            label += " " + id.ToString();
            try
            {
                var packetInfo = _outboundInboundRepository.GetPacketById(id.Value, direction);



            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public ActionResult DownloadFile(string filename, string sentido, string ano, string mes)
        {
            FileStream stream = new FileStream(_outboundInboundRepository.GetFilePath(filename, sentido, ano, mes), FileMode.Open);


            FileStreamResult fsr = new FileStreamResult(stream, "application/octet-stream");
            if (sentido.ToLower() == "out")
                fsr.FileDownloadName = _outboundInboundRepository.GetFilenameWithRealExtension(filename);
            else
                fsr.FileDownloadName = filename;

            return fsr;
        }


        #region Helpers
        private List<OutInPresentation> SortByColumnWithOrder(string order, string orderDir, List<OutInPresentation> data, SearchParameters searchParameters)
        {
            List<OutInPresentation> lst = new List<OutInPresentation>();
            try
            {
                // Sorting   
                switch (order)
                {
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Sentido).ToList() : data.OrderBy(p => p.Sentido).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.NomeEmissor).ToList() : data.OrderBy(p => p.NomeEmissor).ToList();
                        break;
                    case "3":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.NifEmissor).ToList() : data.OrderBy(p => p.NifEmissor).ToList();
                        break;
                    case "4":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.NomeReceptor).ToList() : data.OrderBy(p => p.NomeReceptor).ToList();
                        break;
                    case "5":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.NifReceptor).ToList() : data.OrderBy(p => p.NifReceptor).ToList();
                        break;
                    case "6":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.NumDoc).ToList() : data.OrderBy(p => p.NumDoc).ToList();
                        break;
                    case "7":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Quantia).ToList() : data.OrderBy(p => p.Quantia).ToList();
                        break;
                    case "8":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.DataFatura).ToList() : data.OrderBy(p => p.DataFatura).ToList();
                        break;
                    case "9":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.DataCriacao).ToList() : data.OrderBy(p => p.DataCriacao).ToList();
                        break;
                    case "10":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TipoDoc).ToList() : data.OrderBy(p => p.TipoDoc).ToList();
                        break;
                    case "11":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Estado).ToList() : data.OrderBy(p => p.Estado).ToList();
                        break;
                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Sentido).ToList() : data.OrderBy(p => p.Sentido).ToList();
                        break;
                }

                if (!string.IsNullOrEmpty(searchParameters.estado))
                    lst = lst.Where(o => o.Estado == searchParameters.estado).ToList();
                if (!string.IsNullOrEmpty(searchParameters.numDoc))
                    lst = lst.Where(o => o.NumDoc == searchParameters.numDoc).ToList();
                if (!string.IsNullOrEmpty(searchParameters.nifEmi))
                    lst = lst.Where(o => o.NifEmissor == searchParameters.nifEmi).ToList();
                if (!string.IsNullOrEmpty(searchParameters.nifRec))
                    lst = lst.Where(o => o.NifReceptor == searchParameters.nifRec).ToList();
                if (!string.IsNullOrEmpty(searchParameters.dataInicial))
                    lst = lst.Where(o => DateTime.Parse(o.DataFatura) >= DateTime.Parse(searchParameters.dataInicial)).ToList();
                if (!string.IsNullOrEmpty(searchParameters.dataFinal))
                    lst = lst.Where(o => DateTime.Parse(o.DataFatura) <= DateTime.Parse(searchParameters.dataFinal)).ToList();


            }
            catch (Exception ex)
            {
                // info.   
                Console.Write(ex);
            }
            // info.   
            return lst;
        }

        public ActionResult ToCsv(string filter, string dateRange, string state, string numDoc, string nifRecetor, bool isTransport = false)
        {
            try
            {
                //obter as comunicações filtradas
                List<ComATPackets> listPacks;
                List<ComATPackets_Guias> listPacksTransport;

                string tempFilename = "";

                List<List<string>> rows = new List<List<string>>();

                if (!isTransport)
                {
                    listPacks = String.IsNullOrWhiteSpace(filter) ?
                        _comATPacketRepository.GetComATPackets(_context.UserIdentity.Instances) : _comATPacketRepository.GetFilteredComATPackets(filter, dateRange, state, numDoc, nifRecetor);

                    tempFilename = "csvListATPacks";

                    foreach (ComATPackets atPacket in listPacks)
                    {
                        List<string> tempList = new List<string>();
                        tempList.Add(!String.IsNullOrWhiteSpace(atPacket.NIFReceptor) ? atPacket.NIFReceptor : "N/A");
                        tempList.Add(!String.IsNullOrWhiteSpace(atPacket.NumeroDocumento) ? atPacket.NumeroDocumento : "N/A");
                        tempList.Add(atPacket.DataDocumento.ToShortDateString());
                        tempList.Add(atPacket.TotalComIva.ToString());
                        tempList.Add(!String.IsNullOrWhiteSpace(atPacket.EstadoAT) ? atPacket.EstadoAT : "N/A");
                        tempList.Add(!String.IsNullOrWhiteSpace(atPacket.ObsRetornoAT) ? atPacket.ObsRetornoAT : "N/A");
                        tempList.Add(atPacket.LastSentDate.HasValue ? atPacket.LastSentDate.Value.ToString() : "N/A");

                        rows.Add(tempList);
                    }
                }
                else
                {
                    listPacksTransport = String.IsNullOrWhiteSpace(filter) ?
                        _comATPacketGuiasRepository.GetComATPacketsGuias(_context.UserIdentity.Instances) : _comATPacketGuiasRepository.GetFilteredComATPackets(filter, dateRange, state, numDoc);
                    tempFilename = "csvListATPacksTransport";

                    foreach (ComATPackets_Guias atPacketTransport in listPacksTransport)
                    {
                        List<string> tempList = new List<string>();
                        tempList.Add(!String.IsNullOrWhiteSpace(atPacketTransport.NumeroDocumento) ? atPacketTransport.NumeroDocumento : "N/A");
                        tempList.Add(!String.IsNullOrWhiteSpace(atPacketTransport.ATDocCodeID) ? atPacketTransport.ATDocCodeID : "N/A");
                        tempList.Add(!String.IsNullOrWhiteSpace(atPacketTransport.EstadoAT) ? atPacketTransport.EstadoAT : "N/A");
                        tempList.Add(!String.IsNullOrWhiteSpace(atPacketTransport.ObsRetornoAT) ? atPacketTransport.ObsRetornoAT : "N/A");
                        tempList.Add(atPacketTransport.LastSentDate.HasValue ? atPacketTransport.LastSentDate.Value.ToString() : "N/A");

                        rows.Add(tempList);
                    }
                }

                //com as linhas, contruir o CSV
                string filePath = Server.MapPath("..") + @"\ListExports\" + tempFilename + DateTime.Now.ToShortDateString().Replace("/", "-") + "_" + DateTime.Now.ToShortTimeString().Replace(":", String.Empty) + ".csv";
                CreateCsv(rows, filePath, isTransport);
                return File(filePath, "text/csv", "list" + DateTime.Now.ToShortDateString().Replace("/", "-") + "_" + DateTime.Now.ToShortTimeString().Replace(":", String.Empty) + ".csv");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Controí o CSV da listagem ficheiros integrados
        /// </summary>
        /// <param name="lista">'Matriz' de valores, dentro de cada indice da lista existe outra lista com os valores de cada linha</param>
        /// <param name="path">caminho para o CSV gerado</param>
        private void CreateCsv(List<List<string>> lista, string path, bool isTransport)
        {
            try
            {
                //colocar os títulos das colunas
                List<string> cabecalhos = new List<string>();
                if (!isTransport)
                {
                    cabecalhos.Add("NIF Recetor"); cabecalhos.Add("Nº Documento"); cabecalhos.Add("Data Documento"); cabecalhos.Add("Total C/IVA");
                    cabecalhos.Add("Estado"); cabecalhos.Add("Mensagem Retorno AT"); cabecalhos.Add("Último Envio");
                }
                else
                {
                    cabecalhos.Add("Nº Documento"); cabecalhos.Add("Cód. Retorno AT"); cabecalhos.Add("Estado");
                    cabecalhos.Add("Mensagem Retorno AT"); cabecalhos.Add("Último Envio");
                }
                lista.Insert(0, cabecalhos);

                FileInfo fp = new FileInfo(path);
                if (!Directory.Exists(fp.DirectoryName))
                {
                    Directory.CreateDirectory(fp.DirectoryName);
                }

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
                {
                    string csv = String.Empty;
                    //int i = 0;
                    foreach (List<string> list2 in lista)
                    {
                        // remover o contador que é acrescentado no 'GetSubmissionFilesData'
                        //if (i != 0)
                        //	list2.RemoveAt(0);

                        csv = String.Join(";", list2
                            .Select(x => x.ToString()).ToArray());
                        file.WriteLine(csv);

                        //i++;
                    }
                }
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        public ActionResult ToPdf(string filter, string dateRange, string state, string numDoc, string nifRecetor, bool isTransport = false)
        {
            try
            {
                //obter as comunicações filtradas
                List<ComATPackets> listPacks;
                List<ComATPackets_Guias> listPacksTransport;

                string tempFilename = "";

                List<List<string>> rows = new List<List<string>>();

                if (!isTransport)
                {
                    listPacks = String.IsNullOrWhiteSpace(filter) ?
                        _comATPacketRepository.GetComATPackets(_context.UserIdentity.Instances) : _comATPacketRepository.GetFilteredComATPackets(filter, dateRange, state, numDoc, nifRecetor);

                    tempFilename = "csvListATPacks";

                    foreach (ComATPackets atPacket in listPacks)
                    {
                        List<string> tempList = new List<string>();
                        tempList.Add(!String.IsNullOrWhiteSpace(atPacket.NIFReceptor) ? atPacket.NIFReceptor : "N/A");
                        tempList.Add(!String.IsNullOrWhiteSpace(atPacket.NumeroDocumento) ? atPacket.NumeroDocumento : "N/A");
                        tempList.Add(atPacket.DataDocumento.ToShortDateString());
                        tempList.Add(atPacket.TotalComIva.ToString());
                        tempList.Add(!String.IsNullOrWhiteSpace(atPacket.EstadoAT) ? atPacket.EstadoAT : "N/A");
                        tempList.Add(!String.IsNullOrWhiteSpace(atPacket.ObsRetornoAT) ? atPacket.ObsRetornoAT : "N/A");
                        tempList.Add(atPacket.LastSentDate.HasValue ? atPacket.LastSentDate.Value.ToString() : "N/A");

                        rows.Add(tempList);
                    }
                }
                else
                {
                    listPacksTransport = String.IsNullOrWhiteSpace(filter) ?
                        _comATPacketGuiasRepository.GetComATPacketsGuias(_context.UserIdentity.Instances) : _comATPacketGuiasRepository.GetFilteredComATPackets(filter, dateRange, state, numDoc);
                    tempFilename = "csvListATPacksTransport";

                    foreach (ComATPackets_Guias atPacketTransport in listPacksTransport)
                    {
                        List<string> tempList = new List<string>();
                        tempList.Add(!String.IsNullOrWhiteSpace(atPacketTransport.NumeroDocumento) ? atPacketTransport.NumeroDocumento : "N/A");
                        tempList.Add(!String.IsNullOrWhiteSpace(atPacketTransport.ATDocCodeID) ? atPacketTransport.ATDocCodeID : "N/A");
                        tempList.Add(!String.IsNullOrWhiteSpace(atPacketTransport.EstadoAT) ? atPacketTransport.EstadoAT : "N/A");
                        tempList.Add(!String.IsNullOrWhiteSpace(atPacketTransport.ObsRetornoAT) ? atPacketTransport.ObsRetornoAT : "N/A");
                        tempList.Add(atPacketTransport.LastSentDate.HasValue ? atPacketTransport.LastSentDate.Value.ToString() : "N/A");

                        rows.Add(tempList);
                    }
                }

                //com as linhas, contruir o PDF
                string filePath = Server.MapPath("..") + @"\ListExports\" + tempFilename + DateTime.Now.ToShortDateString().Replace("/", "-") + "_" + DateTime.Now.ToShortTimeString().Replace(":", String.Empty) + ".pdf";
                CreatePdf(rows, filePath, isTransport);
                return File(filePath, "application/pdf", "pdflist" + DateTime.Now.ToShortDateString().Replace("/", "-") + "_" + DateTime.Now.ToShortTimeString().Replace(":", String.Empty) + ".pdf");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Controí o PDF da listagem de ficheiros integrados
        /// </summary>
        /// <param name="lista">'Matriz' de valores, dentro de cada indice da lista existe outra lista com os valores de cada linha</param>
        /// <param name="path">caminho para o PDF gerado</param>
        private void CreatePdf(List<List<string>> lista, string path, bool isTransport)
        {
            try
            {
                iTextSharp.text.Font headerFont = FontFactory.GetFont("Verdana", 10, iTextSharp.text.BaseColor.WHITE);
                iTextSharp.text.Font rowfont = FontFactory.GetFont("Verdana", 8, iTextSharp.text.BaseColor.BLUE);
                string[] columns = !isTransport ? new string[7] : new string[5];

                string tipo = "";
                if (!isTransport)
                {
                    tipo = "Faturas e outros";

                    columns[0] = "NIF Recetor"; columns[1] = "Nº Documento"; columns[2] = "Data Documento"; columns[3] = "Total C/IVA";
                    columns[4] = "Estado"; columns[5] = "Mensagem Retorno AT"; columns[6] = "Último Envio";

                }
                else
                {
                    tipo = "Doc. tansporte";

                    columns[0] = "Nº Documento"; columns[1] = "Cód. Retorno AT"; columns[2] = "Estado";
                    columns[3] = "Mensagem Retorno AT"; columns[4] = "Último Envio";
                }

                using (Document doc = new Document(PageSize.A4.Rotate()))
                {
                    using (PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(path, FileMode.OpenOrCreate)))
                    {
                        doc.Open();

                        //colocar cabeçalho
                        iTextSharp.text.Rectangle page = doc.PageSize;
                        PdfPTable head = new PdfPTable(1);
                        head.TotalWidth = page.Width;
                        Phrase phrase = new Phrase("Lista de ficheiros comunicados à AT (" + tipo + ")  -   " +
                          DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + " GMT",
                          new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 8)
                        );
                        PdfPCell c = new PdfPCell(phrase);
                        c.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        c.VerticalAlignment = Element.ALIGN_TOP;
                        c.HorizontalAlignment = Element.ALIGN_CENTER;
                        head.AddCell(c);
                        head.WriteSelectedRows(0, -1, 0, page.Height - doc.TopMargin + head.TotalHeight + 20, writer.DirectContent);

                        //construir listagem
                        //título das colunas
                        PdfPTable table = new PdfPTable(columns.Length);
                        table.WidthPercentage = 100;
                        foreach (var column in columns)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(column, headerFont));
                            cell.BackgroundColor = iTextSharp.text.BaseColor.BLACK;
                            table.AddCell(cell);
                        }

                        //informação dos pacotes
                        foreach (var sublist in lista)
                        {
                            //int i = 0;
                            foreach (var item in sublist)
                            {
                                //se não for o identificador
                                //if (i != 0 && i != 9 && i != 10)
                                //{
                                if (!String.IsNullOrEmpty(item))
                                {
                                    PdfPCell cell5 = new PdfPCell(new Phrase(item.ToString(), rowfont));
                                    table.AddCell(cell5);
                                }
                                else
                                {
                                    PdfPCell cell5 = new PdfPCell(new Phrase(String.Empty, rowfont));
                                    table.AddCell(cell5);
                                }
                                //}
                                //i++;
                            }
                        }
                        doc.Add(table);

                        doc.Close();
                        writer.Close();
                    }
                }
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }


        #endregion
    }

    internal class SearchParameters
    {
        public string estado { get; set; }
        public string numDoc { get; set; }
        public string nifRec { get; set; }
        public string nifEmi { get; set; }
        public string dataInicial { get; set; }

        public string dataFinal { get; set; }
    }

    internal class OutInPresentation
    {
        public string Sentido { get; set; }
        public string NomeEmissor { get; set; }
        public string NifEmissor { get; set; }
        public string NomeReceptor { get; set; }
        public string NifReceptor { get; set; }
        public string NumDoc { get; set; }
        public string Quantia { get; set; }
        public string DataFatura { get; set; }
        public string DataCriacao { get; set; }
        public string TipoDoc { get; set; }
        public string Estado { get; set; }
        public int Id { get; set; }
    }
}