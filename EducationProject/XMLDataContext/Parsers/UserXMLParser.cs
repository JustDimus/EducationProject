using DomainCore.BLL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using XMLDataContext.Interfaces;

namespace XMLDataContext.Parsers
{
    public class UserXMLParser : BaseXMLParser<User>
    {
        public override Func<XElement, User> ParseToClass => (element) =>
        {
            return new User()
            {
                Id = Int32.Parse(element.Attribute("Id").Value),
                FirstName = element.Element("FirstName").Value,
                SecondName = element.Element("SecondName").Value,
                PhoneNumber = element.Element("PhoneNumber").Value,
                Country = element.Element("Country").Value,
                City = element.Element("City").Value,
                Mail = element.Element("Mail").Value,
                BirthDate = DateTime.Parse(element.Element("BirthDate").Value)
            };
        };

        public override Func<User, XElement> ParseToXElement => (element) =>
        {
            XElement result = DefaultXElement;
            result.Add(new XAttribute("Id", element.Id));
            result.Add(new XElement("FirstName", element.FirstName));
            result.Add(new XElement("SecondName", element.SecondName));
            result.Add(new XElement("PhoneNumber", element.PhoneNumber));
            result.Add(new XElement("Country", element.Country));
            result.Add(new XElement("City", element.City));
            result.Add(new XElement("Mail", element.Mail));
            result.Add(new XElement("BirthDate", element.BirthDate));
            return result;
        };

        public override string ElementName => "User";
    }
}
