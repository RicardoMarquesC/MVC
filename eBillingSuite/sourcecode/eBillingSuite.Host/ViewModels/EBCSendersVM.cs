using eBillingSuite.Enumerations;
using eBillingSuite.Globalization.Generators;
using eBillingSuite.Model;
using eBillingSuite.Model.CIC_DB;
using eBillingSuite.Model.EBC_DB;
using eBillingSuite.Models;
using eBillingSuite.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eBillingSuite.ViewModels
{
    public class EBCSendersVM
    {
        public EBCSendersVM(Whitelist model, string senderXmlType, IECCListRepositories _eBCConfigurationsRepository, bool isFromCreate)
        {
            senderXmlType = senderXmlType == null ? "" : senderXmlType;

            AvailableIntegrationMethods = _eBCConfigurationsRepository
                .eConnectorIntegrationFiltersRepository
                .Set
                .Select(v => new SelectListItem
                {
                    Text = v.FriendlyName,
                    Value = v.PKIntegrationFilterID.ToString(),
                    Selected = v.FriendlyName.ToLower() == IntegrationFiltersName.DEFAULT.ToLower()
                })
                .ToList();

            AvailableMarkets = _eBCConfigurationsRepository
                .eBCMarketsRepository
                .Set
                .Select(v => new SelectListItem
                {
                    Text = v.Mercado,
                    Value = v.Mercado,
                    Selected = v.Mercado.ToLower() == model.Mercado.ToLower()
                })
                .ToList();

            //Get the NomenclaturaPDF Types
            AvailableNomenclaturaPDFTypes = _eBCConfigurationsRepository
                .eConnectorTipoNomenclaturaPDFRepository
                .Set
                .Select(v => new SelectListItem
                {
                    Text = v.tiponomenclatura,
                    Value = v.pkid.ToString(),
                    Selected = v.tiponomenclatura.ToLower() == model.Mercado.ToLower()
                })
                .ToList();

            // get available XML Types
            var temp = _eBCConfigurationsRepository
                .eConnectorXmlTemplateRepository
                .GetExistingXmlTypes();

            List<string> xmlList = new List<string>();
            xmlList.Add("");
            foreach (IGrouping<string, xmlTemplate> item in temp)
                xmlList.Add(item.Key);

            AvailableXmlTypes = xmlList
                .Select(a => new SelectListItem
                {
                    Text = a,
                    Value = a,
                    Selected = a.ToLower() == senderXmlType.ToLower()
                })
                .ToList();


            this.Pkid = model.PKWhitelistID;
            this.EmailName = model.EmailName;
            this.EmailAddress = model.EmailAddress;
            this.Nif = model.NIF;
            this.Enabled = model.Enabled;
            this.ConcatAnexos = model.ConcatAnexos;
            this.XMLAss = model.XMLAss.Value;
            this.XMLNAss = model.XMLNAss.Value;
            this.PDFAss = model.PDFAss.Value;
            this.PDFNAss = model.PDFNAss.Value;
            this.PdfLink = model.PdfLink.Value;
            this.PdfLinkBaseURL = model.PdfLinkBaseURL;
            this.UsesPluginSystem = model.UsesPluginSystem.Value;

            //  if (String.IsNullOrWhiteSpace(this.Nif))
            //	this.IsFromCreate = true;
            //else
            //	this.IsFromCreate = false;
            this.IsFromCreate = isFromCreate;

            this.ReplyToAddress = model.ReplyToAddress;
        }

        public EBCSendersVM(Whitelist model, string senderXmlType, IECCListRepositories _eBCConfigurationsRepository, Guid tipoNomenclaturaSender, bool isNomenclaturaPDF, string counter, bool isFromCreate)
        {
            senderXmlType = senderXmlType == null ? "" : senderXmlType;

            AvailableIntegrationMethods = _eBCConfigurationsRepository
                .eConnectorIntegrationFiltersRepository
                .Set
                .Select(v => new SelectListItem
                {
                    Text = v.FriendlyName,
                    Value = v.PKIntegrationFilterID.ToString(),
                    Selected = v.FriendlyName.ToLower() == IntegrationFiltersName.DEFAULT.ToLower()
                })
                .ToList();

            AvailableMarkets = _eBCConfigurationsRepository
                .eBCMarketsRepository
                .Set
                .Select(v => new SelectListItem
                {
                    Text = v.Mercado,
                    Value = v.Mercado,
                    Selected = v.Mercado.ToLower() == model.Mercado.ToLower()
                })
                .ToList();

            //Get the NomenclaturaPDF Types
            AvailableNomenclaturaPDFTypes = _eBCConfigurationsRepository
                .eConnectorTipoNomenclaturaPDFRepository
                .Set
                .Select(v => new SelectListItem
                {
                    Text = v.tiponomenclatura,
                    Value = v.pkid.ToString(),
                    Selected = (tipoNomenclaturaSender == Guid.Empty ? false : v.pkid.ToString() == tipoNomenclaturaSender.ToString())
                })
                .ToList();

            // get available XML Types
            var temp = _eBCConfigurationsRepository
                .eConnectorXmlTemplateRepository
                .GetExistingXmlTypes();

            List<string> xmlList = new List<string>();
            xmlList.Add("");
            foreach (IGrouping<string, xmlTemplate> item in temp)
                xmlList.Add(item.Key);

            AvailableXmlTypes = xmlList
                .Select(a => new SelectListItem
                {
                    Text = a,
                    Value = a,
                    Selected = a.ToLower() == senderXmlType.ToLower()
                })
                .ToList();


            this.Pkid = model.PKWhitelistID;
            this.EmailName = model.EmailName;
            this.EmailAddress = model.EmailAddress;
            this.Nif = model.NIF;
            this.Enabled = model.Enabled;
            this.ConcatAnexos = model.ConcatAnexos;
            this.XMLAss = model.XMLAss.Value;
            this.XMLNAss = model.XMLNAss.Value;
            this.PDFAss = model.PDFAss.Value;
            this.PDFNAss = model.PDFNAss.Value;
            this.PdfLink = model.PdfLink.Value;
            this.PdfLinkBaseURL = model.PdfLinkBaseURL;
            this.DoYouWantForwardEmail = model.DoYouWantForwardEmail;
            this.DoYouWantForwardFTP = model.DoYouWantForwardFTP;
            this.ftpServer = model.ftpServer;
            this.username = model.username;
            this.password = model.password;
            this.port = model.port;
            this.listEmails = model.listEmails;
            this.UsesPluginSystem = model.UsesPluginSystem.Value;

            //  if (String.IsNullOrWhiteSpace(this.Nif))
            //	this.IsFromCreate = true;
            //else
            //	this.IsFromCreate = false;
            this.IsFromCreate = isFromCreate;

            this.isNomenclaturaPDF = isNomenclaturaPDF;
            this.counterValue = counter;

            this.ReplyToAddress = model.ReplyToAddress;
        }

        #region Properties
        public Guid Pkid { get; set; }

        public string EmailName { get; set; }
        public string EmailAddress { get; set; }
        public string Nif { get; set; }

        public IEnumerable<SelectListItem> AvailableIntegrationMethods { get; private set; }
        public Guid FkIntegrationMethod { get; set; }

        public bool Enabled { get; set; }
        public bool ConcatAnexos { get; set; }

        public IEnumerable<SelectListItem> AvailableMarkets { get; private set; }
        public string Mercado { get; set; }

        public IEnumerable<SelectListItem> AvailableXmlTypes { get; private set; }
        public string XmlType { get; set; }

        public IEnumerable<SelectListItem> AvailableNomenclaturaPDFTypes { get; private set; }
        public string NomenclaturaPDFType { get; set; }

        public bool XMLAss { get; set; }
        public bool XMLNAss { get; set; }
        public bool PDFAss { get; set; }
        public bool PDFNAss { get; set; }
        public bool UsesPluginSystem { get; set; }
        public bool PdfLink { get; set; }
        public string PdfLinkBaseURL { get; set; }
        public bool IsFromCreate { get; set; }
        public bool DoYouWantForward { get; set; }
        public bool? DoYouWantForwardEmail { get; set; }
        public bool? DoYouWantForwardFTP { get; set; }
        public bool isNomenclaturaPDF { get; set; }
        public string counterValue { get; set; }
        public string ReplyToAddress { get; set; }

        #region FTP
        public string username { get; set; }
        public string ftpServer { get; set; }
        public string password { get; set; }
        public string port { get; set; }
        #endregion

        #region email
        public string listEmails { get; set; }

        #endregion
        #endregion
    }
}