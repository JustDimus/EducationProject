using DomainCore.BLL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using XMLDataContext.Interfaces;

namespace XMLDataContext.Parsers
{
    public class UserXMLParser : IXMLParser<User>
    {
        public Func<XElement, User> ParseToClass => (element) =>
        {
            return new User()
            {

            };
        };

        public Func<User, XElement> ParseToXElement => throw new NotImplementedException();

        public string ElementName => throw new NotImplementedException();
    }
}
