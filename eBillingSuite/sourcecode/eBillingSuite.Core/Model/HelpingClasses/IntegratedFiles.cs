﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.Model.HelpingClasses
{
    public class IntegratedFiles
    {
        public int ID { get; set; }
        public string NumDoc { get; set; }
        public string SubmissionFile { get; set; }
        public DateTime SubmissionDate { get; set; }

        public static List<IntegratedFiles> GetSubmissionFilesData(string instances)
        {
            List<IntegratedFiles> topcostumers = new List<IntegratedFiles>();
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

                //Buscar os dados
                var recetores = (from p in listaGlobal
                                 .Where(g => !(String.IsNullOrEmpty(g.SubmissionFile)))
                                 select new
                                 {
                                     SubmissionFile = p.SubmissionFile,
                                     SubmissionData = p.SubmissionDate,
                                     NumDoc = p.NumFactura
                                 });

                int counter = 0;
                //Para cada um dos dados, guardar no objeto.
                foreach (var item in recetores)
                {
                    IntegratedFiles ifiles = new IntegratedFiles();
                    ifiles.ID = counter;
                    ifiles.NumDoc = item.NumDoc;
                    ifiles.SubmissionFile = item.SubmissionFile;
                    ifiles.SubmissionDate = (DateTime)item.SubmissionData;
                    topcostumers.Add(ifiles);
                    counter++;
                }

                topcostumers = topcostumers.OrderByDescending(x => x.SubmissionDate).ToList();
            }

            return topcostumers;
        }
    }
}
