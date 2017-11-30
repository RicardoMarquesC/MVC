/*
[INSTRUCTIONS]

[JS]
new AjaxForms("#modal-form'","#modal-form-content");

[HTML]
<button type="button" 
	data-rel="ajax-form-trigger" 					 
	data-url="/PeoplePortal.Tridec/Shortcut/Person/6/WorkingTimeEvent/DoSomething"
	data-panel-id="my-panel">something</button>
<div class="panel-body" 
	id="my-panel"
	data-rel="ajax-update-panel" 
	data-update-url="/MyControler/MyAction/MyID"
	data-create-url="/MyControler/MyCreateAction/MyID"
	data-modal-size="modal-lg"
	data-update-init="functionKey">
	<table>
		<tr>
			<td>My Column(s)</td>
			<td>
				<button type="button" 
					data-rel="ajax-edit-trigger" 					 
					data-url="/PeoplePortal.Tridec/Shortcut/Person/6/WorkingTimeEvent/Edit/34"
					(optional)
					get-on-close="true">edit</button>
				<button type="button" 
					data-rel="ajax-delete-trigger" 					 
					data-url="/PeoplePortal.Tridec/Shortcut/Person/6/WorkingTimeEvent/Delete/34">delete</button>
			</td>
		</tr>
	</table>
	<button type="button" data-rel="ajax-create-trigger" data-url="/MyControler/MyCreateAction/MyID">create</button>
	<button type="button" data-rel="ajax-update-trigger">update</button>
</div>

[SERVER]
Server Reply: {
	Command="{empty}|close-modal|handle-create}",
	ModalHtml="...",
	PanelHtml="...",
	TargetUrl="..."
	Messages = [{ Title="",Content="",Type=""}]
}

[LOGIC]
Server Reply Callback
	If ModalHtml not empty, update Modal content, if Modal hidden then show
	If PanelHtml not empty, update Panel content
	if Command = empty do nothing
	if Command = close-modal then hide modal, clear modal html, updates panel (new ajax request)
	if Command = handle-create then 
		updates panel (new ajax request)
		$get(TargetUrl) -> Server Reply Callback (same thing as edit)

Update Workflow
	User clicks data-rel="ajax-update-trigger"
	AjaxForms does $get data-update-url="/MyControler/MyAction/MyID" from closest data-rel="ajax-update-panel" 
	AjaxForms expects ServerReply {Command = "empty",PanelHtml="..."}

Delete Workflow
	User clicks data-rel="ajax-delete-trigger"
	AjaxForms does $post data-url="/MyControler/MyAction/MyID" from element with data-rel="ajax-delete-trigger"
	AjaxForms expects ServerReply {Command = "success"}
	AjaxForms refreshes panel

Edit (GET) Workflow
	User clicks data-rel="ajax-edit-trigger"
	AjaxForms does $get data-url="/MyControler/MyAction/MyID" from element with data-rel="ajax-edit-trigger"
	AjaxForms expects ServerReply {Command = "empty",ModalHtml="..."}

Edit (POST) Workflow
	User submits modal form
	AjaxForms does $post using the action of the form submitted inside the modal
	AjaxForms expects:
		ServerReply {Command = "empty",ModalHtml="..."} when submition contains new html for the modal (error messages, etc)
		ServerReply {Command = "close-modal"} when submition is completed and no further input is needed

CREATE Workflow
	User clicks data-rel="ajax-create-trigger"
	AjaxForms does POST data-create-url="/MyControler/MyAction/MyID" from closest data-rel="ajax-update-panel" 
	AjaxForms expects ServerReply {Command = "handle-create",TargetUrl="..."}
*/
var AjaxForms = (function () {

	function AjaxForms(modalID, containerID)
	{
		this._$container = $("#" +containerID);
		this._$modal = $("#" + modalID);
		this.attachTo();
	}

	AjaxForms.prototype.runAjaxInitFormFunctions = function () {
		for (var key in ajaxInitFormFunctions)
			ajaxInitFormFunctions[key](this._$container);
	};

	AjaxForms.prototype.attachTo = function () {
		var self = this;

		self.runAjaxInitFormFunctions();

		self._$modal.on('hidden.bs.modal', function (e) {
			self._$container.html('');
		})

		self._$modal.on('click', '[data-rel=submit]', function (e) {
			e.preventDefault();
			$('#' + $(this).data('form')).submit();
			return false;
		});

		self._$modal.on('click', '[data-rel=delete]', function (e) {
			var $trigger = $(this);
			var panelID = self._$modal.data('panel-id');
			$.post($trigger.data('url')).success(function (data) {
				self.handleServerReply(data, null, panelID);
			});
		});

		self._$modal.on('submit', 'form', function (e) {
			e.preventDefault();

			var panelID = self._$modal.data('panel-id');
			var $form = $(this);
			var dataToSend = $form.serialize();

			$.post($form.prop('action'), dataToSend)
				.success(function (data) {
					self.handleServerReply(data, $form, panelID);
				});

			return false;
		});

		$('[data-rel=ajax-form-trigger]').click(function () {
			var $trigger = $(this);

			var panelID = $trigger.data('panel-id');							
			var url = $trigger.data('url');

			$.get(url, function (data) {
				self.handleServerReply(data, $trigger, panelID);
			});
		});

		$('[data-rel=ajax-update-panel]').parent().on("click", "[data-rel=ajax-update-trigger]", (function () {
			var $trigger = $(this);
			var panelID = self.getPanel($trigger).prop('id');

			self.updatePanel(panelID);
		}));

		$('[data-rel=ajax-update-panel]').parent().on("click", "[data-rel=ajax-edit-trigger]", (function () {
			var $trigger = $(this);
			var panelID = self.getPanel($trigger).prop('id');

			var url = $trigger.data('url');			

			$.get(url, function (data) {
				self.handleServerReply(data, $trigger, panelID);
			});
		}));

		$('[data-rel=ajax-update-panel]').parent().on("click", "[data-rel=ajax-create-trigger]", (function () {
			var $trigger = $(this);

			var $panel = self.getPanel($trigger);

			var url = $trigger.data('url');
			if (!url)
				url = $panel.data('create-url');

			if ($trigger.data('method') === 'get')
				$.get(url).success(function (data) {
					self.handleServerReply(data, $trigger, $panel.prop('id'));
				});
			else			
				$.post(url).success(function (data) {
					self.handleServerReply(data, $trigger, $panel.prop('id'));
				});
		}));

		$('[data-rel=ajax-update-panel]').parent().on("click", "[data-rel=ajax-delete-trigger]", (function () {
			var $trigger = $(this);
			var panelID = self.getPanel($trigger).prop('id');
			var url = $trigger.data('url');
			$.post(url).success(function (data) {
				self.handleServerReply(data, $trigger, panelID);
			});
		}));

		$('#jq-datatables').on("click", "tr.detailOBJ", (function () {
		    var $trigger = $(this);
		    var panelID = self.getPanel($trigger).prop('id');
		    var url = $trigger.data('url');
		    $.post(url).success(function (data) {
		        self.handleServerReply(data, $trigger, panelID);
		    });
		}));
	};

	AjaxForms.prototype.getPanel = function ($trigger) {
		return $($trigger.closest('[data-rel=ajax-update-panel]'));
	};

	AjaxForms.prototype.updatePanel = function (panelID) {

		var self = this;

		var url = $('#'+panelID).data('update-url');

		if(url)
			$.get(url, function (data) {
				self.handleServerReply(data, null, panelID);
			});
	};

	AjaxForms.prototype.handleServerReply = function (data, $trigger, panelID) {

		var self = this;
		var $targetPanel = $('#' + panelID);

		var modalIsVisible = self._$container.is(':visible');

		if (data.ModalHtml && !(data.ModalHtml === '')) {

			self._$container.html(data.ModalHtml);
			self.runAjaxInitFormFunctions();

			if (!modalIsVisible) {

				// reset size
				var $dialog = $(self._$modal.children().first());
				$dialog.removeClass("modal-lg");

				// set size
				if ($targetPanel) {
					var size = $targetPanel.data('modal-size');
					if (size)
						$dialog.addClass(size);										
				}

				// show
				self._$modal.modal('show');
				self._$modal.data('panel-id', panelID);
			}
		}
		
		// update panel content
		if ($targetPanel && data.PanelHtml && !(data.PanelHtml === '')) {

			var initFunctionKey = $targetPanel.data('update-init');
			
			$targetPanel.replaceWith(data.PanelHtml);
			
			if (initFunctionKey)
				ajaxInitPanelFunctions[initFunctionKey]();
		}

		// close + success
		if (data.Command == 'close-modal' || data.Command == 'success') {

			if ($trigger && $trigger.data('get-on-close') || $targetPanel && $targetPanel.data('get-on-close'))
			{
				window.location = window.location;
			}
			else if (panelID)
				self.updatePanel(panelID);

			if (modalIsVisible)
			{
				self._$modal.modal('hide');
				self._$modal.data('panel-id', null);
				modalIsVisible = false;
			}
		}

		// create
		if (data.Command == 'handle-create') {
			self.updatePanel(panelID);
			$.get(data.TargetUrl, function (data) {
				self.handleServerReply(data, $trigger, panelID);
			});
		}

		// handle flash
		if (data.Messages && data.Messages.length > 0)
			for (var index in data.Messages)
			{
				var msg = data.Messages[index];
				if (modalIsVisible) {
					var $alert = $('<div class="alert alert-page alert-dark">');
					$alert.addClass('alert-' + (msg.Type == 'error' ? 'danger' : msg.Type));
					$alert.append($('<button type="button" class="close" data-dismiss="alert">×</button>'));
					$alert.append(msg.Content ? msg.Content : msg.Title);
					self._$container.find('.modal-body:first').before($alert);
				}
				else {
					$.growl({ title: msg.Title, message: msg.Content, size: 'large', style: (msg.Type == 'success' ? 'notice' : msg.Type) });
				}
			}
	};

	return AjaxForms;

})();
