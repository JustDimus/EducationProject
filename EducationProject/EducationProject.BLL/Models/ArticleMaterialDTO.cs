using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.Models
{
    public class ArticleMaterialDTO : MaterialDTO
    {
        public string URI { get; set; }

        public DateTime PublicationDate { get; set; }
    }
}
