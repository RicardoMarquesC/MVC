using eBillingSuite.Globalization.Generators;
using eBillingSuite.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBillingSuite.ViewModels
{
	public class DiaryVM
	{
		public DiaryVM()
		{
			entries = new List<DiaryEntries>();
			DiaryEntries a = new DiaryEntries
			{
				ID = 1,
				Date = new DateTime(2014,9,30,16,29,12),
				User = "Felisberto Moreira",
				Tipo = "Entrada em cokpit",
				Comentarios = "O utilizador 'Felizberto Moreira' acedeu ao documento 'Documento 1'",

			};

			DiaryEntries b = new DiaryEntries
			{
				ID = 1,
				Date = new DateTime(2014, 10, 3, 17, 29, 12),
				User = "Felisberto Moreira",
				Tipo = "Pré-Validação",
				Comentarios = "Documento pré-validado",

			};

			DiaryEntries c = new DiaryEntries
			{
				ID = 1,
				Date = new DateTime(2014, 10, 3, 17, 29, 13),
				User = "Felisberto Moreira",
				Tipo = "Pendente de aprovação",
				Comentarios = "Mudança de interlocutor. Próximo aprovador: Joaquim Alegre",

			};

			DiaryEntries d = new DiaryEntries
			{
				ID = 1,
				Date = new DateTime(2014, 10, 3, 17, 29, 15),
				User = "Joaquim Alegre",
				Tipo = "Novo pedido de aprovação",
				Comentarios = "O documento 'Documento 1' está pendente de aprovação final por parte de 'Joaquim Alegre'",

			};
			entries.Add(a); entries.Add(b); entries.Add(c); entries.Add(d);
		}

		public class DiaryEntries
		{
			public int ID { get; set; }
			public DateTime Date { get; set; }
			public string User { get; set; }
			public string Tipo { get; set; }
			public string Comentarios { get; set; }
		}

		public List<DiaryEntries> entries;
	}
}