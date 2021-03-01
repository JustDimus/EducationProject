using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.Models
{
    public class ChangeAccountMaterialDTO : TokenBasedDTO
    {
        public int AccountId { get; set; }

        public int MaterialId { get; set; }
    }
}
