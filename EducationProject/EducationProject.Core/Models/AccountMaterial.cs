using System;
using System.Collections.Generic;
using System.Text;
using EducationProject.Core.Models.Enums;

namespace EducationProject.Core.Models
{
    public class AccountMaterial
    {
        public int AccountId { get; set; }

        public Account Account { get; set; }

        public int MaterialId { get; set; }

        public BaseMaterial Material { get; set; }

        public ProgressStatus Status { get; set; }
    }
}
