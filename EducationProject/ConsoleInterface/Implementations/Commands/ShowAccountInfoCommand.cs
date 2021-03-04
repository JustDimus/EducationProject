using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class ShowAccountInfoCommand : BaseCommand
    {
        private IAccountService accountService;

        public ShowAccountInfoCommand(IAccountService accountService,
            string commandName)
            : base(commandName)
        {
            this.accountService = accountService;
        }

        public async override void Run(int accountId)
        {
            Console.WriteLine("Showing account info");

            var actionresult = await this.accountService.GetAccountInfoAsync(accountId);

            if(!actionresult.IsSuccessful)
            {
                Console.WriteLine("Error");
                Console.WriteLine();
                return;
            }

            StringBuilder builder = new StringBuilder();

            var account = actionresult.Result;

            builder.Append($"ID: {account.Id} Email: {account.Email}\n");
            builder.Append($"Registration date: {account.RegistrationDate}\n");
            builder.Append($"Name: {account.FirstName} {account.SecondName}\n");
            builder.Append($"Passed courses({account.PassedCoursesCount}):\n\t");
            builder.Append(String.Join("\n\t", account.PassedCourses
                .Select(c => $"ID: {c.CourseId} Title: {c.Title}")));
            builder.Append($"\nCourses in progress({account.CoursesInProgressCount}):\n\t");
            builder.Append(String.Join("\n\t", account.CoursesInProgress
                .Select(c => $"ID: {c.CourseId} Title: {c.Title}")));
            builder.Append("\nSkills:\n\t");
            builder.Append(String.Join("\n\t", account.AccountSkills
                .Select(s => $"Id: {s.SkillId}. Title: {s.Title}. Results: Lvl {s.Level}, value {s.CurrentResult}")));

            Console.WriteLine(builder);
            Console.WriteLine();
        }
    }
}
