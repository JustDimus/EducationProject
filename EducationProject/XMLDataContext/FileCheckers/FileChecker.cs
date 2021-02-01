using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace XMLDataContext.FileCheckers
{
    internal class FileChecker
    {
        private string _connection;

        public FileChecker(string Connection)
        {
            _connection = Connection;
        }

        public XDocument Get()
        {
            if (!File.Exists(_connection))
            {
                using (StreamWriter writer = new StreamWriter(File.Create(_connection)))
                {
                    writer.Write("<root></root>");
                }
            }

            return XDocument.Load(_connection);
        }
    }
}
