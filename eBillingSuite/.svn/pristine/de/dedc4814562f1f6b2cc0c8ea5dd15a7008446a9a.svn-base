using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shortcut.Repositories;
using eBillingSuite.Model;
using eBillingSuite.Security;
using System.Security.Cryptography.X509Certificates;
using Ninject;
using eBillingSuite.Model.EBC_DB;

namespace eBillingSuite.Repositories
{
	public class EConnectorXmlTemplateRepository : GenericRepository<IeBillingSuiteEBCDBContext, xmlTemplate>, IEConnectorXmlTemplateRepository
	{
		[Inject]
		public EConnectorXmlTemplateRepository(IeBillingSuiteEBCDBContext context)
			: base(context)
		{
		}

		public List<xmlTemplate> GetDataByType(string type)
		{
			return this
				.Where(xt => xt.TipoXML == type
				&&
				xt.isATfield == true)
				.ToList();
		}

		public List<xmlTemplate> GetAllXmlDataByType(string type)
		{
			return this
				.Where(xt => xt.TipoXML == type)
				.OrderBy(xt => xt.Tipo)
				.ThenBy(xt => xt.NomeCampo)
				.ToList();
		}

		public int GetCustomXmlCount()
		{
			try
			{
				var list = this.Set
					.Where(xt => xt.TipoXML.ToLower().StartsWith("custom"))
					.GroupBy(xt => xt.TipoXML)
					.ToList();

				if (list.Any())
				{
					return list.Count();
				}
				else
				{
					return 0;
				}
			}
			catch (Exception e)
			{
				throw;
			}
		}


		public void InsertXmlField(string xmlPath, string fieldName, string docArea, string xmlTypeName, bool isATField, int customCount)
		{
			var newDB = new xmlTemplate
			{
				pkid = Guid.NewGuid(),
				CaminhoXML = xmlPath,
				NomeCampo = fieldName,
				Tipo = docArea,
				TipoXML = xmlTypeName,
				isATfield = isATField
			};

			this.Add(newDB).Save();
		}

		List<IGrouping<string, xmlTemplate>> IEConnectorXmlTemplateRepository.GetExistingXmlTypes()
		{
			return this.Set
				.OrderBy(xt => xt.TipoXML)
				.GroupBy(xt => xt.TipoXML)
				.ToList();
		}

		public List<string> GetExistingXmlFields(IGrouping<string, xmlTemplate> grouping)
		{
			return this.Where(xt => xt.TipoXML == grouping.Key
									&&
									xt.Tipo == "Cabecalho"
			)
			.Select(v => v.NomeCampo).ToList(); 
		}
	}
}
