using EducationProject.Core.BLL;
using EducationProject.DAL.Interfaces;
using Infrastructure.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;

namespace Infrastructure.DAL.Mappings
{
    /*
    public class MaterialMapping : IMapping<BaseMaterial>
    {
        private UnitOfWork _uow;

        public MaterialMapping(UnitOfWork UOW)
        {
            _uow = UOW;
        }

        public void Create(BaseMaterial Entity)
        {
            var material = new EducationProject.Core.DAL.MaterialDBO()
            {
                Description = Entity.Description,
                Title = Entity.Title,
                //Type = Entity.Type
            };

            material.Data = GetSerializedData(Entity);

            _uow.Repository<EducationProject.Core.DAL.MaterialDBO>().Create(material);

            Entity.Id = material.Id;
        }

        private string GetSerializedData(BaseMaterial material)
        {
            string jsonData = "";
            switch (material)
            {
                case VideoMaterial video:
                    jsonData = JsonSerializer.Serialize(video.VideoData);
                    break;
                case ArticleMaterial article:
                    jsonData = JsonSerializer.Serialize(article.ArticleData);
                    break;
                case BookMaterial book:
                    jsonData = JsonSerializer.Serialize(book.BookData);
                    break;
                default:
                    throw new ArgumentException();
            }
            return jsonData;
        }

        public void Delete(BaseMaterial Entity)
        {
            Delete(e => e.Id == Entity.Id);
        }

        public void Delete(int Id)
        {
            Delete(e => e.Id == Id);
        }

        public BaseMaterial Get(int Id)
        {
            return Get(e => e.Id == Id, 0, 30).FirstOrDefault();
        }

        public void Save()
        {
            _uow.Save();
        }

        public void Update(BaseMaterial Entity)
        {
            Update(Entity, e => e.Id == Entity.Id);
        }

        public void Update(BaseMaterial Entity, Expression<Func<BaseMaterial, bool>> Condition)
        {
            foreach(var element in Get(Condition, 0, 30))
            {
                _uow.Repository<EducationProject.Core.DAL.MaterialDBO>()
                    .Update(new EducationProject.Core.DAL.MaterialDBO()
                    {
                        Data = GetSerializedData(element),
                        Description = Entity.Description,
                        Title = Entity.Title,
                       //Type = Entity.Type,
                        Id = element.Id
                    });
            }
        }

        public IEnumerable<BaseMaterial> Get(Expression<Func<BaseMaterial, bool>> condition, int pageNumber, int pageSize)
        {
            var predicate = condition.Compile();
            return _uow.Repository<EducationProject.Core.DAL.MaterialDBO>().Get(t => true)
                .Select(e =>
                {
                    BaseMaterial material = null;
                    switch (e.Type)
                    {
                        case "Video":
                            material = new VideoMaterial()
                            {
                                Id = e.Id,
                                Description = e.Description,
                                Title = e.Title,
                                //Type = e.Type,
                                VideoData = (VideoData)JsonSerializer.Deserialize(e.Data, typeof(VideoData))
                            };
                            break;
                        case "Article":
                            material = new ArticleMaterial()
                            {
                                Id = e.Id,
                                Description = e.Description,
                                Title = e.Title,
                                //Type = e.Type,
                                ArticleData = (ArticleData)JsonSerializer.Deserialize(e.Data, typeof(ArticleData))
                            };
                            break;
                        case "Book":
                            material = new BookMaterial()
                            {
                                Id = e.Id,
                                Description = e.Description,
                                Title = e.Title,
                                //Type = e.Type,
                                BookData = (BookData)JsonSerializer.Deserialize(e.Data, typeof(BookData))
                            };
                            break;
                    }
                    return material;
                }).Where(c => predicate(c) == true);
        }

        public void Delete(Expression<Func<BaseMaterial, bool>> condition)
        {
            foreach (var element in Get(condition, 0, 30))
            {
                _uow.Repository<EducationProject.Core.DAL.MaterialDBO>().Delete(element.Id);
            }
        }

        public bool Any(Expression<Func<BaseMaterial, bool>> condition)
        {
            throw new NotImplementedException();
        }
    }

    */
}
