using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.Interfaces
{
    public interface IMapping<T>
    {
        void Create(T Entity);

        IEnumerable<T> Get(Predicate<T> Condition);

        T Get(int Id);

        void Update(T Entity);

        void Update(T Entity, Predicate<T> Condition);

        void Delete(T Entity);

        void Delete(Predicate<T> Condition);

        void Delete(int Id);

        void Save();
    }
}
