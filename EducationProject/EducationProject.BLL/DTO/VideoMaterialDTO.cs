using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.DTO
{
    public class VideoMaterialDTO : MaterialDTO
    {
        public string URI { get; set; }

        public int Duration { get; set; }

        public int Quality { get; set; }
    }
}
