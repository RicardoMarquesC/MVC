using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shortcut.Repositories;
using eBillingSuite.Model;
using eBillingSuite.Security;
using System.Security.Cryptography.X509Certificates;
using Ninject;
using eBillingSuite.Model.EBC_DB;

namespace eBillingSuite.Repositories
{
	public class EConnectorXmlClientRepository : GenericRepository<IeBillingSuiteEBCDBContext, EBC_XMLClient>, IEConnectorXmlClientRepository
	{
		private IeBillingSuiteEBCDBContext _context;
		
		[Inject]
		public EConnectorXmlClientRepository(IeBillingSuiteEBCDBContext context)
			: base(context)
		{
			_context = context;
		}
		
		public int GetXMLClientNumber()
		{
			var number = 0;
			var exists = Set.Count();
			if(exists > 0)
				number = Set.Max(exc => exc.NumeroXML);
			
			number = number + 1;
			
			return number;
		}


		public string GetXmlTypeByFKClient(Guid fkclient)
		{			
			var xmlnumber = this.Where(exc => exc.FkCliente == fkclient).FirstOrDefault().NumeroXML;

			var exists = _context.EBC_XML.Where(ex => ex.NumeroXML == xmlnumber).Count();
			if(exists==0)
			{
				exists = _context.EBC_XMLLines.Where(ex => ex.NumeroXML == xmlnumber).Count();
				if (exists == 0)
				{
					exists = _context.EBC_XMLResumoIVA.Where(ex => ex.NumeroXML == xmlnumber).Count();
					if(exists>0)
					{
						var xmltype = _context.EBC_XMLResumoIVA
						 .Where(ex => ex.NumeroXML == xmlnumber)
						 .GroupBy(ex => ex.TipoXML)
						 .Select(g => new { TipoXml = g.FirstOrDefault().TipoXML }).FirstOrDefault().TipoXml;
						return xmltype;
					}
					else
						return eBillingSuite.Enumerations.XmlTypes.UBL; 
				}				
				else
				{
					var xmltype = _context.EBC_XMLLines
				   .Where(ex => ex.NumeroXML == xmlnumber)
				   .GroupBy(ex => ex.TipoXML)
				   .Select(g => new { TipoXml = g.FirstOrDefault().TipoXML }).FirstOrDefault().TipoXml;
					return xmltype;
				}
			}
			else
			{
				var xmltype = _context.EBC_XMLLines
					   .Where(ex => ex.NumeroXML == xmlnumber)
					   .GroupBy(ex => ex.TipoXML)
					   .Select(g => new { TipoXml = g.FirstOrDefault().TipoXML }).FirstOrDefault().TipoXml;
				return xmltype;				 
			}
		}


		public int GetXMLClientNumberByFKClient(Guid fkclient)
		{
			return this
				.Where(exc => exc.FkCliente == fkclient)
				.FirstOrDefault()
				.NumeroXML;
		}


		public int GetXMLClientNumberByFKClient(Guid fkclient, string tipoxml)
		{
			var numberxml = this.Where(exc => exc.FkCliente == fkclient)
				.FirstOrDefault()
				.NumeroXML;

			var exists = _context.EBC_XML.Where(ex => ex.NumeroXML == numberxml).Count();
			if(exists > 0)
			{ 
				var typexml = _context.EBC_XML.Where(ex => ex.NumeroXML == numberxml).FirstOrDefault().TipoXML;
				if (typexml != tipoxml)
					return -1;
			}
			else
			{
				exists = _context.EBC_XMLLines.Where(ex => ex.NumeroXML == numberxml).Count();
				if (exists > 0)
				{
					var typexml = _context.EBC_XMLLines.Where(ex => ex.NumeroXML == numberxml).FirstOrDefault().TipoXML;
					if (typexml != tipoxml)
						return -1;
				}
				else
				{
					exists = _context.EBC_XMLResumoIVA.Where(ex => ex.NumeroXML == numberxml).Count();
					if (exists > 0)
					{
						var typexml = _context.EBC_XMLResumoIVA.Where(ex => ex.NumeroXML == numberxml).FirstOrDefault().TipoXML;
						if (typexml != tipoxml)
							return -1;
					}
				}
			}

			return numberxml;
		}
	}
}
