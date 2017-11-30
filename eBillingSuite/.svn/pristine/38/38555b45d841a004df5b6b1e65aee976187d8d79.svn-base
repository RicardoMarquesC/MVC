using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shortcut.Repositories;
using eBillingSuite.Model;
using eBillingSuite.Security;
using eBillingSuite.Model.EBC_DB;

namespace eBillingSuite.Repositories
{
    //public interface IComATPacketsRepository : IGenericRepository<IeBillingSuiteEBCDBContext, ComATPackets>
    //{
    public interface IComATPacketsRepository 
    {
        //List<ComATPackets> GetComATPackets();

		List<ComATPackets> GetComATPackets(string instancias);

        List<ComATPackets> GetFilteredComATPackets(string pesquisa, string dateRange, string state, string numDoc, string nifRecetor);

        int Count();
	}
}
