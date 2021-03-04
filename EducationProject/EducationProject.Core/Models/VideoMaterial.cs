using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.Models
{
    public class VideoMaterial: BaseMaterial
    {
        public string URI { get; set; }

        public int Duration { get; set; }

        public int Quality { get; set; }
    }
}
