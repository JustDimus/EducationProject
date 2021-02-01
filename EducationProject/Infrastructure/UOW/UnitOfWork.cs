using EducationProject.Core;
using EducationProject.DAL;
using Infrastructure.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using XMLDataContext.DataContext;
using EducationProject.Core.DAL;
using XMLDataContext.Interfaces;
using System.Linq;


namespace Infrastructure.UOW
{
    public class UnitOfWork
    {
        private XMLContext _dataContext; 

        public UnitOfWork(XMLContext dataContext)
        {
            _dataContext = dataContext;
        }

        private Dictionary<Type, object> _reposes = new Dictionary<Type, object>();

        private IEnumerable<IDbSet> _dbSets;

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
