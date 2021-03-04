using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.Models
{
    public class BookMaterial: BaseMaterial
    {
        public string Author { get; set; }

        public int Pages { get; set; }
    }
}
