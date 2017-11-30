﻿using eBillingSuite.Helper;
using eBillingSuite.HelperTools.Interfaces;
using eBillingSuite.Model.eBillingConfigurations;
using eBillingSuite.Repositories.Interfaces;
using eBillingSuite.Resources;
using FluentValidation;
using FluentValidation.Results;
using Ninject;
using SimpleCrypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBillingSuite.Models.Validators
{
    public class SignInDatataValidator : AbstractValidator<SignInData>
    {
        private IeBillingSuiteRequestContext _context;

        //Password
        public const string password = @"^([a-zA-Z0-9.#]{1,126})+$";
        //Username
        public const string username = @"^([a-zA-Z0-9.#]{1,20})+$";

        private ICryptoService _crytoService;
        private ILoginRepository _loginRepository;
        private IEncryption _encryption;

        [Inject]
        public SignInDatataValidator(IeBillingSuiteRequestContext context,
            ICryptoService crytoService,
            ILoginRepository loginRepository,
            IEncryption encryption)
        {

            _crytoService = crytoService;
            _loginRepository = loginRepository;
            _encryption = encryption;
            _context = context;

            RuleFor(x => x.Username)
                .NotEmpty()
                .Matches(username);

            RuleFor(x => x.Password)
                .NotEmpty()
                .Matches(password);

            Custom(ValidateCredentials);

        }

        private ValidationFailure ValidateCredentials(SignInData data)
        {
            eBC_Login match = null;
            match = _loginRepository.Where(l => l.username == data.Username).FirstOrDefault();

            // no match? nothing else to do...
            if (match == null)
                return new ValidationFailure("Username",Texts.InvalidCredentials);
            else
            { 
                // attempt standard authentication
                return ValidateStandardCredentials(match, data.Password);
            }
        }

        private ValidationFailure ValidateStandardCredentials(eBC_Login user, string password)
        {
            // compute password
            var hashedPassword = _encryption.Sha512Encrypt(password);

            // check password
            if (_crytoService.Compare(hashedPassword, user.password))
                return null;

            return new ValidationFailure("Password",Texts.InvalidCredentials);
        }
    }
}