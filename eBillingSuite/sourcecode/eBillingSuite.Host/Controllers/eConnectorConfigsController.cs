using eBillingSuite.Globalization;
using eBillingSuite.Models;
using eBillingSuite.Repositories;
using eBillingSuite.ViewModels;
using MvcFlash.Core;
using MvcFlash.Core.Extensions;
using Shortcut.PixelAdmin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FluentValidation.Results;
using FluentValidation;
using System.Text.RegularExpressions;
using System.Xml;
using Sgml;
using System.Text;
using eBillingSuite;
using eBillingSuite.Security;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;
using eBillingSuite.Enumerations;
using eBillingSuite.Model;
using eBillingSuite.Model.eBillingConfigurations;
using eBillingSuite.Model.EBC_DB;
using eBillingSuite.Resources;

namespace eBillingSuite.Controllers
{
    public class eConnectorConfigsController : Controller
    {
        private static List<string> allowed = new List<string> { ".cer", ".pfx", ".p7b", ".p12",".cert" };
        private const string emailregex = @"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$";

        private IPixelAdminPageContext _pixelAdminPageContext;
        protected readonly IeBillingSuiteRequestContext _context;
        private IECCListRepositories _eCConfigRepositories;
        private IPIInfoConfigurationsRepository _pIInfoConfigurationsRepository;

        public bool wantsRealTimeReports { get; private set; }

        public eConnectorConfigsController(
            IPixelAdminPageContext pixelAdminPageContext,
            IECCListRepositories eCConfigRepositories,
            IPIInfoConfigurationsRepository pIInfoConfigurationsRepository,
            IeBillingSuiteRequestContext context
        )
        {
            _pixelAdminPageContext = pixelAdminPageContext;
            _eCConfigRepositories = eCConfigRepositories;
            _context = context;
            _pIInfoConfigurationsRepository = pIInfoConfigurationsRepository;
        }

        #region EBCCONFIGURATIONS

        [PersonAuthorize(Permissions.ECONNECTOR_CONFIG_CONTAEMAIL)]
        public ActionResult Index(Guid? ID)
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);
            var listaInstances = _eCConfigRepositories.instancesRepository.GetEBC_Instances(_context.UserIdentity.Instances);
            if (listaInstances == null)
            {
                Flash.Instance.Success(Texts.InstancesNULL);
                return RedirectToAction("Index", "Home");
            }

            Guid guidInstance = ID.HasValue ? ID.Value : listaInstances.FirstOrDefault().PKID;
            var instance = _eCConfigRepositories.instancesRepository.Where(o=> o.PKID == guidInstance).FirstOrDefault();


            var model = _eCConfigRepositories.eBCConfigurationsRepository
                       .GetConfigsByID(instance.PKID);

            if (model == null || model.Count <= 0)
            {
                Flash.Instance.Success(Texts.FaltaDadosEBCCONFIGURATIONS);
                return RedirectToAction("Index", "Home");
            }

            var saphetyCred = _eCConfigRepositories.saphetyCredentialsRepository.Set.FirstOrDefault(d => d.instance == instance.NIF);

            var modelVM = new EBCConfigurationsVM(
                model,
                instance.PKID,
                _eCConfigRepositories.eBCConfigurationsRepository,
                _eCConfigRepositories.instancesRepository, saphetyCred, _context);

            if (Request.IsAjaxRequest())
                return Json(this.PanelContentReply(modelVM), JsonRequestBehavior.AllowGet);
            else
                return View(modelVM);
        }

        [HttpPost]
        [PersonAuthorize(Permissions.ECONNECTOR_CONFIG_CONTAEMAIL)]
        public ActionResult Index(Guid ID, List<ConfigData> configs, SaphetyCredentialsData _saphetyCred)
        {
            using (var dbContextTransaction = _eCConfigRepositories.instancesRepository.Context.Database.BeginTransaction())
            {
                try
                {
                    foreach (ConfigData cd in configs)
                    {
                        var dataFromDB = _eCConfigRepositories.eBCConfigurationsRepository.Where(d => d.PKID == cd.PKID).FirstOrDefault();

                        if (cd.Text.ToLower() == "protocolo de recepção")
                        {
                            switch (cd.Value)
                            {
                                case "1":
                                    cd.Value = ReceptionProtocols.POP3;
                                    break;
                                case "2":
                                    cd.Value = ReceptionProtocols.IMAP4;
                                    break;
                                case "3":
                                    cd.Value = ReceptionProtocols.HTTP;
                                    break;
                                default:
                                    cd.Value = ReceptionProtocols.POP3;
                                    break;
                            }
                        }

                        if (cd.Value == null)
                            dataFromDB.KeyValue = "";
                        else
                            dataFromDB.KeyValue = cd.Value;

                        _eCConfigRepositories.eBCConfigurationsRepository
                            .Edit(dataFromDB)
                            .Save();
                    }

                    //update the SaphetyCredentials
                    if ((String.IsNullOrWhiteSpace(_saphetyCred.instance)) && !((String.IsNullOrWhiteSpace(_saphetyCred.username))))
                    {
                        _eCConfigRepositories.saphetyCredentialsRepository
                            .Add(new SaphetyCredentials
                            {
                                pkid = Guid.NewGuid(),
                                instance = _eCConfigRepositories.instancesRepository.Set.FirstOrDefault(i => i.PKID == ID).NIF,
                                username = _saphetyCred.username,
                                password = _saphetyCred.password
                            })
                            .Save();
                    }
                    else if (!(String.IsNullOrWhiteSpace(_saphetyCred.instance)))
                    {
                        if (String.IsNullOrWhiteSpace(_saphetyCred.username) || String.IsNullOrWhiteSpace(_saphetyCred.password))
                        {
                            dbContextTransaction.Rollback();
                            Flash.Instance.Error("Existem campos por preencher");

                            this.SetPixelAdminPageContext(_pixelAdminPageContext);

                            var model = _eCConfigRepositories.eBCConfigurationsRepository
                                .GetConfigsByID(ID);

                            var Instance = _eCConfigRepositories.instancesRepository.GetEBC_Instances(_context.UserIdentity.Instances).FirstOrDefault();

                            var saphetyCred = _eCConfigRepositories.saphetyCredentialsRepository.Set.FirstOrDefault(d => d.instance == Instance.NIF);

                            var modelVM = new EBCConfigurationsVM(
                                        model,
                                        Instance.PKID,
                                        _eCConfigRepositories.eBCConfigurationsRepository,
                                        _eCConfigRepositories.instancesRepository, saphetyCred, _context);

                            return View(modelVM);
                        }

                        //update
                        var dFromDB = _eCConfigRepositories.saphetyCredentialsRepository.Set.FirstOrDefault(sc => sc.instance == _saphetyCred.instance);
                        dFromDB.password = _saphetyCred.password;
                        dFromDB.username = _saphetyCred.username;
                        _eCConfigRepositories
                            .saphetyCredentialsRepository
                            .Edit(dFromDB)
                            .Save();
                    }

                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

                    dbContextTransaction.Rollback();
                    Flash.Instance.Error(Texts.DBErrors);

                    this.SetPixelAdminPageContext(_pixelAdminPageContext);

                    var model = _eCConfigRepositories.eBCConfigurationsRepository
                        .GetConfigsByID(ID);

                    var Instance = _eCConfigRepositories.instancesRepository.GetEBC_Instances(_context.UserIdentity.Instances).FirstOrDefault();

                    var saphetyCred = _eCConfigRepositories.saphetyCredentialsRepository.Set.FirstOrDefault(d => d.instance == Instance.NIF);

                    var modelVM = new EBCConfigurationsVM(
                                model,
                                Instance.PKID,
                                _eCConfigRepositories.eBCConfigurationsRepository,
                                _eCConfigRepositories.instancesRepository, saphetyCred, _context);

                    return View(modelVM);
                }
            }

            Flash.Instance.Success(Texts.EditOperationSuccess);

            return RedirectToAction("Index", new { ID = ID });
        }

        #endregion

        #region CERTIFICATECONFIGURATIONS
        [PersonAuthorize(Permissions.ECONNECTOR_CONFIG_CERTIFICADODIGITAL)]
        public ActionResult DigitalCertificate(Guid? ID)
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            var listaInstances = _eCConfigRepositories.instancesRepository.GetEBC_Instances(_context.UserIdentity.Instances);
            if (listaInstances == null)
            {
                Flash.Instance.Success(Texts.InstancesNULL);
                return RedirectToAction("Index");
            }

            Guid guidInstance = ID.HasValue ? ID.Value : listaInstances.FirstOrDefault().PKID;
            var instance = _eCConfigRepositories.instancesRepository.Where(o => o.PKID == guidInstance).FirstOrDefault();

            var model = _eCConfigRepositories.eBCCertificatesRepository
                .GetConfigsByID(instance.PKID);

            if (model == null || model.Count <= 0)
            {
                Flash.Instance.Success(Texts.FaltaDadosDigitalCertificate);
                return RedirectToAction("Index");
            }


            var modelVM = new EBCDigitalCertificateVM(
                model,
                //instance.PKID,
                _eCConfigRepositories,
                _context);

            if (Request.IsAjaxRequest())
                return Json(this.PanelContentReply(modelVM), JsonRequestBehavior.AllowGet);
            else
                return View(modelVM);
        }

        [HttpPost]
        [PersonAuthorize(Permissions.ECONNECTOR_CONFIG_CERTIFICADODIGITAL)]
        public ActionResult DigitalCertificate(Guid ID, MarketCertData data, HttpPostedFileBase NovoFicheiro)
        {
            using (var dbContextTransaction = _eCConfigRepositories.instancesRepository.Context.Database.BeginTransaction())
            {
                try
                {
                    //faz as validações
                    if (!ModelState.IsValid)
                    {
                        this.SetPixelAdminPageContext(_pixelAdminPageContext);

                        //dbContextTransaction.Rollback();
                        //Flash.Instance.Error(Texts.DBErrors);

                        var model = _eCConfigRepositories.eBCCertificatesRepository
                            .GetConfigsByID(ID);

                        var modelVM = new EBCDigitalCertificateVM(
                                model,
                                _eCConfigRepositories,
                                _context);

                        return View(modelVM);
                    }

                    if (NovoFicheiro != null && NovoFicheiro.ContentLength > 0)
                    {
                        var ext = Path.GetExtension(NovoFicheiro.FileName);
                        if (!allowed.Contains(ext))
                        {
                            dbContextTransaction.Rollback();

                            this.SetPixelAdminPageContext(_pixelAdminPageContext);

                            Flash.Instance.Error(Texts.FicheiroFormatoInvalido);

                            var model = _eCConfigRepositories.eBCCertificatesRepository
                                .GetConfigsByID(ID);

                            var modelVM = new EBCDigitalCertificateVM(
                                    model,
                                    _eCConfigRepositories,
                                    _context);

                            return View(modelVM);
                        }

                        data.MarketInfo.Caminho = _eCConfigRepositories.suiteConfigurationsRepository.ConfigValue("InstallDir") + @"Certificados\" + NovoFicheiro.FileName;

                        //// Save certificate file
                        try
                        {
                            NovoFicheiro.SaveAs(data.MarketInfo.Caminho);
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }

                    var dataMarketFromDB = _eCConfigRepositories.eBCMarketCertificatesRepository
                        .Where(mc => mc.fkInstance == ID
                            && mc.fkMercado == data.MarketInfo.fkMercado)
                            .FirstOrDefault();

                    var dataCertDetailsFromDB = _eCConfigRepositories.eCertificatesDetailsRepository
                        .Where(mc => mc.fkInstance == ID
                            && mc.fkMercado == data.MarketInfo.fkMercado)
                            .FirstOrDefault();

                    if (dataCertDetailsFromDB == null)
                    {
                        dataCertDetailsFromDB = new EBC_CertSignatureDetails
                        {
                            fkInstance = ID,
                            fkMercado = data.MarketInfo.fkMercado,
                            Author = data.CertDetails.Author.Trim(),
                            Title = data.CertDetails.Title.Trim(),
                            Subject = data.CertDetails.Subject.Trim(),
                            Keywords = data.CertDetails.Keywords.Trim(),
                            Creator = data.CertDetails.Creator.Trim(),
                            Producer = data.CertDetails.Producer.Trim(),
                            SigReason = data.CertDetails.SigReason.Trim(),
                            SigContact = data.CertDetails.SigContact.Trim(),
                            SigLocation = data.CertDetails.SigLocation.Trim(),
                            SigVisible = data.CertDetails.SigVisible
                        };

                        _eCConfigRepositories
                            .eCertificatesDetailsRepository
                            .Add(dataCertDetailsFromDB)
                            .Save();
                    }
                    else
                    {
                        dataCertDetailsFromDB.Author = data.CertDetails.Author.Trim();
                        dataCertDetailsFromDB.Title = data.CertDetails.Title.Trim();
                        dataCertDetailsFromDB.Subject = data.CertDetails.Subject.Trim();
                        dataCertDetailsFromDB.Keywords = data.CertDetails.Keywords.Trim();
                        dataCertDetailsFromDB.Creator = data.CertDetails.Creator.Trim();
                        dataCertDetailsFromDB.Producer = data.CertDetails.Producer.Trim();
                        dataCertDetailsFromDB.SigReason = data.CertDetails.SigReason.Trim();
                        dataCertDetailsFromDB.SigContact = data.CertDetails.SigContact.Trim();
                        dataCertDetailsFromDB.SigLocation = data.CertDetails.SigLocation.Trim();
                        dataCertDetailsFromDB.SigVisible = data.CertDetails.SigVisible;

                        _eCConfigRepositories
                            .eCertificatesDetailsRepository
                            .Edit(dataCertDetailsFromDB)
                            .Save();
                    }

                    if (!dataMarketFromDB.PasswordCert.Equals(data.MarketInfo.PasswordCert))
                        dataMarketFromDB.PasswordCert = PasswordEncoding(data.MarketInfo.PasswordCert);


                    //atualizar o dataMarket
                    dataMarketFromDB.Caminho = data.MarketInfo.Caminho;
                    dataMarketFromDB.serialnumber = data.MarketInfo.serialnumber;
                    dataMarketFromDB.fkMercado = data.MarketInfo.fkMercado;
                    dataMarketFromDB.certEmailNotification = data.MarketInfo.certEmailNotification;
                    dataMarketFromDB.usePDFWS = data.MarketInfo.usePDFWS;
                    dataMarketFromDB.WS_Url = data.MarketInfo.WS_Url;
                    dataMarketFromDB.WS_Username = data.MarketInfo.WS_Username;
                    dataMarketFromDB.WS_Password = data.MarketInfo.WS_Password;
                    dataMarketFromDB.WS_Domain = data.MarketInfo.WS_Domain;


                    _eCConfigRepositories
                        .eBCMarketCertificatesRepository
                        .Edit(dataMarketFromDB)
                        .Save();

                    dbContextTransaction.Commit();
                }

                catch (Exception ex)
                {
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

                    dbContextTransaction.Rollback();
                    Flash.Instance.Error(Texts.DBErrors);

                    this.SetPixelAdminPageContext(_pixelAdminPageContext);

                    var model = _eCConfigRepositories.eBCCertificatesRepository
                            .GetConfigsByID(ID);

                    var modelVM = new EBCDigitalCertificateVM(
                            model,
                            _eCConfigRepositories,
                            _context);

                    return View(modelVM);
                }
            }

            Flash.Instance.Success(Texts.EditOperationSuccess);

            return RedirectToAction("DigitalCertificate", new { ID = ID });
        }

        private string PasswordEncoding(string password)
        {
            //Encoding the password
            byte[] encData_byte = new byte[password.Length];
            encData_byte = System.Text.Encoding.UTF8.GetBytes(password);

            return Convert.ToBase64String(encData_byte);
        }
        #endregion

        #region EMAILCONFIGURATIONS
        [PersonAuthorize(Permissions.ECONNECTOR_CONFIG_DEFINICOESEMAIL)]
        public ActionResult EmailConfigs(Guid? ID)
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            var specificOption = _eCConfigRepositories
                .instancesRepository
                .GetSpecificOptionIDByInstance(ID.HasValue ? ID.Value : _eCConfigRepositories.instancesRepository.GetEBC_Instances(_context.UserIdentity.Instances).FirstOrDefault().PKID);

            var model = _eCConfigRepositories.connectorSpecificDeliveryOptionsRepository.GetSpecificOptionsByID(specificOption);

            var modelVM = new EBCEmailConfigVM(model, _eCConfigRepositories, _context, ID.HasValue ? ID.Value : _eCConfigRepositories.instancesRepository.GetEBC_Instances(_context.UserIdentity.Instances).FirstOrDefault().PKID);

            if (Request.IsAjaxRequest())
                return Json(this.PanelContentReply(modelVM), JsonRequestBehavior.AllowGet);
            else
                return View(modelVM);
        }

        [HttpPost]
        [PersonAuthorize(Permissions.ECONNECTOR_CONFIG_DEFINICOESEMAIL)]
        public ActionResult EmailConfigs(Guid ID, List<ConfigData> configs, SpecificOptionsData data)
        {
            using (var dbContextTransaction = _eCConfigRepositories.instancesRepository.Context.Database.BeginTransaction())
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        this.SetPixelAdminPageContext(_pixelAdminPageContext);

                        var specificOption = _eCConfigRepositories
                                                    .instancesRepository
                                                    .GetSpecificOptionIDByInstance(ID);

                        var model = _eCConfigRepositories.connectorSpecificDeliveryOptionsRepository.GetSpecificOptionsByID(specificOption);

                        var modelVM = new EBCEmailConfigVM(model, _eCConfigRepositories, _context, ID);

                        return View(modelVM);
                    }

                    var dataFromDB = _eCConfigRepositories.connectorSpecificDeliveryOptionsRepository.Find(data.PKID);
                    dataFromDB.NotificationEmailFunctional = data.NotificationEmailFunctional;
                    dataFromDB.NotificationEmailTecnical = data.NotificationEmailTecnical;
                    dataFromDB.resendAfterCount = data.resendAfterCount;
                    dataFromDB.resendAfterPeriodUnit = data.resendAfterPeriodUnit;
                    dataFromDB.resendAfterPeriodUnitType = data.resendAfterPeriodUnitType;
                    dataFromDB.WaitForEfectiveResponseUnit = data.WaitForEfectiveResponseUnit;
                    dataFromDB.WaitForEffectiveResponseUnitType = data.WaitForEffectiveResponseUnitType;

                    //update SpecificDeliveryOptions
                    _eCConfigRepositories
                        .connectorSpecificDeliveryOptionsRepository
                        .Edit(dataFromDB)
                        .Save();

                    //Atualiza as configurações - já não são usadas (manter aqui pois poderá ser necessário no futuro)
                    //foreach (ConfigData cd in configs)
                    //{
                    //	var FromDB = _eCConfigRepositories.eBCConfigurationsRepository.Where(d => d.PKID == cd.PKID).FirstOrDefault();
                    //	if (cd.Value == null)
                    //		FromDB.KeyValue = "";
                    //	else
                    //		FromDB.KeyValue = cd.Value;

                    //	_eCConfigRepositories.eBCConfigurationsRepository
                    //		.Edit(FromDB)
                    //		.Save();
                    //}

                    dbContextTransaction.Commit();

                }
                catch (Exception ex)
                {
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

                    dbContextTransaction.Rollback();
                    Flash.Instance.Error(Texts.DBErrors);

                    this.SetPixelAdminPageContext(_pixelAdminPageContext);

                    var specificOption = _eCConfigRepositories
                            .instancesRepository
                            .GetSpecificOptionIDByInstance(ID);

                    var model = _eCConfigRepositories.connectorSpecificDeliveryOptionsRepository.GetSpecificOptionsByID(specificOption);

                    var modelVM = new EBCEmailConfigVM(model, _eCConfigRepositories, _context, ID);

                    return View(modelVM);
                }
            }

            Flash.Instance.Success(Texts.EditOperationSuccess);

            return RedirectToAction("EmailConfigs", new { ID = ID });
        }
        #endregion

        #region RealTimeAlertsConfigurations
        [PersonAuthorize(Permissions.ECONNECTOR_CONFIG_DEFINICOESALERTA)]
        public ActionResult AlertsConfigs(Guid? ID)
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            if (!ID.HasValue)
                ID = _eCConfigRepositories.instancesRepository.GetEBC_Instances(_context.UserIdentity.Instances).FirstOrDefault().PKID;

            var model = _eCConfigRepositories
                .eBCConfigurationsRepository
                    .Where(ebcc => ebcc.ConfigSuiteType == "ebcRTime"
                    && ebcc.FKInstanceID == ID)
                    .OrderBy(ebbc => ebbc.Position)
                    .ToList();

            var modelVM = new EBCRealTimeAlertsConfigVM(model, _eCConfigRepositories, _context, ID.Value);

            if (Request.IsAjaxRequest())
                return Json(this.PanelContentReply(modelVM), JsonRequestBehavior.AllowGet);
            else
                return View(modelVM);
        }

        [HttpPost]
        [PersonAuthorize(Permissions.ECONNECTOR_CONFIG_DEFINICOESALERTA)]
        public ActionResult AlertsConfigs(Guid ID, List<ConfigData> configs)
        {
            using (var dbContextTransaction = _eCConfigRepositories.instancesRepository.Context.Database.BeginTransaction())
            {
                try
                {
                    foreach (ConfigData cd in configs)
                    {
                        bool result = false;
                        bool valueIsBool = bool.TryParse(cd.Value, out result);
                        if (!valueIsBool)
                        {
                            if (wantsRealTimeReports)
                            {
                                string[] emails = cd.Value.Split(',');

                                foreach (string email in emails)
                                {
                                    if (!Regex.IsMatch(email, emailregex, RegexOptions.IgnoreCase))
                                    {
                                        Flash.Instance.Error(Texts.EmailIncorrectFormat);

                                        this.SetPixelAdminPageContext(_pixelAdminPageContext);

                                        var model = _eCConfigRepositories
                                            .eBCConfigurationsRepository
                                                .Where(ebcc => ebcc.ConfigSuiteType == "ebcRTime"
                                                && ebcc.FKInstanceID == ID)
                                                .OrderBy(ebbc => ebbc.Position)
                                                .ToList();

                                        var modelVM = new EBCRealTimeAlertsConfigVM(model, _eCConfigRepositories, _context, ID);

                                        return View(modelVM);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //is boolean
                            wantsRealTimeReports = bool.Parse(cd.Value);
                        }

                        var dataFromDB = _eCConfigRepositories
                                .eBCConfigurationsRepository
                                    .Where(d => d.PKID == cd.PKID)
                                    .FirstOrDefault();

                        if (cd.Value == null)
                            dataFromDB.KeyValue = "";
                        else
                            dataFromDB.KeyValue = cd.Value;

                        _eCConfigRepositories.eBCConfigurationsRepository
                            .Edit(dataFromDB)
                            .Save();
                    }

                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

                    dbContextTransaction.Rollback();
                    Flash.Instance.Error(Texts.DBErrors);

                    this.SetPixelAdminPageContext(_pixelAdminPageContext);

                    var model = _eCConfigRepositories
                                .eBCConfigurationsRepository
                                    .Where(ebcc => ebcc.ConfigSuiteType == "ebcRTime"
                                    && ebcc.FKInstanceID == ID)
                                    .OrderBy(ebbc => ebbc.Position)
                                    .ToList();

                    var modelVM = new EBCRealTimeAlertsConfigVM(model, _eCConfigRepositories, _context, ID);

                    return View(modelVM);
                }
            }

            Flash.Instance.Success(Texts.EditOperationSuccess);

            return RedirectToAction("AlertsConfigs", new { ID = ID });
        }
        #endregion

        #region PackageConfigurations
        [PersonAuthorize(Permissions.ECONNECTOR_CONFIG_DEFINICOESENVELOPE)]
        public ActionResult PacketConfigs(Guid? ID)
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            if (!ID.HasValue)
                ID = _eCConfigRepositories.instancesRepository.GetEBC_Instances(_context.UserIdentity.Instances).FirstOrDefault().PKID;

            var FKEmailContentID = _eCConfigRepositories.instancesRepository.Find(ID).FKEmailContentID;
            var model = _eCConfigRepositories.connectorEmailContentRepository.Find(FKEmailContentID);
            var modelVM = new EBCPacketConfigsVM(model, _eCConfigRepositories, _context, ID.Value);

            if (Request.IsAjaxRequest())
                return Json(this.PanelContentReply(modelVM), JsonRequestBehavior.AllowGet);
            else
                return View(modelVM);
        }

        [HttpPost]
        [ValidateInput(false)]
        [PersonAuthorize(Permissions.ECONNECTOR_CONFIG_DEFINICOESENVELOPE)]
        public ActionResult PacketConfigs(Guid ID, PacketConfigsData data, string EmailBody)
        {
            using (var dbContextTransaction = _eCConfigRepositories.instancesRepository.Context.Database.BeginTransaction())
            {
                try
                {
                    var dataFromDB = _eCConfigRepositories.connectorEmailContentRepository.Find(data.PKID);
                    dataFromDB.Subject = TransformStringIntoXSL(data.Subject, 0, data.XML);
                    dataFromDB.Body = TransformStringIntoXSL(EmailBody.Replace("&nbsp;", " "), 1, data.XML);

                    _eCConfigRepositories
                        .connectorEmailContentRepository
                        .Edit(dataFromDB)
                        .Save();

                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

                    dbContextTransaction.Rollback();
                    Flash.Instance.Error(Texts.DBErrors);

                    this.SetPixelAdminPageContext(_pixelAdminPageContext);

                    var FKEmailContentID = _eCConfigRepositories.instancesRepository.Find(ID).FKEmailContentID;
                    var model = _eCConfigRepositories.connectorEmailContentRepository.Find(FKEmailContentID);
                    var modelVM = new EBCPacketConfigsVM(model, _eCConfigRepositories, _context, ID);

                    return View(modelVM);
                }
            }

            Flash.Instance.Success(Texts.EditOperationSuccess);

            return RedirectToAction("PacketConfigs", new { ID = ID });
        }
        #endregion

        #region TXTConfigurations
        [PersonAuthorize(Permissions.ECONNECTOR_CONFIG_CONFIGURACAOTXT)]
        public ActionResult TXTConfigs(Guid? id)
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            if (!id.HasValue)
                id = _eCConfigRepositories.instancesRepository.GetEBC_Instances(_context.UserIdentity.Instances).FirstOrDefault().PKID;

            var model = _eCConfigRepositories.connectorConfigTXTRepository.GetConfigTXTbyInstanceID(id.Value);
            var modelVM = new EBCTXTConfigsVM(model, _eCConfigRepositories, _context, id.Value);

            if (Request.IsAjaxRequest())
                return Json(this.PanelContentReply(modelVM), JsonRequestBehavior.AllowGet);
            else
                return View(modelVM);

        }

        [HttpPost]
        [PersonAuthorize(Permissions.ECONNECTOR_CONFIG_CONFIGURACAOTXT)]
        public ActionResult CreateTXTConfigs(Guid id)
        {
            var newDB = new EBC_ConfigTXT
            {
                pkid = Guid.NewGuid(),
                NomeCampo = _eCConfigRepositories.connectorConfigTXTRepository
                                .Set
                                .GroupBy(ect => ect.NomeCampo)
                                .Select(g => new { NomeCampo = g.FirstOrDefault().NomeCampo }).FirstOrDefault().NomeCampo,
                FKInstanceID = id,
                Posicao = _eCConfigRepositories
                            .connectorConfigTXTRepository
                            .GetMaxPositionFromType(_eCConfigRepositories.connectorConfigTXTRepository
                                .Set
                                .GroupBy(ect => ect.NomeCampo)
                                .Select(g => new { NomeCampo = g.FirstOrDefault().NomeCampo }).FirstOrDefault().NomeCampo),
                Tipo = _eCConfigRepositories.connectorConfigTXTRepository
                                .Set
                                .GroupBy(ect => ect.NomeCampo)
                                .Select(g => new { Tipo = g.FirstOrDefault().Tipo }).FirstOrDefault().Tipo,
                Regex = _eCConfigRepositories.connectorRegexTypesRepository.Set.FirstOrDefault().Regex

            };

            _eCConfigRepositories
                .connectorConfigTXTRepository
                .Add(newDB)
                .Save();

            if (Request.IsAjaxRequest())
                return Json(this.HandleCreateReply("EditTXTConfigs", new { pkid = newDB.pkid, instance = id }));
            else
                return RedirectToAction("EditTXTConfigs", new { pkid = newDB.pkid, instance = id });
        }

        [PersonAuthorize(Permissions.ECONNECTOR_CONFIG_CONFIGURACAOTXT)]
        public ActionResult EditTXTConfigs(Guid pkid, Guid instance)
        {
            var model = _eCConfigRepositories.connectorConfigTXTRepository.Find(pkid);

            var modelVM = new EBCTXTConfigsVM(model, _eCConfigRepositories, _context, instance, pkid);

            if (Request.IsAjaxRequest())
                return Json(this.ModalContentReply(modelVM), JsonRequestBehavior.AllowGet);
            else
                return PartialView(modelVM);
        }

        [HttpPost]
        [PersonAuthorize(Permissions.ECONNECTOR_CONFIG_CONFIGURACAOTXT)]
        public ActionResult DeleteTXTConfigs(Guid pkid)
        {
            using (var dbContextTransaction = _eCConfigRepositories.instancesRepository.Context.Database.BeginTransaction())
            {
                try
                {
                    var dataFromDB = _eCConfigRepositories.connectorConfigTXTRepository.Find(pkid);
                    if (dataFromDB != null)
                    {
                        _eCConfigRepositories.connectorConfigTXTRepository
                        .Delete(dataFromDB)
                        .Save();


                        //eliminou o registo. Agora vai buscar todos os registos com posicao acima daquela
                        var registos = _eCConfigRepositories
                            .connectorConfigTXTRepository
                            .Where(ct => ct.Tipo == dataFromDB.Tipo)
                            .ToList();

                        int value;
                        bool result = int.TryParse(dataFromDB.Posicao, out value);

                        foreach (var r in registos)
                        {
                            if (int.Parse(r.Posicao) > int.Parse(dataFromDB.Posicao))
                            {
                                if (value < 10)
                                    r.Posicao = "0" + (int.Parse(r.Posicao) - 1).ToString();
                                else
                                    r.Posicao = (int.Parse(r.Posicao) - 1).ToString();

                                _eCConfigRepositories.connectorConfigTXTRepository.Edit(r).Save();
                            }
                        }
                        dbContextTransaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

                    dbContextTransaction.Rollback();
                    Flash.Instance.Error(Texts.DBErrors);
                    return RedirectToAction("TXTConfigs");
                }
            }

            return RedirectToAction("TXTConfigs");
        }

        [HttpPost]
        [PersonAuthorize(Permissions.ECONNECTOR_CONFIG_CONFIGURACAOTXT)]
        public ActionResult EditTXTConfigs(Guid pkid, Guid FKInstanceID, ConfigTXTData data)
        {
            var dataFromDB = _eCConfigRepositories.connectorConfigTXTRepository.Find(pkid);

            if (!ModelState.IsValid)
            {
                this.SetPixelAdminPageContext(_pixelAdminPageContext);

                var model = _eCConfigRepositories.connectorConfigTXTRepository.Find(pkid);
                var modelVM = new EBCTXTConfigsVM(model, _eCConfigRepositories, _context, FKInstanceID, pkid);

                if (Request.IsAjaxRequest())
                    return Json(this.ModalContentReply(modelVM));
                else
                    return PartialView(modelVM);
            }

            using (var dbContextTransaction = _eCConfigRepositories.instancesRepository.Context.Database.BeginTransaction())
            {
                try
                {
                    dataFromDB.NomeCampo = data.NomeCampo;
                    dataFromDB.Posicao = data.Posicao;
                    dataFromDB.Regex = _eCConfigRepositories.connectorRegexTypesRepository.Where(rt => rt.TipoRegex == data.Regex).FirstOrDefault().Regex;
                    dataFromDB.Tipo = data.Tipo;

                    _eCConfigRepositories.connectorConfigTXTRepository
                        .Edit(dataFromDB)
                        .Save();

                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

                    dbContextTransaction.Rollback();
                    Flash.Instance.Error(Texts.DBErrors);

                    this.SetPixelAdminPageContext(_pixelAdminPageContext);

                    var model = _eCConfigRepositories.connectorConfigTXTRepository.Find(pkid);
                    var modelVM = new EBCTXTConfigsVM(model, _eCConfigRepositories, _context, FKInstanceID, pkid);

                    if (Request.IsAjaxRequest())
                        return Json(this.ModalContentReply(modelVM));
                    else
                        return PartialView(modelVM);
                }
            }

            Flash.Instance.Success(Texts.EditOperationSuccess);
            return Json(this.CloseModalReply());
        }
        #endregion

        #region TXTInboundConfigurations
        [PersonAuthorize(Permissions.ECONNECTOR_CONFIG_MAPEARTXTINBOUND)]
        public ActionResult TXTInboundConfigs(Guid? id)
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            if (!id.HasValue)
                id = _eCConfigRepositories.instancesRepository.GetEBC_Instances(_context.UserIdentity.Instances).FirstOrDefault().PKID;

            var model = _eCConfigRepositories.connectorConfigInboundTXTRepository.GetConfigTXTbyInstanceID(id.Value);
            var modelVM = new EBCTXTInboundConfigsVM(model, _eCConfigRepositories, _context, id.Value);

            if (Request.IsAjaxRequest())
                return Json(this.PanelContentReply(modelVM), JsonRequestBehavior.AllowGet);
            else
                return View(modelVM);
        }

        [HttpPost]
        [PersonAuthorize(Permissions.ECONNECTOR_CONFIG_MAPEARTXTINBOUND)]
        public ActionResult CreateTXTInboundConfigs(Guid id)
        {
            var newDB = new EBC_XmlToTxtTransform
            {
                pkid = Guid.NewGuid(),
                InboundPacketPropertyName = _eCConfigRepositories.connectorInboundPacketInfoObjectPropertiesRepository
                                .Set
                                .GroupBy(ect => ect.PropertyName)
                                .Select(g => new { PropertyName = g.FirstOrDefault().PropertyName }).FirstOrDefault().PropertyName,
                fkInstanceId = id,
                posicaoTxt = _eCConfigRepositories
                            .connectorConfigInboundTXTRepository
                            .GetMaxPositionFromType(_eCConfigRepositories.connectorInboundPacketInfoObjectPropertiesRepository
                                .Set
                                .GroupBy(ect => ect.PropertyName)
                                .Select(g => new { PropertyName = g.FirstOrDefault().PropertyName }).FirstOrDefault().PropertyName),
                tipo = "Cabecalho",

            };

            _eCConfigRepositories
                .connectorConfigInboundTXTRepository
                .Add(newDB)
                .Save();

            if (Request.IsAjaxRequest())
                return Json(this.HandleCreateReply("EditTXTInboundConfigs", new { pkid = newDB.pkid, instance = id }));
            else
                return RedirectToAction("EditTXTInboundConfigs", new { pkid = newDB.pkid, instance = id });
        }

        [PersonAuthorize(Permissions.ECONNECTOR_CONFIG_MAPEARTXTINBOUND)]
        public ActionResult EditTXTInboundConfigs(Guid pkid, Guid instance)
        {
            var model = _eCConfigRepositories.connectorConfigInboundTXTRepository.Find(pkid);

            var modelVM = new EBCTXTInboundConfigsVM(model, _eCConfigRepositories, _context, instance, pkid);

            if (Request.IsAjaxRequest())
                return Json(this.ModalContentReply(modelVM), JsonRequestBehavior.AllowGet);
            else
                return PartialView(modelVM);
        }

        [HttpPost]
        [PersonAuthorize(Permissions.ECONNECTOR_CONFIG_MAPEARTXTINBOUND)]
        public ActionResult DeleteTXTInboundConfigs(Guid pkid)
        {
            using (var dbContextTransaction = _eCConfigRepositories.instancesRepository.Context.Database.BeginTransaction())
            {
                try
                {
                    var dataFromDB = _eCConfigRepositories.connectorConfigInboundTXTRepository.Find(pkid);
                    if (dataFromDB != null)
                    {
                        _eCConfigRepositories.connectorConfigInboundTXTRepository
                        .Delete(dataFromDB)
                        .Save();


                        //eliminou o registo. Agora vai buscar todos os registos com posicao acima daquela
                        var registos = _eCConfigRepositories
                            .connectorConfigInboundTXTRepository
                            .Where(ct => ct.tipo == dataFromDB.tipo)
                            .ToList();

                        foreach (var r in registos)
                        {
                            if (r.posicaoTxt > dataFromDB.posicaoTxt)
                            {
                                r.posicaoTxt = r.posicaoTxt - 1;
                                _eCConfigRepositories.connectorConfigInboundTXTRepository.Edit(r).Save();
                            }
                        }
                        dbContextTransaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

                    dbContextTransaction.Rollback();
                    Flash.Instance.Error(Texts.DBErrors);
                    return RedirectToAction("TXTInboundConfigs");
                }
            }

            return RedirectToAction("TXTInboundConfigs");
        }

        [HttpPost]
        [PersonAuthorize(Permissions.ECONNECTOR_CONFIG_MAPEARTXTINBOUND)]
        public ActionResult EditTXTInboundConfigs(Guid pkid, Guid FKInstanceID, ConfigInboundTXTData data)
        {
            var dataFromDB = _eCConfigRepositories.connectorConfigInboundTXTRepository.Find(pkid);

            if (!ModelState.IsValid)
            {
                this.SetPixelAdminPageContext(_pixelAdminPageContext);

                var model = _eCConfigRepositories.connectorConfigInboundTXTRepository.Find(pkid);
                var modelVM = new EBCTXTInboundConfigsVM(model, _eCConfigRepositories, _context, FKInstanceID, pkid);

                if (Request.IsAjaxRequest())
                    return Json(this.ModalContentReply(modelVM));
                else
                    return PartialView(modelVM);
            }

            using (var dbContextTransaction = _eCConfigRepositories.instancesRepository.Context.Database.BeginTransaction())
            {
                try
                {
                    dataFromDB.InboundPacketPropertyName = data.InboundPacketPropertyName;
                    dataFromDB.posicaoTxt = data.posicaoTxt;
                    dataFromDB.tipo = data.tipo;

                    _eCConfigRepositories.connectorConfigInboundTXTRepository
                        .Edit(dataFromDB)
                        .Save();

                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

                    dbContextTransaction.Rollback();
                    Flash.Instance.Error(Texts.DBErrors);

                    this.SetPixelAdminPageContext(_pixelAdminPageContext);

                    var model = _eCConfigRepositories.connectorConfigInboundTXTRepository.Find(pkid);
                    var modelVM = new EBCTXTInboundConfigsVM(model, _eCConfigRepositories, _context, FKInstanceID, pkid);

                    if (Request.IsAjaxRequest())
                        return Json(this.ModalContentReply(modelVM));
                    else
                        return PartialView(modelVM);
                }
            }

            Flash.Instance.Success(Texts.EditOperationSuccess);
            return Json(this.CloseModalReply());
        }
        #endregion

        #region InstanceConfiguration
        //[HttpPost]
        //[PersonAuthorize(Permissions.ECONNECTOR_CONFIG_CONFIGURACAOINSTANCIA)]
        //public ActionResult InstancesConfigs(Guid id, InstanceData instance)
        //{
        //	var dataFromDB = _eCConfigRepositories.instancesRepository.Find(id);
        //	if (instance.Name == dataFromDB.Name)
        //	{
        //		return RedirectToAction("InstancesConfigs", new { id = id });
        //	}
        //	else
        //	{
        //		using (var dbContextTransaction = _eCConfigRepositories.instancesRepository.Context.Database.BeginTransaction())
        //		{
        //			try
        //			{
        //				////ALTERAR O GRUPO
        //				//AlterLocalGroupByInstance(dataFromDB.Name, instance.Name);

        //				//ALTERAR PIINFO
        //				var dataFromDB_PIInfo = _pIInfoConfigurationsRepository
        //					.Where(p => p.Nome == dataFromDB.Name).FirstOrDefault();

        //				dataFromDB_PIInfo.Nome = instance.Name;

        //				_pIInfoConfigurationsRepository
        //					.Edit(dataFromDB_PIInfo)
        //					.Save();

        //				//ALTERAR EBC_EMAILCONTENT
        //				var dataFromDB_EmailContent = _eCConfigRepositories.
        //					connectorEmailContentRepository
        //					.Where(ec => ec.PKID == dataFromDB.FKEmailContentID).FirstOrDefault();

        //				dataFromDB_EmailContent.Body = dataFromDB_EmailContent.Body.Replace(dataFromDB.Name, instance.Name);

        //				_eCConfigRepositories.
        //					connectorEmailContentRepository
        //					.Edit(dataFromDB_EmailContent)
        //					.Save();

        //				//ALTERAR EBC_INSTANCE
        //				dataFromDB.Name = instance.Name;
        //				_eCConfigRepositories
        //					.instancesRepository
        //					.Edit(dataFromDB)
        //					.Save();

        //				dbContextTransaction.Commit();
        //			}
        //			catch (Exception ex)
        //			{
        //				Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

        //				dbContextTransaction.Rollback();
        //				Flash.Instance.Error(Texts.DBErrors);

        //				this.SetPixelAdminPageContext(_pixelAdminPageContext);

        //				var model = _eCConfigRepositories.instancesRepository.GetEBC_Instances(_context.UserIdentity.Instances);
        //				var modelVM = new EBCInstancesConfigsVM(model, _eCConfigRepositories, _context, id);

        //				return View(modelVM);
        //			}
        //		}

        //		Flash.Instance.Success(Texts.EditOperationSuccess);
        //		return RedirectToAction("InstancesConfigs", new { id = id });
        //	}
        //}

        [PersonAuthorize(Permissions.ECONNECTOR_CONFIG_CONFIGURACAOINSTANCIA)]
        public ActionResult InstancesConfigs()
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            var model = _eCConfigRepositories.instancesRepository.GetEBC_Instances(_context.UserIdentity.Instances);

            if (Request.IsAjaxRequest())
                return Json(this.PanelContentReply(model), JsonRequestBehavior.AllowGet);
            else
                return View(model);
        }

        [PersonAuthorize(Permissions.ECONNECTOR_CONFIG_CONFIGURACAOINSTANCIA)]
        public ActionResult EditInstance(Guid pkid)
        {
            var instance = _eCConfigRepositories.instancesRepository.Find(pkid);

            // get all whitelist entries that AREN'T instances
            var whiteListEntries = _eCConfigRepositories.eConnectorSendersRepository
                .GetAllSendersNotInstances(_eCConfigRepositories.instancesRepository)
                .Where(x => x.NIF.ToLower() != instance.NIF.ToLower())
                .ToList();

            var model = new EBCInstanceEditVM(instance, whiteListEntries, _eCConfigRepositories.instancesDeniedSendersRepository);

            if (Request.IsAjaxRequest())
                return Json(this.ModalContentReply(model), JsonRequestBehavior.AllowGet);
            else
                return PartialView(model);
        }

        [HttpPost]
        [PersonAuthorize(Permissions.ECONNECTOR_CONFIG_CONFIGURACAOINSTANCIA)]
        public ActionResult EditInstance(InstanceData data, List<string> whitelistEntries)
        {
            using (var dbContextTransaction = _eCConfigRepositories.instancesDeniedSendersRepository.Context.Database.BeginTransaction())
            {
                var dataFromDB = _eCConfigRepositories.instancesRepository.Find(data.Instance.PKID);

                try
                {
                    if (!ModelState.IsValid)
                    {
                        Flash.Instance.Error(Texts.DBErrors);

                        var whiteListEntries = _eCConfigRepositories.eConnectorSendersRepository
                            .GetAllSendersNotInstances(_eCConfigRepositories.instancesRepository)
                            .Where(x => x.NIF.ToLower() != dataFromDB.NIF.ToLower())
                            .ToList();

                        var model = new EBCInstanceEditVM(dataFromDB, whiteListEntries, _eCConfigRepositories.instancesDeniedSendersRepository);

                        if (Request.IsAjaxRequest())
                            return Json(this.ModalContentReply(model));
                        else
                            return PartialView(model);
                    }

                    // update denied senders for this instance
                    _eCConfigRepositories.instancesDeniedSendersRepository.UpdateAllowedSendersByInstanceId(dataFromDB.PKID, whitelistEntries);

                    // update instance
                    dataFromDB.HasInternalProcess = data.Instance.HasInternalProcess;

                    _eCConfigRepositories
                        .instancesRepository
                        .Edit(dataFromDB)
                        .Save();

                    dbContextTransaction.Commit();

                    Flash.Instance.Success(Texts.EditOperationSuccess);
                    return Json(this.CloseModalReply());
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

                    Flash.Instance.Error(Texts.DBErrors);

                    var whiteListEntries = _eCConfigRepositories.eConnectorSendersRepository
                            .GetAllSendersNotInstances(_eCConfigRepositories.instancesRepository)
                            .Where(x => x.NIF.ToLower() != dataFromDB.NIF.ToLower())
                            .ToList();

                    var model = new EBCInstanceEditVM(dataFromDB, whiteListEntries, _eCConfigRepositories.instancesDeniedSendersRepository);

                    if (Request.IsAjaxRequest())
                        return Json(this.ModalContentReply(model));
                    else
                        return PartialView(model);
                }
            }
        }

        #endregion

        #region AuxiliaryMethods
        private string TransformStringIntoXSL(string value, int tipo, string tipoxml)
        {
            var xslContent = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            xslContent += "<xsl:stylesheet version=\"1.0\" xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\">";
            if (tipo == 1)
            {
                xslContent += "<xsl:template match=\"/\">";
                xslContent += "<body style=\"background-color:white;\">";
            }
            else
            {
                xslContent += "<xsl:template match=\"/\"><xsl:text>";
            }

            if (tipo == 0)
            {
                xslContent += value;
                xslContent += "</xsl:text>";
                //xslContent += "</body>";
            }
            else
            {
                xslContent += value;
                //xslContent += "</xsl:text>";
                xslContent += "</body>";
            }


            xslContent += "</xsl:template>";
            xslContent += "<xsl:output method=\"html\" omit-xml-declaration=\"yes\"/>";
            xslContent += "</xsl:stylesheet>";

            var exists = xslContent.Contains("<div id=\"DataInvoice\">");
            if (exists)
            {
                xslContent = InsertXMLData(xslContent.IndexOf("Nome do Cliente:"), "Nome do Cliente:", "Nome Receptor", tipoxml, xslContent);
                xslContent = InsertXMLData(xslContent.IndexOf("E-mail do Cliente:"), "E-mail do Cliente:", "Email Cliente", tipoxml, xslContent);
                xslContent = InsertXMLData(xslContent.IndexOf("NIF do Cliente:"), "NIF do Cliente:", "NIF Receptor", tipoxml, xslContent);
                xslContent = InsertXMLData(xslContent.IndexOf("Número do documento:"), "Número do documento:", "Nº Factura", tipoxml, xslContent);
                xslContent = InsertXMLData(xslContent.IndexOf("Data do documento:"), "Data do documento:", "Data Factura", tipoxml, xslContent);
                xslContent = InsertXMLData(xslContent.IndexOf("Valor líquido:"), "Valor líquido:", "Quantia C/IVA", tipoxml, xslContent);
            }


            byte[] encodedByte = Encoding.Default.GetBytes(xslContent);
            xslContent = ProcessString(Encoding.Default.GetString(encodedByte));

            return xslContent;
        }

        private string InsertXMLSubjectData(string value, string xslContent, string tipoxml)
        {
            char[] delimitador = { ' ' };
            var values = value.Split(delimitador);
            value = value.Remove(value.IndexOf("["));
            foreach (string v in values)
            {
                if (v.StartsWith("["))
                {
                    var field = v.Replace("[", "").Replace("]", "");
                    var xmlpath = _eCConfigRepositories.eConnectorXmlTemplateRepository
                            .Where(xt => xt.TipoXML == tipoxml
                            &&
                            xt.NomeCampo == field).FirstOrDefault().CaminhoXML;

                    xslContent += " <xsl:value-of select=" + xmlpath + "></xsl:value-of>";
                }
                else
                    xslContent += v;
            }

            return xslContent;
        }

        private string InsertXMLData(int IndexOf, string stringOf, string nomeBD, string tipoxml, string xslContent)
        {
            var xmlpath = _eCConfigRepositories.eConnectorXmlTemplateRepository
                            .Where(xt => xt.TipoXML == tipoxml
                            &&
                            xt.NomeCampo == nomeBD).FirstOrDefault().CaminhoXML;
            xslContent = xslContent.Insert((IndexOf + stringOf.Length) + 1, " <span><xsl:value-of select=" + xmlpath + "></xsl:value-of></span> ");

            return xslContent;
        }

        public string ProcessString(string strInputHtml)
        {
            StringWriter sw = new StringWriter();
            using (SgmlReader reader = new SgmlReader())
            {
                reader.DocType = "HTML";
                using (StringReader sr = new System.IO.StringReader(strInputHtml))
                {
                    reader.InputStream = sr;
                    using (XmlTextWriter w = new XmlTextWriter(sw))
                    {
                        reader.Read();
                        while (!reader.EOF)
                            w.WriteNode(reader, true);
                    }
                }
            }

            return sw.ToString();
        }

        //public void  AlterLocalGroupByInstance(string instanceOld, string instanceNew)
        //{			
        //	PrincipalContext context = new PrincipalContext(ContextType.Machine,);
        //	//GroupPrincipal group = new GroupPrincipal(context);
        //	//PrincipalSearcher ps = new PrincipalSearcher(group);
        //	//PrincipalSearchResult<Principal> results = ps.FindAll();
        //	//Principal foundGroup = results.Where(item => item.Name == "eBillingConnector_" + instanceOld + "_Operators").SingleOrDefault();

        //	//if (foundGroup != null)
        //	//{
        //	//	//foundGroup.Name = "eBillingConnector_" + instanceNew + "_Operators";
        //	//	//foundGroup.Description = "eBillingConnector_" + instanceNew + "_Operators";
        //	//	//foundGroup.SamAccountName = "eBillingConnector_" + instanceNew + "_Operators";
        //	//	//foundGroup.Save();
        //	//	foundGroup.Delete();
        //	//}

        //	//var ad = new DirectoryEntry("WinNT://" + Environment.MachineName + ",computer");
        //	//DirectoryEntry newGroup = ad.Children.Add("eBillingConnector_" + instanceNew + "_Operators", "group");
        //	//newGroup.Invoke("Put", new object[] { "Description", "eBillingConnector_" + instanceNew + "_Operators" });
        //	//newGroup.CommitChanges();

        //	try
        //	{
        //		DirectoryEntry objADAM;   // Binding object.
        //		DirectoryEntry objGroup;  // Group object.
        //		using (objADAM = new DirectoryEntry("WinNT://" + Environment.MachineName, "SHORTCUT\\TiagoEsteves", "SH2014TE#."))
        //		{
        //			using(objGroup = objADAM.Children.Find("eBillingConnector_" + instanceOld + "_Operators", "group"))
        //			{
        //				objGroup.AuthenticationType = AuthenticationTypes.Anonymous;
        //				objGroup.Password = "SH2014TE#.";
        //				objGroup.Username = "SHORTCUT\\TiagoEsteves";
        //				objADAM.Children.Remove(objGroup);
        //			}					
        //		}
        //	}
        //	catch (Exception ex)
        //	{
        //		throw;
        //	}
        //}

        #endregion
    }
}