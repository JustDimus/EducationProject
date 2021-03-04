using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.Models
{
    public class ArticleMaterial: BaseMaterial
    {
        public string URI { get; set; }

        public DateTime PublicationDate { get; set; }
    }
}
