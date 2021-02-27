using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.Models
{
    public class GetAccountSkillsDTO : TokenBasedDTO
    {
        public int AccountId { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}
