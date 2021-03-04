using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.DTO
{
    public class UpdateAccountDTO : AccountIdBasedDTO
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string PhoneNumber { get; set; }
    }
}
