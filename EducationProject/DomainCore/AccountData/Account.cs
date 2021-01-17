using DomainCore.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.AccountData
{
    public class Account: BaseEntity
    {
        public RegistrationData RegistrationData { get; set; }

        public UserInformation UserInformation { get; set; }

        public UserRecord Record { get; set; }

        public IEnumerable<PassedCourse> Courses { get; set; }

        public AuthorizationInfo AuthorizationInfo { get; set; }
    }
}
