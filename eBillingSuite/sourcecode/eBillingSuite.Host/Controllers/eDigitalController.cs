﻿using eBillingSuite.Security;
using eBillingSuite.ViewModels;
using System.Web.Mvc;

using eBillingSuite.Globalization;
using eBillingSuite.Models;
using eBillingSuite.Repositories;
using MvcFlash.Core;
using MvcFlash.Core.Extensions;
using Shortcut.PixelAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using eBillingSuite.Integrations;
using System.IO;
using eBillingSuite.Enumerations;
using eBillingSuite.HelperTools;
using eBillingSuite.Support;
using System.Data;
using eBillingSuite.Model.Desmaterializacao;
using eBillingSuite.Model.eBillingConfigurations;
using System.Web.Script.Serialization;
using eBillingSuite.Repositories.Interfaces.eDigital;
using eBillingSuite.Resources;

namespace eBillingSuite.Controllers
{
    public class eDigitalController : Controller
    {
        private IPixelAdminPageContext _pixelAdminPageContext;
        private IeBillingSuiteRequestContext _context;
        private IEDigitalSuppliersRepository _eDigitalSuppliersRepository;
        private ISupplierSyncronization _supplierSyncronization;
        private IEDigitalDocTypeRepository _eDigitalDocTypeRepository;
        private IEDigitalDocTypeXmlDataRepository _eDigitalDocTypeXmlDataRepository;
        private IEDigitalTemplateNameRepository _eDigitalTemplateNameRepository;
        private IEDigitalSupplierXmlDataRepository _eDigitalSupplierXmlDataRepository;
        private IEDigitalDocExpirationRepository _eDigitalDocExpirationRepository;
        private IEDigitalDocHistoryRepository _eDigitalDocHistoryRepository;
        private IEDigitalXmlFieldsRepository _eDigitalXmlFieldsRepository;
        private IEDigitalMasterizationHeaderRepository _eDigitalMasterizationHeaderRepository;
        private IEDigitalMasterizationLinesRepository _eDigitalMasterizationLinesRepository;
        private IEDigitalMasterizationVatRepository _eDigitalMasterizationVatRepository;
        private IEDigitalIntancesRepository _eDigitalInstancesRepository;
        private IEDigitalMasterizationProcRepository _eDigitalMasterizationProcRepository;
        private IDigitalConfigurationsRepository _eDigitalConfigurationsRepository;
        private IAccountingData _accountingData;
        private IEDigitalInstancesMailRepository _eDigitalInstancesMailRepository;

        public eDigitalController(IPixelAdminPageContext pixelAdminPageContext,
            IeBillingSuiteRequestContext context,
            IEDigitalSuppliersRepository eDigitalSuppliersRepository,
            ISupplierSyncronization supplierSyncronization,
            IEDigitalDocTypeRepository eDigitalDocTypeRepository,
            IEDigitalDocTypeXmlDataRepository eDigitalDocTypeXmlDataRepository,
            IEDigitalTemplateNameRepository eDigitalTemplateNameRepository,
            IEDigitalSupplierXmlDataRepository eDigitalSupplierXmlDataRepository,
            IEDigitalDocExpirationRepository eDigitalDocExpirationRepository,
            IEDigitalDocHistoryRepository eDigitalDocHistoryRepository,
            IEDigitalXmlFieldsRepository eDigitalXmlFieldsRepository,
            IEDigitalMasterizationHeaderRepository eDigitalMasterizationHeaderRepository,
            IEDigitalMasterizationLinesRepository eDigitalMasterizationLinesRepository,
            IEDigitalIntancesRepository eDigitalInstancesRepository,
            IEDigitalMasterizationProcRepository eDigitalMasterizationProcRepository,
            IEDigitalMasterizationVatRepository eDigitalMasterizationVatRepository,
            IDigitalConfigurationsRepository eDigitalConfigurationsRepository,
            IAccountingData accountingData,

            IEDigitalInstancesMailRepository eDigitalInstancesMailRepository)
        {
            _pixelAdminPageContext = pixelAdminPageContext;
            _context = context;
            _eDigitalSuppliersRepository = eDigitalSuppliersRepository;
            _eDigitalConfigurationsRepository = eDigitalConfigurationsRepository;
            _supplierSyncronization = supplierSyncronization;
            _eDigitalDocTypeRepository = eDigitalDocTypeRepository;
            _eDigitalDocTypeXmlDataRepository = eDigitalDocTypeXmlDataRepository;
            _eDigitalTemplateNameRepository = eDigitalTemplateNameRepository;
            _eDigitalSupplierXmlDataRepository = eDigitalSupplierXmlDataRepository;
            _eDigitalDocExpirationRepository = eDigitalDocExpirationRepository;
            _eDigitalDocHistoryRepository = eDigitalDocHistoryRepository;
            _eDigitalXmlFieldsRepository = eDigitalXmlFieldsRepository;
            _eDigitalMasterizationHeaderRepository = eDigitalMasterizationHeaderRepository;
            _eDigitalMasterizationLinesRepository = eDigitalMasterizationLinesRepository;
            _eDigitalMasterizationVatRepository = eDigitalMasterizationVatRepository;
            _eDigitalInstancesRepository = eDigitalInstancesRepository;
            _eDigitalMasterizationProcRepository = eDigitalMasterizationProcRepository;
            _accountingData = accountingData;
            _eDigitalInstancesMailRepository = eDigitalInstancesMailRepository;
        }

        #region Fornecedores
        //[PersonAuthorize(Permissions.EDIGITAL_FORNECEDORES)]
        //public ActionResult Index()
        //{
        //	this.SetPixelAdminPageContext(_pixelAdminPageContext);

        //	if (!_context.UserIdentity.IsEBDActive)
        //		return View("InactiveModule");

        //	var model = _eDigitalSuppliersRepository
        //		.Set
        //		.OrderBy(x => x.Nome)
        //		.ToList();

        //	if (Request.IsAjaxRequest())
        //		return Json(this.PanelContentReply(model), JsonRequestBehavior.AllowGet);
        //	else
        //		return View(model);
        //}
        [PersonAuthorize(Permissions.EDIGITAL_FORNECEDORES)]
        public ActionResult Index(JQueryDataTableParamModel param)
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            if (!_context.UserIdentity.IsEBDActive)
                return View("InactiveModule");

            if (!Request.IsAjaxRequest())
            {
                return View("Index");
            }

            var model = _eDigitalSuppliersRepository
                .Set
                .OrderBy(x => x.Nome)
                .ToList();

            //para a ordenação
            var isNomeSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isNIFSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isValidacoesSortable = Convert.ToBoolean(Request["bSortable_3"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);

            Func<Fornecedores, string> orderingFunction = (c => sortColumnIndex == 0 && isNomeSortable ? c.Nome :
                sortColumnIndex == 1 && isNIFSortable ? c.Contribuinte.ToString() :
                sortColumnIndex == 2 && isValidacoesSortable ? c.WantMainValidations.ToString() :
                                                           "");

            var orderedQuery = Request["sSortDir_0"] == "asc" ?
                model.OrderBy(orderingFunction) :
                model.OrderByDescending(orderingFunction);

            if (param.iDisplayLength != 0)
            {
                var recordCount = model.Count();
                var records = orderedQuery
                    .Skip(param.iDisplayStart)
                    .Take(param.iDisplayLength)
                    .ToList();

                var reply = new JsonNetResult();
                reply.Data = new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = recordCount,
                    iTotalDisplayRecords = param.iDisplayLength,
                    aaData = records
                };

                return reply;
            }
            else
            {
                var records = orderedQuery.Take(10).ToList();
                var reply = new JsonNetResult();
                reply.Data = new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = model.Count(),
                    iTotalDisplayRecords = records.Count,
                    aaData = orderedQuery
                };

                return reply;
            }

            //if (Request.IsAjaxRequest())
            //	return Json(this.PanelContentReply(model), JsonRequestBehavior.AllowGet);
            //else
            //	return View(model);
        }

        [PersonAuthorize(Permissions.EDIGITAL_FORNECEDORES)]
        public ActionResult EditSupplier(Guid pkid)
        {
            var model = _eDigitalSuppliersRepository.Find(pkid);

            //ViewBag.SupplierSync = _eDigitalConfigurationsRepository.GetConfigurationByKey("SincFornecedoresWS") != null ?
            //    ((bool)_eDigitalConfigurationsRepository.GetConfigurationByKey("SincFornecedoresWS") ? "true" : "false") : "false";

            string wantSupplierSync = String.IsNullOrEmpty(_eDigitalConfigurationsRepository.GetConfigurationByKey("SincFornecedoresWS").ToString())
                                      ? "false" : "true";

            ViewBag.SupplierSync = wantSupplierSync;

            if (Request.IsAjaxRequest())
                return Json(this.ModalContentReply(model), JsonRequestBehavior.AllowGet);
            else
                return PartialView(model);
        }

        [HttpPost]
        //[PersonAuthorize(Permissions.EDIGITAL_FORNECEDORES)]
        public ActionResult EditSupplier(Guid pkid, DigitalSupplierData data, bool returnInsertedId = false)
        {
            var dataFromDB = _eDigitalSuppliersRepository.Find(pkid);

            if (!ModelState.IsValid)
            {
                Flash.Instance.Error(Texts.FixModelErrors);


                string wantSupplierSync = String.IsNullOrEmpty(_eDigitalConfigurationsRepository.GetConfigurationByKey("SincFornecedoresWS").ToString())
                                      ? "false" : "true";

                ViewBag.SupplierSync = wantSupplierSync;

                // Used by the project with AEP, readyWS calls this method
                JsonReturnObject json = new JsonReturnObject();
                json.StatusCode = ResponseStatus.ERROR;
                if (returnInsertedId)
                {
                    var modelErrors = ModelState.Values
                        .SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                        .ToList();

                    if (modelErrors != null && modelErrors.Count > 0)
                        json.Message = modelErrors[0];
                    else
                        json.Message = "Not specified error with the model state.";

                    return Json(json);
                }

                if (Request.IsAjaxRequest())
                    return Json(this.ModalContentReply(dataFromDB));
                else
                    return PartialView(dataFromDB);
            }

            using (var dbContextTransaction = _eDigitalSuppliersRepository.Context.Database.BeginTransaction())
            {
                try
                {
                    dataFromDB.Nome = data.Nome;
                    //dataFromDB.Contribuinte = data.Contribuinte;
                    dataFromDB.Morada = data.Morada;
                    dataFromDB.Telefone = data.Telefone;
                    dataFromDB.Fax = data.Fax;
                    dataFromDB.Email = data.Email;
                    dataFromDB.WebSite = data.WebSite;
                    dataFromDB.WantMainValidations = data.WantMainValidations;

                    _eDigitalSuppliersRepository
                        .Edit(dataFromDB)
                        .Save();

                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

                    dbContextTransaction.Rollback();
                    Flash.Instance.Error(Texts.DBErrors);

                    //ViewBag.SupplierSync = _eDigitalConfigurationsRepository.GetConfigurationByKey("SincFornecedoresWS") != null ?
                    //    ((bool)_eDigitalConfigurationsRepository.GetConfigurationByKey("SincFornecedoresWS") ? "true" : "false") : "false";

                    string wantSupplierSync = String.IsNullOrEmpty(_eDigitalConfigurationsRepository.GetConfigurationByKey("SincFornecedoresWS").ToString())
                                      ? "false" : "true";

                    ViewBag.SupplierSync = wantSupplierSync;

                    // Used by the project with AEP, readyWS calls this method
                    if (returnInsertedId)
                    {
                        JsonReturnObject jsonObj = new JsonReturnObject();
                        jsonObj.StatusCode = ResponseStatus.ERROR;
                        jsonObj.Message = ex.Message;
                        return Json(jsonObj);
                    }

                    if (Request.IsAjaxRequest())
                        return Json(this.ModalContentReply(dataFromDB));
                    else
                        return PartialView(dataFromDB);
                }
            }

            // Used by the project with AEP, readyWS calls this method
            if (returnInsertedId)
            {
                JsonReturnObject jsonObj = new JsonReturnObject();
                jsonObj.StatusCode = ResponseStatus.OK;
                jsonObj.Message = data.pkid.ToString();
                return Json(jsonObj);
            }

            Flash.Instance.Success(Texts.EditOperationSuccess);
            return Json(this.CloseModalReply());
        }

        [HttpPost]
        [PersonAuthorize(Permissions.EDIGITAL_FORNECEDORES)]
        public ActionResult CreateSupplier()
        {
            string wantSupplierSync = String.IsNullOrEmpty(_eDigitalConfigurationsRepository.GetConfigurationByKey("SincFornecedoresWS").ToString())
                                      ? "false" : "true";

            ViewBag.SupplierSync = wantSupplierSync;

            var newDB = new Fornecedores { Contribuinte = "" };

            if (Request.IsAjaxRequest())
                return Json(this.ModalContentReply(newDB), JsonRequestBehavior.AllowGet);
            else
                return PartialView(newDB);
        }

        [HttpPost]
        //[PersonAuthorize(Permissions.EDIGITAL_FORNECEDORES)]
        public ActionResult CreateDigitalSupplier(DigitalCreateSupplierData data, bool returnInsertedId = false)
        {
            //ViewBag.SupplierSync = _eDigitalConfigurationsRepository.GetConfigurationByKey("SincFornecedoresWS") != null ?
            //        ((bool)_eDigitalConfigurationsRepository.GetConfigurationByKey("SincFornecedoresWS") ? "true" : "false") : "false";

            string wantSupplierSync = String.IsNullOrEmpty(_eDigitalConfigurationsRepository.GetConfigurationByKey("SincFornecedoresWS").ToString())
                                      ? "false" : "true";

            ViewBag.SupplierSync = wantSupplierSync;

            var newDB = new Fornecedores
            {
                pkid = Guid.NewGuid(),
                Contribuinte = data.Contribuinte,
                Nome = data.Nome,
                Morada = data.Morada,
                Telefone = data.Telefone,
                Fax = data.Fax,
                Email = data.Email,
                WebSite = data.WebSite,
                WantMainValidations = true
            };

            if (!ModelState.IsValid)
            {
                Flash.Instance.Error(Texts.FixModelErrors);

                // Used by the project with AEP, readyWS calls this method
                JsonReturnObject json = new JsonReturnObject();
                json.StatusCode = ResponseStatus.ERROR;
                if (returnInsertedId)
                {
                    var modelErrors = ModelState.Values
                        .SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                        .ToList();

                    if (modelErrors != null && modelErrors.Count > 0)
                        json.Message = modelErrors[0];
                    else
                        json.Message = "Not specified error with the model state.";

                    return Json(json);
                }

                if (Request.IsAjaxRequest())
                    return Json(this.ModalContentReply("CreateSupplier", newDB));
                else
                    return PartialView("CreateSupplier", newDB);
            }

            using (var dbContextTransaction = _eDigitalSuppliersRepository.Context.Database.BeginTransaction())
            {
                try
                {
                    // ver se o Contribuinte ja existe
                    if (_eDigitalSuppliersRepository.ExistsNif(data.Contribuinte))
                    {
                        var existingSupplier = _eDigitalSuppliersRepository.Set.First(x => x.Contribuinte.ToLower().Replace(" ", "") == data.Contribuinte.ToLower().Replace(" ", ""));

                        if (existingSupplier != null)
                        {
                            // Used by the project with AEP, readyWS calls this method
                            if (returnInsertedId)
                            {
                                JsonReturnObject jsonObj = new JsonReturnObject();
                                jsonObj.StatusCode = ResponseStatus.OK;
                                jsonObj.Message = existingSupplier.pkid.ToString();
                                return Json(jsonObj);
                            }

                            Flash.Instance.Error(Texts.CampoJaExistente);

                            if (Request.IsAjaxRequest())
                                return Json(this.ModalContentReply("CreateSupplier", newDB));
                            else
                                return PartialView("CreateSupplier", newDB);
                        }
                    }

                    _eDigitalSuppliersRepository
                        .Add(newDB)
                        .Save();

                    // obter tipos de documentos com template definido
                    var docTypesWithTemplate = _eDigitalDocTypeRepository.GetTypesWithTemplate();

                    // para cada tipo de documento com template definido:
                    foreach (var docType in docTypesWithTemplate)
                    {
                        // obter estrutura do template
                        var templateStruct = _eDigitalDocTypeXmlDataRepository.GetDocumentTemplateStructure(docType.pkid);

                        // inserir em nome template
                        var nomeTemplate = new NomeTemplate
                        {
                            pkid = Guid.NewGuid(),
                            NomeTemplate1 = templateStruct.ElementAt(0).NomeTemplate + "_" + data.Contribuinte,
                            TipoXML = XmlTypes.UBL,
                            fkfornecedor = newDB.pkid,
                            fktipofact = docType.pkid,
                            Masterizado = false,
                            NomeOriginal = templateStruct.ElementAt(0).NomeTemplate
                        };
                        //_eDigitalTemplateNameRepository.Add(nomeTemplate).Save();

                        // para a estrutura base definida para este tipo de documento, associa-la ao fornecedor (tabela 'DadosTemplate')
                        List<DadosTemplateXmlDBTable> fieldsToInsert = new List<DadosTemplateXmlDBTable>();
                        templateStruct.ForEach(ts =>
                            fieldsToInsert.Add(new DadosTemplateXmlDBTable
                            {
                                Pkid = Guid.NewGuid(),
                                Fknometemplate = nomeTemplate.pkid,
                                NomeCampo = ts.NomeCampo,
                                Localizacao = ts.Localizacao,
                                Posicao = ts.Posicao,
                                CasasDecimais = ts.Formato.HasValue ? ts.Formato.Value : 0,
                                Obrigatorio = ts.Obrigatorio.Value,
                                CampoXmlRegex = ts.Regex ?? "",
                                DeOrigem = true,
                                TipoExtraccao = ts.TipoExtraccao,
                                Formula = ts.Formula,
                                Expressao = "",
                                LabelUI = "",
                                IsComboBox = (!ts.IsComboBox.HasValue || ts.IsComboBox == null) ? false : ts.IsComboBox.Value,
                                IsReadOnly = false,
                                PersistValueToNextDoc = false
                            })
                        );
                        string json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(fieldsToInsert);
                        _eDigitalSupplierXmlDataRepository.Add(new DadosTemplate { FKNomeTemplate = nomeTemplate.pkid, XmlFields = json }).Save();

                        // if it's a generic document type (same template/masterizatio for all suppliers)
                        bool masterizationInserted = false;
                        if (docType.IsGenericDocument.Value)
                        {
                            // get the first masterization found for this type of document, if it exists
                            var firstTemplateName = _eDigitalTemplateNameRepository.Where(x => x.fktipofact == docType.pkid && x.Masterizado == true).FirstOrDefault();

                            // get the masterization structure for the firs template found
                            if (firstTemplateName != null)
                            {
                                // header
                                var headerMasterizationFields =
                                    _eDigitalMasterizationHeaderRepository.Where(x => x.FKNomeTemplate == firstTemplateName.pkid).ToList();

                                if (headerMasterizationFields.Count() > 0)
                                {
                                    List<MasterizacaoCabecalho> headerMasterizationFieldsTemp = new List<MasterizacaoCabecalho>();
                                    foreach (var headerMasterizationField in headerMasterizationFields)
                                    {
                                        headerMasterizationFieldsTemp.Add(new MasterizacaoCabecalho
                                        {
                                            pkid = Guid.NewGuid(),
                                            FKNomeTemplate = nomeTemplate.pkid,
                                            NomeCampo = headerMasterizationField.NomeCampo,
                                            Topo = headerMasterizationField.Topo,
                                            Fundo = headerMasterizationField.Fundo,
                                            Esquerda = headerMasterizationField.Esquerda,
                                            Direita = headerMasterizationField.Direita,
                                            RegionId = headerMasterizationField.RegionId,
                                            LinhaId = headerMasterizationField.LinhaId,
                                            WordId = headerMasterizationField.WordId,
                                            WordPage = headerMasterizationField.WordPage,
                                            Word = headerMasterizationField.Word
                                        });
                                    }

                                    _eDigitalMasterizationHeaderRepository.Set.AddRange(headerMasterizationFieldsTemp);
                                    _eDigitalMasterizationHeaderRepository.Save();
                                    masterizationInserted = true;
                                }

                                // lines
                                var linesMasterizationFields =
                                    _eDigitalMasterizationLinesRepository.Where(x => x.FKNomeTemplate == firstTemplateName.pkid).ToList();

                                if (linesMasterizationFields.Count() > 0)
                                {
                                    List<MasterizacaoLineItems> linesMasterizationFieldsTemp = new List<MasterizacaoLineItems>();
                                    foreach (var linesMasterizationField in linesMasterizationFields)
                                    {
                                        linesMasterizationFieldsTemp.Add(new MasterizacaoLineItems
                                        {
                                            pkid = Guid.NewGuid(),
                                            FKNomeTemplate = nomeTemplate.pkid,
                                            NomeCampo = linesMasterizationField.NomeCampo,
                                            Topo = linesMasterizationField.Topo,
                                            Fundo = linesMasterizationField.Fundo,
                                            Esquerda = linesMasterizationField.Esquerda,
                                            Direita = linesMasterizationField.Direita,
                                            RegionId = linesMasterizationField.RegionId,
                                            LinhaId = linesMasterizationField.LinhaId,
                                            WordId = linesMasterizationField.WordId,
                                            WordPage = linesMasterizationField.WordPage,
                                            Word = linesMasterizationField.Word
                                        });
                                    }

                                    _eDigitalMasterizationLinesRepository.Set.AddRange(linesMasterizationFieldsTemp);
                                    _eDigitalMasterizationLinesRepository.Save();
                                    masterizationInserted = true;
                                }

                                // vat
                                var vatMasterizationFields =
                                    _eDigitalMasterizationVatRepository.Where(x => x.FKNomeTemplate == firstTemplateName.pkid).ToList();

                                if (vatMasterizationFields.Count() > 0)
                                {
                                    List<MasterizacaoIva> vatMasterizationFieldsTemp = new List<MasterizacaoIva>();
                                    foreach (var vatMasterizationField in vatMasterizationFields)
                                    {
                                        vatMasterizationFieldsTemp.Add(new MasterizacaoIva
                                        {
                                            pkid = Guid.NewGuid(),
                                            FKNomeTemplate = nomeTemplate.pkid,
                                            NomeCampo = vatMasterizationField.NomeCampo,
                                            Topo = vatMasterizationField.Topo,
                                            Fundo = vatMasterizationField.Fundo,
                                            Esquerda = vatMasterizationField.Esquerda,
                                            Direita = vatMasterizationField.Direita,
                                            RegionId = vatMasterizationField.RegionId,
                                            LinhaId = vatMasterizationField.LinhaId,
                                            WordId = vatMasterizationField.WordId,
                                            WordPage = vatMasterizationField.WordPage,
                                            Word = vatMasterizationField.Word
                                        });
                                    }

                                    _eDigitalMasterizationVatRepository.Set.AddRange(vatMasterizationFieldsTemp);
                                    _eDigitalMasterizationVatRepository.Save();
                                    masterizationInserted = true;
                                }
                            }
                        }

                        // if a masterization was inserted, mark this template has masterizated
                        if (masterizationInserted)
                            nomeTemplate.Masterizado = true;
                        _eDigitalTemplateNameRepository.Add(nomeTemplate).Save();
                    }

                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    Elmah.ErrorSignal.FromCurrentContext().Raise(e);

                    dbContextTransaction.Rollback();
                    Flash.Instance.Error(Texts.DBErrors);

                    // Used by the project with AEP, readyWS calls this method
                    if (returnInsertedId)
                    {
                        JsonReturnObject jsonObj = new JsonReturnObject();
                        jsonObj.StatusCode = ResponseStatus.ERROR;
                        jsonObj.Message = e.Message;
                        return Json(jsonObj);
                    }

                    if (Request.IsAjaxRequest())
                        return Json(this.ModalContentReply("CreateSupplier", newDB));
                    else
                        return PartialView("CreateSupplier", newDB);
                }
            }

            // Used by the project with AEP, readyWS calls this method
            if (returnInsertedId)
            {
                JsonReturnObject jsonObj = new JsonReturnObject();
                jsonObj.StatusCode = ResponseStatus.OK;
                jsonObj.Message = newDB.pkid.ToString();
                return Json(jsonObj);
            }

            Flash.Instance.Success(Texts.EditOperationSuccess);
            return Json(this.CloseModalReply());
        }

        [HttpPost]
        [PersonAuthorize(Permissions.EDIGITAL_FORNECEDORES)]
        public ActionResult GetSupplierData(string nif)
        {
            try
            {
                var model = _supplierSyncronization.GetDadosFornecedorFromWS(nif);

                if (Request.IsAjaxRequest())
                    return Json(model);
                else
                    return View("Costumers", model);
            }
            catch (Exception e)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(e);

                Flash.Instance.Error(e.Message);
                var forn = new Fornecedores { Contribuinte = nif };


                string wantSupplierSync = String.IsNullOrEmpty(_eDigitalConfigurationsRepository.GetConfigurationByKey("SincFornecedoresWS").ToString())
                                      ? "false" : "true";

                ViewBag.SupplierSync = wantSupplierSync;
                //ViewBag.SupplierSync = _eDigitalConfigurationsRepository.GetConfigurationByKey("SincFornecedoresWS") != null ?
                //    ((bool)_eDigitalConfigurationsRepository.GetConfigurationByKey("SincFornecedoresWS") ? "true" : "false") : "false";

                return View("CreateSupplier", forn);
            }
        }

        /// <summary>
        /// Procura na tabela de fornecedores se o texto a pesquisar existe no NIF e/ou Nome do fornecedor
        /// </summary>
        /// <param name="searchText">texto a pesquisar</param>
        /// <returns></returns>
        //[HttpPost]
        public ActionResult SearchSupplier(string searchText)
        {
            JsonReturnFoundSuppliers jsonObj = new JsonReturnFoundSuppliers();

            try
            {
                var model = _eDigitalSuppliersRepository
                    .Set
                    .Where(x => x.Contribuinte.ToLower().Contains(searchText.ToLower()) || x.Nome.ToLower().Contains(searchText.ToLower()))
                    .OrderBy(x => x.Nome)
                    .ToList();

                JsonReturnFornecedor[] forecedores = new JsonReturnFornecedor[model.Count];

                int i = 0;
                foreach (Fornecedores fornecedor in model)
                {
                    forecedores[i] = new JsonReturnFornecedor { NIF = fornecedor.Contribuinte, Nome = fornecedor.Nome };

                    i++;
                }

                jsonObj.StatusCode = ResponseStatus.OK;
                jsonObj.Message = model.Count().ToString();
                jsonObj.Fornecedores = forecedores;

                return Json(jsonObj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                jsonObj.StatusCode = ResponseStatus.ERROR;
                jsonObj.Message = "0";

                return Json(jsonObj, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Templates
        [PersonAuthorize(Permissions.EDIGITAL_TEMPLATES)]
        public ActionResult Template(Guid? docTypeId)
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            var availableDocTypes = _eDigitalDocTypeRepository
                .Set
                .ToList();

            var model = new EDigitalTemplatesVM(availableDocTypes,
                docTypeId.HasValue ? docTypeId.Value : Guid.Empty,
                _eDigitalDocTypeXmlDataRepository,
                _eDigitalTemplateNameRepository);

            if (Request.IsAjaxRequest())
                return Json(this.PanelContentReply(model), JsonRequestBehavior.AllowGet);
            else
                return View(model);
        }

        [PersonAuthorize(Permissions.EDIGITAL_TEMPLATES)]
        public ActionResult CreateDocumentType()
        {
            var newDB = new EDigitalCreateDocTypeVM(new TipoFacturas { nome = "" }, "", false);

            if (Request.IsAjaxRequest())
                return Json(this.ModalContentReply(newDB), JsonRequestBehavior.AllowGet);
            else
                return PartialView(newDB);
        }

        [HttpPost]
        [PersonAuthorize(Permissions.EDIGITAL_TEMPLATES)]
        public ActionResult CreateDocumentType(DigitalDocumentTypeData data)
        {
            var newDB = new TipoFacturas
            {
                pkid = Guid.NewGuid(),
                nome = data.TipoFactura.nome,
                RecognitionTags = data.TipoFactura.RecognitionTags,
                IsGenericDocument = data.IsGenericDocument
            };

            var newDBNomeTemplate = new NomeTemplate
            {
                TipoXML = "UBL2.0",
                fktipofact = newDB.pkid,
                Masterizado = false,
                NomeOriginal = data.NomeTemplate
            };

            var viewModel = new EDigitalCreateDocTypeVM(data.TipoFactura, data.NomeTemplate, data.IsGenericDocument);

            if (!ModelState.IsValid)
            {
                Flash.Instance.Error(Texts.FixModelErrors);

                if (Request.IsAjaxRequest())
                    return Json(this.ModalContentReply("CreateDocumentType", viewModel));
                else
                    return PartialView("CreateDocumentType", viewModel);
            }
            using (var dbContextTransaction = _eDigitalSuppliersRepository.Context.Database.BeginTransaction())
            {
                try
                {
                    // insert new document type
                    _eDigitalDocTypeRepository.Add(newDB).Save();

                    // insert document type template name (for each existing supplier)
                    Queries.InsertTemplateNameToAllEntitiesByDocType(newDBNomeTemplate, dbContextTransaction.UnderlyingTransaction);

                    // insert "Document Number" as a default XML field associated to each supplier
                    Queries.InsertTemplateDataToAllEntitiesByDocType("Nº Documento", DigitalDocumentAreas.HEADER, true, 0, true, DigitalExtractionTypes.EXTRACTED,
                        null, null, newDB.pkid, dbContextTransaction.UnderlyingTransaction, false, false, "", "",
                        _eDigitalSupplierXmlDataRepository, _eDigitalXmlFieldsRepository, true);

                    // insert "Document Number" as a default XML field associated to document type
                    _eDigitalDocTypeXmlDataRepository.InsertXmlField(newDB.pkid, "Nº Documento", DigitalDocumentAreas.HEADER, 1, true,
                        newDBNomeTemplate.NomeOriginal, 0, DigitalExtractionTypes.EXTRACTED, null, false, false, "", false, "", dbContextTransaction.UnderlyingTransaction);

                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    Elmah.ErrorSignal.FromCurrentContext().Raise(e);

                    dbContextTransaction.Rollback();
                    Flash.Instance.Error(Texts.DBErrors);

                    if (Request.IsAjaxRequest())
                        return Json(this.ModalContentReply("CreateDocumentType", viewModel));
                    else
                        return PartialView("CreateDocumentType", viewModel);
                }
            }

            Flash.Instance.Success(Texts.EditOperationSuccess);
            return Json(this.CloseModalReply());
        }

        [HttpPost]
        [PersonAuthorize(Permissions.EDIGITAL_TEMPLATES)]
        public ActionResult TemplateStructureExists(Guid docTypeId)
        {
            try
            {
                var exists = _eDigitalDocTypeXmlDataRepository.Set.Any(d => d.fkTipoFactura == docTypeId);

                if (Request.IsAjaxRequest())
                    return Json(exists);
                else
                    return RedirectToAction("Template", new { docTypeId = docTypeId });
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

                if (Request.IsAjaxRequest())
                    return Json("nok");
                else
                    return RedirectToAction("Template", new { docTypeId = docTypeId });
            }
        }

        // Action used to open modal do create XML field
        [HttpPost]
        [PersonAuthorize(Permissions.EDIGITAL_TEMPLATES)]
        public ActionResult CreateField(string location, Guid docTypePkid)
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            var local = location == "c" ? DigitalDocumentAreas.HEADER : (location == "l" ? DigitalDocumentAreas.LINES : DigitalDocumentAreas.VAT);

            var xmlFields = _eDigitalXmlFieldsRepository.Set
                .Where(x => x.Tipo.ToLower() == local && x.TipoXML == "UBL2.0")
                .OrderBy(x => x.NomeCampo)
                .ToList();

            var model = new EDigitalXmlFieldVM(xmlFields, local, docTypePkid);

            if (Request.IsAjaxRequest())
                return Json(this.ModalContentReply(model), JsonRequestBehavior.AllowGet);
            else
                return PartialView();
        }

        // Used do submit XML field creation
        [HttpPost]
        [PersonAuthorize(Permissions.EDIGITAL_TEMPLATES)]
        public ActionResult CreateXmlField(DigitalXmlFieldData data)
        {
            var xmlFields = _eDigitalXmlFieldsRepository.Set
                    .Where(x => x.Tipo.ToLower() == data.Localizacao.ToLower() && x.TipoXML == "UBL2.0")
                    .OrderBy(x => x.NomeCampo)
                    .ToList();

            data.Action = StandardActions.CREATE;

            var viewModel = new EDigitalXmlFieldVM(xmlFields, data);

            if (!ModelState.IsValid)
            {
                Flash.Instance.Error(Texts.FixModelErrors);

                if (Request.IsAjaxRequest())
                    return Json(this.ModalContentReply("CreateField", viewModel));
                else
                    return PartialView("CreateField", viewModel);
            }

            using (var dbContextTransaction = _eDigitalSuppliersRepository.Context.Database.BeginTransaction())
            {
                try
                {
                    var templateNames = _eDigitalTemplateNameRepository
                        .Where(t => t.fktipofact == data.TipoDocPkid);

                    // get all PKID from NomeTemplate associated with this document type
                    List<Guid> templateNameIds = templateNames
                        .Select(n => n.pkid)
                        .ToList();

                    // get original template name
                    string originalTemplateName = templateNames
                        .Select(n => n.NomeOriginal)
                        .FirstOrDefault();

                    // get last position for this location
                    var lastXmlField = _eDigitalDocTypeXmlDataRepository.GetDocumentTemplateStructure(data.TipoDocPkid)
                        .LastOrDefault(f => f.Localizacao.ToLower() == data.Localizacao.ToLower());
                    int pos = lastXmlField == null ? 1 : lastXmlField.Posicao + 1;

                    // For each supplier insert into DadosTemplate
                    Queries.InsertTemplateDataToAllEntitiesByDocType(data.NomeCampo, data.Localizacao, true, data.DecimalPlaces, data.IsRequired, data.ExtractionType,
                        data.Formula, null, data.TipoDocPkid, dbContextTransaction.UnderlyingTransaction, data.IsComboBox, data.IsReadOnly, data.LabelUI, data.DefaultValue,
                        _eDigitalSupplierXmlDataRepository, _eDigitalXmlFieldsRepository, false);

                    // Insert into TipoFacturasDadosXml
                    _eDigitalDocTypeXmlDataRepository.InsertXmlField(data.TipoDocPkid, data.NomeCampo, data.Localizacao, pos,
                        data.IsRequired, originalTemplateName, data.DecimalPlaces, data.ExtractionType, data.Formula, data.IsComboBox, data.IsReadOnly, data.LabelUI,
                        data.PersistValueToNextDoc, data.DefaultValue, dbContextTransaction.UnderlyingTransaction);

                    // Add the field to the masterization table, IF this document type for each supplier already exists (masterization)
                    if (data.Localizacao.ToLower() == DigitalDocumentAreas.HEADER)
                    {
                        MasterizacaoCabecalho masterizacaoCabecalho = new MasterizacaoCabecalho
                        {
                            NomeCampo = data.NomeCampo,
                            Topo = "-1",
                            Fundo = "-1",
                            Esquerda = "-1",
                            Direita = "-1",
                            RegionId = "-1",
                            LinhaId = "-1",
                            WordId = "-1",
                            WordPage = "-1",
                            Word = ""
                        };
                        Queries.InsertHeaderMasterization(masterizacaoCabecalho, data.TipoDocPkid, dbContextTransaction.UnderlyingTransaction);
                    }
                    else if (data.Localizacao.ToLower() == DigitalDocumentAreas.LINES)
                    {
                        MasterizacaoLineItems masterizacaoLineItems = new MasterizacaoLineItems
                        {
                            NomeCampo = data.NomeCampo,
                            Topo = "-1",
                            Fundo = "-1",
                            Esquerda = "-1",
                            Direita = "-1",
                            RegionId = "-1",
                            LinhaId = "-1",
                            WordId = "-1",
                            WordPage = "-1",
                            Word = ""
                        };
                        Queries.InsertLineMasterization(masterizacaoLineItems, data.TipoDocPkid, dbContextTransaction.UnderlyingTransaction);

                        // if there isn't fields for LINES, we must insert "Linhas Inicio" and "Linhas Fim" pseudo fields
                        if (lastXmlField == null)
                        {
                            MasterizacaoLineItems masterizacaoLineItemsBegin = new MasterizacaoLineItems
                            {
                                NomeCampo = "Linhas Inicio",
                                Topo = "-1",
                                Fundo = "-1",
                                Esquerda = "-1",
                                Direita = "-1",
                                RegionId = "-1",
                                LinhaId = "-1",
                                WordId = "-1",
                                WordPage = "-1",
                                Word = ""
                            };
                            Queries.InsertLineMasterization(masterizacaoLineItemsBegin, data.TipoDocPkid, dbContextTransaction.UnderlyingTransaction);

                            MasterizacaoLineItems masterizacaoLineItemsEnd = new MasterizacaoLineItems
                            {
                                NomeCampo = "Linhas Fim",
                                Topo = "-1",
                                Fundo = "-1",
                                Esquerda = "-1",
                                Direita = "-1",
                                RegionId = "-1",
                                LinhaId = "-1",
                                WordId = "-1",
                                WordPage = "-1",
                                Word = ""
                            };
                            Queries.InsertLineMasterization(masterizacaoLineItemsEnd, data.TipoDocPkid, dbContextTransaction.UnderlyingTransaction);
                        }
                    }
                    else if (data.Localizacao.ToLower() == DigitalDocumentAreas.VAT)
                    {
                        MasterizacaoIva masterizacaoIva = new MasterizacaoIva
                        {
                            NomeCampo = data.NomeCampo,
                            Topo = "-1",
                            Fundo = "-1",
                            Esquerda = "-1",
                            Direita = "-1",
                            RegionId = "-1",
                            LinhaId = "-1",
                            WordId = "-1",
                            WordPage = "-1",
                            Word = ""
                        };
                        Queries.InsertVatMasterization(masterizacaoIva, data.TipoDocPkid, dbContextTransaction.UnderlyingTransaction);

                        // if there isn't fields for LINES, we must insert "Linhas Inicio" and "Linhas Fim" pseudo fields
                        if (lastXmlField == null)
                        {
                            MasterizacaoIva masterizacaoIvaBegin = new MasterizacaoIva
                            {
                                NomeCampo = "Iva Inicio",
                                Topo = "-1",
                                Fundo = "-1",
                                Esquerda = "-1",
                                Direita = "-1",
                                RegionId = "-1",
                                LinhaId = "-1",
                                WordId = "-1",
                                WordPage = "-1",
                                Word = ""
                            };
                            Queries.InsertVatMasterization(masterizacaoIvaBegin, data.TipoDocPkid, dbContextTransaction.UnderlyingTransaction);

                            MasterizacaoIva masterizacaoIvaEnd = new MasterizacaoIva
                            {
                                NomeCampo = "Iva Fim",
                                Topo = "-1",
                                Fundo = "-1",
                                Esquerda = "-1",
                                Direita = "-1",
                                RegionId = "-1",
                                LinhaId = "-1",
                                WordId = "-1",
                                WordPage = "-1",
                                Word = ""
                            };
                            Queries.InsertVatMasterization(masterizacaoIvaEnd, data.TipoDocPkid, dbContextTransaction.UnderlyingTransaction);
                        }
                    }

                    dbContextTransaction.Commit();

                }
                catch (Exception e)
                {
                    Elmah.ErrorSignal.FromCurrentContext().Raise(e);

                    dbContextTransaction.Rollback();
                    Flash.Instance.Error(Texts.DBErrors);

                    if (Request.IsAjaxRequest())
                        return Json(this.ModalContentReply("CreateField", viewModel));
                    else
                        return PartialView("CreateField", viewModel);
                }
            }

            Flash.Instance.Success(Texts.EditOperationSuccess);
            return Json(this.CloseModalReply());
        }

        // Used to update DataTables after POST
        [PersonAuthorize(Permissions.EDIGITAL_TEMPLATES)]
        public ActionResult DocumentTypeFieldsUpdateAction(string location, Guid docTypePkid)
        {
            var local = location == "c" ? DigitalDocumentAreas.HEADER : (location == "l" ? DigitalDocumentAreas.LINES : DigitalDocumentAreas.VAT);

            var viewName = location == "c" ? "_templateCab" : (location == "l" ? "_templateLines" : "_templateVat");

            var model = _eDigitalDocTypeXmlDataRepository
                .Set
                .Where(x => x.Localizacao.ToLower() == local && x.fkTipoFactura == docTypePkid)
                .ToList();

            // re-asigned ViewData, because ViewData values get lost between redirects, actions and views (we should use TempData)
            ViewData["docTypePkid"] = docTypePkid;

            if (Request.IsAjaxRequest())
                return Json(this.PanelContentReply(viewName, model), JsonRequestBehavior.AllowGet);
            else
                return View(viewName, model);
        }

        // Used to open modal do edit XML field
        [PersonAuthorize(Permissions.EDIGITAL_TEMPLATES)]
        public ActionResult EditField(Guid pkid)
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            var dataFromDb = _eDigitalDocTypeXmlDataRepository.Find(pkid);

            var xmlFields = _eDigitalXmlFieldsRepository.Set
                .Where(x => x.Tipo.ToLower() == dataFromDb.Localizacao.ToLower() && x.TipoXML == "UBL2.0")
                .OrderBy(x => x.NomeCampo)
                .ToList();

            var model = new EDigitalXmlFieldVM(dataFromDb, xmlFields);

            if (Request.IsAjaxRequest())
                return Json(this.ModalContentReply(model), JsonRequestBehavior.AllowGet);
            else
                return PartialView();
        }

        // Used to submit XML field edit
        [HttpPost]
        [PersonAuthorize(Permissions.EDIGITAL_TEMPLATES)]
        public ActionResult EditField(DigitalXmlFieldData data)
        {
            var xmlFields = _eDigitalXmlFieldsRepository.Set
                    .Where(x => x.Tipo.ToLower() == data.Localizacao.ToLower() && x.TipoXML == "UBL2.0")
                    .OrderBy(x => x.NomeCampo)
                    .ToList();

            data.Action = StandardActions.EDIT;

            var viewModel = new EDigitalXmlFieldVM(xmlFields, data);

            if (!ModelState.IsValid)
            {
                Flash.Instance.Error(Texts.FixModelErrors);

                if (Request.IsAjaxRequest())
                    return Json(this.ModalContentReply("EditField", viewModel));
                else
                    return PartialView("EditField", viewModel);
            }

            using (var dbContextTransaction = _eDigitalSupplierXmlDataRepository.Context.Database.BeginTransaction())
            {
                try
                {
                    // get data from DB
                    var dataFromDb = _eDigitalDocTypeXmlDataRepository.Find(data.Pkid);

                    var dadosTemplates = _eDigitalSupplierXmlDataRepository.GetDadosTemplateFromDocumentType(dataFromDb.fkTipoFactura);

                    foreach (var dadosTemplate in dadosTemplates)
                    {
                        var xmlFieldsDeserialized = _eDigitalSupplierXmlDataRepository.GetDeserializedFields(dadosTemplate);

                        foreach (var xmlFieldDeserialized in xmlFieldsDeserialized)
                        {
                            if (xmlFieldDeserialized.NomeCampo.ToLower() == dataFromDb.NomeCampo.ToLower() && xmlFieldDeserialized.Localizacao.ToLower() == dataFromDb.Localizacao.ToLower()
                                && xmlFieldDeserialized.CasasDecimais == dataFromDb.Formato && xmlFieldDeserialized.Obrigatorio == dataFromDb.Obrigatorio
                                && xmlFieldDeserialized.TipoExtraccao.ToLower() == dataFromDb.TipoExtraccao.ToLower() && xmlFieldDeserialized.IsComboBox == dataFromDb.IsComboBox)
                            {
                                xmlFieldDeserialized.CasasDecimais = data.DecimalPlaces;
                                xmlFieldDeserialized.Formula = data.Formula;
                                xmlFieldDeserialized.Obrigatorio = data.IsRequired;
                                xmlFieldDeserialized.TipoExtraccao = data.ExtractionType;
                                xmlFieldDeserialized.IsComboBox = data.IsComboBox;
                                xmlFieldDeserialized.IsReadOnly = data.IsReadOnly;
                                xmlFieldDeserialized.LabelUI = data.LabelUI;
                                xmlFieldDeserialized.DefaultValue = data.DefaultValue;

                                break;
                            }
                        }

                        string xmlFieldsSerialized = new JavaScriptSerializer().Serialize(xmlFieldsDeserialized);

                        dadosTemplate.XmlFields = xmlFieldsSerialized;

                        _eDigitalSupplierXmlDataRepository.Edit(dadosTemplate).Save();
                    }

                    // update TipoFacturaDadosXML
                    dataFromDb.Formato = data.DecimalPlaces;
                    dataFromDb.Formula = data.Formula;
                    dataFromDb.Obrigatorio = data.IsRequired;
                    dataFromDb.TipoExtraccao = data.ExtractionType;
                    dataFromDb.IsComboBox = data.IsComboBox;
                    dataFromDb.IsReadOnly = data.IsReadOnly;
                    dataFromDb.LabelUI = data.LabelUI;
                    dataFromDb.DefaultValue = data.DefaultValue;

                    _eDigitalDocTypeXmlDataRepository.Edit(dataFromDb).Save();

                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    Elmah.ErrorSignal.FromCurrentContext().Raise(e);

                    dbContextTransaction.Rollback();
                    Flash.Instance.Error(Texts.DBErrors);

                    if (Request.IsAjaxRequest())
                        return Json(this.ModalContentReply("EditField", viewModel));
                    else
                        return PartialView("EditField", viewModel);
                }
            }

            Flash.Instance.Success(Texts.EditOperationSuccess);
            return Json(this.CloseModalReply());
        }

        // Used to submit XML field delete
        [HttpPost]
        [PersonAuthorize(Permissions.EDIGITAL_TEMPLATES)]
        public ActionResult DeleteField(Guid pkid, Guid docTypePkid)
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            var dataFromDb = _eDigitalDocTypeXmlDataRepository.Find(pkid);
            var fieldLocal = dataFromDb.Localizacao == DigitalDocumentAreas.HEADER ? "c" : dataFromDb.Localizacao == DigitalDocumentAreas.LINES ? "l" : "i";

            using (var dbContextTransaction = _eDigitalDocTypeXmlDataRepository.Context.Database.BeginTransaction())
            {
                try
                {
                    if (dataFromDb.NomeCampo == "Nº Documento")
                    {
                        Flash.Instance.Error(Texts.DigitalCannotDeleteDocumentNumber);
                        return RedirectToAction("DocumentTypeFieldsUpdateAction", new { location = fieldLocal, docTypePkid = docTypePkid });
                    }

                    // delete from TipoFacturasDadosXML
                    _eDigitalDocTypeXmlDataRepository.Delete(dataFromDb).Save();

                    // update TipoFacturasDadosXML field position
                    _eDigitalDocTypeXmlDataRepository
                        .Where(x => x.Posicao > dataFromDb.Posicao && x.fkTipoFactura == dataFromDb.fkTipoFactura
                                && x.Localizacao == dataFromDb.Localizacao)
                        .ToList()
                        .ForEach(x =>
                        {
                            var lineFromDb = _eDigitalDocTypeXmlDataRepository.Find(x.pkid);
                            lineFromDb.Posicao = x.Posicao - 1;
                            _eDigitalDocTypeXmlDataRepository.Edit(lineFromDb).Save();
                        });

                    // delete from DadosTemplate AND update DadosTemplate field position
                    // aqui fazer em 2 partes: 1-obter a linha que interessa, depois deserializar e percorrer para obter o campo que queremos
                    //var dadosTemplatePkis = (from dt in _eDigitalSupplierXmlDataRepository.Context.DadosTemplate
                    //                         from nt in _eDigitalTemplateNameRepository.Context.NomeTemplate
                    //                         where dt.fkNomeTemplate == nt.pkid
                    //                          && dt.NomeCampo == dataFromDb.NomeCampo && dt.Localizacao == dataFromDb.Localizacao
                    //                          && dt.Formato == dataFromDb.Formato && dt.Obrigatorio == dataFromDb.Obrigatorio
                    //                          && dt.TipoExtraccao == dataFromDb.TipoExtraccao
                    //                          && dt.IsComboBox == dataFromDb.IsComboBox
                    //                          && nt.fktipofact == dataFromDb.fkTipoFactura
                    //                         select dt.pkid).ToList();

                    //dadosTemplatePkis.ForEach(pk =>
                    //{
                    //    // delete from DadosTemplate
                    //    var lineFromDb = _eDigitalSupplierXmlDataRepository.Find(pk);
                    //    _eDigitalSupplierXmlDataRepository.Delete(lineFromDb).Save();

                    //    // update DadosTemplate field position
                    //    _eDigitalSupplierXmlDataRepository
                    //        .Where(x => x.Posicao > lineFromDb.Posicao && x.fkNomeTemplate == lineFromDb.fkNomeTemplate && x.Localizacao == lineFromDb.Localizacao)
                    //        .ToList()
                    //        .ForEach(x =>
                    //        {
                    //            var lineToUpdate = _eDigitalSupplierXmlDataRepository.Find(x.pkid);
                    //            lineToUpdate.Posicao = x.Posicao - 1;
                    //            _eDigitalSupplierXmlDataRepository.Edit(lineToUpdate).Save();
                    //        });
                    //});
                    var dadosTemplates = _eDigitalSupplierXmlDataRepository.GetDadosTemplateFromDocumentType(dataFromDb.fkTipoFactura);

                    foreach (var dadosTemplate in dadosTemplates)
                    {
                        var xmlFieldsDeserialized = _eDigitalSupplierXmlDataRepository.GetDeserializedFields(dadosTemplate);

                        List<DadosTemplateXmlDBTable> fieldsToRemove = new List<DadosTemplateXmlDBTable>();

                        foreach (var xmlFieldDeserialized in xmlFieldsDeserialized)
                        {
                            if (xmlFieldDeserialized.NomeCampo.ToLower() == dataFromDb.NomeCampo.ToLower() && xmlFieldDeserialized.Localizacao.ToLower() == dataFromDb.Localizacao.ToLower()
                                && xmlFieldDeserialized.CasasDecimais == dataFromDb.Formato && xmlFieldDeserialized.Obrigatorio == dataFromDb.Obrigatorio
                                && xmlFieldDeserialized.TipoExtraccao.ToLower() == dataFromDb.TipoExtraccao.ToLower() && xmlFieldDeserialized.IsComboBox == dataFromDb.IsComboBox)
                            {
                                fieldsToRemove.Add(xmlFieldDeserialized);
                            }
                            else
                            {
                                xmlFieldDeserialized.Posicao = xmlFieldDeserialized.Posicao - 1;
                            }
                        }

                        foreach (var fieldToRemove in fieldsToRemove)
                            xmlFieldsDeserialized.Remove(fieldToRemove);

                        string xmlFieldsSerialized = new JavaScriptSerializer().Serialize(xmlFieldsDeserialized);

                        dadosTemplate.XmlFields = xmlFieldsSerialized;

                        _eDigitalSupplierXmlDataRepository.Edit(dadosTemplate).Save();

                        // delete from Masterizacao
                        switch (fieldLocal)
                        {
                            case "c":
                                _eDigitalMasterizationHeaderRepository.DeleteMasterization(dadosTemplate.FKNomeTemplate, dataFromDb.NomeCampo);
                                break;
                            case "l":
                                _eDigitalMasterizationLinesRepository.DeleteMasterization(dadosTemplate.FKNomeTemplate, dataFromDb.NomeCampo);
                                break;
                            case "i":
                                _eDigitalMasterizationVatRepository.DeleteMasterization(dadosTemplate.FKNomeTemplate, dataFromDb.NomeCampo);
                                break;
                            default:
                                break;
                        }
                    }

                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    Elmah.ErrorSignal.FromCurrentContext().Raise(e);

                    dbContextTransaction.Rollback();
                    Flash.Instance.Error(Texts.DBErrors);
                    return RedirectToAction("DocumentTypeFieldsUpdateAction", new { location = fieldLocal, docTypePkid = docTypePkid });
                }
            }

            Flash.Instance.Success(Texts.DeleteOperationSuccess);
            return RedirectToAction("DocumentTypeFieldsUpdateAction", new { location = fieldLocal, docTypePkid = docTypePkid });
        }

        #endregion

        #region Gestão XML
        [PersonAuthorize(Permissions.EDIGITAL_GESTAOXML)]
        public ActionResult XmlManagment(Guid? supplierId, Guid? docTypeId)
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            var availableSuppliers = _eDigitalSuppliersRepository.Set.ToList();

            var availableDocTypes = _eDigitalDocTypeRepository.Set.ToList();

            var model = new EDigitalXmlManagmentVM(availableSuppliers, availableDocTypes,
                docTypeId.HasValue ? docTypeId.Value : Guid.Empty,
                supplierId.HasValue ? supplierId.Value : Guid.Empty,
                _eDigitalSupplierXmlDataRepository,
                _eDigitalTemplateNameRepository);

            if (Request.IsAjaxRequest())
                return Json(this.PanelContentReply(model), JsonRequestBehavior.AllowGet);
            else
                return View(model);
        }

        [HttpPost]
        [PersonAuthorize(Permissions.EDIGITAL_GESTAOXML)]
        public ActionResult SupplierTemplateStructureExists(Guid docTypeId, Guid supplierId)
        {
            try
            {
                var nomeTemplateId = _eDigitalTemplateNameRepository.Set
                    .FirstOrDefault(n => n.fkfornecedor == supplierId && n.fktipofact == docTypeId);

                if (nomeTemplateId == null)
                {
                    Flash.Instance.Error(Texts.NoTemplateForGivenDocAndSupplier);

                    if (Request.IsAjaxRequest())
                        return Json(false);
                    else
                        return RedirectToAction("XmlManagment", new { docTypeId = docTypeId, supplierId = supplierId });
                }

                var exists = _eDigitalSupplierXmlDataRepository.Set.Any(d => d.FKNomeTemplate == nomeTemplateId.pkid);

                if (Request.IsAjaxRequest())
                    return Json(exists);
                else
                    return RedirectToAction("XmlManagment", new { docTypeId = docTypeId, supplierId = supplierId });
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

                if (Request.IsAjaxRequest())
                    return Json("nok");
                else
                    return RedirectToAction("XmlManagment", new { docTypeId = docTypeId, supplierId = supplierId });
            }
        }

        // Action used to open modal do create XML field
        [HttpPost]
        [PersonAuthorize(Permissions.EDIGITAL_GESTAOXML)]
        public ActionResult CreateFieldSupplier(string location, Guid docTypePkid, Guid supplierPkid)
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            var local = location == "c" ? DigitalDocumentAreas.HEADER : (location == "l" ? DigitalDocumentAreas.LINES : DigitalDocumentAreas.VAT);

            var xmlFields = _eDigitalXmlFieldsRepository.Set
                .Where(x => x.Tipo.ToLower() == local && x.TipoXML == "UBL2.0")
                .OrderBy(x => x.NomeCampo)
                .ToList();

            var model = new EDigitalXmlFieldVM(xmlFields, local, docTypePkid, supplierPkid);

            if (Request.IsAjaxRequest())
                return Json(this.ModalContentReply(model), JsonRequestBehavior.AllowGet);
            else
                return PartialView();
        }

        // Used do submit XML field creation
        [HttpPost]
        [PersonAuthorize(Permissions.EDIGITAL_GESTAOXML)]
        public ActionResult CreateXmlFieldSupplier(DigitalXmlFieldData data)
        {
            var xmlFields = _eDigitalXmlFieldsRepository.Set
                    .Where(x => x.Tipo.ToLower() == data.Localizacao.ToLower() && x.TipoXML == "UBL2.0")
                    .OrderBy(x => x.NomeCampo)
                    .ToList();

            data.Action = StandardActions.CREATE;

            var viewModel = new EDigitalXmlFieldVM(xmlFields, data);

            if (!ModelState.IsValid)
            {
                Flash.Instance.Error(Texts.FixModelErrors);

                if (Request.IsAjaxRequest())
                    return Json(this.ModalContentReply("CreateFieldSupplier", viewModel));
                else
                    return PartialView("CreateFieldSupplier", viewModel);
            }

            using (var dbContextTransaction = _eDigitalSupplierXmlDataRepository.Context.Database.BeginTransaction())
            {
                try
                {
                    var nomeTemplate = _eDigitalTemplateNameRepository.Set
                        .FirstOrDefault(n => n.fkfornecedor == data.SupplierPkid && n.fktipofact == data.TipoDocPkid);

                    if (nomeTemplate == null)
                    {
                        dbContextTransaction.Rollback();
                        Flash.Instance.Error(Texts.NoTemplateForGivenDocAndSupplier);

                        if (Request.IsAjaxRequest())
                            return Json(this.ModalContentReply("CreateFieldSupplier", viewModel));
                        else
                            return PartialView("CreateFieldSupplier", viewModel);
                    }

                    // alterado 12-03-2015
                    // since we are modifying the template structure, delete Masterizacoes (header, lines and vat)
                    //_eDigitalMasterizationHeaderRepository.Set.RemoveRange(_eDigitalMasterizationHeaderRepository.Set.Where(mh => mh.FKNomeTemplate == nomeTemplate.pkid));
                    //_eDigitalMasterizationHeaderRepository.Context.SaveChanges();
                    //_eDigitalMasterizationLinesRepository.Set.RemoveRange(_eDigitalMasterizationLinesRepository.Set.Where(ml => ml.FKNomeTemplate == nomeTemplate.pkid));
                    //_eDigitalMasterizationLinesRepository.Context.SaveChanges();
                    //_eDigitalMasterizationVatRepository.Set.RemoveRange(_eDigitalMasterizationVatRepository.Set.Where(mv => mv.FKNomeTemplate == nomeTemplate.pkid));
                    //_eDigitalMasterizationVatRepository.Context.SaveChanges();
                    // acrescentar à masterização do fornecedor
                    if (data.Localizacao.ToLower() == DigitalDocumentAreas.HEADER)
                    {
                        // se tem masterização
                        var mast = _eDigitalMasterizationHeaderRepository.Set.Where(mh => mh.FKNomeTemplate == nomeTemplate.pkid).FirstOrDefault();
                        if (mast != null)
                        {
                            _eDigitalMasterizationHeaderRepository.Set.Add(
                                new MasterizacaoCabecalho
                                {
                                    pkid = Guid.NewGuid(),
                                    FKNomeTemplate = nomeTemplate.pkid,
                                    NomeCampo = data.NomeCampo,
                                    Topo = "-1",
                                    Fundo = "-1",
                                    Esquerda = "-1",
                                    Direita = "-1",
                                    RegionId = "-1",
                                    LinhaId = "-1",
                                    WordId = "-1",
                                    WordPage = "-1",
                                    Word = ""
                                });
                        }
                    }
                    else if (data.Localizacao.ToLower() == DigitalDocumentAreas.LINES)
                    {
                        // se tem masterização
                        var mast = _eDigitalMasterizationLinesRepository.Set.Where(mh => mh.FKNomeTemplate == nomeTemplate.pkid).FirstOrDefault();

                        // se ainda não tinha masterização, insere Inicio e Fim
                        if (mast == null)
                        {
                            _eDigitalMasterizationLinesRepository.Set.Add(
                                new MasterizacaoLineItems
                                {
                                    pkid = Guid.NewGuid(),
                                    FKNomeTemplate = nomeTemplate.pkid,
                                    NomeCampo = "Linhas Inicio",
                                    Topo = "-1",
                                    Fundo = "-1",
                                    Esquerda = "-1",
                                    Direita = "-1",
                                    RegionId = "-1",
                                    LinhaId = "-1",
                                    WordId = "-1",
                                    WordPage = "-1",
                                    Word = ""
                                });

                            _eDigitalMasterizationLinesRepository.Set.Add(
                                new MasterizacaoLineItems
                                {
                                    pkid = Guid.NewGuid(),
                                    FKNomeTemplate = nomeTemplate.pkid,
                                    NomeCampo = "Linhas Fim",
                                    Topo = "-1",
                                    Fundo = "-1",
                                    Esquerda = "-1",
                                    Direita = "-1",
                                    RegionId = "-1",
                                    LinhaId = "-1",
                                    WordId = "-1",
                                    WordPage = "-1",
                                    Word = ""
                                });
                        }

                        //if (mast != null)
                        //{
                        _eDigitalMasterizationLinesRepository.Set.Add(
                            new MasterizacaoLineItems
                            {
                                pkid = Guid.NewGuid(),
                                FKNomeTemplate = nomeTemplate.pkid,
                                NomeCampo = data.NomeCampo,
                                Topo = "-1",
                                Fundo = "-1",
                                Esquerda = "-1",
                                Direita = "-1",
                                RegionId = "-1",
                                LinhaId = "-1",
                                WordId = "-1",
                                WordPage = "-1",
                                Word = ""
                            });
                        //}
                    }
                    else if (data.Localizacao.ToLower() == DigitalDocumentAreas.VAT)
                    {
                        // se tem masterização
                        var mast = _eDigitalMasterizationVatRepository.Set.Where(mh => mh.FKNomeTemplate == nomeTemplate.pkid).FirstOrDefault();

                        // se ainda não tinha masterização, insere Inicio e Fim
                        if (mast == null)
                        {
                            _eDigitalMasterizationVatRepository.Set.Add(
                                 new MasterizacaoIva
                                 {
                                     pkid = Guid.NewGuid(),
                                     FKNomeTemplate = nomeTemplate.pkid,
                                     NomeCampo = "Iva Inicio",
                                     Topo = "-1",
                                     Fundo = "-1",
                                     Esquerda = "-1",
                                     Direita = "-1",
                                     RegionId = "-1",
                                     LinhaId = "-1",
                                     WordId = "-1",
                                     WordPage = "-1",
                                     Word = ""
                                 });

                            _eDigitalMasterizationVatRepository.Set.Add(
                                new MasterizacaoIva
                                {
                                    pkid = Guid.NewGuid(),
                                    FKNomeTemplate = nomeTemplate.pkid,
                                    NomeCampo = "Iva Fim",
                                    Topo = "-1",
                                    Fundo = "-1",
                                    Esquerda = "-1",
                                    Direita = "-1",
                                    RegionId = "-1",
                                    LinhaId = "-1",
                                    WordId = "-1",
                                    WordPage = "-1",
                                    Word = ""
                                });
                        }

                        //if (mast != null)
                        //{
                        _eDigitalMasterizationVatRepository.Set.Add(
                            new MasterizacaoIva
                            {
                                pkid = Guid.NewGuid(),
                                FKNomeTemplate = nomeTemplate.pkid,
                                NomeCampo = data.NomeCampo,
                                Topo = "-1",
                                Fundo = "-1",
                                Esquerda = "-1",
                                Direita = "-1",
                                RegionId = "-1",
                                LinhaId = "-1",
                                WordId = "-1",
                                WordPage = "-1",
                                Word = ""
                            });
                        //}
                    }

                    // mark the template has NOT "masterizado" - alterado 12-03-2015
                    //nomeTemplate.Masterizado = false;
                    //_eDigitalTemplateNameRepository.Edit(nomeTemplate).Save();

                    // insert into DadosTemplate
                    _eDigitalSupplierXmlDataRepository.InsertXmlField(data.NomeCampo, data.Localizacao, false, data.DecimalPlaces,
                        data.IsRequired, data.ExtractionType, data.Formula, data.Expression, new List<Guid> { nomeTemplate.pkid }, data.IsComboBox, data.DefaultValue);

                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    Elmah.ErrorSignal.FromCurrentContext().Raise(e);

                    dbContextTransaction.Rollback();
                    Flash.Instance.Error(Texts.DBErrors);

                    if (Request.IsAjaxRequest())
                        return Json(this.ModalContentReply("CreateFieldSupplier", viewModel));
                    else
                        return PartialView("CreateFieldSupplier", viewModel);
                }
            }

            Flash.Instance.Success(Texts.EditOperationSuccess);
            return Json(this.CloseModalReply());
        }

        // Used to update DataTables after POST
        [PersonAuthorize(Permissions.EDIGITAL_GESTAOXML)]
        public ActionResult SupplierFieldsUpdateAction(string location, Guid docTypePkid, Guid supplierPkid)
        {
            var local = location == "c" ? DigitalDocumentAreas.HEADER : (location == "l" ? DigitalDocumentAreas.LINES : DigitalDocumentAreas.VAT);

            var viewName = location == "c" ? "_templateCabSupplier" : (location == "l" ? "_templateLinesSupplier" : "_templateVatSupplier");

            var nomeTemplate = _eDigitalTemplateNameRepository.Set
                .FirstOrDefault(tn => tn.fkfornecedor == supplierPkid && tn.fkfornecedor == supplierPkid);

            //var model = _eDigitalSupplierXmlDataRepository
            //    .Set
            //    .Where(x => x.Localizacao.ToLower() == local && x.fkNomeTemplate == nomeTemplate.pkid)
            //    .ToList();
            var dadosTemplate = _eDigitalSupplierXmlDataRepository.Set.FirstOrDefault(x => x.FKNomeTemplate == nomeTemplate.pkid);
            var xmlFields = _eDigitalSupplierXmlDataRepository.GetDeserializedFields(dadosTemplate);
            var model = xmlFields.Where(x => x.Localizacao.ToLower() == local).ToList();

            // re-asigned ViewData, because ViewData values get lost between redirects, actions and views (we should use TempData)
            ViewData["docTypePkid"] = docTypePkid;
            ViewData["supplierPkid"] = supplierPkid;

            if (Request.IsAjaxRequest())
                return Json(this.PanelContentReply(viewName, model), JsonRequestBehavior.AllowGet);
            else
                return View(viewName, model);
        }

        // Used to open modal do edit XML field
        [PersonAuthorize(Permissions.EDIGITAL_GESTAOXML)]
        public ActionResult EditFieldSupplier(Guid pkid, Guid fkNomeTemplate)
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            //var dataFromDb = _eDigitalSupplierXmlDataRepository.Find(pkid);
            var dadosTemplate = _eDigitalSupplierXmlDataRepository.Set.FirstOrDefault(x => x.FKNomeTemplate == fkNomeTemplate);
            var dadosTemplateFields = _eDigitalSupplierXmlDataRepository.GetDeserializedFields(dadosTemplate);
            var dataFromDb = dadosTemplateFields.FirstOrDefault(x => x.Pkid == pkid);

            var xmlFields = _eDigitalXmlFieldsRepository.Set
                .Where(x => x.Tipo.ToLower() == dataFromDb.Localizacao.ToLower() && x.TipoXML == "UBL2.0")
                .OrderBy(x => x.NomeCampo)
                .ToList();

            var nomeTemplate = _eDigitalTemplateNameRepository.Set.FirstOrDefault(tn => tn.pkid == dataFromDb.Fknometemplate);

            if (nomeTemplate == null)
            {
                Flash.Instance.Error(Texts.FixModelErrors);

                return RedirectToAction("XmlManagment");
            }

            var model = new EDigitalXmlFieldVM(dataFromDb, xmlFields, nomeTemplate.fktipofact.Value, nomeTemplate.fkfornecedor.Value, fkNomeTemplate);

            if (Request.IsAjaxRequest())
                return Json(this.ModalContentReply(model), JsonRequestBehavior.AllowGet);
            else
                return PartialView();
        }

        // Used to submit XML field edit
        [HttpPost]
        [PersonAuthorize(Permissions.EDIGITAL_GESTAOXML)]
        public ActionResult EditFieldSupplier(DigitalXmlFieldData data, Guid fkNomeTemplate)
        {
            var xmlFields = _eDigitalXmlFieldsRepository.Set
                    .Where(x => x.Tipo.ToLower() == data.Localizacao.ToLower() && x.TipoXML == "UBL2.0")
                    .OrderBy(x => x.NomeCampo)
                    .ToList();

            data.Action = StandardActions.EDIT;

            var viewModel = new EDigitalXmlFieldVM(xmlFields, data);

            if (!ModelState.IsValid)
            {
                Flash.Instance.Error(Texts.FixModelErrors);

                if (Request.IsAjaxRequest())
                    return Json(this.ModalContentReply("EditFieldSupplier", viewModel));
                else
                    return PartialView("EditFieldSupplier", viewModel);
            }

            using (var dbContextTransaction = _eDigitalSupplierXmlDataRepository.Context.Database.BeginTransaction())
            {
                try
                {
                    // get data from DB
                    //var dataFromDb = _eDigitalSupplierXmlDataRepository.Find(data.Pkid);
                    var dataFromDb = _eDigitalSupplierXmlDataRepository.Set.FirstOrDefault(x => x.FKNomeTemplate == fkNomeTemplate);
                    var dadosTemplateFields = _eDigitalSupplierXmlDataRepository.GetDeserializedFields(dataFromDb);

                    //var dataFromDb = dadosTemplateFields.FirstOrDefault(x => x.Pkid == data.Pkid);
                    foreach (DadosTemplateXmlDBTable xmlField in dadosTemplateFields)
                    {
                        if (xmlField.Pkid == data.Pkid)
                        {
                            // if it's document type field, only allow edit 'Expression' field
                            xmlField.Expressao = data.Expression;
                            xmlField.IsComboBox = data.IsComboBox;

                            //Tiago Borges: comentado para funcionar o save do campo
                            //if (!dataFromDb.DeOrigem)
                            //if (xmlField.DeOrigem)
                            //{
                            xmlField.CasasDecimais = data.DecimalPlaces;
                            xmlField.Formula = data.Formula;
                            xmlField.Obrigatorio = data.IsRequired;
                            xmlField.TipoExtraccao = data.ExtractionType;

                            xmlField.DefaultValue = data.DefaultValue;
                            //}

                            break;
                        }
                    }

                    string jsonXmlFields = new JavaScriptSerializer().Serialize(dadosTemplateFields);
                    dataFromDb.XmlFields = jsonXmlFields;

                    _eDigitalSupplierXmlDataRepository.Edit(dataFromDb).Save();

                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    Elmah.ErrorSignal.FromCurrentContext().Raise(e);

                    dbContextTransaction.Rollback();
                    Flash.Instance.Error(Texts.DBErrors);

                    if (Request.IsAjaxRequest())
                        return Json(this.ModalContentReply("EditFieldSupplier", viewModel));
                    else
                        return PartialView("EditFieldSupplier", viewModel);
                }
            }

            Flash.Instance.Success(Texts.EditOperationSuccess);
            return Json(this.CloseModalReply());
        }

        // Used to submit XML field delete
        [HttpPost]
        [PersonAuthorize(Permissions.EDIGITAL_GESTAOXML)]
        public ActionResult DeleteFieldSupplier(Guid pkid, Guid fkNomeTemplate)
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            //var dataFromDb = _eDigitalSupplierXmlDataRepository.Find(pkid);
            var dataFromDb = _eDigitalSupplierXmlDataRepository.Set.FirstOrDefault(x => x.FKNomeTemplate == fkNomeTemplate);
            var dadosTemplateFields = _eDigitalSupplierXmlDataRepository.GetDeserializedFields(dataFromDb);
            var fieldLocal = "c";
            var nomeTemplate = _eDigitalTemplateNameRepository.Find(fkNomeTemplate);
            string fieldName = "";

            using (var dbContextTransaction = _eDigitalDocTypeXmlDataRepository.Context.Database.BeginTransaction())
            {
                try
                {
                    bool foundFieldToUpdate = false;
                    int positionToRemove = 0;
                    int i = 0;
                    foreach (DadosTemplateXmlDBTable xmlField in dadosTemplateFields)
                    {
                        if (xmlField.Pkid == pkid)
                        {
                            foundFieldToUpdate = true;

                            fieldLocal = xmlField.Localizacao == DigitalDocumentAreas.HEADER ? "c" : xmlField.Localizacao == DigitalDocumentAreas.LINES ? "l" : "i";

                            positionToRemove = i;

                            fieldName = xmlField.NomeCampo;
                        }
                        else
                        {
                            // so atualizamos a posição dos posteriores ao campo eliminado
                            if (foundFieldToUpdate)
                                xmlField.Posicao--;
                        }

                        i++;
                    }

                    // delete from DadosTemplate
                    dadosTemplateFields.RemoveAt(positionToRemove);
                    string jsonXmlFields = new JavaScriptSerializer().Serialize(dadosTemplateFields);
                    dataFromDb.XmlFields = jsonXmlFields;

                    _eDigitalSupplierXmlDataRepository.Edit(dataFromDb).Save();

                    // delete from Masterizacao
                    switch (fieldLocal)
                    {
                        case "c":
                            _eDigitalMasterizationHeaderRepository.DeleteMasterization(fkNomeTemplate, fieldName);
                            break;
                        case "l":
                            _eDigitalMasterizationLinesRepository.DeleteMasterization(fkNomeTemplate, fieldName);
                            break;
                        case "i":
                            _eDigitalMasterizationVatRepository.DeleteMasterization(fkNomeTemplate, fieldName);
                            break;
                        default:
                            break;
                    }

                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    Elmah.ErrorSignal.FromCurrentContext().Raise(e);

                    dbContextTransaction.Rollback();
                    Flash.Instance.Error(Texts.DBErrors);
                    return RedirectToAction("SupplierFieldsUpdateAction", new { location = fieldLocal, docTypePkid = nomeTemplate.fktipofact, supplierPkid = nomeTemplate.fkfornecedor });
                }
            }

            Flash.Instance.Success(Texts.DeleteOperationSuccess);
            return RedirectToAction("SupplierFieldsUpdateAction", new { location = fieldLocal, docTypePkid = nomeTemplate.fktipofact, supplierPkid = nomeTemplate.fkfornecedor });
        }

        #endregion

        #region Expiração de documentos
        [PersonAuthorize(Permissions.EDIGITAL_EXPIRACAODOCUMENTOS)]
        public ActionResult DocumentsExpiration()
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            var model = new EDigitalDocumentExpirationVM(_eDigitalDocExpirationRepository.Set.ToList());

            if (Request.IsAjaxRequest())
                return Json(this.PanelContentReply(model), JsonRequestBehavior.AllowGet);
            else
                return View(model);
        }

        [HttpPost]
        [PersonAuthorize(Permissions.EDIGITAL_EXPIRACAODOCUMENTOS)]
        public ActionResult DocumentsExpiration(DigitalDocumentExpirationData data)
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            var model = new EDigitalDocumentExpirationVM(data);

            if (!ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                    return Json(this.ModalContentReply("DocumentsExpiration", model));
                else
                    return PartialView("DocumentsExpiration", model);
            }

            using (var dbContextTransaction = _eDigitalDocExpirationRepository.Context.Database.BeginTransaction())
            {
                try
                {
                    var tratamentoFromDB = _eDigitalDocExpirationRepository.Find(data.WaitListPkid);
                    tratamentoFromDB.TempoExpirarFactura = data.WaitList;
                    _eDigitalDocExpirationRepository.Edit(tratamentoFromDB).Save();

                    var separacaoFromDB = _eDigitalDocExpirationRepository.Find(data.SeparacaoPkid);
                    separacaoFromDB.TempoExpirarFactura = data.Separacao;
                    _eDigitalDocExpirationRepository.Edit(separacaoFromDB).Save();

                    var processamentoFromDB = _eDigitalDocExpirationRepository.Find(data.ProcessamentoPkid);
                    processamentoFromDB.TempoExpirarFactura = data.Processamento;
                    _eDigitalDocExpirationRepository.Edit(processamentoFromDB).Save();

                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

                    dbContextTransaction.Rollback();
                    Flash.Instance.Error(Texts.DBErrors);

                    this.SetPixelAdminPageContext(_pixelAdminPageContext);

                    return View(model);
                }
            }

            Flash.Instance.Success(Texts.EditOperationSuccess);
            return View(model);
        }

        #endregion

        #region Histórico de documentos
        [PersonAuthorize(Permissions.EDIGITAL_HISTORICODOCUMENTOS)]
        public ActionResult DocumentsHistoric()
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            var docsFromDB = _eDigitalDocHistoryRepository
                .Set
                .Where(x => x.Estado == 4)
                .ToList();

            var model = new EDigitalDocumentHistoryVM(docsFromDB, _eDigitalSuppliersRepository, _eDigitalDocHistoryRepository);

            if (Request.IsAjaxRequest())
                return Json(this.PanelContentReply(model), JsonRequestBehavior.AllowGet);
            else
                return View(model);
        }

        #endregion

        #region Definições de sincronização
        [PersonAuthorize(Permissions.EDIGITAL_SINCRONIZACAO)]
        public ActionResult Synchronization()
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            var model = new EDigitalSupplierSyncVM(
                _eDigitalConfigurationsRepository
                .Set
                .Where(c => c.Nome.ToLower() == "sincfornecedoresws"
                    || c.Nome.ToLower() == "sincfornecedoreswsuser"
                    || c.Nome.ToLower() == "sincfornecedoreswspass")
                .ToList()
                );

            if (Request.IsAjaxRequest())
                return Json(this.PanelContentReply(model), JsonRequestBehavior.AllowGet);
            else
                return View(model);
        }

        [HttpPost]
        [PersonAuthorize(Permissions.EDIGITAL_SINCRONIZACAO)]
        public ActionResult Synchronization(DigitalSupplierSyncData data)
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            var model = new EDigitalSupplierSyncVM(data);

            if (!ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                    return Json(this.ModalContentReply("Synchronization", model));
                else
                    return PartialView("Synchronization", model);
            }

            using (var dbContextTransaction = _eDigitalDocExpirationRepository.Context.Database.BeginTransaction())
            {
                try
                {
                    var dataFromDB = _eDigitalConfigurationsRepository.Find(data.SyncUrlConfigPkid);
                    dataFromDB.Dados = data.WantSync ? data.SyncUrlConfig : "";
                    _eDigitalConfigurationsRepository.Edit(dataFromDB).Save();

                    dataFromDB = _eDigitalConfigurationsRepository.Find(data.SyncUserConfigPkid);
                    dataFromDB.Dados = data.WantSync ? data.SyncUserConfig : "";
                    _eDigitalConfigurationsRepository.Edit(dataFromDB).Save();

                    dataFromDB = _eDigitalConfigurationsRepository.Find(data.SyncPassConfigPkid);
                    dataFromDB.Dados = data.WantSync ? data.SyncPassConfig : "";
                    _eDigitalConfigurationsRepository.Edit(dataFromDB).Save();

                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                    dbContextTransaction.Rollback();
                    Flash.Instance.Error(Texts.DBErrors);

                    this.SetPixelAdminPageContext(_pixelAdminPageContext);

                    return View(model);
                }
            }

            Flash.Instance.Success(Texts.EditOperationSuccess);
            return View(model);
        }

        #endregion

        #region Definições de integração
        [PersonAuthorize(Permissions.EDIGITAL_INTEGRACAO)]
        public ActionResult IntegrationConfigurations()
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            var model = _eDigitalConfigurationsRepository.Set
                .FirstOrDefault(c => c.Nome.ToLower() == "gestaoDocIntegracaoURL");

            // check if want document integration
            var wantIntegrationConfig = _eDigitalConfigurationsRepository.Set
                .FirstOrDefault(c => c.Nome.ToLower() == "gestaoDocIntegracao");

            var wantIntegrationConfigValue = false;
            if (wantIntegrationConfig != null)
            {
                try
                {
                    wantIntegrationConfigValue = bool.Parse(wantIntegrationConfig.Dados);
                }
                catch (Exception ex)
                {
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }

            ViewBag.WantIntegration = wantIntegrationConfigValue;

            if (Request.IsAjaxRequest())
                return Json(this.PanelContentReply(model), JsonRequestBehavior.AllowGet);
            else
                return View(model);
        }

        [HttpPost]
        [PersonAuthorize(Permissions.EDIGITAL_INTEGRACAO)]
        public ActionResult IntegrationConfigurations(DigitalConfigurations data)
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            if (!ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                    return Json(this.ModalContentReply("IntegrationConfigurations", data));
                else
                    return PartialView("IntegrationConfigurations", data);
            }

            using (var dbContextTransaction = _eDigitalDocExpirationRepository.Context.Database.BeginTransaction())
            {
                try
                {
                    var dataFromDB = _eDigitalConfigurationsRepository.Find(data.Pkid);
                    dataFromDB.Dados = data.Dados;
                    _eDigitalConfigurationsRepository.Edit(dataFromDB).Save();

                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

                    dbContextTransaction.Rollback();
                    Flash.Instance.Error(Texts.DBErrors);

                    this.SetPixelAdminPageContext(_pixelAdminPageContext);

                    return View(data);
                }
            }

            Flash.Instance.Success(Texts.EditOperationSuccess);
            return View(data);
        }

        #endregion

        #region Estatisticas
        [PersonAuthorize(Permissions.EDIGITAL_STATS)]
        public ActionResult DigitalStats()
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            var model = new StatsDigitalVM(_eDigitalDocHistoryRepository, _eDigitalInstancesRepository, _eDigitalDocTypeRepository);

            return View(model);
        }
        #endregion

        #region Others

        public ActionResult DownloadFile(string filename)
        {
            #region correção para qd o ficheiro nao existe
            //FileStream stream = new FileStream(_eDigitalDocHistoryRepository.GetFilePath(filename), FileMode.Open);

            //FileStreamResult fsr = new FileStreamResult(stream, "application/octet-stream");
            //fsr.FileDownloadName = filename;

            //return fsr;
            #endregion

            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            string path = _eDigitalConfigurationsRepository.GetFilePath(filename);
            if (!String.IsNullOrEmpty(path))
            {
                FileStream stream = new FileStream(_eDigitalConfigurationsRepository.GetFilePath(filename), FileMode.Open);

                FileStreamResult fsr = new FileStreamResult(stream, "application/octet-stream");
                fsr.FileDownloadName = filename;

                return fsr;
            }
            else
            {
                Flash.Instance.Error("Erro ao carregar ficheiro.");
                return RedirectToAction("DocumentsHistoric");
            }
        }


        public ActionResult InactiveModule()
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            //string inactiveModuleText = _context.GetDictionaryValue(DictionaryEntryKeys.InactiveModuleAlertText);

            return View();
        }

        #endregion

        #region Imprinter Counter

        /// <summary>
        /// Retorna o número do próximo contador de documento / página
        /// </summary>
        /// <returns></returns>
        public ContentResult GetNextPageCounter(string company)
        {
            int counter = 1;

            int.TryParse(_eDigitalInstancesRepository.Where(o => o.InternalCode == company).Select(o => o.NextPageCounter).FirstOrDefault(), out counter);


            //int.TryParse(((int)_eDigitalConfigurationsRepository.GetConfigurationByKey(DigitalSettingKeys.NextPageCounter.ToString())).ToString(), out counter);

            // Não aceita contador inferior ou igual a zero
            if (counter <= 0)
            {
                counter = 1;
            }

            return Content(counter.ToString());
        }

        /// <summary>
        /// Reserva determinado número de páginas ao contador e retorna o próximo número. Exemplo: Contador está a 20. Utilizador reserva 5, retorna 21 (o próximo) e atualiza o contador para 25
        /// </summary>
        /// <returns></returns>
        public ContentResult SetAndGetNextPageCounter(int pageCount, string company)
        {
            int counter = 1;
            using (var dbContextTransaction = _eDigitalConfigurationsRepository.Context.Database.BeginTransaction(IsolationLevel.Serializable))
            {
                try
                {
                    // Força lock no registo de setting NextPageTimeStamp
                    _eDigitalConfigurationsRepository.SaveSetting(DigitalSettingKeys.NextPageTimeStamp, DateTime.Now.Ticks);

                    // Obtém o valor do contador atual
                    var auxToSave = _eDigitalInstancesRepository.Where(o => o.InternalCode == company).FirstOrDefault();
                    int.TryParse(auxToSave.NextPageCounter, out counter);

                    //int.TryParse(((int)_eDigitalConfigurationsRepository.GetConfigurationByKey(DigitalSettingKeys.NextPageCounter.ToString())).ToString(), out counter);

                    // Não aceita contador inferior ou igual a zero
                    if (counter <= 0)
                    {
                        counter = 1;
                    }

                    // Calcula o próximo contador (com base no número de páginas)
                    var newCounter = counter + pageCount;
                    auxToSave.NextPageCounter = newCounter.ToString();
                    _eDigitalInstancesRepository.Edit(auxToSave).Save();

                    //_eDigitalConfigurationsRepository.SaveSetting(DigitalSettingKeys.NextPageCounter, newCounter);

                    // Se correu tudo bem faz commit das alterações
                    dbContextTransaction.Commit();
                }
                catch (Exception exc)
                {
                    // Se correu um erro, faz um rollback
                    dbContextTransaction.Rollback();
                    throw;
                }
            }

            return Content(counter.ToString());
        }

        /// <summary>
        /// Grava o próximo contador de documento / página
        /// </summary>
        /// <returns></returns>
        public ContentResult SetLastPageCounter(int nextPageCounter)
        {
            if (nextPageCounter <= 0)
            {
                return Content(false.ToString());
            }

            int currentCounter = 1;
            int.TryParse(((int)_eDigitalConfigurationsRepository.GetConfigurationByKey(DigitalSettingKeys.NextPageCounter.ToString())).ToString(), out currentCounter);

            // O contador enviado é superior ao atual, atualiza na BD
            if (nextPageCounter > currentCounter)
            {
                _eDigitalConfigurationsRepository.SaveSetting(DigitalSettingKeys.NextPageCounter, nextPageCounter);
            }

            return Content(true.ToString());
        }

        #endregion

        #region Processamento
        [PersonAuthorize(Permissions.EDIGITAL_PROCESSAMENTO)]
        public ActionResult DigitalProc(JQueryDataTableParamModel param)
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            if (!Request.IsAjaxRequest())
            {
                return View("DigitalProc");
            }

            var model = _eDigitalMasterizationProcRepository
                        .Where(x => x.Estado == 0)
                        .OrderBy(x => x.Prioridade)
                        .ToList();

            var result = model.Select(o => new
            {
                Prioridade = o.Prioridade,
                nomeficheiro = o.nomeficheiro,
                dtaCriacao = o.dtaCriacao.ToDataTableLongFormat(),
                DtaModificacao = o.DtaModificacao.ToDataTableLongFormat(),
                Utilizador = o.Utilizador
            });

            if (param.iDisplayLength != 0)
            {
                var recordCount = result.Count();
                var records = result
                    .Skip(param.iDisplayStart)
                    .Take(param.iDisplayLength)
                    .ToList();

                var reply = new JsonNetResult();
                reply.Data = new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = recordCount,
                    iTotalDisplayRecords = param.iDisplayLength,
                    aaData = records
                };

                return reply;
            }
            else
            {
                var records = result.Take(10).ToList();
                var reply = new JsonNetResult();
                reply.Data = new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = result.Count(),
                    iTotalDisplayRecords = records.Count,
                    aaData = result
                };

                return reply;
            }
        }
        #endregion

        #region eDigitalMail
        public ActionResult instancesMails(JQueryDataTableParamModel param)
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);
            if (!Request.IsAjaxRequest())
            {
                return View();
            }
            var data = _eDigitalInstancesMailRepository.Set.ToList();


            var r = data.Select(o => new
            {
                id = o.id,
                nameAccount = o.nameAccount,
                email = o.email,
                serverType = o.serverType,
                serverUrl = o.serverURL,
                serverUsername = o.serverUsername,
                serverPassword = o.serverPassword,
                serverPort = o.serverPort,
                instance = String.IsNullOrWhiteSpace(o.instance) ? "N.A." : o.instance,
                isSSL = o.isSSL,
            });

            if (param.iDisplayLength != 0)
            {
                var recordCount = r.Count();
                var records = r
                    .Skip(param.iDisplayStart)
                    .Take(param.iDisplayLength)
                    .ToList();

                var reply = new JsonNetResult();
                reply.Data = new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = recordCount,
                    iTotalDisplayRecords = param.iDisplayLength,
                    aaData = records
                };

                return reply;
            }
            else
            {
                var records = r.Take(10).ToList();
                var reply = new JsonNetResult();
                reply.Data = new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = r.Count(),
                    iTotalDisplayRecords = records.Count,
                    aaData = r
                };

                return reply;
            }
        }

        public ActionResult EditMail(int pkid)
        {
            var model = _eDigitalInstancesMailRepository.Find(pkid);

            DigitalMailData temp = new DigitalMailData
            {
                id = model.id,
                nameAccount = model.nameAccount,
                email = model.email,
                serverURL = model.serverURL,
                serverType = model.serverType,
                serverUsername = model.serverUsername,
                serverPassword = model.serverPassword,
                serverPort = model.serverPort,
                isSSL = model.isSSL,
                instance = model.instance,
                obs = model.obs,
                protocolReception = getProtocolReception(),
                instancesList = getInstanceList()
            };

            if (Request.IsAjaxRequest())
                return Json(this.ModalContentReply(temp), JsonRequestBehavior.AllowGet);
            else
                return PartialView(temp);
        }

        [HttpPost]
        public ActionResult EditMail(int id, DigitalMailData data)
        {
            var dataFromDB = _eDigitalInstancesMailRepository.Find(id);

            if (!ModelState.IsValid)
            {
                Flash.Instance.Error(Texts.FixModelErrors);

                if (Request.IsAjaxRequest())
                    return Json(this.ModalContentReply(dataFromDB));
                else
                    return PartialView(dataFromDB);
            }

            using (var dbContextTransaction = _eDigitalInstancesMailRepository.Context.Database.BeginTransaction())
            {
                try
                {
                    dataFromDB.nameAccount = data.nameAccount;
                    dataFromDB.serverType = data.serverType;
                    dataFromDB.email = data.email;
                    dataFromDB.isSSL = data.isSSL;
                    dataFromDB.obs = data.obs;
                    dataFromDB.serverPassword = data.serverPassword;
                    dataFromDB.serverURL = data.serverURL;
                    dataFromDB.serverUsername = data.serverUsername;
                    dataFromDB.serverPort = data.serverPort;
                    dataFromDB.instance = data.instance;

                    _eDigitalInstancesMailRepository
                        .Edit(dataFromDB)
                        .Save();

                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

                    dbContextTransaction.Rollback();
                    Flash.Instance.Error(Texts.DBErrors);


                    if (Request.IsAjaxRequest())
                        return Json(this.ModalContentReply(dataFromDB));
                    else
                        return PartialView(dataFromDB);
                }
            }


            Flash.Instance.Success(Texts.EditOperationSuccess);
            return Json(this.CloseModalReply());
        }

        public ActionResult CreateInstanceMail()
        {
            var newDB = new DigitalMailData();

            newDB.protocolReception = getProtocolReception();
            newDB.instancesList = getInstanceList();

            if (Request.IsAjaxRequest())
                return Json(this.ModalContentReply(newDB), JsonRequestBehavior.AllowGet);
            else
                return PartialView(newDB);
        }

        [HttpPost]
        public ActionResult CreateInstanceMailS(DigitalMailData data)
        {
            if (!ModelState.IsValid)
            {
                Flash.Instance.Error(Texts.FixModelErrors);

                if (Request.IsAjaxRequest())
                    return Json(this.ModalContentReply(data));
                else
                    return PartialView(data);
            }

            using (var dbContextTransaction = _eDigitalInstancesMailRepository.Context.Database.BeginTransaction())
            {
                try
                {
                    InstancesMail iMail = new InstancesMail
                    {
                        nameAccount = data.nameAccount,
                        serverType = data.serverType,
                        email = data.email,
                        isSSL = data.isSSL,
                        obs = data.obs,
                        serverPassword = data.serverPassword,
                        serverURL = data.serverURL,
                        serverUsername = data.serverUsername,
                        serverPort = data.serverPort,
                        instance = data.instance                        
                    };

                    _eDigitalInstancesMailRepository
                        .Add(iMail)
                        .Save();

                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

                    dbContextTransaction.Rollback();
                    Flash.Instance.Error(Texts.DBErrors);


                    if (Request.IsAjaxRequest())
                        return Json(this.ModalContentReply(data));
                    else
                        return PartialView(data);
                }
            }


            Flash.Instance.Success(Texts.CreateOperationSuccess);
            return Json(this.CloseModalReply());
        }


        #region auxiliarMethods
        private static List<SelectListItem> getProtocolReception()
        {
            return new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "POP3",
                    Value = "pop3"
                },
                new SelectListItem
                {
                    Text = "IMAP",
                    Value = "imap4"
                }
            };
        }
        private List<SelectListItem> getInstanceList()
        {
            List<SelectListItem> listaCompanhias = new List<SelectListItem>();

            foreach(var instance in _eDigitalInstancesRepository.Set.ToList())
            {
                listaCompanhias.Add(new SelectListItem
                {
                    Text = instance.Name,
                    Value = instance.InternalCode
                });
            }

            return listaCompanhias;
        }
        #endregion
        #endregion

        #region instancias

        [PersonAuthorize(Permissions.EDIGITAL_INSTANCES)]
        public ActionResult DigitalInstances(JQueryDataTableParamModel param)
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            if (!_context.UserIdentity.IsEBDActive)
                return View("InactiveModule");

            if (!Request.IsAjaxRequest())
            {
                return View("DigitalInstances");
            }

            var model = _eDigitalInstancesRepository
                .Set
                .OrderBy(x => x.Name)
                .ToList();

            //para a ordenação
            var isNomeSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isNIFSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isValidacoesSortable = Convert.ToBoolean(Request["bSortable_3"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);

            Func<Instances, string> orderingFunction = (c => sortColumnIndex == 0 && isNomeSortable ? c.Name :
                sortColumnIndex == 1 && isNIFSortable ? c.VatNumber.ToString() :
                sortColumnIndex == 2 && isValidacoesSortable ? c.InternalCode.ToString() :
                                                           "");

            var orderedQuery = Request["sSortDir_0"] == "asc" ?
                model.OrderBy(orderingFunction) :
                model.OrderByDescending(orderingFunction);

            if (param.iDisplayLength != 0)
            {
                var recordCount = model.Count();
                var records = orderedQuery
                    .Skip(param.iDisplayStart)
                    .Take(param.iDisplayLength)
                    .ToList();

                var reply = new JsonNetResult();
                reply.Data = new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = recordCount,
                    iTotalDisplayRecords = param.iDisplayLength,
                    aaData = records
                };

                return reply;
            }
            else
            {
                var records = orderedQuery.Take(10).ToList();
                var reply = new JsonNetResult();
                reply.Data = new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = model.Count(),
                    iTotalDisplayRecords = records.Count,
                    aaData = orderedQuery
                };

                return reply;
            }
        }

        [HttpPost]
        [PersonAuthorize(Permissions.EDIGITAL_INSTANCES)]
        public ActionResult CreateInstance()
        {
            var newDB = new Instances { VatNumber = "" };

            if (Request.IsAjaxRequest())
                return Json(this.ModalContentReply(newDB), JsonRequestBehavior.AllowGet);
            else
                return PartialView(newDB);
        }

        [HttpPost]
        //[PersonAuthorize(Permissions.EDIGITAL_FORNECEDORES)]
        public ActionResult CreateDigitalInstance(DigitalCreateInstanceData data)
        {
            var newDB = new Instances
            {
                VatNumber = data.VatNumber,
                Name = data.Name,
                InternalCode = data.InternalCode
            };

            if (!ModelState.IsValid)
            {
                Flash.Instance.Error(Texts.FixModelErrors);

                if (Request.IsAjaxRequest())
                    return Json(this.ModalContentReply("CreateInstance", newDB));
                else
                    return PartialView("CreateInstance", newDB);
            }

            using (var dbContextTransaction = _eDigitalSuppliersRepository.Context.Database.BeginTransaction())
            {
                try
                {
                    // ver se o Contribuinte ja existe
                    if (_eDigitalInstancesRepository.VatNumberExists(data.VatNumber))
                    {
                        Flash.Instance.Error(Texts.NifJaExiste);

                        if (Request.IsAjaxRequest())
                            return Json(this.ModalContentReply("CreateInstance", newDB));
                        else
                            return PartialView("CreateInstance", newDB);
                    }

                    _eDigitalInstancesRepository
                        .Add(newDB)
                        .Save();

                    var temp = newDB.id;

                    // inserir na tabela "CompanySequentialNumbers". A BD chama-se "LaxSageData" mas poderá ser usado este modelo para outros clientes
                    string accountingDataConnStr = Tools.GetDatabaseConnectionString("AccountingData");
                    if (!String.IsNullOrWhiteSpace(accountingDataConnStr))
                        _accountingData.InsertInstanceEmptySequentialNumbers(newDB.id, accountingDataConnStr);

                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    Elmah.ErrorSignal.FromCurrentContext().Raise(e);

                    dbContextTransaction.Rollback();
                    Flash.Instance.Error(Texts.DBErrors);

                    if (Request.IsAjaxRequest())
                        return Json(this.ModalContentReply("CreateInstance", newDB));
                    else
                        return PartialView("CreateInstance", newDB);
                }
            }

            Flash.Instance.Success(Texts.EditOperationSuccess);
            return Json(this.CloseModalReply());
        }

        #endregion

        #region upload dados contabilísticos

        [PersonAuthorize(Permissions.EDIGITAL_ACCOUNTING_DATA)]
        public ActionResult DigitalAccountingData()
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            if (Request.IsAjaxRequest())
                return Json(this.PanelContentReply(null), JsonRequestBehavior.AllowGet);
            else
                return View();
        }

        [HttpPost]
        public ActionResult UploadAccountingData(System.Web.HttpPostedFileBase FicheiroCSV)
        {
            try
            {
                if (FicheiroCSV.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(String.Format("{0}_{1}", FicheiroCSV.FileName, DateTime.Now.Ticks.ToString()));
                    var path = Path.Combine(Server.MapPath("~/App_Data/"), fileName);
                    FicheiroCSV.SaveAs(path);

                    List<string> errors = Tools.ManageAccountingCSV(path, _eDigitalInstancesRepository, _eDigitalDocTypeRepository, _eDigitalSuppliersRepository);

                    if (errors.Count == 0)
                        Flash.Instance.Success(Texts.CreateOperationSuccess);
                    else
                    {
                        string errorsToView = String.Join("<br/>", errors);

                        Flash.Instance.Warning(Texts.AccountingCsvErrors, errorsToView);
                    }
                }
                else
                {
                    Flash.Instance.Error(Texts.FileIsEmpty);
                }

                return RedirectToAction("DigitalAccountingData");
            }
            catch (Exception e)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(e);

                Flash.Instance.Error(Texts.XmlNotUploaded, e.Message);

                return RedirectToAction("DigitalAccountingData");
            }
        }

        #endregion
    }
}