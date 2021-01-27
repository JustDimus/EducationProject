using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.BLL
{
    public class VideoData
    {
        public string URI { get; set; }

        public int Duration { get; set; }

        public int Quality { get; set; }
    }

    public class VideoMaterial: BaseMaterial
    {
        public VideoData VideoData { get; set; }
    }
}
