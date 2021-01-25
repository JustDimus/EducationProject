using EducationProject.Core.BLL;
using EducationProject.DAL.Mappings;
using Infrastructure.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.DAL.Mappings
{
    public class MaterialMapping : IMapping<Material>
    {
        private UnitOfWork _uow;

        public MaterialMapping(UnitOfWork UOW)
        {
            _uow = UOW;
        }

        public void Create(Material Entity)
        {
            var material = new EducationProject.Core.DAL.Material()
            {
                Description = Entity.Description,
                Data = Entity.Data,
                Title = Entity.Title,
                Type = Entity.Type
            };

            _uow.Repository<EducationProject.Core.DAL.Material>().Create(material);

            Entity.Id = material.Id;
        }

        public void Delete(Material Entity)
        {
            Delete(e => e.Id == Entity.Id);
        }

        public void Delete(Predicate<Material> Condition)
        {
            foreach(var element in Get(Condition))
            {
                _uow.Repository<EducationProject.Core.DAL.Material>().Delete(element.Id);
            }
        }

        public void Delete(int Id)
        {
            Delete(e => e.Id == Id);
        }

        public IEnumerable<Material> Get(Predicate<Material> Condition)
        {
            return _uow.Repository<EducationProject.Core.DAL.Material>().Get(t => true)
                .Select(e => new Material()
                {
                    Data = e.Data,
                    Description = e.Description,
                    Id = e.Id,
                    Title = e.Title,
                    Type = e.Type
                }).Where(e => Condition(e) == true);
        }

        public Material Get(int Id)
        {
            return Get(e => e.Id == Id).FirstOrDefault();
        }

        public void Save()
        {
            _uow.Save();
        }

        public void Update(Material Entity)
        {
            Update(Entity, e => e.Id == Entity.Id);
        }

        public void Update(Material Entity, Predicate<Material> Condition)
        {
            foreach(var element in Get(Condition))
            {
                _uow.Repository<EducationProject.Core.DAL.Material>()
                    .Update(new EducationProject.Core.DAL.Material()
                    {
                        Data = Entity.Data,
                        Description = Entity.Description,
                        Title = Entity.Title,
                        Type = Entity.Type,
                        Id = element.Id
                    });
            }
        }
    }
}
