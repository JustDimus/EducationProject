using System;
using System.Collections.Generic;
using System.Text;

namespace XMLDataContext.Interfaces
{
    public interface IDbSet<T>
    {
        void Create(T Entity);

        T Get(int Id);

        T Get(T Entity);

        IEnumerable<T> Get(Predicate<T> Condition);

        void Update(T Entity, Predicate<T> Condition);

        void Update(T Entity);

        void Delete(T Entity);

        void Delete(int Id);

        void Delete(Predicate<T> Condition);

        void Save();

        int CurrentId { get; }
    }
}
