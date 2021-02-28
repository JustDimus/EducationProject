﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.Models
{
    public class FullCourseInfoDTO : ShortCourseInfoDTO
    {
        public int? CreatorId { get; set; }

        public IEnumerable<CourseSkillDTO> Skills { get; set; }

        public IEnumerable<CourseMaterialDTO> Materials { get; set; }
    }
}
