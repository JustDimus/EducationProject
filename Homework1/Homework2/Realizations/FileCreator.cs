using Homework2.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

namespace Homework2.Realizations
{
    public class FileCreator
    {
        private List<PropertyInfo> _fields;

        public FileCreator()
        {
            _fields = new List<PropertyInfo>();
        }

        public void SetFields(string FieldsData)
        {
            var types = FieldsData.Trim().Replace(" ", "").Split(',');

            Type personType = typeof(Person);

            _fields.Clear();

            _fields.AddRange((from f in personType.GetProperties() where types.Contains(f.Name) select f));
        }

        public StringBuilder CreateFile(string FileName)
        {
            StringBuilder builder = new StringBuilder();
            
            for(int i = 0; i < _fields.Count; i++)
            {
                builder.Append(_fields[i].Name);

                if(i + 1 < _fields.Count)
                {
                    builder.Append(",");
                }
            }

            builder.Append('\n');

            foreach(var person in PersonList.GetListPerson())
            {
                for (int i = 0; i < _fields.Count; i++)
                {
                    builder.Append(_fields[i].GetValue(person));

                    if (i + 1 < _fields.Count)
                    {
                        builder.Append(",");
                    }
                }
                builder.Append('\n');
            }

            return builder;
        }
    }
}
