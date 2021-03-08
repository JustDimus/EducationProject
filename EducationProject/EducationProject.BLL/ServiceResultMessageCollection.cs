using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL
{
    public class ServiceResultMessageCollection
    {
        public string LogInError => "LogInError";

        public string EmailExist => "EmailExist";

        public string ConnectionFailed => "ConnectionFailed";

        public string PermissionDenied => "PermissionDenied";

        public string AccountNotExist => "AccountExist";
    }
}
