﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace eBillingSuite.ebcExternalWS {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ebcExternalWSSoap", Namespace="http://tempuri.org/")]
    public partial class ebcExternalWS : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback HelloWorldOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetAllOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetAllOutboundOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetAllOutboundByNIFOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetAllOutboundByRangeDateOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetAllOutboundByNumDocOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetAllInboundOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetByIDOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public ebcExternalWS() {
            this.Url = global::eBillingSuite.Properties.Settings.Default.eBillingSuite_Host_ebcExternalWS_ebcExternalWS;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event HelloWorldCompletedEventHandler HelloWorldCompleted;
        
        /// <remarks/>
        public event GetAllCompletedEventHandler GetAllCompleted;
        
        /// <remarks/>
        public event GetAllOutboundCompletedEventHandler GetAllOutboundCompleted;
        
        /// <remarks/>
        public event GetAllOutboundByNIFCompletedEventHandler GetAllOutboundByNIFCompleted;
        
        /// <remarks/>
        public event GetAllOutboundByRangeDateCompletedEventHandler GetAllOutboundByRangeDateCompleted;
        
        /// <remarks/>
        public event GetAllOutboundByNumDocCompletedEventHandler GetAllOutboundByNumDocCompleted;
        
        /// <remarks/>
        public event GetAllInboundCompletedEventHandler GetAllInboundCompleted;
        
        /// <remarks/>
        public event GetByIDCompletedEventHandler GetByIDCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/HelloWorld", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string HelloWorld() {
            object[] results = this.Invoke("HelloWorld", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void HelloWorldAsync() {
            this.HelloWorldAsync(null);
        }
        
        /// <remarks/>
        public void HelloWorldAsync(object userState) {
            if ((this.HelloWorldOperationCompleted == null)) {
                this.HelloWorldOperationCompleted = new System.Threading.SendOrPostCallback(this.OnHelloWorldOperationCompleted);
            }
            this.InvokeAsync("HelloWorld", new object[0], this.HelloWorldOperationCompleted, userState);
        }
        
        private void OnHelloWorldOperationCompleted(object arg) {
            if ((this.HelloWorldCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.HelloWorldCompleted(this, new HelloWorldCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetAll", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetAll() {
            object[] results = this.Invoke("GetAll", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetAllAsync() {
            this.GetAllAsync(null);
        }
        
        /// <remarks/>
        public void GetAllAsync(object userState) {
            if ((this.GetAllOperationCompleted == null)) {
                this.GetAllOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetAllOperationCompleted);
            }
            this.InvokeAsync("GetAll", new object[0], this.GetAllOperationCompleted, userState);
        }
        
        private void OnGetAllOperationCompleted(object arg) {
            if ((this.GetAllCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetAllCompleted(this, new GetAllCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetAllOutbound", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetAllOutbound() {
            object[] results = this.Invoke("GetAllOutbound", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetAllOutboundAsync() {
            this.GetAllOutboundAsync(null);
        }
        
        /// <remarks/>
        public void GetAllOutboundAsync(object userState) {
            if ((this.GetAllOutboundOperationCompleted == null)) {
                this.GetAllOutboundOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetAllOutboundOperationCompleted);
            }
            this.InvokeAsync("GetAllOutbound", new object[0], this.GetAllOutboundOperationCompleted, userState);
        }
        
        private void OnGetAllOutboundOperationCompleted(object arg) {
            if ((this.GetAllOutboundCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetAllOutboundCompleted(this, new GetAllOutboundCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetAllOutboundByNIF", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetAllOutboundByNIF(string nifRecetor, string nifEmissor) {
            object[] results = this.Invoke("GetAllOutboundByNIF", new object[] {
                        nifRecetor,
                        nifEmissor});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetAllOutboundByNIFAsync(string nifRecetor, string nifEmissor) {
            this.GetAllOutboundByNIFAsync(nifRecetor, nifEmissor, null);
        }
        
        /// <remarks/>
        public void GetAllOutboundByNIFAsync(string nifRecetor, string nifEmissor, object userState) {
            if ((this.GetAllOutboundByNIFOperationCompleted == null)) {
                this.GetAllOutboundByNIFOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetAllOutboundByNIFOperationCompleted);
            }
            this.InvokeAsync("GetAllOutboundByNIF", new object[] {
                        nifRecetor,
                        nifEmissor}, this.GetAllOutboundByNIFOperationCompleted, userState);
        }
        
        private void OnGetAllOutboundByNIFOperationCompleted(object arg) {
            if ((this.GetAllOutboundByNIFCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetAllOutboundByNIFCompleted(this, new GetAllOutboundByNIFCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetAllOutboundByRangeDate", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetAllOutboundByRangeDate(string dataI, string dataF) {
            object[] results = this.Invoke("GetAllOutboundByRangeDate", new object[] {
                        dataI,
                        dataF});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetAllOutboundByRangeDateAsync(string dataI, string dataF) {
            this.GetAllOutboundByRangeDateAsync(dataI, dataF, null);
        }
        
        /// <remarks/>
        public void GetAllOutboundByRangeDateAsync(string dataI, string dataF, object userState) {
            if ((this.GetAllOutboundByRangeDateOperationCompleted == null)) {
                this.GetAllOutboundByRangeDateOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetAllOutboundByRangeDateOperationCompleted);
            }
            this.InvokeAsync("GetAllOutboundByRangeDate", new object[] {
                        dataI,
                        dataF}, this.GetAllOutboundByRangeDateOperationCompleted, userState);
        }
        
        private void OnGetAllOutboundByRangeDateOperationCompleted(object arg) {
            if ((this.GetAllOutboundByRangeDateCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetAllOutboundByRangeDateCompleted(this, new GetAllOutboundByRangeDateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetAllOutboundByNumDoc", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetAllOutboundByNumDoc(string numDocument) {
            object[] results = this.Invoke("GetAllOutboundByNumDoc", new object[] {
                        numDocument});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetAllOutboundByNumDocAsync(string numDocument) {
            this.GetAllOutboundByNumDocAsync(numDocument, null);
        }
        
        /// <remarks/>
        public void GetAllOutboundByNumDocAsync(string numDocument, object userState) {
            if ((this.GetAllOutboundByNumDocOperationCompleted == null)) {
                this.GetAllOutboundByNumDocOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetAllOutboundByNumDocOperationCompleted);
            }
            this.InvokeAsync("GetAllOutboundByNumDoc", new object[] {
                        numDocument}, this.GetAllOutboundByNumDocOperationCompleted, userState);
        }
        
        private void OnGetAllOutboundByNumDocOperationCompleted(object arg) {
            if ((this.GetAllOutboundByNumDocCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetAllOutboundByNumDocCompleted(this, new GetAllOutboundByNumDocCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetAllInbound", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetAllInbound() {
            object[] results = this.Invoke("GetAllInbound", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetAllInboundAsync() {
            this.GetAllInboundAsync(null);
        }
        
        /// <remarks/>
        public void GetAllInboundAsync(object userState) {
            if ((this.GetAllInboundOperationCompleted == null)) {
                this.GetAllInboundOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetAllInboundOperationCompleted);
            }
            this.InvokeAsync("GetAllInbound", new object[0], this.GetAllInboundOperationCompleted, userState);
        }
        
        private void OnGetAllInboundOperationCompleted(object arg) {
            if ((this.GetAllInboundCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetAllInboundCompleted(this, new GetAllInboundCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetByID", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetByID(int ID, int OutOrIn) {
            object[] results = this.Invoke("GetByID", new object[] {
                        ID,
                        OutOrIn});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetByIDAsync(int ID, int OutOrIn) {
            this.GetByIDAsync(ID, OutOrIn, null);
        }
        
        /// <remarks/>
        public void GetByIDAsync(int ID, int OutOrIn, object userState) {
            if ((this.GetByIDOperationCompleted == null)) {
                this.GetByIDOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetByIDOperationCompleted);
            }
            this.InvokeAsync("GetByID", new object[] {
                        ID,
                        OutOrIn}, this.GetByIDOperationCompleted, userState);
        }
        
        private void OnGetByIDOperationCompleted(object arg) {
            if ((this.GetByIDCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetByIDCompleted(this, new GetByIDCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    public delegate void HelloWorldCompletedEventHandler(object sender, HelloWorldCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class HelloWorldCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal HelloWorldCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    public delegate void GetAllCompletedEventHandler(object sender, GetAllCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetAllCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetAllCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    public delegate void GetAllOutboundCompletedEventHandler(object sender, GetAllOutboundCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetAllOutboundCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetAllOutboundCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    public delegate void GetAllOutboundByNIFCompletedEventHandler(object sender, GetAllOutboundByNIFCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetAllOutboundByNIFCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetAllOutboundByNIFCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    public delegate void GetAllOutboundByRangeDateCompletedEventHandler(object sender, GetAllOutboundByRangeDateCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetAllOutboundByRangeDateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetAllOutboundByRangeDateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    public delegate void GetAllOutboundByNumDocCompletedEventHandler(object sender, GetAllOutboundByNumDocCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetAllOutboundByNumDocCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetAllOutboundByNumDocCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    public delegate void GetAllInboundCompletedEventHandler(object sender, GetAllInboundCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetAllInboundCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetAllInboundCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    public delegate void GetByIDCompletedEventHandler(object sender, GetByIDCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetByIDCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetByIDCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591