using DomainCore.BLL;
using System;
using XMLDataContext.Interfaces;

namespace XMLDataContext.DataContext
{
    public class XMLContext
    {
        public readonly IDbSet<User> Users;

        public void Save()
        {
            //TODO
        }
    }
}
