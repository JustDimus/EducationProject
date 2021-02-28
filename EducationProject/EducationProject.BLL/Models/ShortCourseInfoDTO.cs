using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.Models
{
    public class ShortCourseInfoDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int? CreatorId { get; set; }
    }
}
