using EducationProject.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.DTO
{
    public class MaterialDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public MaterialType Type { get; set; }
    }
}
