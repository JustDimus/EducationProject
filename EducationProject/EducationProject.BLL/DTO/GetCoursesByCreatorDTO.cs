using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.DTO
{
    public class GetCoursesByCreatorDTO : AccountIdBasedDTO
    {
        public PageInfoDTO PageInfo { get; set; }
    }
}
