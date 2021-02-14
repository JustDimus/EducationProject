using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.PL.EF
{
    public class VideoMaterial : BaseMaterialPL
    {
        public string URI { get; set; }

        public int Duration { get; set; }

        public int Quality { get; set; }
    }
}
