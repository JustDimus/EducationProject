using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.DAL
{
    public class Material: BaseEntity 
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public string Data { get; set; }
    }
}
