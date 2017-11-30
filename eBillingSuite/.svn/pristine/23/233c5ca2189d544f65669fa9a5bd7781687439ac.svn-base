using eBillingSuite.Model;
using Ninject;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using eBillingSuite.Model.EBC_DB;

namespace eBillingSuite.Repositories
{
	public class ComATPacketsGuiasRepository : GenericRepository<IeBillingSuiteEBCDBContext, ComATPackets_Guias>, IComATPacketsGuiasRepository
	{
		[Inject]
		public ComATPacketsGuiasRepository(IeBillingSuiteEBCDBContext context)
			: base(context)
		{
		}

		public List<ComATPackets_Guias> GetComATPacketsGuias(string instances)
		{
            List<string> listInstances = instances.Split(';').ToList();

            if (instances == "*")
                return Set.ToList();

            return Where(o => listInstances.Contains(o.NIFEmissor)).ToList();

			//return Set.ToList();
		}

		//public List<ComATPackets_Guias> GetComATPacketsGuias(string pesquisa)
		//{
			
		//	List<ComATPackets_Guias> newlist = this.Where(gt => gt.ATDocCodeID.ToLower().Contains(pesquisa.ToLower())
		//		||
		//		gt.NumeroDocumento.ToLower().Contains(pesquisa.ToLower())
		//		||
		//		gt.LastSentDate.ToString().Contains(pesquisa.ToLower())
		//		||
		//		gt.EstadoAT.Contains(pesquisa.ToLower())
		//		||
		//		gt.ObsRetornoAT.ToLower().Contains(pesquisa.ToLower())).ToList();
			
		//	return newlist;
		//}

        public List<ComATPackets_Guias> GetFilteredComATPackets(string pesquisa, string dateRange, string state, string numDoc)
        {
            List<ComATPackets_Guias> newlist;
            if (!String.IsNullOrWhiteSpace(pesquisa))
                pesquisa = pesquisa.ToLower();

            try
            {
                using (EBC_DB entidadeEBC = new EBC_DB())
                {
                    DateTime tmp;
                    if (DateTime.TryParse(pesquisa, out tmp))
                    {
                        tmp = tmp.Date;
                    }

                    IQueryable<ComATPackets_Guias> query = entidadeEBC.ComATPackets_Guias;

                    char[] delimitador = { '-' };
                    if (!String.IsNullOrWhiteSpace(dateRange))
                    {
                        DateTime dI;
                        DateTime dF;
                        var dateI = dateRange.Split(delimitador)[0];
                        if (DateTime.TryParse(dateI, out dI))
                        {
                            dI = dI.Date;
                        }
                        var dateF = dateRange.Split(delimitador)[1];
                        if (DateTime.TryParse(dateF, out dF))
                        {
                            dF = dF.Date;
                        }

                        query = query.Where(gt => (System.Data.Entity.DbFunctions.TruncateTime(gt.LastSentDate) >= dI) && (System.Data.Entity.DbFunctions.TruncateTime(gt.LastSentDate) <= dF));
                    }

                    if (!String.IsNullOrWhiteSpace(state))
                    {
                        query = query.Where(gt => gt.EstadoAT == state);
                    }

                    if (!String.IsNullOrWhiteSpace(numDoc))
                    {
                        query = query.Where(gt => gt.NumeroDocumento == numDoc);
                    }


                    if (!String.IsNullOrWhiteSpace(pesquisa))
                    {
                        query = query.Where(gt =>
                        gt.NumeroDocumento.ToLower().Contains(pesquisa)
                        ||
                        (gt.ATDocCodeID.ToLower().Contains(pesquisa) || (pesquisa == "n/a" && (gt.ATDocCodeID == null || gt.ATDocCodeID.ToLower() == "null")))
                        ||
                        gt.EstadoAT.Contains(pesquisa)
                        ||
                        gt.ObsRetornoAT.ToLower().Contains(pesquisa)
                        ||
                        (System.Data.Entity.DbFunctions.TruncateTime(gt.LastSentDate) == tmp));
                    }

                    newlist = query
                        .OrderByDescending(x => x.LastSentDate)
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
