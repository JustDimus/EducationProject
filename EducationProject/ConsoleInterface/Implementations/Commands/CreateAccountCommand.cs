using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using ConsoleInterface.Validators;

namespace ConsoleInterface.Implementations.Commands
{
    public class CreateAccountCommand : BaseCommand
    {
        private IAccountService accountService;

        private ChangeEntityValidator<ShortAccountInfoDTO> changeEntityValidator;

        public CreateAccountCommand(
            IAccountService accountService,
            ChangeEntityValidator<ShortAccountInfoDTO> changeEntityValidator,
            string commandName)
            : base(commandName)
        {
            this.accountService = accountService;

            this.changeEntityValidator = changeEntityValidator;
        }

        public async override void Run(int accountId)
        {            
            Console.WriteLine("Creating new account");

            Console.Write("Email: ");

            var email = Console.ReadLine();

            Console.Write("Password: ");

            var password = Console.ReadLine();

            var changeEntity = new ChangeEntityDTO<ShortAccountInfoDTO>()
            {
                AccountId = accountId,
                Entity = new ShortAccountInfoDTO()
                {
                    Email = email,
                    Password = password
                }
            };

            if (!this.ValidateEntity(changeEntity))
            {
                return;
            }

            var actionResult = await this.accountService.CreateAsync(changeEntity);

            if (!actionResult.IsSuccessful)
            {
                Console.WriteLine("Error");
            }
            else
            {
                Console.WriteLine("Succesful");
            }

            Console.WriteLine();
        }

        private bool ValidateEntity(ChangeEntityDTO<ShortAccountInfoDTO> changeEntity)
        {
            var validationresult = this.changeEntityValidator.Validate(changeEntity);

            if (!validationresult.IsValid)
            {
                Console.WriteLine(string.Join("\n", validationresult.Errors));
                Console.WriteLine();
                return false;
            }

            return true;
        }
    }
}
