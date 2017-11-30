using MvcFlash.Core;
using MvcFlash.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eBillingSuite
{
	public class AjaxFormReply
	{
		public const string SuccessCommand = "success";
		public const string FailureCommand = "failure";
		public const string EmptyCommand = "";
		public const string CloseModalCommand = "close-modal";
		public const string HandleCreateCommand = "handle-create";

		public AjaxFormReply()
		{
			MessageBase msg = null;
			Messages = new List<FlashMessage>();

			while ((msg = Flash.Instance.Pop()) != null)
				Messages.Add(new FlashMessage(msg));
		}

		public string Command { get; set; }
		public string ModalHtml { get; set; }
		public string PanelHtml { get; set; }
		public string TargetUrl { get; set; }

		public List<FlashMessage> Messages { get; private set; }
	}

	public class FlashMessage
	{

		public FlashMessage()
		{
		}

		public FlashMessage(MessageBase msg)
		{
			Title = msg.Title;
			Content = msg.Content;
			Type = msg.MessageType;
		}

		public string Title { get; set; }
		public string Content { get; set; }
		public string Type { get; set; }
	}

	public static class Extensions
	{
		public static AjaxFormReply SuccessReply(this Controller self)
		{
			return new AjaxFormReply
			{
				Command = AjaxFormReply.SuccessCommand
			};
		}

		public static AjaxFormReply FailureReply(this Controller self)
		{
			return new AjaxFormReply
			{
				Command = AjaxFormReply.FailureCommand
			};
		}

		public static AjaxFormReply ModalContentReply(this Controller self, object model)
		{
			return self.ModalContentReply(self.RouteData.Values["action"] as string, model);
		}

		public static AjaxFormReply ModalContentReply(this Controller self, string viewName, object model)
		{
			return new AjaxFormReply
			{
				Command = AjaxFormReply.EmptyCommand,
				ModalHtml = self.RenderRazorViewToString(viewName, model)
			};
		}

		public static AjaxFormReply PanelContentReply(this Controller self, object model)
		{
			return self.PanelContentReply(self.RouteData.Values["action"] as string, model);
		}

		public static AjaxFormReply PanelContentReply(this Controller self, string viewName, object model)
		{
			return new AjaxFormReply
			{
				Command = AjaxFormReply.EmptyCommand,
				PanelHtml = self.RenderRazorViewToString(viewName, model)
			};
		}

		public static AjaxFormReply CloseModalReply(this Controller self)
		{
			return new AjaxFormReply
			{
				Command = AjaxFormReply.CloseModalCommand
			};
		}

		public static AjaxFormReply HandleCreateReply(this Controller self, string action, object routeValues)
		{
			return self.HandleCreateReply(action, null, routeValues);
		}

		public static AjaxFormReply HandleCreateReply(this Controller self, string action, string controler, object routeValues)
		{
			return new AjaxFormReply
			{
				Command = AjaxFormReply.HandleCreateCommand,
				TargetUrl = self.Url.Action(action, controler, routeValues)
			};
		}
	}
}