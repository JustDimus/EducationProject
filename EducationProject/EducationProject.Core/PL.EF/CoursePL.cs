using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.PL.EF
{
    public class CoursePL
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsVisible { get; set; }

        public int CreatorId { get; set; }

        public List<SkillPL> Skills { get; set; }

        public List<BaseMaterialPL> Materials { get; set; }
    }
}
