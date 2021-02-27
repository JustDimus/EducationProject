using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.Models
{
    public class BookMaterialDTO : MaterialDTO
    {
        public string Author { get; set; }

        public int Pages { get; set; }
    }
}
