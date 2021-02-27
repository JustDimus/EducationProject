using EducationProject.BLL.Interfaces;
using EducationProject.Core.BLL;
using EducationProject.Core.DAL.EF;
using EducationProject.DAL.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.BLL.EF
{
    /*
    public class CourseConverter : BaseConverter<CourseDBO, CourseBO>
    {
        private IMapper<BaseMaterialDBO, BaseMaterial> materials;

        private IMapper<SkillDBO, SkillBO> skills;

        public CourseConverter(IRepository<CourseDBO> mapping, 
            IMapper<BaseMaterialDBO, BaseMaterial> materialConverter,
            IMapper<SkillDBO, SkillBO> skillConverter)
            :base(mapping)
        {
            this.materials = materialConverter;
            this.skills = skillConverter;
        }

        public override CourseBO Get(CourseDBO entity)
        {
            return new CourseBO()
            { 
                Id = entity.Id,
                Description = entity.Description,
                CreatorId = entity.CreatorId.Value,
                IsVisible = entity.IsVisible,
                Title = entity.Title,
                Materials = entity.CourseMaterials.Select(cm => new CourseMaterialBO()
                {
                    Material = materials.Get(cm.Material)
                }),
                Skills = entity.CourseSkills.Select(cs => new CourseSkillBO()
                {
                    Skill = skills.Get(cs.Skill),
                    SkillChange = cs.Change
                })
            };
        }
    }
    */
}
