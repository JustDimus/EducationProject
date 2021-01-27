using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.Core.BLL;
using EducationProject.Core.PL;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Realizations
{
    public class CourseSectionHandler : ISectionHandler
    {
        public object ResultData => throw new NotImplementedException();

        private IChainHandler _commands;

        private SkillSectionHandler _skillsHandler;

        public CourseSectionHandler(IChainHandler commands, SkillSectionHandler skills)
        {
            _commands = commands;

            _skillsHandler = skills;
        }

        private AccountAuthenticationData _currentAccount;

        private void OperateCommand(string command)
        {
            switch(command)
            {
                case ConsoleCommands.ShowAll:
                    ShowAll();
                    return;
                case ConsoleCommands.CreateNewCommand:
                    CreateNewCourse();
                    return;
                case ConsoleCommands.GotoSkillsCommand:
                    _skillsHandler.Run(_currentAccount);
                    return;
            }
        }

        private void CreateNewCourse()
        {
            string title = string.Empty;

            string description = string.Empty;

            Console.Write("Course title: ");
            title = Console.ReadLine();

            if (title == ConsoleCommands.ExitCommand)
            {
                Console.WriteLine("Returning...");
                return;
            }

            Console.Write("Course description: ");
            description = Console.ReadLine();

            if (description == ConsoleCommands.ExitCommand)
            {
                Console.WriteLine("Returning...");
                return;
            }

            var authResult = _commands["CreateCourse"].Handle(new object[] { _currentAccount, title, description });

            if (authResult.Status == ResultType.Failed)
            {
                Console.WriteLine(authResult.Result);
                return;
            }

            Console.WriteLine("Successfully created course");
        }

        private void ShowAll()
        {
            Predicate<CourseBO> condition = new Predicate<CourseBO>(c => c.IsVisible == true);

            var reqResult = _commands["ShowExistingCourses"].Handle(new object[] { condition });
            if (reqResult.Status == ResultType.Failed)
            {
                Console.WriteLine(reqResult.Result);
                return;
            }

            foreach (var course in ((IEnumerable<CourseBO>)reqResult.Result))
            {
                Console.WriteLine($"Id: {course.Id} '{course.Title}' Creator: {course.CreatorId}");
            }
        }

        public void Run(AccountAuthenticationData data = null)
        {
            _currentAccount = data;

            string currentCommand = String.Empty;

            Console.WriteLine("Entering the courses section");

            do
            {
                if (_currentAccount != null)
                {
                    Console.WriteLine($"Current account: {_currentAccount.Login}");
                }

                currentCommand = Console.ReadLine();

                OperateCommand(currentCommand);

            } while (currentCommand != ConsoleCommands.ExitCommand);

            Console.WriteLine("Returning...");
        }
    }
}
