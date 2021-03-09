using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.DTO
{
    public class EntityInfoPageDTO<TEntity>
    {
        public IEnumerable<TEntity> Entities { get; set; }

        public int CurrentPage { get; set; }

        public int CurrentPageSize { get; set; }

        public bool CanMoveForward { get; set; }

        public bool CanMoveBack { get; set; }
    }
}
