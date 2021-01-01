using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using XMLDataContext.Interfaces;

namespace XMLDataContext.Parsers
{
    public abstract class BaseXMLParser<T> : IXMLParser<T>
    {
        public abstract Func<XElement, T> ParseToClass { get; }

        public abstract Func<T, XElement> ParseToXElement { get; }

        public abstract string ElementName { get; }

        protected XElement DefaultXElement => new XElement(ElementName);
    }
}
