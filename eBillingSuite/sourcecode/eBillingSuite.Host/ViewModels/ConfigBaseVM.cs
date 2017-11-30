using eBillingSuite.Globalization.Generators;
using eBillingSuite.Model;
using eBillingSuite.Model.EBC_DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBillingSuite.ViewModels
{
	public class ConfigBaseVM
	{
		public ConfigBaseVM(List<EBC_Config> model)
		{
			Configs = new List<ConfigInformation>();			
			foreach (EBC_Config config in model)
			{
				ConfigInformation ci = new ConfigInformation();
				ci.PKID = config.PKID;
				ci.Text = GetConfigName(config.KeyName);
				if (config.KeyValue.ToLower().Equals("true"))
				{
					ci.Value = true;
					ci.isSwitcher = true;
				}
				else if (config.KeyValue.ToLower().Equals("false"))
				{
					ci.Value = false;
					ci.isSwitcher = true;
				}
				else
				{
					ci.Value = config.KeyValue;
					ci.isSwitcher = false;
				}
				Configs.Add(ci);
			}
		}

		[DoNotGenerateDictionaryEntry]
		protected List<ConfigInformation> Configs { get; set; }

		[DoNotGenerateDictionaryEntry]
		protected class ConfigInformation
		{
			public string Text { get; set; }
			public object Value { get; set; }
			public Guid PKID { get; set; }
			public bool isSwitcher { get; set; }
		}

		protected string GetConfigName(string KeyName)
		{
			if (KeyName.Equals("Intragrupo"))
				return "Instâncias Intra-Grupo?";
			else if (KeyName.Equals("username"))
				return "Utilizador(POP3/IMAP4)";
			else if (KeyName.Equals("imap4ServerPort"))
				return "Porta";
			else if (KeyName.Equals("httpserver"))
				return "Servidor de HTTP MAIL";
			else if (KeyName.Equals("smtpPassword"))
				return "Password Servidor SMTP";
			else if (KeyName.Equals("imap4Server"))
				return "Servidor de IMAP4";
			else if (KeyName.Equals("smtpauthenticate"))
				return "Autenticação de Servidor de SMTP";
			else if (KeyName.Equals("smtpServer"))
				return "Servidor de SMTP";
			else if (KeyName.Equals("smtpUserName"))
				return "Utilizador Servidor SMTP";
			else if (KeyName.Equals("emailAddress"))
				return "Endereço de Correio Electrónico";
			else if (KeyName.Equals("EnvioAT"))
				return "Comunicação com AT por Faturação Eletrónica";
			else if (KeyName.Equals("FicheiroTXT"))
				return "Usa Ficheiro TXT";
			else if (KeyName.Equals("pop3Server"))
				return "Servidor de POP3";
			else if (KeyName.Equals("REP_LogFiles"))
				return "Monitorização dos Log Files";
			else if (KeyName.Equals("password"))
				return "Palavra Chave(POP3/IMAP4)";
			else if (KeyName.Equals("emailName"))
				return "Nome";
			else if (KeyName.Equals("IMAP4Folder"))
				return "Pasta IMAP4";
			else if (KeyName.Equals("notificationEmailMonitoring"))
				return "Endereço de notificações de monitorização";
			else if (KeyName.Equals("ComATFichGuias"))
				return "Ficheiro de Estado Comunicações AT - Transportes";
			else if (KeyName.Equals("httpserverport"))
				return "Porta";
			else if (KeyName.Equals("pop3Port"))
				return "Porta";
			else if (KeyName.Equals("ComATFich"))
				return "Ficheiro de Estado Comunicações AT - Facturas";
			else if (KeyName.Equals("smtpPort"))
				return "Porta";
			else if (KeyName.Equals("smtpSSL"))
				return "SMTP Seguro";
			else if (KeyName.Equals("ReceptionProtocol"))
				return "Protocolo de Recepção";
			else if (KeyName.Equals("serialNumber"))
				return "Certificado Digital";
			else if (KeyName.Equals("Interpreter"))
				return "Interpretador Postscript";
			else if (KeyName.Equals("InterpreterArguments"))
				return "Argumentos do Interpretador Postscript";
			else if (KeyName.Equals("certEmailNotification"))
				return "Alertas sobre o certificado";
			else if (KeyName.Equals("pop3imap4SSL"))
				return "POP3/IMAP4 Seguro";
			else
				return null;
			////////MUDAR PARA QUANDO FOR PARA PRODUCAO
			//return _context.GetDictionaryValue(KeyName);
		}
	}
}