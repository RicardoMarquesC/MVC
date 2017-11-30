using eBillingSuite.Repositories;
using eBillingSuite.Resources;
using FluentValidation;
using FluentValidation.Results;
using Ninject;
using System.Linq;

namespace eBillingSuite.Models.Validators
{
	public class ConfigInboundTXTDataValidator : AbstractValidator<ConfigInboundTXTData>
	{
		private IeBillingSuiteRequestContext _context;
		private IConnectorConfigInboundTXTRepository _connectorConfigInboundTXTRepository;
		//Password
		public const string password = @"^([a-zA-Z0-9\s:-_.]){8,16}$";
		//Username
		public const string username = @"^[0-9]{9,9}/[0-9]{4,4}$";

		[Inject]
		public ConfigInboundTXTDataValidator(IeBillingSuiteRequestContext context,
			IConnectorConfigInboundTXTRepository connectorConfigInboundTXTRepository)
		{
			_context = context;
			_connectorConfigInboundTXTRepository = connectorConfigInboundTXTRepository;

			RuleFor(c => c.posicaoTxt)
				.NotEmpty()
				.WithMessage(Texts.CampoPosicaoDeveEstarPreenchido);


			Custom(ValidateAction);

		}


		private ValidationFailure ValidateAction(ConfigInboundTXTData data)
		{
			var dadosBD = _connectorConfigInboundTXTRepository.Find(data.pkid);
			if (data.posicaoTxt != dadosBD.posicaoTxt)
			{
				var dataFromDB = _connectorConfigInboundTXTRepository
				.Exists(ct => ct.posicaoTxt == data.posicaoTxt
					&& ct.tipo == data.tipo);

				if (dataFromDB)
				{
					return new ValidationFailure("Posicao", Texts.PosicaoFailedRequirement);
				}
			}

			if (data.InboundPacketPropertyName != dadosBD.InboundPacketPropertyName)
			{
				var dataFromDB = _connectorConfigInboundTXTRepository
				.Exists(ct => ct.InboundPacketPropertyName == data.InboundPacketPropertyName
					&&
					ct.tipo == data.tipo);

				if (dataFromDB)
				{
					return new ValidationFailure("NomeCampo", Texts.NomeCampoFailedRequirement);
				}
			}
			
			return null;
		}
	}
}