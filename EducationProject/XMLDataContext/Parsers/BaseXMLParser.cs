using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using XMLDataContext.Interfaces;
using System.Reflection;

namespace XMLDataContext.Parsers
{
    public class BaseXMLParser<T> : IXMLParser<T>
    {
        public Func<XElement, T> ParseToClass 
        {
            get => (xelement) =>
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(new StringReader(xelement.ToString()));
            };
        }

        public Func<T, XElement> ParseToXElement
        {
            get => (element) =>
            {
                var writer = new StringWriter();
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(writer, element);
                return XElement.Parse(writer.ToString());
            };
        }

        private string _elementName;

        public string ElementName
        {
            get
            {
                if(_elementName == null)
                {
                    _elementName = typeof(T).Name;
                }
                return _elementName;
            }
        }

        protected XElement DefaultXElement => new XElement(ElementName);
    }
}
