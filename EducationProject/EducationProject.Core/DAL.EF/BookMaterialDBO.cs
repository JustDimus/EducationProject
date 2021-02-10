using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.DAL.EF
{
    public class BookMaterialDBO: BaseEntity
    {
        public BaseMaterialDBO BaseMaterial { get; set; }

        public string Author { get; set; }

        public int Pages { get; set; }
    }
}
