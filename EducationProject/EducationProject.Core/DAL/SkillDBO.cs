using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.DAL
{
    public class SkillDBO: BaseEntity
    {
        public string Title { get; set; }

        public int MaxValue { get; set; }
    }
}
