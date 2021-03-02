using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class ShowAccountInfoCommand : ICommand
    {
        public string Name => "_getAccountInfo";

        private IAccountService accounts;

        private int dataLimit = 3;

        public ShowAccountInfoCommand(IAccountService accountService)
        {
            this.accounts = accountService;
        }

        public void Run(ref string token)
        {
            Console.WriteLine("Showing account info");

            var account = this.accounts.GetAccountInfo(token);

            if(account is null)
            {
                Console.WriteLine("Error");
                Console.WriteLine();
                return;
            }

            StringBuilder builder = new StringBuilder();

            builder.Append($"ID: {account.Id} Email: {account.Email}\n");
            builder.Append($"Registration date: {account.RegistrationDate}\n");
            builder.Append($"Name: {account.FirstName} {account.SecondName}\n");
            builder.Append($"Passed courses({account.PassedCoursesCount}):\n\t");
            builder.Append(String.Join("\n\t", account.PassedCourses
                .Take(dataLimit).Select(c => $"ID: {c.CourseId} Title: {c.Title}")));
            builder.Append($"\nCourses in progress({account.CoursesInProgressCount}):\n\t");
            builder.Append(String.Join("\n\t", account.CoursesInProgress
                .Take(dataLimit).Select(c => $"ID: {c.CourseId} Title: {c.Title}")));
            builder.Append("\nSkills:\n\t");
            builder.Append(String.Join("\n\t", account.AccountSkills
                .Take(dataLimit).Select(s => $"Id: {s.SkillId}. Title: {s.Title}. Results: Lvl {s.Level}, value {s.CurrentResult}")));

            Console.WriteLine(builder);
            Console.WriteLine();
        }
    }
}
