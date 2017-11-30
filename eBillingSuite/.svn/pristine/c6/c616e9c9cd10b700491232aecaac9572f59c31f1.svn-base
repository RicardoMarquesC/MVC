using eBillingSuite.Model;
using Ninject;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using eBillingSuite.Model.eBillingConfigurations;

namespace eBillingSuite.Repositories
{
	public class ProductsRepository : GenericRepository<IeBillingSuiteConfigurationsContext, Produtos>, IProductsRepository
	{
		[Inject]
		public ProductsRepository(IeBillingSuiteConfigurationsContext context)
			: base(context)
		{
		}

        public List<eSuite_Produtos> GetAllProducts()
        {
            List<eSuite_Produtos> aux = new List<eSuite_Produtos>();
            foreach(var p in Set.ToList())
            {
                aux.Add(new eSuite_Produtos
                {
                    produto = p.pkid,
                    utilizador = "tastas",
                    activo = true
                });
            }
            return aux;
        }

        public Guid GetProductsIDByName(string nameProduct)
        {
            return this
                .Where(p => p.nome == nameProduct)
                .Select(p => p.pkid).FirstOrDefault();
        }

        public string GetProductsByID(Guid? id)
		{
			return this
				.Where(p => p.pkid == id)
				.Select(p => p.nome).FirstOrDefault().ToString();
		}
	}
}
