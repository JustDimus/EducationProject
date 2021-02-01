using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace XMLDataContext.Interfaces
{
    public interface IXMLParser<T>
    {
        Func<XElement, T> ParseToClass { get; }

        Func<T, XElement> ParseToXElement { get; }

        string ElementName { get; }
    }
}
