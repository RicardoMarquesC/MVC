using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shortcut.Repositories;
using eBillingSuite.Model;
using eBillingSuite.Security;
using eBillingSuite.Model.Desmaterializacao;

namespace eBillingSuite.Repositories
{
	public interface IEDigitalSupplierXmlDataRepository : IGenericRepository<IeBillingSuiteDesmaterializacaoContext, DadosTemplate>
	{
        List<HelperTools.DadosTemplateXmlDBTable> GetDeserializedFields(DadosTemplate dadosTemplate);

        void InsertXmlField(string xmlFieldName, string local, bool origin, int format, bool obrig, string extractionType, string formula, string expression, List<Guid> templateNameIds, bool isComboBox, string defaultValue);

		bool ExistsXmlField(string nomeCampo, string local, Guid tipoDocPkid, Guid supplierPkid);

        List<DadosTemplate> GetDadosTemplateFromDocumentType(Guid fkTipoFactura);
    }
}
