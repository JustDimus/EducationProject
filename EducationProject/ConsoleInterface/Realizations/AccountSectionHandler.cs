using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.Core.BLL;
using EducationProject.Core.PL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleInterface.Realizations
{
    public class AccountSectionHandler : ISectionHandler
    {
        public object ResultData => null;

        private IChainHandler _commands;

        private CourseSectionHandler _courseHandler;

        private AccountAuthenticationData _currentAccount;

        private SkillSectionHandler _skillsHandler;

        private MaterialSectionHandler _materials;

        public AccountSectionHandler(IChainHandler commands, 
            CourseSectionHandler courseHandler, 
            SkillSectionHandler skills,
            MaterialSectionHandler materials)
        {
            _commands = commands;

            _courseHandler = courseHandler;

            _skillsHandler = skills;

            _materials = materials;
        }

        public void Run(AccountAuthenticationData data = null)
        {
            string currentCommand = String.Empty;

            Console.WriteLine("Entering to account section...");

            do
            {
                if(_currentAccount != null)
                {
                    Console.WriteLine($"Current account: {_currentAccount.Login}");
                }

                currentCommand = Console.ReadLine();

                OperateCommand(currentCommand);

                Console.WriteLine();
            } while (currentCommand != ConsoleCommands.ExitCommand);

            Console.WriteLine("\nProgramEnding");
        }

        private void OperateCommand(string command)
        {
            switch(command)
            {
                case ConsoleCommands.ExitCommand:
                    return;
                case ConsoleCommands.LoginCommand:
                    LogIn();
                    return;
                case ConsoleCommands.CreateNewCommand:
                    Registration();
                    return;
                case ConsoleCommands.ShowDataCommand:
                    ShowAccountData();
                    return;
                case ConsoleCommands.LogoutCommand:
                    LogOut();
                    return;
                case ConsoleCommands.GotoCoursesCommand:
                    _courseHandler.Run(_currentAccount);
                    return;
                case ConsoleCommands.GotoSkillsCommand:
                    _skillsHandler.Run(_currentAccount);
                    return;
                case ConsoleCommands.AddCourseToAccountCommand:
                    AddCourseToAccount();
                    return;
                case ConsoleCommands.PassCourseCommand:
                    PassCourse();
                    return;
            }
        }

        private void AddCourseToAccount()
        {
            if(_currentAccount is null)
            {
                Console.WriteLine("Please log in...");
                return;
            }

            string currentMessage = String.Empty;

            int courseId;

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
           
            Console.WriteLine(_commands["AddExistingCourseToAccount"]
                .Handle(new object[] { _currentAccount, _currentAccount.AccountData.Id, courseId }).Result);
        }

        private void LogOut()
        {
            if(_currentAccount is null)
            {
                Console.WriteLine("You're not authorized");
                return;
            }

            _commands["DeauthorizeAccount"].Handle(new object[] { _currentAccount.Token });

            //TODO
            //_currentAccount = null;

            Console.WriteLine("Succesfully logged out");
        }

        private void ShowAccountData()
        {
            if(_currentAccount is null)
            {
                Console.WriteLine("Please, log in");
                return;
            }

            Console.WriteLine($"Email: {_currentAccount.AccountData.Email}\n" +
                $"First name: {_currentAccount.AccountData.FirstName}\n" +
                $"Second name: {_currentAccount.AccountData.SecondName}\n" +
                $"Phone number: {_currentAccount.AccountData.PhoneNumber}\n" +
                $"Registration date: {_currentAccount.AccountData.RegistrationDate}\n" +
                $"Courses in progress: {_currentAccount.AccountData.CoursesInProgress.Count()}\n" +
                $"\t{String.Join("\n\t", _currentAccount.AccountData.CoursesInProgress.Select(c => $"Id: {c.Id}: '{c.Title}'"))}\n" +
                $"Passed courses: {_currentAccount.AccountData.PassedCourses.Count()}\n" +
                $"\t{String.Join("\n\t", _currentAccount.AccountData.PassedCourses.Select(c => $"Id: {c.Id} '{c.Title}'"))}\n" +
                $"Skills:\n\t{String.Join("\n\t", _currentAccount.AccountData.SkillResults.Select(s => $"{s.Skill.Title} Lvl: {s.Level} Current progress: {s.CurrentResult}"))}");
        }

        private void PassCourse()
        {
            if (_currentAccount is null)
            {
                Console.WriteLine("Please log in...");
                return;
            }

            string currentMessage = String.Empty;

            int courseId;

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

            Console.WriteLine(_commands["MoveCourseToPassed"]
                .Handle(new object[] { _currentAccount, _currentAccount.AccountData.Id, courseId }).Result);
        }

        private void Registration()
        {
            Console.WriteLine("Registration process...");

            string login = String.Empty;
            string password = String.Empty;

            Console.Write("Login: ");
            login = Console.ReadLine();

            if (login == ConsoleCommands.ExitCommand)
            {
                Console.WriteLine("Returning...");
                return;
            }

            Console.Write("Password: ");
            password = Console.ReadLine();

            if (password == ConsoleCommands.ExitCommand)
            {
                Console.WriteLine("Returning...");
                return;
            }

            var authResult = _commands["CreateAccount"].Handle(new object[] { login, password });

            if (authResult.Status == ResultType.Failed)
            {
                Console.WriteLine(authResult.Result);
                return;
            }

            Console.WriteLine("Successfully created account");
        }

        private void LogIn()
        {
            if(_currentAccount != null)
            {
                Console.WriteLine("Please log out");
                return;
            }

            Console.WriteLine("LogIn process");

            string login = String.Empty;
            string password = String.Empty;

            Console.Write("Login: ");
            login = Console.ReadLine();

            if(login == ConsoleCommands.ExitCommand)
            {
                Console.WriteLine("Returning...");
                return;
            }

            Console.Write("Password: ");
            password = Console.ReadLine();

            if (password == ConsoleCommands.ExitCommand)
            {
                Console.WriteLine("Returning...");
                return;
            }

            var authResult = _commands["AuthorizeAccount"].Handle(new object[] { login, password });
            
            if(authResult.Status == ResultType.Failed)
            {
                Console.WriteLine(authResult.Result);
                return;
            }

            _currentAccount = (AccountAuthenticationData)authResult.Result;

            Console.WriteLine("Successfully logged in");
        }
    }
}
