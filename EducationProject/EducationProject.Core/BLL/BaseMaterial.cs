using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.BLL
{
    public abstract class BaseMaterial: BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public enum MaterialType { Video, Article, Book }
    }
}
