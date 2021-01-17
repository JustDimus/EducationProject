using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.AccountData
{
    public class AuthorizationInfo
    {
        public bool IsLoggedIn { get; set; }

        public string Token { get; set; }
    }
}
