using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.Core.BLL;
using EducationProject.Core.PL;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations
{
    public class MaterialSectionHandler : ISectionHandler
    {
        public object ResultData => throw new NotImplementedException();

        private AccountAuthenticationData _currentAccount;

        private IChainHandler _commands;

        public MaterialSectionHandler(IChainHandler commands)
        {
            _commands = commands;
        }

        public void Run(AccountAuthenticationData data = null)
        {
            _currentAccount = data;

            string currentCommand = String.Empty;

            Console.WriteLine("Entering the materials section");

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

        private void ShowAllMaterials()
        {
            var reqResult = _commands["ShowExistingMaterials"].Handle(new object[] { null });
            if (reqResult.Status == ResultType.Failed)
            {
                Console.WriteLine(reqResult.Result);
                return;
            }

            foreach (var material in ((IEnumerable<BaseMaterial>)reqResult.Result))
            {
                Console.WriteLine($"Id: {material.Id} '{material.Title}' Type: {material.Type}");
                
                switch(material)
                {
                    case VideoMaterial video:
                        Console.WriteLine($"\tURI: {video.VideoData.URI}\n\t" +
                            $"Quality: {video.VideoData.Quality}\n\t" +
                            $"Duration: {video.VideoData.Duration/3600}h {video.VideoData.Duration%3600/60}m {video.VideoData.Duration%60}s");
                        break;
                    case ArticleMaterial article:
                        Console.WriteLine($"\tURI: {article.ArticleData.URI}\n\t" +
                            $"Publication date: {article.ArticleData.PublicationDate.ToString("dd.MM.YYYY")}");
                        break;
                    case BookMaterial book:
                        Console.WriteLine($"\tAuthor(s): {book.BookData.Author}\n\t" +
                            $"Pages: {book.BookData.Pages}");
                        break;
                }
            }
        }

        private void CreateNewMaterial()
        {
            Console.WriteLine("Creating new material...");

            string title = String.Empty;
            string description = String.Empty;
            string type = String.Empty;

            string sData1 = String.Empty;
            string sData2 = String.Empty;
            string sData3 = String.Empty;

            int nType = 0;

            int nData1;
            int nData2;
            DateTime dtData1;

            Console.Write("Title: ");
            title = Console.ReadLine();

            if (title == ConsoleCommands.ExitCommand)
            {
                Console.WriteLine("Returning...");
                return;
            }

            Console.Write("Description: ");
            description = Console.ReadLine();

            if (description == ConsoleCommands.ExitCommand)
            {
                Console.WriteLine("Returning...");
                return;
            }

            Console.Write("Choose the type of material\nEnter '1' - for video, '2' - for article, '3' - for book: ");

            do
            {
                type = Console.ReadLine();
                if (type == ConsoleCommands.ExitCommand)
                {
                    Console.WriteLine("Returning...");
                    return;
                }
            } while (Int32.TryParse(type, out nType) == false);

            IOperationResult procResult = null;

            switch(nType)
            {
                case 1:
                    type = "Video";
                    Console.Write("Video URI: ");
                    sData1 = Console.ReadLine();
                    do
                    {
                        Console.Write("Enter the video duration (in seconds): ");
                        sData2 = Console.ReadLine();
                        if (sData2 == ConsoleCommands.ExitCommand)
                        {
                            Console.WriteLine("Returning...");
                            return;
                        }
                    } while (Int32.TryParse(sData2, out nData1) == false);

                    do
                    {
                        Console.Write("Enter the video quality (in pixels): ");
                        sData3 = Console.ReadLine();
                        if (sData2 == ConsoleCommands.ExitCommand)
                        {
                            Console.WriteLine("Returning...");
                            return;
                        }
                    } while (Int32.TryParse(sData3, out nData2) == false);

                    procResult = _commands["CreateMaterial"].Handle(new object[] { _currentAccount, title, description, type, sData1, nData1, nData2 });
                    break;
                case 2:
                    type = "Article";
                    Console.Write("Article URI: ");
                    sData1 = Console.ReadLine();

                    do
                    {
                        Console.Write("Enter the publication date of article (dd.MM.yyyy): ");
                        sData2 = Console.ReadLine();
                        if (sData2 == ConsoleCommands.ExitCommand)
                        {
                            Console.WriteLine("Returning...");
                            return;
                        }
                    } while (DateTime.TryParseExact(sData2, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out dtData1) == false);

                    procResult = _commands["CreateMaterial"].Handle(new object[] { _currentAccount, title, description, type, sData1, dtData1 });
                    break;
                case 3:
                    type = "Book";
                    Console.Write("Book author(s): ");
                    sData1 = Console.ReadLine();

                    do
                    {
                        Console.Write("Enter the pages count: ");
                        sData2 = Console.ReadLine();
                        if (sData2 == ConsoleCommands.ExitCommand)
                        {
                            Console.WriteLine("Returning...");
                            return;
                        }
                    } while (Int32.TryParse(sData2, out nData1) == false);

                    procResult = _commands["CreateMaterial"].Handle(new object[] { _currentAccount, title, description, type, sData1, nData1 });
                    break;
                default:
                    Console.WriteLine("Returning...");
                    return;     
            }

            if (procResult.Status == ResultType.Failed)
            {
                Console.WriteLine(procResult.Result);
                return;
            }

            Console.WriteLine($"Succesfully created {type} material");
        }

        private void OperateCommand(string command)
        {
            switch(command)
            {
                case ConsoleCommands.ShowAllCommand:
                    ShowAllMaterials();
                    return;
                case ConsoleCommands.CreateNewCommand:
                    CreateNewMaterial();
                    return;
            }
        }
    }
}
