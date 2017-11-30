using eBillingSuite.Globalization;
using eBillingSuite.Model.EDI_DB;
using eBillingSuite.Model.HelpingClasses;
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
using System.Web.UI;

namespace eBillingSuite.Controllers
{
	public class eEDIController : Controller
    {
		private IPixelAdminPageContext _pixelAdminPageContext;
		private IeBillingSuiteRequestContext _context;
		private IEEDIInstancesRepository _instancesRepository;
		private IEEDICostumersRepository _eEDICostumersRepository;
		private IEEDISendersRepository _eEDISendersRepository;
		private IEEDIReceivedDocsRepository _eEDIReceivedDocsRepository;
		private IEEDISentDocsRepository _eEDISentDocsRepository;
		private IEEDISentDocsDetailsRepository _eEDISentDocsDetailsRepository;

		public eEDIController(
			IPixelAdminPageContext pixelAdminPageContext,
			IEEDIInstancesRepository instancesRepository,
			IEEDICostumersRepository eEDICostumersRepository,
			IEEDISendersRepository eEDISendersRepository,
			IEEDIReceivedDocsRepository eEDIReceivedDocsRepository,
			IEEDISentDocsRepository eEDISentDocsRepository,
			IEEDISentDocsDetailsRepository eEDISentDocsDetailsRepository,
			IeBillingSuiteRequestContext context)
		{
			_pixelAdminPageContext = pixelAdminPageContext;
			_instancesRepository = instancesRepository;
			_eEDISendersRepository = eEDISendersRepository;
			_eEDICostumersRepository = eEDICostumersRepository;
			_eEDIReceivedDocsRepository = eEDIReceivedDocsRepository;
			_eEDISentDocsRepository = eEDISentDocsRepository;
			_eEDISentDocsDetailsRepository = eEDISentDocsDetailsRepository;
			_context = context;
		}

		#region EDICostumers
		[PersonAuthorize(Permissions.EEDI_CLIENTES)]
		[ActionName("Index")]
        public ActionResult Costumers(Guid? ID)
        {
			this.SetPixelAdminPageContext(_pixelAdminPageContext);

			if (!_context.UserIdentity.IsEBEActive)
				return View("InactiveModule");

			var availableInstances = _instancesRepository.GetInstances();
			var data = _eEDICostumersRepository
				.GetCostumersByInstanceID(ID.HasValue ? ID.Value : availableInstances.FirstOrDefault().PKID);

			var model = new EEDICostumersVM(
				data,
				availableInstances
				);

			if (Request.IsAjaxRequest())
				return Json(this.PanelContentReply("Costumers", model), JsonRequestBehavior.AllowGet);
			else
				return View("Costumers",model);
        }

		[HttpPost]
		[PersonAuthorize(Permissions.EEDI_CLIENTES)]
		public ActionResult CreateCostumer(Guid ID)
		{
			var availableInstances = _instancesRepository.GetInstances();

			var newDB = new Clientes
			{
				PKID = Guid.NewGuid(),
				Nome = Texts.Untitled,
				FKInstanciaID = ID,
				NIF = "PT999999990",
				URL = "http://testing.pt",
				TempoEspera = 0,
				TempoEsperaUnidade = 0,
				Tentativas = 0,
				Intervalo = 0,
				IntervaloUnidade = 0
			};

			_eEDICostumersRepository
				.Add(newDB)
				.Save();

			if (Request.IsAjaxRequest())
				return Json(this.HandleCreateReply("EditCostumer", new {pkid = newDB.PKID }));
			else
				return RedirectToAction("EditCostumer", new { pkid = newDB.PKID });
		}

		[PersonAuthorize(Permissions.EEDI_CLIENTES)]
		public ActionResult EditCostumer(Guid pkid)
		{
			var model = _eEDICostumersRepository.Find(pkid);
			var modelVM = new EEDICostumersVM(model);

			if (Request.IsAjaxRequest())
				return Json(this.ModalContentReply(modelVM), JsonRequestBehavior.AllowGet);
			else
				return PartialView(modelVM);
		}

		[HttpPost]
		[PersonAuthorize(Permissions.EEDI_CLIENTES)]
		public ActionResult EditCostumer(Guid pkid, EDICostumerData data)
		{
			var dataFromDB = _eEDICostumersRepository.Find(pkid);
			var modelVM = new EEDICostumersVM(data);
			if (!ModelState.IsValid)
			{	
				Flash.Instance.Error(Texts.FixModelErrors);
				
				if (Request.IsAjaxRequest())
					return Json(this.ModalContentReply(modelVM));
				else
					return PartialView(modelVM);
			}

			using (var dbContextTransaction = _eEDICostumersRepository.Context.Database.BeginTransaction())
			{
				try
				{
					dataFromDB.Intervalo = data.Intervalo;
					dataFromDB.IntervaloUnidade = data.Intervalo;
					dataFromDB.NIF = data.NIF;
					dataFromDB.Nome = data.Nome;
					dataFromDB.TempoEspera = data.TempoEspera;
					dataFromDB.TempoEsperaUnidade = data.TempoEsperaUnidade;
					dataFromDB.URL = data.URL;
					
					_eEDICostumersRepository
						.Edit(dataFromDB)
						.Save();

					dbContextTransaction.Commit();
				}
				catch(Exception ex)
				{
					Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

					dbContextTransaction.Rollback();
					Flash.Instance.Error(Texts.DBErrors);

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
		[PersonAuthorize(Permissions.EEDI_CLIENTES)]
		public ActionResult DeleteCostumer(Guid pkid)
		{
			using (var dbContextTransaction = _eEDICostumersRepository.Context.Database.BeginTransaction())
			{
				try
				{
					var dataFromDB = _eEDICostumersRepository.Find(pkid);
					_eEDICostumersRepository
						.Delete(dataFromDB)
						.Save();

					dbContextTransaction.Commit();
				}
				catch (Exception ex)
				{
					Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

					dbContextTransaction.Rollback();
					Flash.Instance.Error(Texts.DBErrors);
					return RedirectToAction("");
				}
			}

			return RedirectToAction("");
		}

		#endregion

		#region EDISenders
		[PersonAuthorize(Permissions.EEDI_REMETENTES)]
		public ActionResult Senders()
		{
			this.SetPixelAdminPageContext(_pixelAdminPageContext);
			var availableInstances = _instancesRepository.GetInstances();
			var model = _eEDISendersRepository
							.Set
							.ToList();

			if (Request.IsAjaxRequest())
				return Json(this.PanelContentReply("Senders", model), JsonRequestBehavior.AllowGet);
			else
				return View("Senders", model);
		}

		[HttpPost]
		[PersonAuthorize(Permissions.EEDI_REMETENTES)]
		public ActionResult CreateSender()
		{
			var newDB = new Remetentes
			{
				PKID = Guid.NewGuid(),
				Nome = Texts.Untitled,
				NIF = "PT999999990",
				URL = "http://testing.pt",
				Activo = true
			};

			_eEDISendersRepository
				.Add(newDB)
				.Save();

			if (Request.IsAjaxRequest())
				return Json(this.HandleCreateReply("EditSender", new { pkid = newDB.PKID }));
			else
				return RedirectToAction("EditSender", new { pkid = newDB.PKID });
		}

		[PersonAuthorize(Permissions.EEDI_REMETENTES)]
		public ActionResult EditSender(Guid pkid)
		{
			var model = _eEDISendersRepository.Find(pkid);

			if (Request.IsAjaxRequest())
				return Json(this.ModalContentReply(model), JsonRequestBehavior.AllowGet);
			else
				return PartialView(model);
		}

		[HttpPost]
		[PersonAuthorize(Permissions.EEDI_REMETENTES)]
		public ActionResult EditSender(Guid pkid, EDISenderData data)
		{
			var dataFromDB = _eEDISendersRepository.Find(pkid);

			if (!ModelState.IsValid)
			{
				Flash.Instance.Error(Texts.FixModelErrors);

				if (Request.IsAjaxRequest())
					return Json(this.ModalContentReply(dataFromDB));
				else
					return PartialView(dataFromDB);
			}

			using (var dbContextTransaction = _eEDISendersRepository.Context.Database.BeginTransaction())
			{
				try
				{
					dataFromDB.Nome = data.Nome;
					dataFromDB.URL = data.URL;
					dataFromDB.NIF = data.NIF;
					dataFromDB.Activo = data.Activo;

					_eEDISendersRepository
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

		[HttpPost]
		[PersonAuthorize(Permissions.EEDI_REMETENTES)]
		public ActionResult DeleteSender(Guid pkid)
		{
			using (var dbContextTransaction = _eEDISendersRepository.Context.Database.BeginTransaction())
			{
				try
				{
					var dataFromDB = _eEDISendersRepository.Find(pkid);
					_eEDISendersRepository
						.Delete(dataFromDB)
						.Save();

					dbContextTransaction.Commit();
				}
				catch (Exception ex)
				{
					Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

					dbContextTransaction.Rollback();
					Flash.Instance.Error(Texts.DBErrors);
					return RedirectToAction("Senders");
				}
			}

			return RedirectToAction("Senders");
		}
		#endregion

		#region ReceivedDocs
		[PersonAuthorize(Permissions.EEDI_DOCUMENTOSRECEBIDOS)]
		public ActionResult ReceivedDocs(JQueryDataTableParamModel param)
		{
			this.SetPixelAdminPageContext(_pixelAdminPageContext);
			var model = _eEDIReceivedDocsRepository.GetInboundPacket();

			List<InboundPacket> filteredDocs;			

			if (!string.IsNullOrWhiteSpace(param.sSearch))
				filteredDocs = _eEDIReceivedDocsRepository.GetInboundPacket(param.sSearch);
			else
				filteredDocs = model;

			//para a ordenação
			var isNumDocSortable = Convert.ToBoolean(Request["bSortable_1"]);
			var isNomeFornecedorSortable = Convert.ToBoolean(Request["bSortable_2"]);
			var isNomeClienteSortable = Convert.ToBoolean(Request["bSortable_3"]);
			var isQuantiaSortable = Convert.ToBoolean(Request["bSortable_4"]);
			var isDataSortable = Convert.ToBoolean(Request["bSortable_5"]);
			var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);

			Func<InboundPacket, string> orderingFunction = (c => sortColumnIndex == 0 && isNumDocSortable ? c.NumFactura :
														   sortColumnIndex == 1 && isNomeFornecedorSortable ? c.NomeEmissor :
														   sortColumnIndex == 2 && isNomeClienteSortable ? c.NomeReceptor.ToString() :
														   sortColumnIndex == 3 && isQuantiaSortable ? c.QuantiaComIVA.ToString() :
														   sortColumnIndex == 4 && isDataSortable ? c.DataRecepcao.ToString() :
														   "");

			var sortDirection = Request["sSortDir_0"]; // asc or desc
			if (sortDirection == "asc")
				filteredDocs = filteredDocs.OrderBy(orderingFunction).ToList();
			else
				filteredDocs = filteredDocs.OrderByDescending(orderingFunction).ToList();

			if (param.iDisplayLength != 0)
			{
				model = filteredDocs.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

				var result = from c in model
							 select new[] { c.NumFactura, c.NomeEmissor, c.NomeReceptor, 
					c.QuantiaComIVA, c.DataRecepcao.ToDataTableShortFormat(), CreateEditButton(c.InternalProcessID.ToString(), "EDIInboundDetail")};
				return Json(new
				{
					sEcho = param.sEcho,
					iTotalRecords = model.Count(),
					iTotalDisplayRecords = filteredDocs.Count(),
					aaData = result
				},
				JsonRequestBehavior.AllowGet);
			}
			else
			{
				model = filteredDocs.Skip(0).Take(10).ToList();
				
				if (Request.IsAjaxRequest())
					return Json(this.PanelContentReply("ReceivedDocs", model), JsonRequestBehavior.AllowGet);
				else
					return View("ReceivedDocs", model);
			}
		}
		#endregion

		#region SentDocs
		[PersonAuthorize(Permissions.EEDI_DOCUMENTOSENVIADOS)]
		public ActionResult SentDocs(JQueryDataTableParamModel param)
		{
			this.SetPixelAdminPageContext(_pixelAdminPageContext);
			var model = _eEDISentDocsRepository
				.Set
				.OrderByDescending(o => o.DataCriacao)
				.ToList();

			List<OutboundPacket> filteredDocs;

			if (!string.IsNullOrWhiteSpace(param.sSearch))
				filteredDocs = _eEDISentDocsRepository.GetOutboundPackets(param.sSearch)
					.OrderByDescending(o => o.DataCriacao)
					.ToList();
			else
				filteredDocs = model;

			//para a ordenação
			var isNumDocSortable = Convert.ToBoolean(Request["bSortable_1"]);
			var isNomeFornecedorSortable = Convert.ToBoolean(Request["bSortable_3"]);
			var isNomeClienteSortable = Convert.ToBoolean(Request["bSortable_2"]);
			var isQuantiaSortable = Convert.ToBoolean(Request["bSortable_4"]);
			var isDataSortable = Convert.ToBoolean(Request["bSortable_5"]);
			var isEstornoSortable = Convert.ToBoolean(Request["bSortable_6"]);
			var isStateSortable = Convert.ToBoolean(Request["bSortable_7"]);
			var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);

			Func<OutboundPacket, string> orderingFunction = (c => sortColumnIndex == 0 && isNumDocSortable ? c.NumFactura :
														   sortColumnIndex == 1 && isNomeClienteSortable ? c.NomeReceptor :
														   sortColumnIndex == 2 && isNomeFornecedorSortable ? c.NomeEmissor.ToString() :
														   sortColumnIndex == 3 && isQuantiaSortable ? c.QuantiaComIVA.ToString() :
														   sortColumnIndex == 4 && isDataSortable ? c.DataCriacao.ToString() :
														   sortColumnIndex == 5 && isEstornoSortable ? c.DocOriginal.ToString() :
														   sortColumnIndex == 6 && isStateSortable ? c.Estado.ToString() :
														   "");

			var sortDirection = Request["sSortDir_0"]; // asc or desc
			if (sortDirection == "asc")
				filteredDocs = filteredDocs.OrderBy(orderingFunction).ToList();
			else
				filteredDocs = filteredDocs.OrderByDescending(orderingFunction).ToList();

			if (param.iDisplayLength != 0)
			{
				model = filteredDocs.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

				var result = from c in model
							 select new[] { c.NumFactura, c.NomeReceptor, c.NomeEmissor, 
					c.QuantiaComIVA, c.DataCriacao.ToDataTableShortFormat(), 
					String.IsNullOrWhiteSpace(c.DocOriginal) ? Texts.Nao : Texts.Sim, 
					c.Estado, CreateEditButton(c.PKEDIPacketID.ToString(), "EDIOutboundDetail")};

				return Json(new
				{
					sEcho = param.sEcho,
					iTotalRecords = model.Count(),
					iTotalDisplayRecords = filteredDocs.Count(),
					aaData = result
				},
				JsonRequestBehavior.AllowGet);
			}
			else
			{
				model = filteredDocs.Skip(0).Take(10).ToList();

				if (Request.IsAjaxRequest())
					return Json(this.PanelContentReply("SentDocs", model), JsonRequestBehavior.AllowGet);
				else
					return View("SentDocs", model);
			}
		}
		#endregion

		#region InboundDetails
		[PersonAuthorize(Permissions.EEDI_DOCUMENTOSRECEBIDOS)]
		public ActionResult EDIInboundDetail(Guid id)
		{
			var model = _eEDIReceivedDocsRepository.Find(id);

			if (Request.IsAjaxRequest())
				return Json(this.ModalContentReply(model), JsonRequestBehavior.AllowGet);
			else
				return PartialView(model);
		}
		#endregion

		#region OutboundDetails
		[PersonAuthorize(Permissions.EEDI_DOCUMENTOSENVIADOS)]
		public ActionResult EDIOutboundDetail(Guid id)
		{
			var modelDB = _eEDISentDocsRepository.Find(id);

			var model = new EEDIOutboundDetailVM(
				modelDB,
				_eEDISentDocsDetailsRepository
				);

			if (Request.IsAjaxRequest())
				return Json(this.ModalContentReply(model), JsonRequestBehavior.AllowGet);
			else
				return PartialView(model);
		}
		#endregion

		#region MetodosAux
		private string CreateEditButton(string InternalProcessID, string accao)
		{
			StringWriter stringWriter = new StringWriter();

			using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Class, "btn btn-primary btn-sm");
				writer.AddAttribute("data-rel", "ajax-edit-trigger");				
				writer.AddAttribute("data-url", Url.Action(accao, new { id=InternalProcessID}));
				writer.RenderBeginTag(HtmlTextWriterTag.Button);
				writer.AddAttribute(HtmlTextWriterAttribute.Class, "fa fa-pencil");
				writer.RenderBeginTag(HtmlTextWriterTag.I);
				writer.RenderEndTag();

				writer.RenderEndTag();
			}

			return stringWriter.ToString();
		}

		public ActionResult InactiveModule()
		{
			this.SetPixelAdminPageContext(_pixelAdminPageContext);

			//string inactiveModuleText = _context.GetDictionaryValue(DictionaryEntryKeys.InactiveModuleAlertText);

			return View();
		}
		#endregion

	}

}