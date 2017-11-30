using eBillingSuite.Enumerations;
using eBillingSuite.HelperTools.Interfaces;
using eBillingSuite.Repositories;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace eBillingSuite.HelperTools
{
	public class XmlHelper : IXmlHelper
	{
		private IECCListRepositories _eCCListRepositories;
		private IEConnectorSendersRepository _eConnectorSendersRepository;

		[Inject]
		public XmlHelper(IECCListRepositories eCCListRepositories, IEConnectorSendersRepository eConnectorSendersRepository)
		{
			_eCCListRepositories = eCCListRepositories;
			_eConnectorSendersRepository = eConnectorSendersRepository;
		}

		public void ParseXmlTagsAndSave(string path, Guid fkSender)
		{
			try
			{
				System.Xml.XmlDocument xmlfiledados = new System.Xml.XmlDocument();
				xmlfiledados.Load(path);
				string caminhoxml = ((XmlNode)xmlfiledados.DocumentElement).Name;
				string raiz = caminhoxml;

				List<string> xmlPaths = new List<string>();

				xmlPaths = RecurseXmlDocument((XmlNode)xmlfiledados.DocumentElement, 0, raiz, raiz, xmlPaths);

				SaveXmlStructure(xmlPaths, fkSender);
			}
			catch (Exception e)
			{
				throw;
			}
		}

		public List<string> RecurseXmlDocument(XmlNode root, int flag, string temp, string tipoxml, List<string> caminhos)
		{
			if (flag == 1)
				temp = temp + "/" + root.Name;
			else if (flag == 2)
			{
				if (tipoxml.ToLower().Equals(root.ParentNode.Name.ToLower()))
					temp = root.ParentNode.Name + "/" + root.Name;
				else
					temp = tipoxml + "/" + root.ParentNode.Name + "/" + root.Name;
			}

			if (root is XmlElement)
			{
				if (root.HasChildNodes && !(root.FirstChild is XmlText))
				{
					RecurseXmlDocument(root.FirstChild, 1, temp, tipoxml, caminhos);
				}
				if (root.NextSibling != null)
				{
					XmlAttributeCollection xac = root.Attributes;
					if (tipoxml.Equals(XmlTypes.BASIC, StringComparison.OrdinalIgnoreCase))
					{
						foreach (XmlAttribute xmla in xac)
						{
							temp = temp + "/" + xmla.Value;
						}
					}

					caminhos.Add(temp);
					RecurseXmlDocument(root.NextSibling, 2, temp, tipoxml, caminhos);
				}
				else
				{
					caminhos.Add(temp);
				}
			}

			return caminhos;
		}

		public void SaveXmlStructure(List<string> filledXmlPaths, Guid fkSender)
		{
			using (var dbContextTransaction = _eCCListRepositories.eConnectorXmlTemplateRepository.Context.Database.BeginTransaction())
			{
				try
				{
					char[] delimitador = { '/' };

					// get number of existing "Custom" xml templates
					int count = _eCCListRepositories.eConnectorXmlTemplateRepository.GetCustomXmlCount();

					// build xml type name
					string senderName = _eConnectorSendersRepository.GetSenderNameById(fkSender);
					string xmlTypeName = String.Format("{0}{1}_{2}", "Custom", (count + 1).ToString(), senderName); 

					// array to used inserted paths
					List<string> inserted = new List<string>();

					// insert into xmlTemplate
					foreach (string xmlPath in filledXmlPaths)
					{
						if (!inserted.Contains(xmlPath))
						{
							string[] tempvalues = xmlPath.Split(delimitador);
							string fieldName = tempvalues[tempvalues.Length - 1];

							_eCCListRepositories.eConnectorXmlTemplateRepository.InsertXmlField(xmlPath, fieldName, DigitalDocumentAreas.GENERIC, xmlTypeName, false, count);

							inserted.Add(xmlPath);
						}
					}

					dbContextTransaction.Commit();
				}
				catch (Exception ex)
				{
					dbContextTransaction.Rollback();
					throw;
				}
			}
		}
	}
}
