using eBillingSuite.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eBillingSuite.Model.Desmaterializacao;

namespace eBillingSuite.Integrations
{
	public interface IAccountingData
    {
		void InsertInstanceEmptySequentialNumbers(int instanceCode, string connectionString);
    }
}
