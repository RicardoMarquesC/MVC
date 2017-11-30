﻿using eBillingSuite.Enumerations;
using eBillingSuite.Repositories;
using eBillingSuite.Resources;
using FluentValidation;
using FluentValidation.Results;
using Ninject;
using System;

namespace eBillingSuite.Models.Validators
{
	public class DigitalXmlFieldDataValidator : AbstractValidator<DigitalXmlFieldData>
	{
		private IeBillingSuiteRequestContext _context;
		private IEDigitalDocTypeXmlDataRepository _eDigitalDocTypeXmlDataRepository;
		private IEDigitalSupplierXmlDataRepository _eDigitalSupplierXmlDataRepository;

		[Inject]
		public DigitalXmlFieldDataValidator(IeBillingSuiteRequestContext context,
			IEDigitalDocTypeXmlDataRepository eDigitalDocTypeXmlDataRepository, IEDigitalSupplierXmlDataRepository eDigitalSupplierXmlDataRepository)
		{
			_context = context;
			_eDigitalDocTypeXmlDataRepository = eDigitalDocTypeXmlDataRepository;
			_eDigitalSupplierXmlDataRepository = eDigitalSupplierXmlDataRepository;

			RuleFor(c => c.NomeCampo)
				.NotEmpty()
				.When(c => c.Action == StandardActions.CREATE)
				.WithMessage(Texts.CampoPorPreencher);

			RuleFor(c => c.DecimalPlaces)
				.Must(HaveValue)
				.WithMessage(Texts.CampoPorPreencher);

			RuleFor(c => c.ExtractionType)
				.NotEmpty()
				.WithMessage(Texts.CampoPorPreencher);

			RuleFor(c => c.Formula)
				.NotEmpty()
				.When(c => c.ExtractionType == DigitalExtractionTypes.FORMULA || c.ExtractionType == DigitalExtractionTypes.EXTRACTED_FORMULA)
				.WithMessage(Texts.CampoPorPreencher);

			Custom(ValidationAction);
		}

		private bool HaveValue(int decimalHouses)
		{
			return (decimalHouses >= 0);
		}

		private ValidationFailure ValidationAction(DigitalXmlFieldData data)
		{
			bool exists = (data.SupplierPkid == null || data.SupplierPkid == Guid.Empty) ?
				(_eDigitalDocTypeXmlDataRepository.ExistsXmlField(data.NomeCampo, data.Localizacao, data.TipoDocPkid)) :
				(_eDigitalSupplierXmlDataRepository.ExistsXmlField(data.NomeCampo, data.Localizacao, data.TipoDocPkid, data.SupplierPkid));

			if (exists && data.Action == StandardActions.CREATE)
				return new ValidationFailure("NomeCampo", Texts.DigitalXmlFieldExists);

			return null;
		}

	}
}