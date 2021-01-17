using DomainCore.Skills.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.AccountData
{
    public class UserRecord
    {
        public int UserId { get; set; }

        public IEnumerable<ISkill> Skills { get; set; }
    }
}
