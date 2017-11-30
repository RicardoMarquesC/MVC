using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.HelperTools
{
	public class ResponseStatus
	{
		public const string OK = "OK";
		public const string ERROR = "ERROR";
	}

	public class JsonReturnObject
	{
		public string StatusCode { get; set; }
		public string Message { get; set; }
	}

	public class JsonReturnFoundSuppliers
	{
		public string StatusCode { get; set; }
		public string Message { get; set; }
		public JsonReturnFornecedor[] Fornecedores { get; set; }
	}

	public class JsonReturnFornecedor
	{
		public string NIF { get; set; }
		public string Nome { get; set; }
	}
}
