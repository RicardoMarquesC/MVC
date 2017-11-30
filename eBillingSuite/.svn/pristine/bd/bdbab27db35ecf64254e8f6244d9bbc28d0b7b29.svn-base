using eBillingSuite.Repositories;
using eBillingSuite.Resources;
using FluentValidation;
using FluentValidation.Results;
using Ninject;
using System.Linq;

namespace eBillingSuite.Models.Validators
{
	public class ConfigTXTDataValidator : AbstractValidator<ConfigTXTData>
	{
		private IeBillingSuiteRequestContext _context;
		private IConnectorConfigTXTRepository _connectorConfigTXTRepository;
		//Password
		public const string password = @"^([a-zA-Z0-9\s:-_.]){8,16}$";
		//Username
		public const string username = @"^[0-9]{9,9}/[0-9]{4,4}$";

		[Inject]
		public ConfigTXTDataValidator(IeBillingSuiteRequestContext context,
			IConnectorConfigTXTRepository connectorConfigTXTRepository)
		{
			_context = context;
			_connectorConfigTXTRepository = connectorConfigTXTRepository;

			RuleFor(c => c.Posicao)
				.NotEmpty()
				.WithMessage(Texts.CampoPosicaoDeveEstarPreenchido);


			Custom(ValidateAction);

		}


		private ValidationFailure ValidateAction(ConfigTXTData data)
		{
			int result;
			bool value = int.TryParse(data.Posicao, out result);

			if (value)
			{
				if (result < 10)
					data.Posicao = "0" + result.ToString();
			}
			else
				return new ValidationFailure("Posicao", Texts.PosicaoFailedRequirement);

			var dadosBD = _connectorConfigTXTRepository.Find(data.pkid);
			if (data.Posicao != dadosBD.Posicao)
			{
				var dataFromDB = _connectorConfigTXTRepository
				.Exists(ct => ct.Posicao == data.Posicao
					&& ct.Tipo == data.Tipo);

				if (dataFromDB)
				{
					return new ValidationFailure("Posicao", Texts.PosicaoFailedRequirement);
				}
			}

			if(data.NomeCampo != dadosBD.NomeCampo)
			{
				var dataFromDB = _connectorConfigTXTRepository
				.Exists(ct => ct.NomeCampo == data.NomeCampo
					&&
					ct.Tipo == data.Tipo);

				if (dataFromDB)
				{
					return new ValidationFailure("NomeCampo", Texts.NomeCampoFailedRequirement);
				}
			}
			
			return null;
		}
	}
}