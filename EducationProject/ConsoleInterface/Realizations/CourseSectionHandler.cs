using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.Core.BLL;
using EducationProject.Core.DAL.EF;
using EducationProject.Core.PL;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ConsoleInterface.Realizations
{
    public class CourseSectionHandler : ISectionHandler
    {
        public object ResultData => throw new NotImplementedException();

        private IChainHandler _commands;

        private SkillSectionHandler _skillsHandler;

        private MaterialSectionHandler _materialsHandler;

        public CourseSectionHandler(IChainHandler commands, 
            SkillSectionHandler skills,
            MaterialSectionHandler materials)
        {
            _commands = commands;

            _skillsHandler = skills;

            _materialsHandler = materials;
        }

        private AccountAuthenticationData _currentAccount;

        private void OperateCommand(string command)
        {
            switch(command)
            {
                case ConsoleCommands.ShowAllCommand:
                    ShowAllCourses();
                    return;
                case ConsoleCommands.CreateNewCommand:
                    CreateNewCourse();
                    return;
                case ConsoleCommands.GotoSkillsCommand:
                    _skillsHandler.Run(_currentAccount);
                    return;
                case ConsoleCommands.GotoMaterialsCommand:
                    _materialsHandler.Run(_currentAccount);
                    return;
                case ConsoleCommands.PublishCourseCommand:
                    ChangeCourseState(true);
                    return;
                case ConsoleCommands.HideCourseCommand:
                    ChangeCourseState(false);
                    return;
                case ConsoleCommands.AddSkillToCourseCommand:
                    AddSkillToCourse();
                    return;
                case ConsoleCommands.AddMaterialToCourseCommand:
                    AddMaterialToCourse();
                    return;
                case ConsoleCommands.ShowMyCoursesCommand:
                    ShowAllCourses(true);
                    return;
            }
        }

        private void ChangeCourseState(bool state)
        {
            Console.WriteLine(state == true? "Publishing course..." : "Hiding course...");

            string command = String.Empty;

            int courseId;

            Console.Write(state == true ? "Enter course id that you want to publish: " : "Enter course id that you want to hide: ");

            command = Console.ReadLine();

            if(Int32.TryParse(command, out courseId) == false)
            {
                Console.WriteLine("Returning...");
                return;
            }

            Console.WriteLine(_commands["ChangeCourseVisibility"]
                .Handle(new object[] { _currentAccount, courseId, state }).Result);
        }

        private void AddSkillToCourse()
        {
            string currentMessage = String.Empty;

            int courseId;

            int skillId;

            int skillChange;

            do
            {
                Console.Write("Enter course id: ");

                currentMessage = Console.ReadLine();

                if(currentMessage == ConsoleCommands.ExitCommand)
                {
                    Console.WriteLine("Returning...");
                    return;
                }

            } while (Int32.TryParse(currentMessage, out courseId) == false);

            do
            {
                Console.Write("Enter skill id: ");

                currentMessage = Console.ReadLine();

                if (currentMessage == ConsoleCommands.ExitCommand)
                {
                    Console.WriteLine("Returning...");
                    return;
                }

            } while (Int32.TryParse(currentMessage, out skillId) == false);

            do
            {
                Console.Write("Enter skill change value (integer): ");

                currentMessage = Console.ReadLine();

                if (currentMessage == ConsoleCommands.ExitCommand)
                {
                    Console.WriteLine("Returning...");
                    return;
                }

            } while (Int32.TryParse(currentMessage, out skillChange) == false);

            Console.WriteLine(_commands["AddExistingSkillToCourse"]
                .Handle(new object[] { _currentAccount, courseId, skillId, skillChange }).Result);
        }

        private void AddMaterialToCourse()
        {
            string currentMessage = String.Empty;

            int courseId;

            int materialId;

            do
            {
                Console.Write("Enter course id: ");

                currentMessage = Console.ReadLine();

                if (currentMessage == ConsoleCommands.ExitCommand)
                {
                    Console.WriteLine("Returning...");
                    return;
                }

            } while (Int32.TryParse(currentMessage, out courseId) == false);

            do
            {
                Console.Write("Enter material id: ");

                currentMessage = Console.ReadLine();

                if (currentMessage == ConsoleCommands.ExitCommand)
                {
                    Console.WriteLine("Returning...");
                    return;
                }

            } while (Int32.TryParse(currentMessage, out materialId) == false);

            Console.WriteLine(_commands["AddExistingMaterialToCourse"]
                .Handle(new object[] { _currentAccount, courseId, materialId }).Result);
        }

        private void CreateNewCourse()
        {
            Console.WriteLine("Creating new course...");

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

        private void ShowAllCourses(bool onlyMine = false)
        {
            Expression<Func<CourseDBO, bool>> condition = c => c.IsVisible == true;

            if(onlyMine == true)
            {
                if(_currentAccount is null)
                {
                    Console.WriteLine("Please log in...");
                    return;
                }
                condition = c => c.CreatorId == _currentAccount.AccountId;
            }

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

                Console.WriteLine();
            } while (currentCommand != ConsoleCommands.ExitCommand);

            Console.WriteLine("Returning...");
        }
    }
}
