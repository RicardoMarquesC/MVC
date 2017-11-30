﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código foi gerado por uma ferramenta.
//     Versão de Tempo de Execução:4.0.30319.34209
//
//     As alterações a este ficheiro poderão provocar um comportamento incorrecto e perder-se-ão se
//     o código for regenerado.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

// 
// This source code was auto-generated by wsdl, Version=4.0.30319.33440.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.33440")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Web.Services.WebServiceBindingAttribute(Name="LicensingSoap", Namespace="http://tempuri.org/")]
public partial class Licensing : System.Web.Services.Protocols.SoapHttpClientProtocol {
    
    private System.Threading.SendOrPostCallback SerialCheckerOperationCompleted;
    
    /// <remarks/>
    public Licensing() {
        this.Url = "http://suitelicensing.pi-co.com/licensing.asmx";
    }
    
    /// <remarks/>
    public event SerialCheckerCompletedEventHandler SerialCheckerCompleted;
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/SerialChecker", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public string SerialChecker(string serial, string empresa, string produto, string macaddress) {
        object[] results = this.Invoke("SerialChecker", new object[] {
                    serial,
                    empresa,
                    produto,
                    macaddress});
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginSerialChecker(string serial, string empresa, string produto, string macaddress, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("SerialChecker", new object[] {
                    serial,
                    empresa,
                    produto,
                    macaddress}, callback, asyncState);
    }
    
    /// <remarks/>
    public string EndSerialChecker(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public void SerialCheckerAsync(string serial, string empresa, string produto, string macaddress) {
        this.SerialCheckerAsync(serial, empresa, produto, macaddress, null);
    }
    
    /// <remarks/>
    public void SerialCheckerAsync(string serial, string empresa, string produto, string macaddress, object userState) {
        if ((this.SerialCheckerOperationCompleted == null)) {
            this.SerialCheckerOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSerialCheckerOperationCompleted);
        }
        this.InvokeAsync("SerialChecker", new object[] {
                    serial,
                    empresa,
                    produto,
                    macaddress}, this.SerialCheckerOperationCompleted, userState);
    }
    
    private void OnSerialCheckerOperationCompleted(object arg) {
        if ((this.SerialCheckerCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.SerialCheckerCompleted(this, new SerialCheckerCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    public new void CancelAsync(object userState) {
        base.CancelAsync(userState);
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.33440")]
public delegate void SerialCheckerCompletedEventHandler(object sender, SerialCheckerCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.33440")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class SerialCheckerCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal SerialCheckerCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
