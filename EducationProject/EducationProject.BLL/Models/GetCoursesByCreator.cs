using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.Models
{
    public class GetCoursesByCreator : TokenBasedDTO
    {
        public int AccountId { get; set; }

        public PageInfoDTO PageInfo { get; set; }
    }
}
