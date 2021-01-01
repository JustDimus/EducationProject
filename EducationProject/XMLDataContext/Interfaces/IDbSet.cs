using DomainCore.BLL;
using System;
using System.Collections.Generic;
using System.Text;

namespace XMLDataContext.Interfaces
{
    public interface IDbSet<T>: IEnumerable<T>
    {
        Dictionary<T, ElementState> Elements { get; }
        
        T Get(int Id);

        IEnumerable<T> Get(Predicate<T> Condition);

        T Get(T Entity);

        void Insert(T Entity);

        void Delete(T Entity);

        void Update(T Entity);

        void Save();
    }
}
