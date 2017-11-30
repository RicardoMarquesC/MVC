using eBillingSuite.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace eBillingSuite.Security
{
	public enum Permissions
	{
        ParseFailed,
        ECONNECTOR_CLIENTES_CLIENTES,
        ECONNECTOR_CLIENTES_CONFIGURACOESXML,
        ECONNECTOR_CONFIG_CERTIFICADODIGITAL,
        ECONNECTOR_CONFIG_CONFIGURACAOINSTANCIA,
        ECONNECTOR_CONFIG_CONFIGURACAOTXT,
        ECONNECTOR_CONFIG_CONTAEMAIL,
        ECONNECTOR_CONFIG_DEFINICOESALERTA,
        ECONNECTOR_CONFIG_DEFINICOESEMAIL,
        ECONNECTOR_CONFIG_DEFINICOESENVELOPE,
        ECONNECTOR_CONFIG_MAPEARTXTINBOUND,
        EDIGITAL_ACCOUNTING_DATA,
        EDIGITAL_EXPIRACAODOCUMENTOS,
        EDIGITAL_FORNECEDORES,
        EDIGITAL_GESTAOXML,
        EDIGITAL_HISTORICODOCUMENTOS,
        EDIGITAL_INSTANCES,
        EDIGITAL_INTEGRACAO,
        EDIGITAL_MAIL,
        EDIGITAL_PROCESSAMENTO,
        EDIGITAL_SINCRONIZACAO,
        EDIGITAL_STATS,
        EDIGITAL_TEMPLATES,
        EECONNECTOR_REMETENTES_CONFIGURACOESXML,
        EECONNECTOR_REMETENTES_MAPEAMENTO,
        EECONNECTOR_REMETENTES_REMETENTES,
        EECONNECTOR_REMETENTES_XMLMAPEAMENTO,
        EEDI_CLIENTES,
        EEDI_DOCUMENTOSENVIADOS,
        EEDI_DOCUMENTOSRECEBIDOS,
        EEDI_REMETENTES,
        EMCAT_CONFIGURACOESENVIO,
        EMCAT_DEFINICOESCREDENCIAIS,
        EMCAT_FATURAS,
        EMCAT_GUIASTRANSPORTES,
        EUSERS,
    }

    public static class EnumExtensions
    {
        public static string GetLocalizedText(this Enum e, string suffix = null)
        {
            suffix = string.IsNullOrWhiteSpace(suffix) ? string.Empty : $"_{suffix.Trim()}";

            var rm = new ResourceManager(typeof(Texts));
            var resourceKey = $"{e.GetType().Name}_{e}{suffix}";
            var resourceDisplayName = rm.GetString(resourceKey);

            return string.IsNullOrWhiteSpace(resourceDisplayName) ? $"[[{resourceKey}]]" : resourceDisplayName;
        }
    }




    public static class PermissionsExtension
	{
		public static Permissions ParseAsPermission(this string value)
		{
			Permissions permision;
			return Enum.TryParse<Permissions>(value, out permision) ? permision : Permissions.ParseFailed;
		}
	}
}
