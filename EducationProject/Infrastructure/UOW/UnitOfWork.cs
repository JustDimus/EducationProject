using DomainCore.BLL;
using DomainServices.Interfaces;
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

        private IRepository<User> _usersRepos;

        public UnitOfWork(XMLContext DataContext)
        {
            _dataContext = DataContext;
        }

        public IRepository<User> Users
        {
            get
            {
                if(_usersRepos == null)
                {
                    _usersRepos = new BaseRepository<User>(_dataContext.Users);
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
