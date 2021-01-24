using EducationProject.Core;
using EducationProject.DAL;
using Infrastructure.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using XMLDataContext.DataContext;
using EducationProject.Core.DAL;

namespace Infrastructure.UOW
{
    public class UnitOfWork
    {
        private XMLContext _dataContext; 

        public UnitOfWork(XMLContext DataContext)
        {
            _dataContext = DataContext;
        }

        private Dictionary<Type, object> _reposes = new Dictionary<Type, object>();

        public IRepository<T> Repository<T>() where T: BaseEntity
        {
            object res = null;

            if (_reposes.TryGetValue(typeof(T), out res) == false)
            {
                res = new BaseRepository<T>(_dataContext.Entity<T>());
                _reposes.Add(typeof(T), res);
            }

            return (IRepository<T>)res;
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
