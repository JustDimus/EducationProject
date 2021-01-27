using EducationProject.Core.BLL;
using EducationProject.DAL.Mappings;
using Infrastructure.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.DAL.Mappings
{
    public class SkillMapping: IMapping<SkillBO>
    {
        private UnitOfWork _uow;

        public SkillMapping(UnitOfWork UOW)
        {
            _uow = UOW;
        }

        public void Create(SkillBO Entity)
        {
            var material = new EducationProject.Core.DAL.SkillDBO()
            {
                MaxValue = Entity.MaxValue,
                Title = Entity.Title
            };

            _uow.Repository<EducationProject.Core.DAL.SkillDBO>().Create(material);

            Entity.Id = material.Id;
        }

        public void Delete(SkillBO Entity)
        {
            Delete(e => e.Id == Entity.Id);
        }

        public void Delete(Predicate<SkillBO> Condition)
        {
            foreach (var element in Get(Condition))
            {
                _uow.Repository<EducationProject.Core.DAL.SkillDBO>().Delete(element.Id);
            }
        }

        public void Delete(int Id)
        {
            Delete(e => e.Id == Id);
        }

        public IEnumerable<SkillBO> Get(Predicate<SkillBO> Condition)
        {
            return _uow.Repository<EducationProject.Core.DAL.SkillDBO>().Get(t => true)
                .Select(e => new SkillBO()
                {
                    MaxValue = e.MaxValue,
                    Id = e.Id,
                    Title = e.Title,
                }).Where(e => Condition(e) == true);
        }

        public SkillBO Get(int Id)
        {
            return Get(e => e.Id == Id).FirstOrDefault();
        }

        public void Save()
        {
            _uow.Save();
        }

        public void Update(SkillBO Entity)
        {
            Update(Entity, e => e.Id == Entity.Id);
        }

        public void Update(SkillBO Entity, Predicate<SkillBO> Condition)
        {
            foreach (var element in Get(Condition))
            {
                _uow.Repository<EducationProject.Core.DAL.SkillDBO>()
                    .Update(new EducationProject.Core.DAL.SkillDBO()
                    {
                        MaxValue = Entity.MaxValue,
                        Title = Entity.Title,
                        Id = element.Id
                    });
            }
        }
    }
}
