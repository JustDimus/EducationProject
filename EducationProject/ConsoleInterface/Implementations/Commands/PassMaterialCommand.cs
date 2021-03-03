using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class PassMaterialCommand : BaseCommand
    {
        private IAccountService accounts;

        public PassMaterialCommand(IAccountService accountService,
            string commandName)
            : base(commandName)
        {
            this.accounts = accountService;
        }

        public override void Run(ref string token)
        {
            int materialId = 0;

            Console.WriteLine("Passing material");

            Console.Write("Material ID: ");

            if (Int32.TryParse(Console.ReadLine(), out materialId) == false)
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            if(accounts.AddAccountMaterial(new ChangeAccountMaterialDTO()
            {
                MaterialId = materialId,
                Token = token
            }) == false)
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
