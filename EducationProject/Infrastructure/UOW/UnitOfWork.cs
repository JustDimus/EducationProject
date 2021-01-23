using EducationProject.Core;
using EducationProject.DAL;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using XMLDataContext.DataContext;

namespace Infrastructure.UOW
{
    public class UnitOfWork
    {
        private XMLContext _dataContext; 

        private IRepository<Account> _usersRepos;

        public UnitOfWork(XMLContext DataContext)
        {
            _dataContext = DataContext;
        }

        public IRepository<Account> Users
        {
            get
            {
                if(_usersRepos == null)
                {
                    _usersRepos = new BaseRepository<Account>(_dataContext.Accounts);
                }
                return _usersRepos;
            }
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
