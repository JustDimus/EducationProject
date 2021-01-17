using DomainCore.Common;
using DomainCore.Skills.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Skills
{
    public class Skill: BaseEntity, ISkill
    {
        public string Type { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public float MaxValue { get; set; }
    }
}
