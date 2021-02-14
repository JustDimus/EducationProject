using EducationProject.Core.DAL.EF.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.PL.EF
{
    public class BaseMaterialPL
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public MaterialType Type { get; set; }
    }
}
