using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.Core.BLL;
using EducationProject.Core.PL;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations
{
    public class SkillSectionHandler : ISectionHandler
    {
        public object ResultData => throw new NotImplementedException();

        private AccountAuthenticationData _currentAccount;

        private IChainHandler _commands;

        public SkillSectionHandler(IChainHandler commands)
        {
            _commands = commands;
        }

        public void Run(AccountAuthenticationData data = null)
        {
            _currentAccount = data;

            string currentCommand = String.Empty;

            Console.WriteLine("Entering the skills section");

            do
            {
                if (_currentAccount != null)
                {
                    Console.WriteLine($"Current account: {_currentAccount.Login}");
                }

                currentCommand = Console.ReadLine();

                OperateCommand(currentCommand);

                Console.WriteLine();
            } while (currentCommand != ConsoleCommands.ExitCommand);

            Console.WriteLine("Returning...");
        }

        private void ShowAll()
        {
            var reqResult = _commands["ShowExistingSkills"].Handle(new object[] { null });
            if (reqResult.Status == ResultType.Failed)
            {
                Console.WriteLine(reqResult.Result);
                return;
            }

            foreach (var skill in ((IEnumerable<SkillBO>)reqResult.Result))
            {
                Console.WriteLine($"Id: {skill.Id} '{skill.Title}' Max value: {skill.MaxValue}");
            }
        }

        private void CreateNew()
        {
            Console.WriteLine("Creating new skill...");

            string title = string.Empty;

            int maxValue;

            string maxValueString = string.Empty;

            Console.Write("Skill title: ");
            title = Console.ReadLine();

            if (title == ConsoleCommands.ExitCommand)
            {
                Console.WriteLine("Returning...");
                return;
            }

            Console.Write("Max skill value: ");
            maxValueString = Console.ReadLine();

            if (maxValueString == ConsoleCommands.ExitCommand)
            {
                Console.WriteLine("Returning...");
                return;
            }

            if(Int32.TryParse(maxValueString, out maxValue) == false)
            {
                Console.WriteLine("Error. Max value is a integer number\nReturning...");
                return;
            }

            var authResult = _commands["CreateSkill"].Handle(new object[] { _currentAccount, title, maxValue });

            if (authResult.Status == ResultType.Failed)
            {
                Console.WriteLine(authResult.Result);
                return;
            }

            Console.WriteLine("Successfully created skill");
        }

        private void OperateCommand(string command)
        {
            switch (command)
            {
                case ConsoleCommands.ShowAllCommand:
                    ShowAll();
                    return;
                case ConsoleCommands.CreateNewCommand:
                    CreateNew();
                    return;
            }
        }
    }
}
