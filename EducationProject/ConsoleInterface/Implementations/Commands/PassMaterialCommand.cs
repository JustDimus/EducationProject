using EducationProject.BLL.Interfaces;
using EducationProject.BLL.DTO;
using System;
using ConsoleInterface.Validators;

namespace ConsoleInterface.Implementations.Commands
{
    public class PassMaterialCommand : BaseCommand
    {
        private IAccountService accountService;

        private ChangeAccountMaterialValidator changeAccountMaterialValidator;

        public PassMaterialCommand(
            IAccountService accountService,
            ChangeAccountMaterialValidator changeAccountMaterialValidator,
            string commandName)
            : base(commandName)
        {
            this.accountService = accountService;

            this.changeAccountMaterialValidator = changeAccountMaterialValidator;
        }

        public async override void Run(int accountId)
        {
            Console.WriteLine("Passing material");

            Console.Write("Material ID: ");

            if (!int.TryParse(Console.ReadLine(), out int materialId))
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            var changeAccountMaterial = new ChangeAccountMaterialDTO()
            {
                MaterialId = materialId,
                AccountId = accountId
            };

            if (!this.ValidateEntity(changeAccountMaterial))
            {
                return;
            }

            var actionResult = await this.accountService.AddAccountMaterialAsync(changeAccountMaterial);

            if (!actionResult.IsSuccessful)
            {
                Console.WriteLine("Error");
            }
            else
            {
                Console.WriteLine("Successful");
            }

            Console.WriteLine();
        }

        public bool ValidateEntity(ChangeAccountMaterialDTO changeAccountMaterial)
        {
            var validationresult = this.changeAccountMaterialValidator.Validate(changeAccountMaterial);

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
