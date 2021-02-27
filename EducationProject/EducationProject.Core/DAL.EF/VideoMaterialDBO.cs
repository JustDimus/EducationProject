﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.DAL.EF
{
    public class VideoMaterialDBO: BaseMaterialDBO
    {
        public string URI { get; set; }

        public int Duration { get; set; }

        public int Quality { get; set; }
    }
}
