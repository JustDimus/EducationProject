using EducationProject.Core.BLL;
using EducationProject.DAL.Mappings;
using Infrastructure.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.DAL.Mappings
{
    public class SkillMapping: IMapping<Skill>
    {
        private UnitOfWork _uow;

        public SkillMapping(UnitOfWork UOW)
        {
            _uow = UOW;
        }

        public void Create(Skill Entity)
        {
            var material = new EducationProject.Core.DAL.Skill()
            {
                MaxValue = Entity.MaxValue,
                Title = Entity.Title
            };

            _uow.Repository<EducationProject.Core.DAL.Skill>().Create(material);

            Entity.Id = material.Id;
        }

        public void Delete(Skill Entity)
        {
            Delete(e => e.Id == Entity.Id);
        }

        public void Delete(Predicate<Skill> Condition)
        {
            foreach (var element in Get(Condition))
            {
                _uow.Repository<EducationProject.Core.DAL.Skill>().Delete(element.Id);
            }
        }

        public void Delete(int Id)
        {
            Delete(e => e.Id == Id);
        }

        public IEnumerable<Skill> Get(Predicate<Skill> Condition)
        {
            return _uow.Repository<EducationProject.Core.DAL.Skill>().Get(t => true)
                .Select(e => new Skill()
                {
                    MaxValue = e.MaxValue,
                    Id = e.Id,
                    Title = e.Title,
                }).Where(e => Condition(e) == true);
        }

        public Skill Get(int Id)
        {
            return Get(e => e.Id == Id).FirstOrDefault();
        }

        public void Save()
        {
            _uow.Save();
        }

        public void Update(Skill Entity)
        {
            Update(Entity, e => e.Id == Entity.Id);
        }

        public void Update(Skill Entity, Predicate<Skill> Condition)
        {
            foreach (var element in Get(Condition))
            {
                _uow.Repository<EducationProject.Core.DAL.Skill>()
                    .Update(new EducationProject.Core.DAL.Skill()
                    {
                        MaxValue = Entity.MaxValue,
                        Title = Entity.Title,
                        Id = element.Id
                    });
            }
        }
    }
}
