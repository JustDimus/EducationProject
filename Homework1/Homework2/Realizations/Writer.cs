using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Homework2.Realizations
{
    public class Writer
    {
        public void CreateFile(string FileName, StringBuilder Data)
        {
            using(var file = File.Create(FileName))
            {
                file.Write(Encoding.UTF8.GetBytes(Data.ToString()));
            }
        }
    }
}
