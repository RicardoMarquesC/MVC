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
	public class EConnectorConfigInboundTXTRepository : GenericRepository<IeBillingSuiteEBCDBContext, EBC_XmlToTxtTransform>, IConnectorConfigInboundTXTRepository
	{
		[Inject]
		public EConnectorConfigInboundTXTRepository(IeBillingSuiteEBCDBContext context)
			: base(context)
		{
		}

		public List<EBC_XmlToTxtTransform> GetConfigTXTbyInstanceID(Guid id)
		{
			return this.
				Where(ect => ect.fkInstanceId == id)
				.OrderBy(e => e.tipo).ThenBy(e => e.posicaoTxt)
				.ToList();
		}

		int IConnectorConfigInboundTXTRepository.GetMaxPositionFromType(string nomecampo)
		{
			//string tipo = this.Where(ect => ect.InboundPacketPropertyName == nomecampo).FirstOrDefault().tipo;
			int posicao = this.Set.Max(ect => ect.posicaoTxt) + 1;
			return posicao;
		}
	}
}
