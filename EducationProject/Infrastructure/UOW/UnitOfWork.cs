using EducationProject.Core;
using EducationProject.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using XMLDataContext.DataContext;
using EducationProject.Core.DAL;
//using XMLDataContext.Interfaces;
using ADODataContext.DataContext;
using System.Linq;
using ADODataContext.Interfaces;

namespace Infrastructure.UOW
{
    public class UnitOfWork
    {
        private ADOContext _dataContext; 

        public UnitOfWork(ADOContext dataContext)
        {
            _dataContext = dataContext;
        }

        private Dictionary<Type, object> _reposes = new Dictionary<Type, object>();

        public IDbSet<T> Repository<T>() where T: BaseEntity
        {
            object res = null;

            if (_reposes.TryGetValue(typeof(T), out res) == false)
            {
                res = _dataContext.Entity<T>();
                //new BaseRepository<T>(_dataContext.Entity<T>());
                _reposes.Add(typeof(T), res);
            }

            return (IDbSet<T>) res;
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
