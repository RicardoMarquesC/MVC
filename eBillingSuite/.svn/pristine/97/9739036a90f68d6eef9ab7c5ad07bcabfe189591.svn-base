using eBillingSuite.Enumerations;
using eBillingSuite.HelperTools;
using eBillingSuite.Model;
using eBillingSuite.Model.Desmaterializacao;
using eBillingSuite.Models;
using eBillingSuite.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eBillingSuite.ViewModels
{
    public class EDigitalXmlManagmentVM
    {
        public EDigitalXmlManagmentVM(List<Fornecedores> fornecedores, List<TipoFacturas> tipoFacturas, Guid selectedDocType, Guid selectedSupplier,
            IEDigitalSupplierXmlDataRepository eDigitalSupplierXmlDataRepository, IEDigitalTemplateNameRepository eDigitalTemplateNameRepository)
        {
            // fornecedores
            FkSupplier = selectedSupplier;

            fornecedores.Insert(0, new Fornecedores { pkid = Guid.Empty, Contribuinte = "" });
            AvailableSuppliers = fornecedores
                .Select(v => new SelectListItem
                {
                    Text = v.Nome + "-" + v.Contribuinte,
                    Value = v.pkid.ToString(),
                    Selected = v.pkid == selectedSupplier
                })
                .OrderBy(x => x.Text)
                .ToList();

            // tipo facturas
            FkDocumentType = selectedDocType;

            tipoFacturas.Insert(0, new TipoFacturas { pkid = Guid.Empty, nome = "" });
            AvailableDocTypes = tipoFacturas
                .Select(v => new SelectListItem
                {
                    Text = v.nome,
                    Value = v.pkid.ToString(),
                    Selected = v.pkid == selectedDocType
                })
                .OrderBy(x => x.Text)
                .ToList();

            // dados do xml
            //DadosXmlCabecalho = new List<DadosTemplate>();
            //DadosXmlLinhas = new List<DadosTemplate>();
            //DadosXmlIva = new List<DadosTemplate>();
            DadosXmlCabecalho = new List<DadosTemplateXmlDBTable>();
            DadosXmlLinhas = new List<DadosTemplateXmlDBTable>();
            DadosXmlIva = new List<DadosTemplateXmlDBTable>();
            if (selectedDocType != Guid.Empty && selectedSupplier != Guid.Empty)
            {
                // get NomeTemplate PKID
                var templatePkid = eDigitalTemplateNameRepository.Set
                    .Where(tn => tn.fkfornecedor == selectedSupplier && tn.fktipofact == selectedDocType)
                    .Select(tn => tn.pkid)
                    .FirstOrDefault();

                var dadosTemplate = eDigitalSupplierXmlDataRepository.Set.FirstOrDefault(x => x.FKNomeTemplate == templatePkid);
                List<DadosTemplateXmlDBTable> xmlFields = eDigitalSupplierXmlDataRepository.GetDeserializedFields(dadosTemplate);

                //           DadosXmlCabecalho = eDigitalSupplierXmlDataRepository
                //.Where(d => d.fkNomeTemplate == templatePkid && d.Localizacao.ToLower() == DigitalDocumentAreas.HEADER)
                //.OrderByDescending(d => d.Posicao)
                //.ToList();
                DadosXmlCabecalho = xmlFields
                    .Where(d => d.Localizacao.ToLower() == DigitalDocumentAreas.HEADER)
                    .OrderByDescending(d => d.Posicao)
                    .ToList();

                //           DadosXmlLinhas = eDigitalSupplierXmlDataRepository
                //.Where(d => d.fkNomeTemplate == templatePkid && d.Localizacao.ToLower() == DigitalDocumentAreas.LINES)
                //.OrderByDescending(d => d.Posicao)
                //.ToList();
                DadosXmlLinhas = xmlFields
                    .Where(d => d.Localizacao.ToLower() == DigitalDocumentAreas.LINES)
                    .OrderByDescending(d => d.Posicao)
                    .ToList();

                //           DadosXmlIva = eDigitalSupplierXmlDataRepository
                //.Where(d => d.fkNomeTemplate == templatePkid && d.Localizacao.ToLower() == DigitalDocumentAreas.VAT)
                //.OrderByDescending(d => d.Posicao)
                //.ToList();
                DadosXmlIva = xmlFields
                    .Where(d => d.Localizacao.ToLower() == DigitalDocumentAreas.VAT)
                    .OrderByDescending(d => d.Posicao)
                    .ToList();
            }
        }

        public EDigitalXmlManagmentVM(DigitalTemplatesData data)
        {
            //this.FkDocumentType = data.FkDocumentType;

            //this.DadosXmlCabecalho = data.DadosXmlCabecalho;
            //this.DadosXmlLinhas = data.DadosXmlLinhas;
            //this.DadosXmlIva = data.DadosXmlIva;
        }

        public IEnumerable<SelectListItem> AvailableSuppliers { get; set; }
        public System.Guid FkSupplier { get; set; }

        public IEnumerable<SelectListItem> AvailableDocTypes { get; set; }
        public System.Guid FkDocumentType { get; set; }

        //public List<DadosTemplate> DadosXmlCabecalho { get; set; }
        //public List<DadosTemplate> DadosXmlLinhas { get; set; }
        //public List<DadosTemplate> DadosXmlIva { get; set; }
        public List<DadosTemplateXmlDBTable> DadosXmlCabecalho { get; set; }
        public List<DadosTemplateXmlDBTable> DadosXmlLinhas { get; set; }
        public List<DadosTemplateXmlDBTable> DadosXmlIva { get; set; }
    }
}