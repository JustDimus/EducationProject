using DomainCore.BLL;
using System;
using System.Collections.Generic;
using System.Text;
using XMLDataContext.DataContext;
using XMLDataContext.Interfaces;

//NONEED

namespace Infrastructure.Repository
{
    public class UsersRepository : BaseRepository<User>
    {
        public UsersRepository(IDbSet<User> elements)
            : base(elements)
        {

        }
    }
}
