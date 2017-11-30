using eBillingSuite.Model;
using Ninject;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Microsoft.Win32;
using eBillingSuite.Model.Desmaterializacao;

namespace eBillingSuite.Repositories
{
	public class EDigitalDocHistoryRepository : GenericRepository<IeBillingSuiteDesmaterializacaoContext, ProcDocs>, IEDigitalDocHistoryRepository
	{
		[Inject]
		public EDigitalDocHistoryRepository(IeBillingSuiteDesmaterializacaoContext context)
			: base(context)
		{
		}

        public string GetFullFileName(ProcDocs doc)
        {
            try
            {
                // nome é sempre <nif_fornecedor><ano><cod_tipo_doc><num_documento>
                string nifFornecedor = doc.fornecedor;
                int ano = doc.dtaCriacao.Year;

                // este pedaço de codigo tem de ser igual ao que está no Digital
                string codTipoDoc = "";
                string tipoDoc = doc.tpoFatura.ToLower();
                if (tipoDoc.Equals("factura") || tipoDoc.Equals("fatura"))
                    codTipoDoc = "FT";
                else if (tipoDoc.Equals("nota de débito") || tipoDoc.Equals("nota de debito")
                    || tipoDoc.Equals("nota débito") || tipoDoc.Equals("nota debito"))
                    codTipoDoc = "ND";
                else if (tipoDoc.Equals("nota de crédito") || tipoDoc.Equals("nota de credito")
                    || tipoDoc.Equals("nota crédito") || tipoDoc.Equals("nota credito"))
                    codTipoDoc = "NC";
                else if (tipoDoc.Equals("recibo"))
                    codTipoDoc = "RC";
                else
                    codTipoDoc = "NULL";

                string numeroDoc = eBillingSuite.Support.StringUtilities.RemoveSpecialCharsForFilename(doc.DocNumber, "-");

                return (String.Format("{0}{1}{2}{3}", nifFornecedor, ano.ToString(), codTipoDoc, numeroDoc));
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
