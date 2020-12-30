using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using XMLDataContext.Interfaces;

namespace XMLDataContext.DataSets
{
    public abstract class BaseDbSet<T> : IDbSet<T>
    {
        public void Delete(T Entity)
        {
            throw new NotImplementedException();
        }

        public T Get(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Get(Predicate<T> Condition)
        {
            throw new NotImplementedException();
        }

        public T Get(T Entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void Insert(T Entity)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(T Entity)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
