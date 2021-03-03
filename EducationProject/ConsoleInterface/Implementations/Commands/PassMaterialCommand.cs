using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class PassMaterialCommand : BaseCommand
    {
        private IAccountService accountService;

        public PassMaterialCommand(IAccountService accountService,
            string commandName)
            : base(commandName)
        {
            this.accountService = accountService;
        }

        public override void Run(ref string token)
        {
            Console.WriteLine("Passing material");

            Console.Write("Material ID: ");

            if (Int32.TryParse(Console.ReadLine(), out int materialId) == false)
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            var actionResult = accountService.AddAccountMaterial(new ChangeAccountMaterialDTO()
            {
                MaterialId = materialId,
                Token = token
            });

            if (actionResult == false)
            {
                Console.WriteLine("Error");
            }
            else
            {
                Console.WriteLine("Successful");
            }

            Console.WriteLine();
        }
    }
}
