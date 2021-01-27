using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.BLL
{
    public class BookData
    {
        public string Author { get; set; }

        public int Pages { get; set; }
    }

    public class BookMaterial: BaseMaterial
    {
        public BookData BookData { get; set; }
    }
}
