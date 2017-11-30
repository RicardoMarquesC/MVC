using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shortcut.Repositories;
using eBillingSuite.Model;
using eBillingSuite.Security;
using eBillingSuite.Model.eBillingConfigurations;

namespace eBillingSuite.Repositories
{
	public interface IProductsRepository : IGenericRepository<IeBillingSuiteConfigurationsContext, Produtos>
	{
		string GetProductsByID(Guid? id);
        Guid GetProductsIDByName(string nameProduct);
        List<eSuite_Produtos> GetAllProducts();

    }
}
