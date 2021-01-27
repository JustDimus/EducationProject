using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.DAL
{
    public class CourseDBO: BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsVisible { get; set; }

        public int CreatorId { get; set; }
    }
}
