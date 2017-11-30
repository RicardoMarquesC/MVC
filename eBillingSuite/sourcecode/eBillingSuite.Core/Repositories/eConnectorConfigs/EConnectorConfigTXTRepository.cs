using eBillingSuite.Model;
using Ninject;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Security.Cryptography.X509Certificates;
using eBillingSuite.Model.EBC_DB;

namespace eBillingSuite.Repositories
{
	public class EConnectorConfigTXTRepository : GenericRepository<IeBillingSuiteEBCDBContext, EBC_ConfigTXT>, IConnectorConfigTXTRepository
	{
		[Inject]
		public EConnectorConfigTXTRepository(IeBillingSuiteEBCDBContext context)
			: base(context)
		{
		}
		
		public List<EBC_ConfigTXT> GetConfigTXTbyInstanceID(Guid id)
		{
			return this.
				Where(ect => ect.FKInstanceID == id)
				.OrderBy(e => e.Tipo).ThenBy(e => e.Posicao)
				.ToList();
		}

		public string GetMaxPositionFromType(string nomecampo)
		{
			string tipo = this.Where(ect => ect.NomeCampo == nomecampo).FirstOrDefault().Tipo;
			string posicao = this.Where(ect => ect.Tipo == tipo).Max(ect => ect.Posicao);

			int value;
			bool result = int.TryParse(posicao,out value);
			value++;
			if (value < 10)
				posicao = "0" + value.ToString();
			else
				posicao = value.ToString();


			return posicao;
		}
	}
}
