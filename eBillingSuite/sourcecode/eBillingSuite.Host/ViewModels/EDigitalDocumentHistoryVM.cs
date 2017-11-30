using eBillingSuite.Model;
using eBillingSuite.Model.Desmaterializacao;
using eBillingSuite.Models;
using eBillingSuite.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBillingSuite.ViewModels
{
	public class EDigitalDocumentHistoryVM
	{
		public List<DocumentDetailsVM> documentDetails { get; set; }

		public EDigitalDocumentHistoryVM(List<ProcDocs> documentos,
            IEDigitalSuppliersRepository eDigitalSuppliersRepository, IEDigitalDocHistoryRepository eDigitalDocHistoryRepository)
		{
			documentDetails = new List<DocumentDetailsVM>();

			foreach (var doc in documentos)
			{
                var supplierNameObj = eDigitalSuppliersRepository.Where(x => x.Contribuinte == doc.fornecedor).FirstOrDefault();

                string supplierName = supplierNameObj != null ? supplierNameObj.Nome : "";

                var documentNumber = doc.DocNumber;

                string filename = eDigitalDocHistoryRepository.GetFullFileName(doc);

                documentDetails.Add(new DocumentDetailsVM
                {
					pkid = doc.pkid,
					DocNumber = documentNumber,
					Fornecedor = supplierName,
					ValidationDate = doc.DtaModificacao != null ? doc.DtaModificacao : DateTime.MinValue,
					ValidationUser = doc.Utilizador,
                    XmlFilename = filename == null ? "" : filename + ".xml",
                    PdfFilename = filename == null ? "" : filename + ".pdf"
                });
			}
		}
	}

	public class DocumentDetailsVM
	{
		public Guid pkid { get; set; }
		public string DocNumber { get; set; }
		public string Fornecedor { get; set; }
		public DateTime ValidationDate { get; set; }
		public string ValidationUser { get; set; }
		public string XmlFilename { get; set; }
		public string PdfFilename { get; set; }
	}
}