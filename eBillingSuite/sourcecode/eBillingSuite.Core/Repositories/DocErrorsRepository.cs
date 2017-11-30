﻿using eBillingSuite.Model.EBC_DB;
using eBillingSuite.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.Repositories
{
    public class DocsErrorsRepository : IDocsErrorsRepository
    {
        public DocsErrorsRepository()
        {
        }

        /// <summary>
        /// Obtém todos os pacotes In e Outbound
        /// </summary>
        /// <returns>Lista com os pacotes</returns>
        public List<DocumentosErros> GetAllErrorDocumentos(string instances)
        {
            List<DocumentosErros> docserrors = new List<DocumentosErros>();
            List<string> listInstances = instances.Split(';').ToList();

            try
            {
                using (EBC_DB entidadeEBC = new EBC_DB())
                {

                    if (instances != "*")
                    {
                        docserrors = entidadeEBC.DocumentosErros.Where(o => listInstances.Contains(o.NifRecetor) || listInstances.Contains(o.NifEmissor)).ToList();
                    }
                    else
                    {
                        docserrors = entidadeEBC.DocumentosErros.OrderByDescending(de => de.DataCriacao).ToList();
                    }


                   
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return docserrors;
        }


        public List<DocumentosErros> GetAllDocumentosErros(string pesquisa)
        {
            List<DocumentosErros> newlist;
            try
            {
                using (EBC_DB entidadeEBC = new EBC_DB())
                {
                    DateTime tmp;
                    if (DateTime.TryParse(pesquisa, out tmp))
                    {
                        tmp = tmp.Date;
                    }

                    var query = entidadeEBC.DocumentosErros.Where(gt =>
                        gt.NumDoc.ToLower().Contains(pesquisa.ToLower())
                        ||
                        gt.NifEmissor.ToLower().Contains(pesquisa.ToLower())
                        ||
                        gt.NifRecetor.Contains(pesquisa.ToLower())
                        ||
                        gt.TipoErro.ToLower().Contains(pesquisa.ToLower())
                        ||
                        gt.DetalheErro.ToLower().Contains(pesquisa.ToLower())
                        ||
                        gt.Ficheiro.ToLower().Contains(pesquisa.ToLower())
                        ||
                        (System.Data.Entity.DbFunctions.TruncateTime(gt.DataCriacao) == tmp));


                    newlist = query
                        .OrderByDescending(x => x.DataCriacao)
                        .ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return newlist;
        }
    }
}