using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.BLL
{
    public class ArticleData
    {
        public string URI { get; set; }

        public DateTime PublicationDate { get; set; }
    }

    public class ArticleMaterial: BaseMaterial
    {
        public ArticleData ArticleData { get; set; }
    }
}
