using System;
using System.Collections.Generic;
using System.Text;
using EducationProject.Core.DAL.EF.Enums;

namespace EducationProject.Core.DAL.EF
{
    public class AccountMaterialDBO
    {
        public int AccountId { get; set; }

        public AccountDBO Account { get; set; }

        public int MaterialId { get; set; }

        public BaseMaterialDBO Material { get; set; }

        public ProgressStatus Status { get; set; }
    }
}
