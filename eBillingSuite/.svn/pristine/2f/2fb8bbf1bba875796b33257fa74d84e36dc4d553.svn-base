using eBillingSuite.Model;
using Ninject;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using eBillingSuite.Model.Desmaterializacao;

namespace eBillingSuite.Repositories
{
    public class EDigitalDocTypeXmlDataRepository : GenericRepository<IeBillingSuiteDesmaterializacaoContext, TipoFacturaDadosXML>, IEDigitalDocTypeXmlDataRepository
    {
        [Inject]
        public EDigitalDocTypeXmlDataRepository(IeBillingSuiteDesmaterializacaoContext context)
            : base(context)
        {
        }

        public List<TipoFacturaDadosXML> GetDocumentTemplateStructure(Guid fkDocumentType)
        {
            return this
                .Where(t => t.fkTipoFactura == fkDocumentType)
                .OrderBy(t => t.Posicao)
                .ToList();
        }

        public void InsertXmlField(Guid fkTipoDoc, string xmlFieldName, string local, int pos, bool obrig, string templateName, int format,
            string extrationType, string formula, bool isComboBox, bool isReadOnly, string labelUI, bool persistValue, string defaultVal,
            System.Data.Common.DbTransaction dbContextTransaction)
        {
            try
            {
                this.Add(new TipoFacturaDadosXML
                {
                    pkid = Guid.NewGuid(),
                    fkTipoFactura = fkTipoDoc,
                    NomeCampo = xmlFieldName,
                    Localizacao = local,
                    Posicao = pos,
                    Obrigatorio = obrig,
                    Regex = this.Context.CamposXML.FirstOrDefault(c => c.Tipo.ToLower() == local.ToLower() && c.NomeCampo.ToLower() == xmlFieldName.ToLower()).Regex,
                    NomeTemplate = templateName,
                    Formato = format,
                    TipoExtraccao = extrationType,
                    Formula = formula == null ? "" : formula,
                    IsComboBox = isComboBox,
                    IsReadOnly = isReadOnly,
                    LabelUI = labelUI,
                    PersistValueToNextDoc = persistValue,
                    DefaultValue = defaultVal
                }).Save();
            }
            catch (Exception e)
            {
                string msg = e.Message;

                throw;
            }
        }


        public bool ExistsXmlField(string nomeCampo, string local, Guid tipoDocPkid)
        {
            return this.Set
                .Any(t => t.fkTipoFactura == tipoDocPkid && t.NomeCampo.ToLower() == nomeCampo.ToLower() && t.Localizacao.ToLower() == local.ToLower());
        }
    }
}
