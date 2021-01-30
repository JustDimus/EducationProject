using EducationProject.Core;
using System;
using System.Collections.Generic;

namespace EducationProject.DAL
{
    public interface IRepository<T> where T: BaseEntity
    {
        void Create(T Entity);

        IEnumerable<T> Get(Predicate<T> Condition);

        T Get(int Id);

        T Get(T Entity);

        void Update(T Entity);

        void Update(T Entity, Predicate<T> Condition);

        void Delete(T Entity);

        void Delete(Predicate<T> Condition);

        void Delete(int Id);

        //void Save();
    }
}
