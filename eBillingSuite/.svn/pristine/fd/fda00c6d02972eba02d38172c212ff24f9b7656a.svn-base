using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shortcut.PixelAdmin;
using eBillingSuite.Model;
using eBillingSuite.Model.EBC_DB;
using eBillingSuite.Model.eBillingConfigurations;
using eBillingSuite.Model.HelpingClasses;
using eBillingSuite.Resources;
using eBillingSuite.HelperTools.Graphs;
using eBillingSuite.Repositories;

namespace eBillingSuite.ViewModels
{
    public class HomeVM
    {
        private IECCListRepositories _eCConfigRepositories;
        private IPixelAdminPageContext _pixelAdminPageContext;
        private IeBillingSuiteRequestContext _context;
        private IGraphsRepository _graphsRepository;
        public string alerta;
        private eBC_Login user;
        /// Donuts
        public int outboundPackets { get; private set; }
        public int inboundPackets { get; private set; }
        public double inboundPacketsInPercent { get; private set; }
        public double outboundPacketsInPercent { get; private set; }
        public int integratedPacket { get; set; }
        public int notIntegratedPacket { get; set; }
        public double integratedPacketsInPercent { get; private set; }
        public double notIntegratedPacketsInPercent { get; private set; }
        public int outboundEntregue { get; set; }
        public int outboundEspera { get; set; }
        public double outboundEntregueInPercent { get; private set; }
        public double outboundEsperaInPercent { get; private set; }
        // Pie Charts
        /// 1
        public int faturasAtSucesso { get; set; }
        public int totalFaturas { get; set; }
        public double faturasAtSucessoInPercent { get; private set; }
        /// 2
        public int guiasComCod { get; set; }
        public int totalGuias { get; set; }
        public double guiasComCodInPercent { get; set; }
        /// 3
        public int erroFatura { get; set; }
        public double erroFaturaInPercent { get; set; }
        /// 4
        public int erroGuia { get; set; }
        public double erroGuiaInPercent { get; set; }

        public List<string> listFactElecGraphs { get; set; }

        public List<OutboundInbound> Bounds { get; set; }

        public HomeVM(IECCListRepositories eCConfigRepositories, IPixelAdminPageContext pixelAdminPageContext,
            IGraphsRepository graphsRepository,
            IeBillingSuiteRequestContext context)
        {
            List<StatsValues> listaStatsValues = new List<StatsValues>();

            this._eCConfigRepositories = eCConfigRepositories;
            this._pixelAdminPageContext = pixelAdminPageContext;
            this._context = context;
            _graphsRepository = graphsRepository;

            EnumGraphs enumGraphs = new EnumGraphs();
            listFactElecGraphs = new List<string>();

            var factElectChoise = _graphsRepository.Where(o => o.username == _context.UserIdentity.Name).FirstOrDefault();
            if (_context.UserIdentity.Name == "tastas")
                factElectChoise = new UserGraphs()
                {
                    FacElect = "0;1;2",
                    username = "tastas",
                    Desmaterializacao = "0;1;2;3;4",
                };
            if (factElectChoise != null)
            {
                if (!String.IsNullOrEmpty(factElectChoise.FacElect))
                    listFactElecGraphs = GetFactElect(enumGraphs, factElectChoise.FacElect);
            }


            // Pie Charts
            /// Faturas AT com Sucesso
            faturasAtSucesso = _eCConfigRepositories.ComATPacketRepository.GetComATPackets(_context.UserIdentity.Instances).Count(v => v.EstadoAT.Equals("1"));
            totalFaturas = _eCConfigRepositories.ComATPacketRepository.GetComATPackets(_context.UserIdentity.Instances).Count();
            faturasAtSucessoInPercent = Math.Round(((double)(faturasAtSucesso) / totalFaturas) * 100, 2);
            /// Guias
            guiasComCod = _eCConfigRepositories.ComATPacketsGuiasRepository.GetComATPacketsGuias(_context.UserIdentity.Instances).Count(v => v.ATDocCodeID != null);
            totalGuias = _eCConfigRepositories.ComATPacketsGuiasRepository.GetComATPacketsGuias(_context.UserIdentity.Instances).Count();
            guiasComCodInPercent = Math.Round(((double)(guiasComCod) / totalGuias) * 100, 2);
            /// Erros na faturação
            erroFatura = _eCConfigRepositories.ComATPacketRepository.GetComATPackets(_context.UserIdentity.Instances).Count(v => v.EstadoAT.Equals("2"));
            erroFaturaInPercent = Math.Round(((double)(erroFatura) / totalFaturas) * 100, 2);
            /// Erros na Guia
            erroGuia = _eCConfigRepositories.ComATPacketsGuiasRepository.GetComATPacketsGuias(_context.UserIdentity.Instances).Count(v => v.EstadoAT.Equals("2"));
            erroGuiaInPercent = Math.Round(((double)(erroGuia) / totalGuias) * 100, 2);

            Bounds = _eCConfigRepositories.outboundInboundRepository.GetAllPackets(_context.UserIdentity);

            //
            //foreach(var eventos in feEvents.ToList())
            //{
            //   // eventos.evento;
            //}




            //Verificar se existe manutenção			
            int exists = _eCConfigRepositories.eManutencaoRepository.Set.Count();
            if (exists == 0)
            {
                //Nao existe nenhuma manutencao. Inserir na bd
                //encriptar a data				
                var dataForDB = new ManutencaoSistema
                {
                    TextoManutencao = "Caro cliente, de acordo com os registos disponíveis, a última manutenção realizada pela a equipa da PI ao sistema e-Billing Suite instalado foi em " +
                                DateTime.Now.ToShortDateString() + "." +
                                "\n\r\n\r" +
                                "De forma a garantir que o produto funcione nas suas capacidades máximas, é recomendado que entre em contacto com a nossa equipa de forma a agendar uma " +
                                "manutenção geral." +
                                "\n\r\n\r" +
                                "Pode entrar em contacto connosco usando:" +
                                "\n\r\n\r" +
                                "Telefone: +351 221452470" +
                                "\n\r\n\r" +
                                "email: pi@pi-co.com" +
                                "\n\r\n\r" +
                                "A não comunicação com a PI não implica o não funcionamento do sistema." +
                                "\n\r\n\r\n\r" +
                                "Com os melhores cumprimentos," +
                                "\n\r" +
                                "a equipa da PI - Portugal Informático.",
                    pkid = Guid.NewGuid(),
                    LastManDate = base64Encode(DateTime.Now.ToShortDateString())
                };

                _eCConfigRepositories
                    .eManutencaoRepository
                    .Add(dataForDB)
                    .Save();
            }
            else
            {
                DateTime lastdate = new DateTime();
                DateTime test = DateTime.Now;
                string lastdatetemp = "";
                //verifica se já passaram 1 mês desde a última manutenção
                var LastCheckUp = _eCConfigRepositories
                    .eManutencaoRepository
                    .Set
                    .OrderByDescending(ms => ms.LastManDate)
                    .FirstOrDefault();

                lastdatetemp = base64Decode(LastCheckUp.LastManDate);
                lastdate = DateTime.Now;
                //DateTime.Parse(lastdatetemp);
                alerta = LastCheckUp.TextoManutencao;

                if (lastdate.Date.AddMonths(1) > DateTime.Now)
                    alerta = "";
                else
                {
                    //update com a nova data.
                    LastCheckUp.TextoManutencao = "Caro cliente, de acordo com os registos disponíveis, a última manutenção realizada pela a equipa da PI ao sistema e-Billing Suite instalado foi em " +
                                DateTime.Now.ToShortDateString() + "." +
                                "\n\r\n\r" +
                                "De forma a garantir que o produto funcione nas suas capacidades máximas, é recomendado que entre em contacto com a nossa equipa de forma a agendar uma " +
                                "manutenção geral." +
                                "\n\r\n\r" +
                                "Pode entrar em contacto connosco usando:" +
                                "\n\r\n\r" +
                                "Telefone: +351 221452470" +
                                "\n\r\n\r" +
                                "email: pi@pi-co.com" +
                                "\n\r\n\r" +
                                "A não comunicação com a PI não implica o não funcionamento do sistema." +
                                "\n\r\n\r\n\r" +
                                "Com os melhores cumprimentos," +
                                "\n\r" +
                                "a equipa da PI - Portugal Informático.";
                    LastCheckUp.LastManDate = base64Encode(DateTime.Now.ToShortDateString());
                    _eCConfigRepositories
                        .eManutencaoRepository
                        .Edit(LastCheckUp)
                        .Save();
                }
            }
        }

        private List<string> GetFactElect(EnumGraphs enumGraphs, string factElectChoise)
        {
            List<string> listaToRetun = new List<string>();
            var total = _eCConfigRepositories.outboundInboundRepository.GetAllPackets(_context.UserIdentity);
            int count = 0; //contador para nao dar erros de codigo dahhhh
            foreach (string choise in factElectChoise.Split(';'))
            {
                string graph = string.Empty;
                switch (choise)
                {
                    case "0":
                        graph =
                            enumGraphs.GetGraph(0, factElectChoise.Split(';').Count(), count, new List<StatsValues>(){
                                new StatsValues
                                {
                                    ValueNormal = total.Count(v => v.Sentido.Equals("Out")),
                                    ValuePercent = Math.Round((double)(total.Count(v => v.Sentido.Equals("Out"))) / total.Count * 100, 2)
                                },
                                new StatsValues
                                {
                                    ValueNormal = total.Count(v => v.Sentido.Equals("In")),
                                    ValuePercent = Math.Round((double)(total.Count(v => v.Sentido.Equals("In"))) / total.Count * 100, 2)
                                }
                            });
                        break;
                    case "1":
                        var totalIntegrated = total.Where(v => v.Estado.Equals("4") || v.Estado.Equals("5")).ToList();
                        graph =
                            enumGraphs.GetGraph(1, factElectChoise.Split(';').Count(), count, new List<StatsValues>()
                            {
                                new StatsValues
                                {
                                    ValueNormal = totalIntegrated.Count(v => v.Estado.Equals("4")),
                                    ValuePercent = Math.Round((double)(totalIntegrated.Count(v => v.Estado.Equals("4"))) / totalIntegrated.Count * 100, 2)
                                },
                                new StatsValues
                                {
                                    ValueNormal = totalIntegrated.Count(v => v.Estado.Equals("5")),
                                    ValuePercent = Math.Round((double)(totalIntegrated.Count(v => v.Estado.Equals("5"))) / totalIntegrated.Count * 100, 2)
                                }
                            });
                        break;
                    case "2":
                        var totalDelivered = total.Where(v => v.Estado.Equals("1") || v.Estado.Equals("2")).ToList();
                        graph =
                            enumGraphs.GetGraph(2, factElectChoise.Split(';').Count(), count, new List<StatsValues>()
                            {
                                new StatsValues
                                {
                                    ValueNormal = totalDelivered.Count(v => v.Estado.Equals("1")),
                                    ValuePercent = Math.Round((double)(totalDelivered.Count(v => v.Estado.Equals("1"))) / totalDelivered.Count * 100, 2)
                                },
                                new StatsValues
                                {
                                    ValueNormal = totalDelivered.Count(v => v.Estado.Equals("2")),
                                    ValuePercent = Math.Round((double)(totalDelivered.Count(v => v.Estado.Equals("2"))) / totalDelivered.Count * 100, 2)
                                }
                            });
                        break;
                    case "3":
                        break;
                    default:
                        break;
                }

                listaToRetun.Add(graph);
                count++;
            }
            return listaToRetun;
        }

        public HomeVM(IECCListRepositories eCConfigRepositories, IPixelAdminPageContext pixelAdminPageContext, IGraphsRepository graphsRepository, IeBillingSuiteRequestContext context, eBC_Login user)
            : this(eCConfigRepositories, pixelAdminPageContext, graphsRepository, context)
        {
            this.user = user;
            this._eCConfigRepositories = eCConfigRepositories;
            this._pixelAdminPageContext = pixelAdminPageContext;
            this._context = context;

            //Verificar se existe manutenção			
            int exists = _eCConfigRepositories.eManutencaoRepository.Set.Count();
            if (exists == 0)
            {
                //Nao existe nenhuma manutencao. Inserir na bd
                //encriptar a data				
                var dataForDB = new ManutencaoSistema
                {
                    TextoManutencao = "Caro cliente, de acordo com os registos disponíveis, a última manutenção realizada pela a equipa da PI ao sistema e-Billing Suite instalado foi em " +
                                DateTime.Now.ToShortDateString() + "." +
                                "\n\r\n\r" +
                                "De forma a garantir que o produto funcione nas suas capacidades máximas, é recomendado que entre em contacto com a nossa equipa de forma a agendar uma " +
                                "manutenção geral." +
                                "\n\r\n\r" +
                                "Pode entrar em contacto connosco usando:" +
                                "\n\r\n\r" +
                                "Telefone: +351 221452470" +
                                "\n\r\n\r" +
                                "email: pi@pi-co.com" +
                                "\n\r\n\r" +
                                "A não comunicação com a PI não implica o não funcionamento do sistema." +
                                "\n\r\n\r\n\r" +
                                "Com os melhores cumprimentos," +
                                "\n\r" +
                                "a equipa da PI - Portugal Informático.",
                    pkid = Guid.NewGuid(),
                    LastManDate = base64Encode(DateTime.Now.ToShortDateString())
                };

                _eCConfigRepositories
                    .eManutencaoRepository
                    .Add(dataForDB)
                    .Save();
            }
            else
            {
                DateTime lastdate = new DateTime();
                string lastdatetemp = "";
                //verifica se já passaram 1 mês desde a última manutenção
                var LastCheckUp = _eCConfigRepositories
                    .eManutencaoRepository
                    .Set
                    .OrderByDescending(ms => ms.LastManDate)
                    .FirstOrDefault();

                lastdatetemp = base64Decode(LastCheckUp.LastManDate);
                lastdate = DateTime.Parse(lastdatetemp);
                alerta = LastCheckUp.TextoManutencao;

                if (lastdate.Date.AddMonths(1) > DateTime.Now)
                    alerta = "";
                else
                {
                    //update com a nova data.
                    LastCheckUp.TextoManutencao = "Caro cliente, de acordo com os registos disponíveis, a última manutenção realizada pela a equipa da PI ao sistema e-Billing Suite instalado foi em " +
                                DateTime.Now.ToShortDateString() + "." +
                                "\n\r\n\r" +
                                "De forma a garantir que o produto funcione nas suas capacidades máximas, é recomendado que entre em contacto com a nossa equipa de forma a agendar uma " +
                                "manutenção geral." +
                                "\n\r\n\r" +
                                "Pode entrar em contacto connosco usando:" +
                                "\n\r\n\r" +
                                "Telefone: +351 221452470" +
                                "\n\r\n\r" +
                                "email: pi@pi-co.com" +
                                "\n\r\n\r" +
                                "A não comunicação com a PI não implica o não funcionamento do sistema." +
                                "\n\r\n\r\n\r" +
                                "Com os melhores cumprimentos," +
                                "\n\r" +
                                "a equipa da PI - Portugal Informático.";
                    LastCheckUp.LastManDate = base64Encode(DateTime.Now.ToShortDateString());
                    _eCConfigRepositories
                        .eManutencaoRepository
                        .Edit(LastCheckUp)
                        .Save();
                }
            }
        }

        public HomeVM() { }
        public string base64Encode(string data)
        {
            try
            {
                byte[] encData_byte = new byte[data.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(data);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public string base64Decode(string data)
        {
            try
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();

                byte[] todecode_byte = Convert.FromBase64String(data);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //últimos eventos FE
        public List<DashboardEventData> feEvents { get; set; }


        //último eventos AT
        public List<DashboardEventData> atEvents { get; set; }

        //erro
        public string error { get; set; }

        public class DashboardSparkData
        {
            public string[] data { get; set; }
            public string processQuant { get; set; }

            public DashboardSparkData(string[] dt, string pq)
            {
                this.data = dt;
                this.processQuant = pq;
            }
        }

        public class DashboardChartData
        {
            public string label { get; set; }
            public double data { get; set; }

            //public DashboardChartData(string l, string d)
            //{
            //    this.label = l;
            //    this.data = d;
            //}
        }

        public class DashboardEventData
        {
            public string evento { get; set; }
            public string eventInfo { get; set; }
            public string eventDate { get; set; }
            public string eventState { get; set; }
            public int eventIdentifier { get; set; }
            public string eventDirection { get; set; }
            //public DashboardEventData(string e, string ed)
            //{
            //    this.evento = e;
            //    this.eventDate = ed;
            //}
        }

        public class DashboarCircularIndicatorData
        {
            public string percent { get; set; }
            public string data { get; set; }

            public DashboarCircularIndicatorData(string p, string d)
            {
                this.percent = p;
                this.data = d;
            }
        }
    }
}