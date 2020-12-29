using DomainCore.BLL;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainServices.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> Get(Predicate<int> Condition);

        T Get(int Id);

        T Get(T Entity);

        void Insert(T Entity);

        void Update(T Entity);

        void Delete(T Entity);

        void Save();
    }
}
