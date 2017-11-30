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
    public interface IEDigitalDocTypeXmlDataRepository : IGenericRepository<IeBillingSuiteDesmaterializacaoContext, TipoFacturaDadosXML>
    {
        List<TipoFacturaDadosXML> GetDocumentTemplateStructure(Guid fkDocumentType);

        void InsertXmlField(Guid fkTipoDoc, string xmlFieldName, string local, int pos, bool obrig, string templateName, int format,
            string extrationType, string formula, bool isComboBox, bool isReadOnly, string labelUI, bool persistValue, string defaultVal,
            System.Data.Common.DbTransaction dbContextTransaction);

        bool ExistsXmlField(string nomeCampo, string local, Guid tipoDocPkid);
    }
}
