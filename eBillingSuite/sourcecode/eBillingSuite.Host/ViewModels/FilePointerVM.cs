using eBillingSuite.Globalization.Generators;
using eBillingSuite.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBillingSuite.ViewModels
{
	public class FilePointerVM
	{
		public FilePointerVM()
		{
			attachments = new List<FilePointer>();
			FilePointer a = new FilePointer
			{
				ID = 1,
				Extension = "PDF",
				LockFilePointers = false,
				Name = "Documento 1.pdf",

			};

			FilePointer b = new FilePointer
			{
				ID = 1,
				Extension = "PDF",
				LockFilePointers = true,
				Name = "Documento aprovação.pdf",

			};

			
			attachments.Add(a); attachments.Add(b);
		}

		public class FilePointer
		{
			public int ID { get; set; }
			public string Extension { get; set; }
			public bool LockFilePointers { get; set; }
			public string Name { get; set; }
		}

		public List<FilePointer> attachments;
	}
}