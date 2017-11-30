using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eBillingSuite.Model;
using eBillingSuite.Repositories;
using Ninject;
using eBillingSuite.Model.Desmaterializacao;

namespace eBillingSuite.Integrations
{
	public class SupplierSyncronization : ISupplierSyncronization
	{
        private IDigitalConfigurationsRepository _eDigitalConfigurationsRepository;
        private IEDigitalIntancesRepository _eDigitalIntancesRepository;

        [Inject]
		public SupplierSyncronization(IDigitalConfigurationsRepository eDigitalConfigurationsRepository, IEDigitalIntancesRepository eDigitalIntancesRepository)
		{
			_eDigitalConfigurationsRepository = eDigitalConfigurationsRepository;
            _eDigitalIntancesRepository = eDigitalIntancesRepository;
		}

		public Fornecedores GetDadosFornecedorFromWS(string nif)
		{
			Fornecedores fInfo = null;
			try
			{
                //obter configuracoes do WS
                //string url = (string)_eDigitalConfigurationsRepository.GetConfigurationByKey(DigitalConfigurationsRepository.ConfigurationsNames.SincFornecedoresWS.ToString());
                string user = (string)_eDigitalConfigurationsRepository.GetConfigurationByKey(DigitalConfigurationsRepository.ConfigurationsNames.SincFornecedoresWSUser.ToString());
                string pass = (string)_eDigitalConfigurationsRepository.GetConfigurationByKey(DigitalConfigurationsRepository.ConfigurationsNames.SincFornecedoresWSPass.ToString());

                //nif da instancia configurada na tabela "Configuracoes" do Digital
                bool isCMP = _eDigitalIntancesRepository.VatNumberExists("501306099");

                //se for a CMP
                if (isCMP)
				{
					//fInfo = GetCMPData(user, pass, nif);

					// NOVOS WS (DLL do Pedro Martins)
					fInfo = GetCMPData_NewWS(user, pass, nif);

					// SIMULAR FORNECEDOR PARA TESTES
					//fInfo = new Fornecedores
					//{
					//	Contribuinte = nif,
					//	Nome = "ah e tal",
					//	Morada = "morada tal",
					//	Telefone = "222222222",
					//	Fax = "111111111",
					//	Email = "",
					//	WebSite = "",
					//	WantMainValidations = true
					//};
				}
			}
			catch (Exception e)
			{
				string msg = e.Message.ToLower() == "notfound" ? "Não foi possível encontrar a entidade na Gestão Centralizada. Por favor, insira-a em primeiro lugar lá e depois volte a tentar." : e.Message; // TODO: traduções
				throw new Exception(msg);
			}
			return fInfo;
		}

		/// <summary>
		/// Processo usado para a CMP
		/// </summary>
		/// <param name="user">User de acesso ao WS</param>
		/// <param name="pass">Pass de acesso ao WS</param>
		/// <param name="StrNIF">NIF a pesquisar (Fornecedor, etc)</param>
		/// <returns></returns>
		private Fornecedores GetCMPData(string user, string pass, string StrNIF)
		{
			string nome = "", morada = "", telefone = "", fax = "", email = "", site = "";

			CMPEntidades gce = new CMPEntidades();

			// definir credencias
			ObjCredAutenticacaoUser cred = new ObjCredAutenticacaoUser();
			cred.aplicacao = user;
			cred.password = pass;

			// definir parametros para pesquisa
			ObjWsInputEntidadePesqUser[] pesq = new ObjWsInputEntidadePesqUser[1];
			pesq[0] = new ObjWsInputEntidadePesqUser { docIdentificacaoNif = StrNIF };

			// invocar metodo para pesquisar entidade
			ObjWsReturnEntPesquisaUser retorno = gce.getEntidadeInfo(cred, pesq);

			// verificar se encontrou entidades
			if (retorno.resultado.resultado.ToLower() != "ok")
				throw new Exception("Erro ao obter dados da entidade."); // TODO: traduções
			else if ((!retorno.resultado.registosRetornados.HasValue && retorno.resultado.registosRetornados < 1) || retorno.detalhes.Length < 1)
				throw new Exception("NOTFOUND");

			// obtém a 1ª entidade encontrada com este NIF
			ObjReturnEntidadePesqUser entidade = retorno.detalhes.ElementAt(0);
			nome = entidade.entidadeNomeDesignacao;
			morada = String.Format("{0} {1}", entidade.moradaToponimio, entidade.moradaNumPolicia);

			// Obter contactos
			ObjWsInputContactoInfoUser[] contactoPesq = new ObjWsInputContactoInfoUser[1];
			contactoPesq[0] = new ObjWsInputContactoInfoUser { codigoEntidade = entidade.codigoEntidade };

			// invocar metodo para obter contactos
			ObjWsReturnContactoInfoUser retornoContacto = gce.getContactoInfo(cred, contactoPesq);

			// verificar se encontrou contactos
			if (retornoContacto.resultado.resultado.ToLower() != "ok")
				throw new Exception("Erro ao obter contactos da entidade '" + StrNIF + "'."); // TODO: traduções

			//obtém contactos que DataVigorFim seja NULL e código igual a "TLF" ou "FAX" ou "MAIL"
			if ((retornoContacto.resultado.registosRetornados.HasValue && retornoContacto.resultado.registosRetornados > 0) || retornoContacto.detalhes.Length > 0)
			{
				ObjReturnContactoInfoUser[] contactos = retornoContacto.detalhes;
				foreach (ObjReturnContactoInfoUser contacto in contactos)
				{
					if (!contacto.dataVigorFim.HasValue)
					{
						string codigoContacto = contacto.codigoContacto.ToLower();
						string valorContacto = contacto.valorContacto;

						switch (codigoContacto)
						{
							case "tlf":
								telefone = valorContacto;
								break;
							case "fax":
								fax = valorContacto;
								break;
							case "mail":
								email = valorContacto;
								break;
							default:
								email = valorContacto;
								break;
						}
					}
				}
			}

			return new Fornecedores
			{
				Nome = nome,
				Morada = morada,
				Telefone = telefone,
				Fax = fax,
				Email = email,
				WebSite = site
			};
		}

		/// <summary>
		/// Processo usado para a CMP para os NOVOS WS
		/// </summary>
		/// <param name="user">User de acesso ao WS</param>
		/// <param name="pass">Pass de acesso ao WS</param>
		/// <param name="StrNIF">NIF a pesquisar (Fornecedor, etc)</param>
		/// <returns></returns>
		private Fornecedores GetCMPData_NewWS(string user, string pass, string StrNIF)
		{
			CMPWs.Interop.Entidades entidadesService = new CMPWs.Interop.Entidades(user, pass);

			// invocar metodo para pesquisar entidade
			CMPWs.Interop.CMPWsEntidade entidade = entidadesService.PesquisaEntidadePorNIF(StrNIF);

			// verificar se encontrou entidades
			if (entidade == null)
				throw new Exception("Erro ao obter dados da entidade.");

			string morada = String.Format("{0} {1}", (String.IsNullOrWhiteSpace(entidade.Morada) ? "" : entidade.Morada), (String.IsNullOrWhiteSpace(entidade.NumPolicia) ? "" : entidade.NumPolicia));

			return new Fornecedores
			{
				Nome = String.IsNullOrWhiteSpace(entidade.Nome) ? "" : entidade.Nome,
				Morada = morada,
				Telefone = String.IsNullOrWhiteSpace(entidade.Telefone) ? "" : entidade.Telefone,
				Fax = String.IsNullOrWhiteSpace(entidade.Fax) ? "" : entidade.Fax,
				Email = String.IsNullOrWhiteSpace(entidade.Email) ? "" : entidade.Email,
				WebSite = String.IsNullOrWhiteSpace(entidade.Website) ? "" : entidade.Website,
			};
		}
	}
}
