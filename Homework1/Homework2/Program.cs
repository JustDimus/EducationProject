using Homework2.Realizations;
using System;
using System.IO;
using System.Text;

namespace Homework2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Print the field names");

            string fieldsInfo = Console.ReadLine();

            FileCreator creator = new FileCreator();

            creator.SetFields(fieldsInfo);

            StringBuilder file = creator.CreateFile("DataFile");

            Writer writer = new Writer();

            writer.CreateFile("")
        }
    }
}
