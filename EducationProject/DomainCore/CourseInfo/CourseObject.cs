using DomainCore.Common;
using DomainCore.CourseInfo.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.CourseInfo
{
    public class CourseObject
    {
        public string Title { get; set; }

        public int Position { get; set; }

        public IEnumerable<ISkillUp> Results { get; set; }
    }
}
