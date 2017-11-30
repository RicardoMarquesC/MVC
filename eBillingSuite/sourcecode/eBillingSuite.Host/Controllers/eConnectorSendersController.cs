using eBillingSuite.Enumerations;
using eBillingSuite.Globalization;
using eBillingSuite.HelperTools.Interfaces;
using eBillingSuite.Model;
using eBillingSuite.Model.CIC_DB;
using eBillingSuite.Model.EBC_DB;
using eBillingSuite.Models;
using eBillingSuite.Repositories;
using eBillingSuite.Resources;
using eBillingSuite.Security;
using eBillingSuite.ViewModels;
using MvcFlash.Core;
using MvcFlash.Core.Extensions;
using Shortcut.PixelAdmin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eBillingSuite.Controllers
{
	public class eConnectorSendersController : Controller
	{
		private static string allowed = ".xml";

		protected readonly IeBillingSuiteRequestContext _context;
		private IPixelAdminPageContext _pixelAdminPageContext;
		private IEConnectorSendersRepository _eConnectorSendersRepository;
		private IECCListRepositories _eCCListRepositories;
		private IXmlHelper _xmlHelper;

		public eConnectorSendersController(IeBillingSuiteRequestContext context,
			IPixelAdminPageContext pixelAdminPageContext,
			IEConnectorSendersRepository eConnectorSendersRepository,
			IECCListRepositories eCConfigRepositories,
			IXmlHelper xmlHelper)
		{
			_context = context;
			_pixelAdminPageContext = pixelAdminPageContext;
			_eConnectorSendersRepository = eConnectorSendersRepository;
			_eCCListRepositories = eCConfigRepositories;
			_xmlHelper = xmlHelper;
		}


		#region Senders

		// GET: eConnectorSenders
		[PersonAuthorize(Permissions.EECONNECTOR_REMETENTES_REMETENTES)]
		public ActionResult Index()
		{
			this.SetPixelAdminPageContext(_pixelAdminPageContext);

			var model = _eConnectorSendersRepository.GetAllSenders();

			if (Request.IsAjaxRequest())
				return Json(this.PanelContentReply(model), JsonRequestBehavior.AllowGet);
			else
				return View(model);
		}

		// GET: Create sender
		[HttpPost]
		[PersonAuthorize(Permissions.EECONNECTOR_REMETENTES_REMETENTES)]
		public ActionResult CreateSender()
		{
			var tempSender = new Whitelist
			{
				ConcatAnexos = false,
				EmailAddress = "",
				EmailName = "",
				Enabled = true,
				FKIntegrationFilterID = _eCCListRepositories.eConnectorIntegrationFiltersRepository.Set
					.FirstOrDefault(x => x.FriendlyName.Equals(Enumerations.IntegrationFiltersName.DEFAULT, StringComparison.OrdinalIgnoreCase))
					.PKIntegrationFilterID,
				HaveXML = false,
                UsesPluginSystem = false,
				Mercado = "Portugal",
				NIF = "",
				PDFAss = true,
				PdfLink = false,
                PdfLinkBaseURL = "",
				PDFNAss = false,
				XMLAss = false,
				XMLNAss = false
            };

			var counterValue = _eCCListRepositories.suiteConfigurationsRepository.ConfigValue("ContadorNomenclaturaPDF");

			var modelVM = new EBCSendersVM(tempSender, String.Empty, _eCCListRepositories, Guid.Empty, false, counterValue, true);

			if (Request.IsAjaxRequest())
				return Json(this.ModalContentReply(modelVM), JsonRequestBehavior.AllowGet);
			else
				return PartialView(modelVM);
		}

		// POST: Create sender
		[HttpPost]
		[PersonAuthorize(Permissions.EECONNECTOR_REMETENTES_REMETENTES)]
		public ActionResult CreateFinalSender(SenderData senderData)
		{
			using (var dbContextTransaction = _eConnectorSendersRepository.Context.Database.BeginTransaction())
			{
				try
				{
					// INSERT into WhiteList
					var newDB = new Whitelist
					{
						PKWhitelistID = Guid.NewGuid(),
						EmailAddress = senderData.EmailAddress,
						EmailName = senderData.EmailName,
						FKIntegrationFilterID = _eCCListRepositories.eConnectorIntegrationFiltersRepository.Set
												.FirstOrDefault(x => x.FriendlyName.Equals(Enumerations.IntegrationFiltersName.DEFAULT, StringComparison.OrdinalIgnoreCase))
												.PKIntegrationFilterID,
						Enabled = senderData.Enabled,
						HaveXML = (senderData.XMLAss == true || senderData.XMLNAss == true) ? true : false,
						ConcatAnexos = senderData.ConcatAnexos,
						Mercado = senderData.Mercado,
						XMLAss = senderData.XMLAss,
						XMLNAss = senderData.XMLNAss,
						PDFAss = senderData.PDFAss,
						PDFNAss = senderData.PDFNAss,
						NIF = senderData.Nif,
						PdfLink = senderData.PdfLink,
                        PdfLinkBaseURL = senderData.PdfLinkBaseURL,
                        UsesPluginSystem = senderData.UsesPluginSystem,
                        DoYouWantForwardEmail = senderData.DoYouWantForwardEmail,
                        DoYouWantForwardFTP = senderData.DoYouWantForwardFTP,
                        ftpServer = senderData.ftpServer,
                        username = senderData.username,
                        password = senderData.password,
                        port = senderData.port,
                        listEmails = senderData.listEmails,
                        ReplyToAddress = senderData.ReplyToAddress
					};

					if (!ModelState.IsValid)
					{
						//Flash.Instance.Error(Texts.DBErrors);

						var modelVM = new EBCSendersVM(newDB, senderData.XmlType, _eCCListRepositories, true);

						if (Request.IsAjaxRequest())
							return Json(this.ModalContentReply("CreateSender", modelVM));
						else
							return PartialView(modelVM);
					}

					_eConnectorSendersRepository
						.Add(newDB)
						.Save();

					if (newDB.HaveXML)
					{
						// GET template UBL2.0 structure
						var xmlData = _eCCListRepositories.eConnectorXmlTemplateRepository.GetDataByType(senderData.XmlType);

						// GET last xml template number
						int xmlNumber = _eCCListRepositories.eConnectorXmlInboundRepository.GetLastInboundXmlNumber();

						// INSERT into XMLInbound
						var newXmlInbound = new EBC_XMLInbound
						{
							pkid = Guid.NewGuid(),
							Fornecedor = senderData.Nif,
							NumeroXML = xmlNumber
						};
						_eCCListRepositories.eConnectorXmlInboundRepository.Add(newXmlInbound).Save();

						// INSERT into Header, Lines and VAT lines
						int i = 1;
						foreach (var data in xmlData.Where(xd => xd.Tipo.ToLower() == DigitalDocumentAreas.HEADER.ToLower()).ToList())
						{
							_eCCListRepositories.eConnectorXmlHeadInboundRepository.Add(new EBC_XMLHeadInbound
							{
								pkid = Guid.NewGuid(),
								NumeroXML = xmlNumber,
								Element = data.CaminhoXML,
								NomeCampo = data.NomeCampo,
								TipoXML = data.TipoXML,
								Obrigatorio = true,
								Posicao = i,
								CampoBD = data.NomeCampo.Replace(" ", ""),
								isATfield = data.isATfield.HasValue ? (data.isATfield.Value == true ? 1 : 0) : 0,
							}).Save();

							i++;
						};

						i = 1;
						foreach (var data in xmlData.Where(xd => xd.Tipo.ToLower() == DigitalDocumentAreas.LINES.ToLower()).ToList())
						{
							_eCCListRepositories.eConnectorXmlLinesInboundRepository.Add(new EBC_XMLLinesInbound
							{
								pkid = Guid.NewGuid(),
								NumeroXML = xmlNumber,
								Element = data.CaminhoXML,
								NomeCampo = data.NomeCampo,
								TipoXML = data.TipoXML,
								Obrigatorio = true,
								Posicao = i,
								CampoBD = data.NomeCampo.Replace(" ", ""),
								isATfield = data.isATfield.HasValue ? data.isATfield.Value : false,
							}).Save();

							i++;
						};

						i = 1;
						foreach (var data in xmlData.Where(xd => xd.Tipo.ToLower() == DigitalDocumentAreas.VAT.ToLower()).ToList())
						{
							_eCCListRepositories.eConnectorXmlVatInboundRepository.Add(new EBC_XMLResumoIVAInbound
							{
								pkid = Guid.NewGuid(),
								NumeroXML = xmlNumber,
								Element = data.CaminhoXML,
								NomeCampo = data.NomeCampo,
								TipoXML = data.TipoXML,
								Obrigatorio = true,
								Posicao = i,
								CampoBD = data.NomeCampo.Replace(" ", ""),
								isATfield = data.isATfield.HasValue ? data.isATfield.Value : false,
							}).Save();

							i++;
						};
					}


					var sender = _eConnectorSendersRepository.Where(s => s.NIF == senderData.Nif).FirstOrDefault();
					//save the TipoNomenclaturaPDF
					if (senderData.isNomenclaturaPDF)
					{
						var pkidnomenclatura = Guid.Parse(senderData.NomenclaturaPDFType);
						var nomenclaturaFromDB = _eCCListRepositories.eConnectorTipoNomenclaturaSenderRepository.GetBySenderID(sender.PKWhitelistID);
						if (nomenclaturaFromDB != null)
						{
							nomenclaturaFromDB.FKtiponomenclatura = pkidnomenclatura;

							_eCCListRepositories.eConnectorTipoNomenclaturaSenderRepository.Edit(nomenclaturaFromDB).Save();
						}
						else
						{
							var pkidnomenclatura2 = Guid.Parse(senderData.NomenclaturaPDFType);
							var nomenclatura = new TipoNomenclaturaSender
							{
								FKRemetente = sender.PKWhitelistID,
								FKtiponomenclatura = pkidnomenclatura2,
							};

							_eCCListRepositories.eConnectorTipoNomenclaturaSenderRepository
								.Add(nomenclatura)
								.Save();
						}

						//save the counter
						var ebc_config = _eCCListRepositories.suiteConfigurationsRepository
										.Where(sc => sc.Name == "ContadorNomenclaturaPDF").FirstOrDefault();

						ebc_config.Data = senderData.counterValue;
						_eCCListRepositories.suiteConfigurationsRepository.Edit(ebc_config).Save();
					}

					dbContextTransaction.Commit();

					Flash.Instance.Success(Texts.CreateOperationSuccess);
					return Json(this.CloseModalReply());
				}
				catch (Exception ex)
				{
					Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
					dbContextTransaction.Rollback();
					Flash.Instance.Error(Texts.DBErrors);
					return RedirectToAction("Index");
				}

			}
		}

		// GET: Open edit sender modal
		[PersonAuthorize(Permissions.EECONNECTOR_REMETENTES_REMETENTES)]
		public ActionResult EditSender(Guid pkid)
		{
			var model = _eConnectorSendersRepository.Find(pkid);

			var senderXmlType = _eCCListRepositories.eConnectorXmlInboundRepository.GetSenderXmlType(model.NIF);
			var counterValue = _eCCListRepositories.suiteConfigurationsRepository.ConfigValue("ContadorNomenclaturaPDF");
			var nomenclatura = _eCCListRepositories.eConnectorTipoNomenclaturaSenderRepository.GetBySenderID(pkid);

			var modelVM = new EBCSendersVM(model, 
				senderXmlType, 
				_eCCListRepositories,
				nomenclatura==null ? Guid.Empty:nomenclatura.FKtiponomenclatura,
				nomenclatura==null ? false : true,
				counterValue,
                false);

			if (Request.IsAjaxRequest())
				return Json(this.ModalContentReply(modelVM), JsonRequestBehavior.AllowGet);
			else
				return PartialView(modelVM);
		}

		//POST: submit edit sender info
		[HttpPost]
		[PersonAuthorize(Permissions.EECONNECTOR_REMETENTES_REMETENTES)]
		public ActionResult EditSender(SenderData data)
		{
			using (var dbContextTransaction = _eConnectorSendersRepository.Context.Database.BeginTransaction())
			{
				var dataFromDB = _eConnectorSendersRepository.Find(data.Pkid);
				try
				{
					if (!ModelState.IsValid)
					{
						Flash.Instance.Error(Texts.DBErrors);

						var modelVM = new EBCSendersVM(dataFromDB, data.XmlType, _eCCListRepositories, data.IsFromCreate);

						if (Request.IsAjaxRequest())
							return Json(this.ModalContentReply(modelVM));
						else
							return PartialView(modelVM);
					}

					// if it changes VAT Number, update EBC_XMLInbound
					if (!data.Nif.Equals(dataFromDB.NIF, StringComparison.OrdinalIgnoreCase) && (data.XMLAss == true || data.XMLNAss == true))
					{
						var xmlInboundFromDB = _eCCListRepositories.eConnectorXmlInboundRepository
							.Set
							.FirstOrDefault(x => x.Fornecedor.Equals(dataFromDB.NIF, StringComparison.OrdinalIgnoreCase));

						xmlInboundFromDB.Fornecedor = data.Nif;

						_eCCListRepositories.eConnectorXmlInboundRepository
							.Edit(xmlInboundFromDB)
							.Save();
					}

					// ckeck for selected XML Type
					var senderXmlType = _eCCListRepositories.eConnectorXmlInboundRepository.GetSenderXmlType(data.Nif);
					if (senderXmlType == "") // if there wasn't defined XML
					{
						if (data.XMLAss == true || data.XMLNAss == true) // if it's defining XML
						{
							// insert into XML tables
							// GET template structure
							var xmlData = _eCCListRepositories.eConnectorXmlTemplateRepository.GetDataByType(data.XmlType);

							// GET last xml template number
							int xmlNumber = _eCCListRepositories.eConnectorXmlInboundRepository.GetLastInboundXmlNumber();

							// INSERT into XMLInbound
							var newXmlInbound = new EBC_XMLInbound
							{
								pkid = Guid.NewGuid(),
								Fornecedor = data.Nif,
								NumeroXML = xmlNumber
							};
							_eCCListRepositories.eConnectorXmlInboundRepository.Add(newXmlInbound).Save();

							// INSERT into Header, Lines and VAT lines
							int i = 1;
							foreach (var data2 in xmlData.Where(xd => xd.Tipo.ToLower() == DigitalDocumentAreas.HEADER.ToLower()).ToList())
							{
								_eCCListRepositories.eConnectorXmlHeadInboundRepository.Add(new EBC_XMLHeadInbound
								{
									pkid = Guid.NewGuid(),
									NumeroXML = xmlNumber,
									Element = data2.CaminhoXML,
									NomeCampo = data2.NomeCampo,
									TipoXML = data2.TipoXML,
									Obrigatorio = true,
									Posicao = i,
									CampoBD = data2.NomeCampo.Replace(" ", ""),
									isATfield = data2.isATfield.HasValue ? (data2.isATfield.Value == true ? 1 : 0) : 0,
								}).Save();

								i++;
							};

							i = 1;
							foreach (var data2 in xmlData.Where(xd => xd.Tipo.ToLower() == DigitalDocumentAreas.LINES.ToLower()).ToList())
							{
								_eCCListRepositories.eConnectorXmlLinesInboundRepository.Add(new EBC_XMLLinesInbound
								{
									pkid = Guid.NewGuid(),
									NumeroXML = xmlNumber,
									Element = data2.CaminhoXML,
									NomeCampo = data2.NomeCampo,
									TipoXML = data2.TipoXML,
									Obrigatorio = true,
									Posicao = i,
									CampoBD = data2.NomeCampo.Replace(" ", ""),
									isATfield = data2.isATfield.HasValue ? data2.isATfield.Value : false,
								}).Save();

								i++;
							};

							i = 1;
							foreach (var data2 in xmlData.Where(xd => xd.Tipo.ToLower() == DigitalDocumentAreas.VAT.ToLower()).ToList())
							{
								_eCCListRepositories.eConnectorXmlVatInboundRepository.Add(new EBC_XMLResumoIVAInbound
								{
									pkid = Guid.NewGuid(),
									NumeroXML = xmlNumber,
									Element = data2.CaminhoXML,
									NomeCampo = data2.NomeCampo,
									TipoXML = data2.TipoXML,
									Obrigatorio = true,
									Posicao = i,
									CampoBD = data2.NomeCampo.Replace(" ", ""),
									isATfield = data2.isATfield.HasValue ? data2.isATfield.Value : false,
								}).Save();

								i++;
							};
						}
					}
					else // if there's a defined XML
					{
						var xmlNumber = _eCCListRepositories.eConnectorXmlInboundRepository.GetXmlNumberBySenderVat(data.Nif);

						//// delete the existing one
						//var headerToDelete = _eCCListRepositories.eConnectorXmlHeadInboundRepository.Set
						//	.Where(x => x.NumeroXML == xmlNumber).AsEnumerable();
						//_eCCListRepositories.eConnectorXmlHeadInboundRepository.Set.RemoveRange(headerToDelete);

						//var linesToDelete = _eCCListRepositories.eConnectorXmlLinesInboundRepository.Set
						//	.Where(x => x.NumeroXML == xmlNumber).AsEnumerable();
						//_eCCListRepositories.eConnectorXmlLinesInboundRepository.Set.RemoveRange(linesToDelete);

						//var vatToDelete = _eCCListRepositories.eConnectorXmlVatInboundRepository.Set
						//	.Where(x => x.NumeroXML == xmlNumber).AsEnumerable();
						//_eCCListRepositories.eConnectorXmlVatInboundRepository.Set.RemoveRange(vatToDelete);

						// if defined XML is diferent than the existing one
						if ((data.XMLAss == true || data.XMLNAss == true) && (data.XmlType.ToLower() != senderXmlType.ToLower()))
						{
							// delete the existing one
							var headerToDelete = _eCCListRepositories.eConnectorXmlHeadInboundRepository.Set
								.Where(x => x.NumeroXML == xmlNumber).AsEnumerable();
							_eCCListRepositories.eConnectorXmlHeadInboundRepository.Set.RemoveRange(headerToDelete);

							var linesToDelete = _eCCListRepositories.eConnectorXmlLinesInboundRepository.Set
								.Where(x => x.NumeroXML == xmlNumber).AsEnumerable();
							_eCCListRepositories.eConnectorXmlLinesInboundRepository.Set.RemoveRange(linesToDelete);

							var vatToDelete = _eCCListRepositories.eConnectorXmlVatInboundRepository.Set
								.Where(x => x.NumeroXML == xmlNumber).AsEnumerable();
							_eCCListRepositories.eConnectorXmlVatInboundRepository.Set.RemoveRange(vatToDelete);

							// insert the new one
							// get template structure
							var xmlData = _eCCListRepositories.eConnectorXmlTemplateRepository.GetDataByType(data.XmlType);

							// INSERT into Header, Lines and VAT lines
							int i = 1;
							foreach (var data2 in xmlData.Where(xd => xd.Tipo.ToLower() == DigitalDocumentAreas.HEADER.ToLower()).ToList())
							{
								_eCCListRepositories.eConnectorXmlHeadInboundRepository.Add(new EBC_XMLHeadInbound
								{
									pkid = Guid.NewGuid(),
									NumeroXML = xmlNumber,
									Element = data2.CaminhoXML,
									NomeCampo = data2.NomeCampo,
									TipoXML = data2.TipoXML,
									Obrigatorio = true,
									Posicao = i,
									CampoBD = data2.NomeCampo.Replace(" ", ""),
									isATfield = data2.isATfield.HasValue ? (data2.isATfield.Value == true ? 1 : 0) : 0,
								}).Save();

								i++;
							};

							i = 1;
							foreach (var data2 in xmlData.Where(xd => xd.Tipo.ToLower() == DigitalDocumentAreas.LINES.ToLower()).ToList())
							{
								_eCCListRepositories.eConnectorXmlLinesInboundRepository.Add(new EBC_XMLLinesInbound
								{
									pkid = Guid.NewGuid(),
									NumeroXML = xmlNumber,
									Element = data2.CaminhoXML,
									NomeCampo = data2.NomeCampo,
									TipoXML = data2.TipoXML,
									Obrigatorio = true,
									Posicao = i,
									CampoBD = data2.NomeCampo.Replace(" ", ""),
									isATfield = data2.isATfield.HasValue ? data2.isATfield.Value : false,
								}).Save();

								i++;
							};

							i = 1;
							foreach (var data2 in xmlData.Where(xd => xd.Tipo.ToLower() == DigitalDocumentAreas.VAT.ToLower()).ToList())
							{
								_eCCListRepositories.eConnectorXmlVatInboundRepository.Add(new EBC_XMLResumoIVAInbound
								{
									pkid = Guid.NewGuid(),
									NumeroXML = xmlNumber,
									Element = data2.CaminhoXML,
									NomeCampo = data2.NomeCampo,
									TipoXML = data2.TipoXML,
									Obrigatorio = true,
									Posicao = i,
									CampoBD = data2.NomeCampo.Replace(" ", ""),
									isATfield = data2.isATfield.HasValue ? data2.isATfield.Value : false,
								}).Save();

								i++;
							};
						}
						else
						{
							// delete from EBC_XMLInbound
							//var line = _eCCListRepositories.eConnectorXmlInboundRepository.Set
							//	.FirstOrDefault(x => x.NumeroXML == xmlNumber && x.Fornecedor == data.Nif);

							//_eCCListRepositories.eConnectorXmlInboundRepository.Delete(line).Save();
						}
					}

					dataFromDB.EmailName = data.EmailName;
					dataFromDB.EmailAddress = data.EmailAddress;
					if (String.IsNullOrWhiteSpace(dataFromDB.NIF)) // only update VAT if it's on create
						dataFromDB.NIF = data.Nif;
					dataFromDB.Enabled = data.Enabled;
					dataFromDB.ConcatAnexos = data.ConcatAnexos;
					dataFromDB.FKIntegrationFilterID = _eCCListRepositories.eConnectorIntegrationFiltersRepository.Set
														.FirstOrDefault(x => x.FriendlyName.Equals(Enumerations.IntegrationFiltersName.DEFAULT, StringComparison.OrdinalIgnoreCase))
														.PKIntegrationFilterID;
					dataFromDB.Mercado = data.Mercado;
					dataFromDB.XMLNAss = data.XMLNAss;
					dataFromDB.XMLAss = data.XMLAss;
					dataFromDB.PDFAss = data.PDFAss;
					dataFromDB.PDFNAss = data.PDFNAss;
					dataFromDB.PdfLink = data.PdfLink;
                    dataFromDB.PdfLinkBaseURL = data.PdfLinkBaseURL;
                    dataFromDB.UsesPluginSystem = data.UsesPluginSystem;
					if (dataFromDB.XMLNAss.Value || dataFromDB.XMLAss.Value)
						dataFromDB.HaveXML = true;
					else
						dataFromDB.HaveXML = false;

                    dataFromDB.DoYouWantForwardEmail = data.DoYouWantForwardEmail;
                    dataFromDB.DoYouWantForwardFTP = data.DoYouWantForwardFTP;
                    if (dataFromDB.DoYouWantForwardEmail == true)
                    {
                        dataFromDB.listEmails = data.listEmails;
                        dataFromDB.ftpServer = "NULL";
                        dataFromDB.username = "NULL";
                        dataFromDB.password = "NULL";
                        dataFromDB.port = "NULL";
                    }
                    else
                    {
                        if (dataFromDB.DoYouWantForwardFTP == true)
                        {
                            dataFromDB.ftpServer = data.ftpServer;
                            dataFromDB.username = data.username;
                            dataFromDB.password = data.password;
                            dataFromDB.port = data.port;
                            dataFromDB.listEmails = "NULL";
                        }
                        else
                        {
                            dataFromDB.listEmails = "NULL";
                            dataFromDB.ftpServer = "NULL";
                            dataFromDB.username = "NULL";
                            dataFromDB.password = "NULL";
                            dataFromDB.port = "NULL";
                        }
                    }
                    dataFromDB.ReplyToAddress = data.ReplyToAddress;

                    _eConnectorSendersRepository
						.Edit(dataFromDB)
						.Save();

					var sender = _eConnectorSendersRepository.Where(s => s.NIF == data.Nif).FirstOrDefault();

					//save the TipoNomenclaturaPDF
					if (data.isNomenclaturaPDF)
					{
						var pkidnomenclatura = Guid.Parse(data.NomenclaturaPDFType);
						var nomenclaturaFromDB = _eCCListRepositories.eConnectorTipoNomenclaturaSenderRepository.GetBySenderID(sender.PKWhitelistID);
						if (nomenclaturaFromDB != null)
						{
							nomenclaturaFromDB.FKtiponomenclatura = pkidnomenclatura;

							_eCCListRepositories.eConnectorTipoNomenclaturaSenderRepository.Edit(nomenclaturaFromDB).Save();
						}
						else
						{
							var pkidnomenclatura2 = Guid.Parse(data.NomenclaturaPDFType);
							var nomenclatura = new TipoNomenclaturaSender
							{
								FKRemetente = sender.PKWhitelistID,
								FKtiponomenclatura = pkidnomenclatura2,
							};

							_eCCListRepositories.eConnectorTipoNomenclaturaSenderRepository
								.Add(nomenclatura)
								.Save();
						}

						//save the counter
						var ebc_config = _eCCListRepositories.suiteConfigurationsRepository
										.Where(sc => sc.Name == "ContadorNomenclaturaPDF").FirstOrDefault();

						ebc_config.Data = data.counterValue;
						_eCCListRepositories.suiteConfigurationsRepository.Edit(ebc_config).Save();
					}
					else
					{
						var nomenclaturaFromDB = _eCCListRepositories.eConnectorTipoNomenclaturaSenderRepository.GetBySenderID(sender.PKWhitelistID);
						if (nomenclaturaFromDB != null)
						{
							_eCCListRepositories.eConnectorTipoNomenclaturaSenderRepository.Delete(nomenclaturaFromDB).Save();
						}
					}

					dbContextTransaction.Commit();
				}
				catch (Exception ex)
				{
					Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

					dbContextTransaction.Rollback();
					Flash.Instance.Error(Texts.DBErrors);

					var modelVM = new EBCSendersVM(dataFromDB, data.XmlType, _eCCListRepositories, data.IsFromCreate);

					if (Request.IsAjaxRequest())
						return Json(this.ModalContentReply(modelVM));
					else
						return PartialView(modelVM);
				}
			}

			Flash.Instance.Success(Texts.EditOperationSuccess);
			return Json(this.CloseModalReply());
		}

        public ActionResult UpdateTable()
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            var model = _eConnectorSendersRepository.GetAllSenders();

            if (Request.IsAjaxRequest())
                return Json(this.PanelContentReply(model), JsonRequestBehavior.AllowGet);
            else
                return View(model);
        }
		#endregion

        #region Customers
        [PersonAuthorize(Permissions.ECONNECTOR_CLIENTES_CLIENTES)]
        public ActionResult Customer(Guid? id)
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            if (!id.HasValue)
                id = _eCCListRepositories.instancesRepository.GetEBC_Instances(_context.UserIdentity.Instances).FirstOrDefault().PKID;

            var model = _eCCListRepositories.instancesRepository.GetEBC_Instances(_context.UserIdentity.Instances);
            var modelVM = new EBCCustomersVM(model, _eCCListRepositories, id.Value);

            if (Request.IsAjaxRequest())
                return Json(this.PanelContentReply(modelVM), JsonRequestBehavior.AllowGet);
            else
                return View(modelVM);
        }

        [HttpPost]
        [PersonAuthorize(Permissions.ECONNECTOR_CLIENTES_CLIENTES)]
        public ActionResult CreateCustomer(Guid id)
        {
            using (var dbContextTransaction = _eCCListRepositories.instancesRepository.Context.Database.BeginTransaction())
            {
                try
                {

                    var newDB = new EBC_Customers
                    {
                        PKID = Guid.NewGuid(),
                        Name = Texts.Untitled,
                        Email = Texts.Untitled,
                        NIF = Texts.Untitled,
                        Mercado = _eCCListRepositories.eBCMarketsRepository.Set.FirstOrDefault().Mercado,
                        FKEmailContentID = _eCCListRepositories.instancesRepository.Where(ebi => ebi.PKID == id).FirstOrDefault().FKEmailContentID,
                        FKSpecificDeliveryOptionsID = _eCCListRepositories.instancesRepository.Where(ebi => ebi.PKID == id).FirstOrDefault().FKSpecificDeliveryOptionsID,
                        FKInstanceID = id,
                        XMLAss = false,
                        XMLNAss = false,
                        PDFAss = false,
                        PDFNAss = false
                    };

                    _eCCListRepositories.eConnectorCustomersRepository
                        .Add(newDB)
                        .Save();

                    //buscar os campos de xml
                    var XmlData = _eCCListRepositories.eConnectorXmlTemplateRepository.GetDataByType("UBL2.0");

                    //buscar numeroxmlcliente
                    var XmlNumber = _eCCListRepositories.eConnectorXmlClientRepository.GetXMLClientNumber();
                    var newDBXMLClient = new EBC_XMLClient
                    {
                        Pkid = Guid.NewGuid(),
                        FkCliente = newDB.PKID,
                        NumeroXML = XmlNumber
                    };

                    var countCab = 0; var countLine = 0; var countIva = 0;
                    _eCCListRepositories.eConnectorXmlClientRepository.Add(newDBXMLClient).Save();
                    XmlData.ForEach(delegate (xmlTemplate xmltempl)
                    {
                        if (xmltempl.Tipo.ToLower().Equals("cabecalho"))
                        {
                            var newDBXMLHeader = new EBC_XML
                            {
                                NumeroXML = XmlNumber,
                                NomeCampo = xmltempl.NomeCampo,
                                Element = xmltempl.CaminhoXML,
                                TipoXML = "UBL2.0",
                                Obrigatorio = true,
                                Posicao = countCab,
                                CampoBD = xmltempl.NomeCampo.Replace(" ", ""),
                                isATfield = true
                            };

                            _eCCListRepositories.eConnectorXmlHeaderRepository
                                .Add(newDBXMLHeader)
                                .Save();

                            countCab++;
                        }
                        else if (xmltempl.Tipo.ToLower().Equals("lineitem"))
                        {
                            var newDBXMLLine = new EBC_XMLLines
                            {
                                NumeroXML = XmlNumber,
                                NomeCampo = xmltempl.NomeCampo,
                                Element = xmltempl.CaminhoXML,
                                TipoXML = "UBL2.0",
                                Obrigatorio = true,
                                Posicao = countLine,
                                CampoBD = xmltempl.NomeCampo.Replace(" ", ""),
                                isATfield = true,
                                pkid = Guid.NewGuid()
                            };

                            _eCCListRepositories.eConnectorXmlLinesRepository
                                .Add(newDBXMLLine)
                                .Save();

                            countLine++;
                        }
                        else
                        {
                            var newDBXMLIva = new EBC_XMLResumoIVA
                            {
                                NumeroXML = XmlNumber,
                                NomeCampo = xmltempl.NomeCampo,
                                Element = xmltempl.CaminhoXML,
                                TipoXML = "UBL2.0",
                                Obrigatorio = true,
                                Posicao = countLine,
                                CampoBD = xmltempl.NomeCampo.Replace(" ", ""),
                                isATfield = true
                            };

                            _eCCListRepositories.eConnectorXmlResumoIvaRepository
                                .Add(newDBXMLIva)
                                .Save();

                            countIva++;
                        }
                    });

                    dbContextTransaction.Commit();

                    if (Request.IsAjaxRequest())
                        return Json(this.HandleCreateReply("EditCustomer", new { pkid = newDB.PKID, instance = id }));
                    else
                        return RedirectToAction("EditCustomer", new { pkid = newDB.PKID, instance = id });

                }
                catch (Exception ex)
                {
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

                    dbContextTransaction.Rollback();
                    Flash.Instance.Error(Texts.DBErrors);
                    return RedirectToAction("Customer");
                }

            }

        }

        [PersonAuthorize(Permissions.ECONNECTOR_CLIENTES_CLIENTES)]
        public ActionResult EditCustomer(Guid pkid, Guid instance)
        {
            var model = _eCCListRepositories.eConnectorCustomersRepository.Find(pkid);

            var markets = _eCCListRepositories.eBCMarketsRepository.Set.ToList();

            var modelVM = new EBCCustomersVM(model, _eCCListRepositories, _context, instance, pkid, markets);

            if (Request.IsAjaxRequest())
                return Json(this.ModalContentReply(modelVM), JsonRequestBehavior.AllowGet);
            else
                return PartialView(modelVM);
        }

        [HttpPost]
        [PersonAuthorize(Permissions.ECONNECTOR_CLIENTES_CLIENTES)]
        public ActionResult EditCustomer(Guid PKID, Guid FKInstanceID, CustomerData data)
        {
            using (var dbContextTransaction = _eCCListRepositories.instancesRepository.Context.Database.BeginTransaction())
            {
                var dataFromDB = _eCCListRepositories.eConnectorCustomersRepository.Find(PKID);
                try
                {
                    if (!ModelState.IsValid)
                    {
                        Flash.Instance.Error(Texts.DBErrors);

                        var model = _eCCListRepositories.instancesRepository.GetEBC_Instances(_context.UserIdentity.Instances);
                        var markets = _eCCListRepositories.eBCMarketsRepository.Set.ToList();
                        var modelVM = new EBCCustomersVM(dataFromDB, _eCCListRepositories, _context, FKInstanceID, PKID, markets);

                        if (Request.IsAjaxRequest())
                            return Json(this.ModalContentReply(modelVM));
                        else
                            return PartialView(modelVM);
                    }


                    dataFromDB.Name = data.Name;
                    dataFromDB.Email = data.Email;
                    dataFromDB.NIF = data.NIF;
                    dataFromDB.Mercado = data.Mercado;
                    dataFromDB.XMLAss = data.XMLAss;
                    dataFromDB.XMLNAss = data.XMLNAss;
                    dataFromDB.PDFAss = data.PDFAss;
                    dataFromDB.PDFNAss = data.PDFNAss;

                    _eCCListRepositories
                        .eConnectorCustomersRepository
                        .Edit(dataFromDB)
                        .Save();

                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

                    dbContextTransaction.Rollback();
                    Flash.Instance.Error(Texts.DBErrors);

                    var model = _eCCListRepositories.instancesRepository.GetEBC_Instances(_context.UserIdentity.Instances);
                    var markets = _eCCListRepositories.eBCMarketsRepository.Set.ToList();
                    var modelVM = new EBCCustomersVM(dataFromDB, _eCCListRepositories, _context, FKInstanceID, PKID, markets);

                    if (Request.IsAjaxRequest())
                        return Json(this.ModalContentReply(modelVM));
                    else
                        return PartialView(modelVM);
                }

            }

            Flash.Instance.Success(Texts.EditOperationSuccess);
            return Json(this.CloseModalReply());
        }

        [HttpPost]
        [PersonAuthorize(Permissions.ECONNECTOR_CLIENTES_CLIENTES)]
        public ActionResult DeleteCustomer(Guid pkid)
        {
            using (var dbContextTransaction = _eCCListRepositories.instancesRepository.Context.Database.BeginTransaction())
            {
                try
                {
                    var dataFromDB = _eCCListRepositories.eConnectorCustomersRepository.Find(pkid);
                    var ebcXmlClient = _eCCListRepositories.eConnectorXmlClientRepository.Where(exc => exc.FkCliente == pkid).FirstOrDefault();
                    var ebcXmlHeader = _eCCListRepositories.eConnectorXmlHeaderRepository.Where(exi => exi.NumeroXML == ebcXmlClient.NumeroXML).ToList();
                    var ebcXmlLine = _eCCListRepositories.eConnectorXmlLinesRepository.Where(exi => exi.NumeroXML == ebcXmlClient.NumeroXML).ToList();
                    var ebcXmlIva = _eCCListRepositories.eConnectorXmlResumoIvaRepository.Where(exi => exi.NumeroXML == ebcXmlClient.NumeroXML).ToList();

                    ebcXmlHeader.ForEach(delegate (EBC_XML xml)
                    {
                        _eCCListRepositories.eConnectorXmlHeaderRepository
                            .Delete(xml)
                            .Save();
                    });

                    ebcXmlLine.ForEach(delegate (EBC_XMLLines xmlline)
                    {
                        _eCCListRepositories.eConnectorXmlLinesRepository
                            .Delete(xmlline)
                            .Save();
                    });

                    ebcXmlIva.ForEach(delegate (EBC_XMLResumoIVA xml)
                    {
                        _eCCListRepositories.eConnectorXmlResumoIvaRepository
                            .Delete(xml)
                            .Save();
                    });

                    _eCCListRepositories
                        .eConnectorXmlClientRepository
                        .Delete(ebcXmlClient)
                        .Save();

                    _eCCListRepositories
                        .eConnectorCustomersRepository
                        .Delete(dataFromDB)
                        .Save();

                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

                    dbContextTransaction.Rollback();
                    Flash.Instance.Error(Texts.DBErrors);
                    return RedirectToAction("Customer");
                }
            }

            return RedirectToAction("Customer");
        }
        #endregion
    }
}