using DomainCore.BLL;
using System;
using System.Collections.Generic;
using System.Text;
using XMLDataContext.Interfaces;

//NONEED

namespace XMLDataContext.DataSets
{
    public class UserDbSet : BaseDbSet<User>
    {
        public UserDbSet(IXMLParser<User> Parser) : base(Parser, null)
        {

        }
    }
}
