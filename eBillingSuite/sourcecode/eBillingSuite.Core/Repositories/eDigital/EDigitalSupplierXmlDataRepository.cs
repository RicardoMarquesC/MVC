﻿using eBillingSuite.Model;
using Ninject;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using eBillingSuite.Enumerations;
using eBillingSuite.Model.Desmaterializacao;
using System.Web.Script.Serialization;
using eBillingSuite.HelperTools;

namespace eBillingSuite.Repositories
{
    public class EDigitalSupplierXmlDataRepository : GenericRepository<IeBillingSuiteDesmaterializacaoContext, DadosTemplate>, IEDigitalSupplierXmlDataRepository
    {
        [Inject]
        public EDigitalSupplierXmlDataRepository(IeBillingSuiteDesmaterializacaoContext context)
            : base(context)
        {
        }


        public List<DadosTemplateXmlDBTable> GetDeserializedFields(DadosTemplate dadosTemplate)
        {
            if (dadosTemplate != null)
            {
                string json = dadosTemplate.XmlFields;

                DadosTemplateXmlDBTable[] fields = new JavaScriptSerializer().Deserialize<DadosTemplateXmlDBTable[]>(json);

                return fields.ToList();
            }

            return (new List<DadosTemplateXmlDBTable>());
        }


        // Modificada 13-03-2015
        public void InsertXmlField(string xmlFieldName, string local, bool origin, int format, bool obrig,
            string extractionType, string formula, string expression, List<Guid> templateNameIds, bool isComboBox, string defaultValue)
        {
            string regex = this.Context.CamposXML.FirstOrDefault(c => c.Tipo.ToLower() == local.ToLower() && c.NomeCampo.ToLower() == xmlFieldName.ToLower()).Regex;
            foreach (Guid guid in templateNameIds)
            {
                //var lastField = this.Set.OrderByDescending(x => x.Posicao).FirstOrDefault(x => x.fkNomeTemplate == guid && x.Localizacao == local);
                var dadosTemplate = this.Set.FirstOrDefault(x => x.FKNomeTemplate == guid);
                var fields = GetDeserializedFields(dadosTemplate);
                var lastField = fields.OrderByDescending(x => x.Posicao).FirstOrDefault(x => x.Localizacao == local);

                //            this.Add(new DadosTemplate
                //{
                //	pkid = Guid.NewGuid(),
                //	fkNomeTemplate = guid,
                //	NomeCampo = xmlFieldName,
                //	Localizacao = local,
                //	Posicao = lastField != null ? lastField.Posicao + 1 : 1,
                //	Obrigatorio = obrig,
                //	Regex = regex,
                //	DeOrigem = origin,
                //	Formato = format,
                //	TipoExtraccao = extractionType,
                //	Formula = formula == null ? "" : formula,
                //	UserExpression = expression == null ? "" : expression,
                //	IsComboBox = isComboBox
                //});
                fields.Add(new DadosTemplateXmlDBTable
                {
                    Pkid = Guid.NewGuid(),
                    Fknometemplate = guid,
                    NomeCampo = xmlFieldName,
                    Localizacao = local,
                    Posicao = lastField != null ? lastField.Posicao + 1 : 1,
                    CasasDecimais = format,
                    Obrigatorio = obrig,
                    CampoXmlRegex = regex ?? "",
                    DeOrigem = origin,
                    TipoExtraccao = extractionType,
                    Formula = formula == null ? "" : formula,
                    Expressao = expression == null ? "" : expression,
                    LabelUI = "",
                    IsComboBox = isComboBox,
                    IsReadOnly = false,
                    PersistValueToNextDoc = false,
                    DefaultValue = defaultValue
                });

                string json = new JavaScriptSerializer().Serialize(fields);
                dadosTemplate.XmlFields = json;
                this.Edit(dadosTemplate);
            }
            this.Save();
        }


        public bool ExistsXmlField(string nomeCampo, string local, Guid tipoDocPkid, Guid supplierPkid)
        {
            var nomeTemplate = this.Context.NomeTemplate.FirstOrDefault(n => n.fkfornecedor == supplierPkid && n.fktipofact == tipoDocPkid);

            if (nomeTemplate == null)
                return true;

            //return this.Set
            //    .Any(t => t.fkNomeTemplate == nomeTemplate.pkid && t.NomeCampo.ToLower() == nomeCampo.ToLower() && t.Localizacao.ToLower() == local.ToLower());
            var dadosTemplate = this.Set.FirstOrDefault(x => x.FKNomeTemplate == nomeTemplate.pkid);
            if (dadosTemplate == null)
                return true;

            var fields = GetDeserializedFields(dadosTemplate);
            return fields.Any(t => t.NomeCampo.ToLower() == nomeCampo.ToLower() && t.Localizacao.ToLower() == local.ToLower());
        }

        public List<DadosTemplate> GetDadosTemplateFromDocumentType(Guid fkTipoFactura)
        {
            var dadosTemplate = (from dt in this.Context.DadosTemplate
                        from nt in this.Context.NomeTemplate
                        where dt.FKNomeTemplate == nt.pkid && nt.fktipofact == fkTipoFactura
                        select dt).ToList();

            return dadosTemplate;
        }
    }
}
