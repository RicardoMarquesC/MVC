using eBillingSuite.Globalization;
using eBillingSuite.Model.EBC_DB;
using eBillingSuite.Repositories.Support;
using eBillingSuite.ViewModels;
using MvcFlash.Core;
using MvcFlash.Core.Extensions;
using Shortcut.PixelAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace eBillingSuite.Controllers
{
    public class SupportController : Controller
    {

        private IPixelAdminPageContext _pixelAdminPageContext;
        private IECCListRepositories _eCConfigRepositories;
        private IeBillingSuiteRequestContext _context;
        private ITicketsRepository _ticketsRepository;



        public SupportController(IPixelAdminPageContext pixelAdminPageContext,
            IECCListRepositories eCConfigRepositories,
            IeBillingSuiteRequestContext context,
            ITicketsRepository ticketsRepository)
        {
            _pixelAdminPageContext = pixelAdminPageContext;
            _eCConfigRepositories = eCConfigRepositories;
            _context = context;
            _ticketsRepository = ticketsRepository;
        }

        // GET: Support
        public ActionResult Index()
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);

            //if (Request.IsAjaxRequest())
            //    return Json(this.PanelContentReply(View()), JsonRequestBehavior.AllowGet);
            //else
            //    return View();

            return View();
        }

        public ActionResult RequestAcessControl()
        {
            this.SetPixelAdminPageContext(_pixelAdminPageContext);
            var modelVM = new SupportChatVM("","","");
            return PartialView("RequestAcessControl", modelVM);

            
        }

        public ActionResult RemoteAcessRequest(string email, string descricao)
        {
            try
            {
                using (var dbContextTransaction = _ticketsRepository.Context.Database.BeginTransaction())
                {
                    var newTicket = new Tickets
                    {
                        pkid = Guid.NewGuid(),
                        TipoPedido = "RemoteAcess",
                        Descricao = descricao,
                        Email = email,
                        Data = DateTime.Now,
                        Tratado = false,
                    };

                    _ticketsRepository.Set.Add(newTicket);
                    _ticketsRepository.Save();
                    dbContextTransaction.Commit();

                    try
                    {
                        MailMessage e_mail = new MailMessage();
                        e_mail.To.Add("tiago.esteves@pi-co.com");
                        e_mail.Subject = "NOMEDAEMPRESA || Pedido de Acesso Remoto "+DateTime.Now;
                        e_mail.From = new MailAddress(email);
                        e_mail.IsBodyHtml = true;
                        e_mail.Body = "<p>Descrição do Problema: "+descricao+"</p>";
                        e_mail.Priority = MailPriority.High;
                        SmtpClient smtp = new SmtpClient("mail.pi-co.com");
                        smtp.Port = 25;
                        //SmtpClient smtp = new SmtpClient("mail.pi-co.com", 25);
                        smtp.EnableSsl = false;
                        System.Net.NetworkCredential cred = new System.Net.NetworkCredential("ebc.teste@pi-co.com", "eBC#123-PI");
                        smtp.Credentials = cred;
                        smtp.UseDefaultCredentials = true;

                        smtp.Send(e_mail);

                    }
                    catch (Exception e)
                    {
                        //a ver com o tiago
                    }

                    return Json(Flash.Instance.Success("Amazing"));
                    //return true;
                }
            }
            catch(Exception ex)
            {
                return Json(Flash.Instance.Warning("Deste erro!"));
            }

            
        }
    }
}