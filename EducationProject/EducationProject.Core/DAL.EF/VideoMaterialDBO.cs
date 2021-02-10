using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.DAL.EF
{
    public class VideoMaterialDBO: BaseEntity
    {
        public BaseMaterialDBO BaseMaterial { get; set; }

        public string URI { get; set; }

        public int Duration { get; set; }

        public int Quality { get; set; }
    }
}
