using DomainCore.BLL;
using System;
using System.Collections.Generic;
using System.Text;
using XMLDataContext.DataContext;
using XMLDataContext.Interfaces;

namespace Infrastructure.Repository
{
    public class UsersRepository : BaseRepository<User>
    {

        public UsersRepository(XMLContext elements)
            : base(elements)
        {
            Elements = elements.Users;
        }
    }
}
