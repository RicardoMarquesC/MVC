using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.HelperTools.Interfaces
{
	public interface IXmlHelper
	{
		void ParseXmlTagsAndSave(string path, Guid fkSender);
	}
}
