using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcInterface.Models.Models
{
    public class AddSkillToCourseViewModel
    {
        public int CourseId { get; set; }

        public int SkillId { get; set; }

        public int ChangeValue { get; set; }
    }
}
