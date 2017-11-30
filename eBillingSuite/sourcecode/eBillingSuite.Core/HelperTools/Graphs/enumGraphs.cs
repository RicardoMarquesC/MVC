using eBillingSuite.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.HelperTools.Graphs
{
    public class EnumGraphs
    {
        public enum factElectGraphs
        {
            allPackets,
            Inbound,
            Outbound,
            docsErrors,
        }

        /// <summary>
        /// Metodo para ir buscar html gráficos através da bd e manda os dados
        /// </summary>
        /// <param name="choise"></param>
        /// <param name="valores">Colocar uma lista de valores normais e de percentagem</param>
        /// <returns></returns>
        public string GetGraph(int choise, int numberTotal, int count, List<StatsValues> valores, object undefined = null)
        {
            string graph = string.Empty;
            int sep = 12 / numberTotal;

            switch ((factElectGraphs)choise)
            {
                case factElectGraphs.allPackets:
                    graph = "<script>" +
                            "    init.push(function () {" +
                            "        Morris.Donut({" +
                           $"            element: 'hero-donut{count}'," +
                            "            data: [" +
                           $"                {{ label: 'Outbound', value: '{valores[0].ValuePercent}' }}," +
                           $"                {{ label: 'Inbound', value: '{valores[1].ValuePercent}' }}" +
                            "            ]," +
                            "            colors: PixelAdmin.settings.consts.COLORS," +
                            "            resize: true," +
                            "            labelColor: '#483D8B'," +
                            "            formatter: function (y) {" +
                            "                    return y + \"%\";" +
                            "            }" +
                            "        });" +
                            "    });" +
                            "</script>" +
                           $"<div class=\"col-sm-6 col-md-{sep}\">" +
                            "   <div>" +
                           $"       <div id=\"hero-donut{count}\" class=\"graph\" style=\"height: 250px; margin: auto;\"></div>" +
                            "   </div>" +
                            "   <div>" +
                            "       <div class=\"bordered\">" +
                           $"           <h4 class=\"padding-sm padding-xs-hr\"><i class=\"fa fa-envelope text-primary\" style=\"margin-right: 8px;\"></i>{Texts.Packets}</h4>" +
                            "           <ul class=\"list-group no-margin\">" +
                            "               <li class=\"list-group-item no-border-hr padding-xs-hr no-bg no-border-radius\">" +
                           $"                   {Texts.OutboundPackets}<span class=\"label label-dark-gray pull-right\" style=\"cursor:auto \">{valores[0].ValueNormal}</span>" +
                            "               </li>" +
                            "               <li class=\"list-group-item no-border-hr padding-xs-hr no-bg\">" +
                           $"                   {Texts.InboundPackets}<span class=\"label label-dark-gray pull-right\" style=\"cursor:auto\">{valores[1].ValueNormal}</span>" +
                            "               </li>" +
                            "           </ul>" +
                            "       </div>" +
                            "   </div>" +
                            "</div>\n";
                    break;
                case factElectGraphs.Inbound:
                    graph = " <script>" +
                            "    init.push(function () {" +
                            "        Morris.Donut({" +
                           $"            element: 'hero-donut{count}'," +
                            "            colors: [\"#FFD700\",\"#006400\"]," +
                            "            data: [" +
                           $"                {{ label: 'Integrated', value: '{valores[0].ValuePercent}' }}," +
                           $"                {{ label: 'Not Integrated', value: '{valores[1].ValuePercent}' }}" +
                            "            ]," +
                            "            resize: true," +
                            "            labelColor: '#A52A2A'," +
                            "            formatter: function (y) {" +
                            "                return y + \"%\";" +
                            "            }" +
                            "        });" +
                            "    });" +
                            "</script>" +
                           $"<div class=\"col-sm-6 col-md-{sep}\">" +
                            "   <div>" +
                           $"       <div id=\"hero-donut{count}\" class=\"graph\" style=\"height: 250px; margin: auto;\"></div>" +
                            "   </div>" +
                            "   <div>" +
                            "       <div class=\"bordered\">" +
                           $"           <h4 class=\"padding-sm padding-xs-hr\"><i class=\"fa fa-envelope text-primary\" style=\"margin-right: 8px;\"></i>{Texts.NewPackets}</h4>" +
                            "           <ul class=\"list-group no-margin\">" +
                            "               <li class=\"list-group-item no-border-hr padding-xs-hr no-bg no-border-radius\">" +
                           $"                  {Texts.IntegratedPackets}<span class=\"label label-dark-gray pull-right\" style=\"cursor:auto\">{valores[0].ValueNormal}</span>" +
                            "               </li>" +
                            "               <li class=\"list-group-item no-border-hr padding-xs-hr no-bg\">" +
                           $"                  {Texts.NIntegratedPackets}<span class=\"label label-pa-purple pull-right\" style=\"cursor:auto\">{valores[1].ValueNormal}</span>" +
                            "               </li>" +
                            "           </ul>" +
                            "       </div>" +
                            "   </div>" +
                            "</div>\n";
                    break;
                case factElectGraphs.Outbound:
                    graph = "<script>" +
                            "    init.push(function () {" +
                            "        Morris.Donut({" +
                           $"            element: 'hero-donut{count}'," +
                            "            data: [" +
                           $"                {{ label: 'Entregue', value: '{valores[0].ValuePercent}' }}," +
                           $"                {{ label: 'Em Espera', value: '{valores[1].ValuePercent}' }}" +
                            "            ]," +
                            "            resize: false," +
                            "            labelColor: '#006400'," +
                            "            formatter: function (y) {" +
                            "                return y + \"%\";" +
                            "            }" +
                            "        });" +
                            "    });" +
                            "</script>" +
                           $"<div class=\"col-sm-6 col-md-{sep}\">" +
                            "   <div>" +
                           $"       <div id=\"hero-donut{count}\" class=\"graph\" style=\"height: 250px; margin: auto;\"></div>" +
                            "   </div>" +
                            "   <div>" +
                            "       <div class=\"bordered\">" +
                           $"           <h4 class=\"padding-sm padding-xs-hr\"><i class=\"fa fa-envelope text-primary\" style=\"margin-right: 8px;\"></i>{Texts.Outbound}</h4>" +
                            "           <ul class=\"list-group no-margin\">" +
                            "               <li class=\"list-group-item no-border-hr padding-xs-hr no-bg no-border-radius\">" +
                           $"                  {Texts.OutboundSend}<span class=\"label label-dark-gray pull-right\" style=\"cursor:auto\">{valores[0].ValueNormal}</span>" +
                            "               </li>" +
                            "               <li class=\"list-group-item no-border-hr padding-xs-hr no-bg\">" +
                           $"                  {Texts.OutboundWait}<span class=\"label label-pa-purple pull-right\" style=\"cursor:auto\">{valores[1].ValueNormal}</span>" +
                            "               </li>" +
                            "           </ul>" +
                            "       </div>" +
                            "   </div>" +
                            "</div>";

                    break;
                case factElectGraphs.docsErrors:
                    string toJoin = string.Empty;
                    foreach (var ev in undefined as List<string>)
                    {
                        toJoin += "<div class=\"profile-activity clearfix border-t padding-sm\">" +
                                  "   <a class=\"badge badge-success\" style=\"cursor:auto\"><i class=\"fa fa-check\"></i></a>" +
                                  "   <label id=\"evtfedesc1_1\">@ev.evento</label> <b id=\"evtfedesc1_2\">@ev.eventInfo</b>" +
                                  "   <div class=\"time\">" +
                                  "       <i class=\"icon-time bigger-110\">@ev.eventDate</i>" +
                                  "       <label id=\"evtfetime1\"></label>" +
                                  "   </div>" +
                                  "   <a id=\"evtfetime1link\"></a>" +
                                  "</div>";
                    }
                    if (string.IsNullOrEmpty(toJoin))
                        toJoin = "Não existe documentos erros";

                    graph = "<div class=\"col-xs-12 col-lg-3\">" +
                            "   <div class=\"page-header padding-sm-hr\">" +
                            "       <h1 class=\"text-center text-left-sm\"><i class=\"fa fa-calendar\" style=\"margin-right:14px;\"></i>@Texts.Eventos</h1>" +
                            "       <i class=\"fa fa-chevron-down\" style=\"cursor:pointer;float: right;line-height: 30px;\" id=\"Arrow2\"></i>" +
                            "   </div>" +
                            "   <div class=\"row padding-sm-hr\" id=\"togglee2\">" +
                                    toJoin+
                            "   </div>" +
                            "</div>";
                    break;
                default:
                    break;
            }



            return graph;
        }

    }

    public class StatsValues
    {
        public double ValueNormal { get; set; }
        public double ValuePercent { get; set; }
    }
}
