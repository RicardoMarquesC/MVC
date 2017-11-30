using eBillingSuite.Globalization;
using eBillingSuite.Model.EBC_DB;
using eBillingSuite.Models;
using eBillingSuite.Repositories;
using eBillingSuite.Security;
using eBillingSuite.ViewModels;
using MvcFlash.Core;
using MvcFlash.Core.Extensions;
using Shortcut.PixelAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eBillingSuite.Controllers
{
    public class eConnectorCustomersController : Controller
    {
		private IPixelAdminPageContext _pixelAdminPageContext;
		protected readonly IeBillingSuiteRequestContext _context;		
		private IECCListRepositories _eCConfigRepositories;

		public eConnectorCustomersController(IPixelAdminPageContext pixelAdminPageContext,
			IECCListRepositories eCConfigRepositories,			
			IeBillingSuiteRequestContext context)
		{
			_pixelAdminPageContext = pixelAdminPageContext;			
			_eCConfigRepositories = eCConfigRepositories;
			_context = context;			
		}

		

		#region XmlConfigs
		[PersonAuthorize(Permissions.ECONNECTOR_CLIENTES_CONFIGURACOESXML)]
		public ActionResult XmlConfigs(Guid? id, Guid? idcustomer, string tipoxml)
		{		

			this.SetPixelAdminPageContext(_pixelAdminPageContext);

			if (!id.HasValue)
				id = _eCConfigRepositories.instancesRepository.GetEBC_Instances().FirstOrDefault().PKID;

			if (!idcustomer.HasValue)
				idcustomer = _eCConfigRepositories.eConnectorCustomersRepository
				.Where(ec => ec.FKInstanceID == id.Value).FirstOrDefault().PKID;

			var existingInstances = _eCConfigRepositories.instancesRepository.GetEBC_Instances();
			var modelVM = new EBCCustomersXMLConfigsVM(existingInstances, _eCConfigRepositories, id.Value, idcustomer.Value, tipoxml);

			if (Request.IsAjaxRequest())
				return Json(this.PanelContentReply(modelVM), JsonRequestBehavior.AllowGet);
			else
				return View(modelVM);
		}

		[PersonAuthorize(Permissions.ECONNECTOR_CLIENTES_CONFIGURACOESXML)]
		public ActionResult CreateXmlConfig(Guid id, Guid idcustomer, string tipoxml)
		{
			var numeroxml = _eCConfigRepositories.eConnectorXmlClientRepository.GetXMLClientNumberByFKClient(idcustomer);
			var modelVM = new EBCCustomersXMLConfigsVM(_eCConfigRepositories, id, idcustomer, tipoxml,numeroxml);

			if (Request.IsAjaxRequest())
				return Json(this.ModalContentReply("AddXmlConfig", modelVM), JsonRequestBehavior.AllowGet);
			else
				return PartialView("AddXmlConfig",modelVM);
		}

		[HttpPost]
		[PersonAuthorize(Permissions.ECONNECTOR_CLIENTES_CONFIGURACOESXML)]
		public ActionResult AddXmlConfig(Guid FKInstanceID, Guid FKCustomerID, XmlConfigData data)
		{
			if (!ModelState.IsValid)
			{
				var numeroxml = _eCConfigRepositories.eConnectorXmlClientRepository.GetXMLClientNumberByFKClient(FKCustomerID);
				var modelVM = new EBCCustomersXMLConfigsVM(_eCConfigRepositories, FKInstanceID, FKCustomerID, data.tipoXml, data.numeroxml);

				if (Request.IsAjaxRequest())
					return Json(this.ModalContentReply(modelVM), JsonRequestBehavior.AllowGet);
				else
					return PartialView(modelVM);
			}
			
			using (var dbContextTransaction = _eCConfigRepositories.instancesRepository.Context.Database.BeginTransaction())
			{
				try
				{
					
					if(data.isEdit)
					{
						char[] delimitador = { '-' };
						var valuestemp = data.selectedField.Split(delimitador);
						var nomecampo = valuestemp[0].TrimEnd();
						var campoBD = valuestemp[0].Replace(" ", "");
												
						if (valuestemp[1].Trim().ToLower().Equals(eBillingSuite.Enumerations.DigitalDocumentAreas.HEADER))
						{
							var dataFromDB = _eCConfigRepositories
								.eConnectorXmlHeaderRepository
								.Where(exh => exh.NumeroXML == data.numeroxml && exh.NomeCampo == nomecampo)
								.FirstOrDefault();

							dataFromDB.Obrigatorio = data.mandatory;

							_eCConfigRepositories
								.eConnectorXmlHeaderRepository
								.Edit(dataFromDB)
								.Save();
							
						}
						else if (valuestemp[1].Trim().ToLower().Equals(eBillingSuite.Enumerations.DigitalDocumentAreas.LINES))
						{
							var dataFromDB = _eCConfigRepositories
								.eConnectorXmlLinesRepository
								.Where(exh => exh.NumeroXML == data.numeroxml && exh.NomeCampo == nomecampo)
								.FirstOrDefault();

							dataFromDB.Obrigatorio = data.mandatory;

							_eCConfigRepositories
								.eConnectorXmlLinesRepository
								.Edit(dataFromDB)
								.Save();
						}
						else
						{
							var dataFromDB = _eCConfigRepositories
								.eConnectorXmlResumoIvaRepository
								.Where(exh => exh.NumeroXML == data.numeroxml && exh.NomeCampo == nomecampo)
								.FirstOrDefault();

							dataFromDB.Obrigatorio = data.mandatory;

							_eCConfigRepositories
								.eConnectorXmlResumoIvaRepository
								.Edit(dataFromDB)
								.Save();
						}
					}
					else
					{
						var tipoxmlOriginal = String.Empty;
						//verifica se o tipo de xml escolhido era o que estava escolhido anteriormente
						var exists = _eCConfigRepositories
							.eConnectorXmlHeaderRepository
							.Set.Any(exh => exh.NumeroXML == data.numeroxml);
						if (!exists)
						{
							exists = _eCConfigRepositories
							.eConnectorXmlLinesRepository
							.Set.Any(exh => exh.NumeroXML == data.numeroxml);
							if (!exists)
							{
								exists = _eCConfigRepositories
									.eConnectorXmlResumoIvaRepository
									.Set.Any(exh => exh.NumeroXML == data.numeroxml);
								if (!exists)
									tipoxmlOriginal = data.tipoXml;
								else
									tipoxmlOriginal = _eCConfigRepositories
									.eConnectorXmlLinesRepository
									.Where(exh => exh.NumeroXML == data.numeroxml)
									.FirstOrDefault()
									.TipoXML;
							}
							else
								tipoxmlOriginal = _eCConfigRepositories
									.eConnectorXmlLinesRepository
									.Where(exh => exh.NumeroXML == data.numeroxml)
									.FirstOrDefault()
									.TipoXML;
						}
						else
						{
							tipoxmlOriginal = _eCConfigRepositories
							.eConnectorXmlHeaderRepository
							.Where(exh => exh.NumeroXML == data.numeroxml)
							.FirstOrDefault()
							.TipoXML;
						}

						if (data.tipoXml != tipoxmlOriginal)
						{
							//tipo xml foi mudado.
							//apagar o que existe desse tipo xml para esse cliente

							_eCConfigRepositories
								.eConnectorXmlHeaderRepository
								.Set
								.RemoveRange(_eCConfigRepositories
								.eConnectorXmlHeaderRepository
								.Where(exh => exh.NumeroXML == data.numeroxml)
								.ToList());

							_eCConfigRepositories
								.eConnectorXmlLinesRepository
								.Set
								.RemoveRange(_eCConfigRepositories
								.eConnectorXmlLinesRepository
								.Where(exh => exh.NumeroXML == data.numeroxml)
								.ToList());

							_eCConfigRepositories
								.eConnectorXmlResumoIvaRepository
								.Set
								.RemoveRange(_eCConfigRepositories
								.eConnectorXmlResumoIvaRepository
								.Where(exh => exh.NumeroXML == data.numeroxml)
								.ToList());

						}

						//verificar se o campo e cabecalho, lineitem ou resumoiva
						char[] delimitador = { '-' };
						var valuestemp = data.selectedField.Split(delimitador);
						var nomecampo = valuestemp[0].TrimEnd();
						var campoBD = valuestemp[0].Replace(" ", "");

						if (valuestemp[1].Trim().ToLower().Equals(eBillingSuite.Enumerations.DigitalDocumentAreas.HEADER))
						{
							var dataForDB = new EBC_XML
							{
								NumeroXML = data.numeroxml,
								Element = _eCConfigRepositories
											.eConnectorXmlTemplateRepository.Where(ext => ext.TipoXML == data.tipoXml
												&&
												ext.Tipo.ToLower() == eBillingSuite.Enumerations.DigitalDocumentAreas.HEADER
												&&
												ext.NomeCampo == nomecampo
												)
												.FirstOrDefault()
												.CaminhoXML,
								NomeCampo = nomecampo,
								TipoXML = data.tipoXml,
								Obrigatorio = data.mandatory,
								Posicao = (_eCConfigRepositories
											.eConnectorXmlHeaderRepository
											.Where(exh => exh.NumeroXML == data.numeroxml)
											.OrderByDescending(exh => exh.Posicao).FirstOrDefault()
											.Posicao)
											+ 1,
								CampoBD = campoBD,
								isATfield = _eCConfigRepositories
												.eConnectorXmlTemplateRepository
												.Where(ext => ext.TipoXML == data.tipoXml
												&&
												ext.Tipo.ToLower() == eBillingSuite.Enumerations.DigitalDocumentAreas.HEADER
												&&
												ext.NomeCampo == nomecampo
												)
												.FirstOrDefault()
												.isATfield
							};

							_eCConfigRepositories
								.eConnectorXmlHeaderRepository
								.Add(dataForDB)
								.Save();
						}
						else if (valuestemp[1].Trim().ToLower().Equals(eBillingSuite.Enumerations.DigitalDocumentAreas.LINES))
						{
							var dataForDB = new EBC_XMLLines
							{
								NumeroXML = data.numeroxml,
								Element = _eCConfigRepositories
											.eConnectorXmlTemplateRepository.Where(ext => ext.TipoXML == data.tipoXml
												&&
												ext.Tipo.ToLower() == eBillingSuite.Enumerations.DigitalDocumentAreas.LINES
												&&
												ext.NomeCampo == nomecampo
												)
												.FirstOrDefault()
												.CaminhoXML,
								NomeCampo = nomecampo,
								TipoXML = data.tipoXml,
								Obrigatorio = data.mandatory,
								Posicao = (_eCConfigRepositories
											.eConnectorXmlLinesRepository
											.Where(exh => exh.NumeroXML == data.numeroxml)
											.OrderByDescending(exh => exh.Posicao)
											.FirstOrDefault()
											.Posicao)
											+ 1,
								CampoBD = campoBD,
								isATfield = _eCConfigRepositories
												.eConnectorXmlTemplateRepository
												.Where(ext => ext.TipoXML == data.tipoXml
												&&
												ext.Tipo.ToLower() == eBillingSuite.Enumerations.DigitalDocumentAreas.LINES
												&&
												ext.NomeCampo == nomecampo
												)
												.FirstOrDefault()
												.isATfield
							};

							_eCConfigRepositories.eConnectorXmlLinesRepository.Add(dataForDB).Save();
						}
						else
						{
							var dataForDB = new EBC_XMLResumoIVA
							{
								NumeroXML = data.numeroxml,
								Element = _eCConfigRepositories
											.eConnectorXmlTemplateRepository.Where(ext => ext.TipoXML == data.tipoXml
												&&
												ext.Tipo.ToLower() == eBillingSuite.Enumerations.DigitalDocumentAreas.VAT
												&&
												ext.NomeCampo == nomecampo
												)
												.FirstOrDefault()
												.CaminhoXML,
								NomeCampo = nomecampo,
								TipoXML = data.tipoXml,
								Obrigatorio = data.mandatory,
								Posicao = (_eCConfigRepositories.eConnectorXmlResumoIvaRepository
												.Where(exh => exh.NumeroXML == data.numeroxml)
												.OrderByDescending(exh => exh.Posicao)
												.FirstOrDefault()
												.Posicao)
												+ 1,
								CampoBD = campoBD,
								isATfield = _eCConfigRepositories
												.eConnectorXmlTemplateRepository
												.Where(ext => ext.TipoXML == data.tipoXml
												&&
												ext.Tipo.ToLower() == eBillingSuite.Enumerations.DigitalDocumentAreas.VAT
												&&
												ext.NomeCampo == nomecampo
												)
												.FirstOrDefault()
												.isATfield
							};

							_eCConfigRepositories
								.eConnectorXmlResumoIvaRepository
								.Add(dataForDB)
								.Save();
						}
					}
					
					dbContextTransaction.Commit();
				}
				catch (Exception ex)
				{
					Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

					dbContextTransaction.Rollback();
					Flash.Instance.Error(_context.GetDictionaryValue(DictionaryEntryKeys.DBErrors));
					
					var modelVM = new EBCCustomersXMLConfigsVM(_eCConfigRepositories, FKInstanceID, FKCustomerID, data.tipoXml, data.numeroxml);

					if (Request.IsAjaxRequest())
						return Json(this.ModalContentReply(modelVM), JsonRequestBehavior.AllowGet);
					else
						return PartialView(modelVM);

				}
			}
			Flash.Instance.Success(_context.GetDictionaryValue(DictionaryEntryKeys.EditOperationSuccess));
			return Json(this.CloseModalReply());
		}

		[PersonAuthorize(Permissions.ECONNECTOR_CLIENTES_CONFIGURACOESXML)]
		public ActionResult EditXmlConfig(Guid id, Guid idcustomer, string tipoxml, int numeroxml, string nomecampo)
		{			
			var modelVM = new EBCCustomersXMLConfigsVM(_eCConfigRepositories, id, idcustomer, tipoxml, numeroxml, nomecampo, true);

			if (Request.IsAjaxRequest())
				return Json(this.ModalContentReply("AddXmlConfig", modelVM), JsonRequestBehavior.AllowGet);
			else
				return PartialView("AddXmlConfig", modelVM);
		}
		#endregion
	}
}