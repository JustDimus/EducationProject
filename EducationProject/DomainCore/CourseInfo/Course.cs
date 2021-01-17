using DomainCore.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.CourseInfo
{
    public class Course: BaseEntity
    {
        public string Title { get; set; }

        public int CreatorId { get; set; }

        public bool IsVisible { get; set; }

        public string Password { get; set; }

        public IEnumerable<CourseObject> Sections { get; set; }
    }
}
