using System;
using System.Collections.Generic;
using System.Text;

namespace XMLDataContext.Interfaces
{
    public interface IDbSet<T>: IDbSet
    {
        void Create(T Entity);

        IEnumerable<T> Get(Predicate<T> Condition);

        T Get(int Id);

        T Get(T Entity);

        void Update(T Entity);

        void Update(T Entity, Predicate<T> Condition);

        void Delete(T Entity);

        void Delete(int Id);

        void Delete(Predicate<T> Condition);

        int CurrentId { get; }
    }
}
