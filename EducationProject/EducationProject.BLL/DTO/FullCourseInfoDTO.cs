using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.DTO
{
    public class FullCourseInfoDTO : ShortCourseInfoDTO
    {
        public string CurrentStatus { get; set; }

        public bool CanBePublished { get; set; }

        public bool CanBeChanged { get; set; }

        public bool CanBePassed { get; set; }

        public EntityInfoPageDTO<CourseSkillDTO> Skills { get; set; }

        public EntityInfoPageDTO<CourseMaterialDTO> Materials { get; set; }
    }
}
