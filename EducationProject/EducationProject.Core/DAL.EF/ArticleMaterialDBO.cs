﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.DAL.EF
{
    public class ArticleMaterialDBO: BaseMaterialDBO
    {
        public string URI { get; set; }

        public DateTime PublicationDate { get; set; }
    }
}
