using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcInterface.Models.Models
{
    public class CreateSkillViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int MaxValue { get; set; }
    }
}
