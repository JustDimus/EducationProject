using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.DTO
{
    public class GetCoursesByCreatorDTO : AccountIdBasedDTO
    {
        public int AccountId { get; set; }

        public PageInfoDTO PageInfo { get; set; }
    }
}
