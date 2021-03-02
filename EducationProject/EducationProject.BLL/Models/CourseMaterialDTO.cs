﻿using EducationProject.Core.DAL.EF.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.Models
{
    public class CourseMaterialDTO
    {
        public int MaterialId { get; set; }

        public string MaterialTitle { get; set; }

        public MaterialType MaterialType { get; set; }
    }
}
