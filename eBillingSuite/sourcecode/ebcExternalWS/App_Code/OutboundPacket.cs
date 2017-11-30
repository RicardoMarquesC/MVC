using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for OutboundPacket
/// </summary>
public class OutboundPacket
{
    public OutboundPacket() {}

    public string DMSID { get; set; }
    public string NomeReceptor { get; set; }
    public string EmailReceptor { get; set; }
    public string NIFReceptor { get; set; }
    public string NumFactura { get; set; }
    public string DataFactura { get; set; }
    public string QuantiaComIVA { get; set; }
    public DateTime CreationDate { get; set; }
    public string CurrentEBCState { get; set; }
    public DateTime LastUpdate { get; set; }
    public string DocOriginal { get; set; }
    public string QuantiaSemIVA { get; set; }
    public string Moeda { get; set; }
    public string QuantiaComIVAMoedaInterna { get; set; }
    public string QuantiaSemIVAMoedaInterna { get; set; }
    public string TaxaCambio { get; set; }
    public string CondicaoPagamento { get; set; }
    public string SWIFT { get; set; }
    public string IBAN { get; set; }
    public string TipoDocumento { get; set; }
    public string NomeEmissor { get; set; }
    public string NIFEmissor { get; set; }
    public string Referencia { get; set; }
    public string Contentor { get; set; }
    public string CGA { get; set; }
    public string NumEncomenda { get; set; }
    public int Identificador { get; set; }
    public bool InorOut { get; set; }

}