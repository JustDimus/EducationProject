using EducationProject.Core;
using System;
using System.Xml.Linq;
using XMLDataContext.Interfaces;

namespace XMLDataContext.Parsers
{
    internal class SkillXMLParser : IXMLParser<Skill>
    {
        public Func<XElement, Skill> ParseToClass => throw new NotImplementedException();

        public Func<Skill, XElement> ParseToXElement => throw new NotImplementedException();

        public string ElementName => throw new NotImplementedException();
    }
}