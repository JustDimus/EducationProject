using DomainCore.CourseInfo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Linq;

namespace XMLDataContext.Parsers
{
    public class CourseXMLParser : BaseXMLParser<Course>
    {
        public override Func<XElement, Course> ParseToClass => throw new NotImplementedException();

        public override Func<Course, XElement> ParseToXElement => (course) =>
        {
            var result = this.DefaultXElement;

            result.Add(new XAttribute("Id", course.Id));
            result.Add(new XElement("CreatorId", course.CreatorId));
            result.Add(new XElement("IsVisible", course.IsVisible));
            result.Add(new XElement("Password", course.Password));

            XElement Sections = new XElement("Sections");

            foreach(var sect in course.Sections)
            {
                XElement section = new XElement("Section");

                section.Add(new XAttribute("Position", sect.Position));

                XElement SkillUps = new XElement("SkillUps");

                foreach(var sk in sect.Results.OfType<CourseSkillUp>())
                {
                    XElement skill = new XElement("Skill");

                    skill.Add(new XAttribute("Skill", sk.Skill.Id));

                    skill.Add(new XElement("Type", "Skill"));

                    skill.Add(new XElement("Change", sk.SkillChange));

                    SkillUps.Add(skill);
                }

                foreach (var sk in sect.Results.OfType<CourseAchievementUp>())
                {
                    XElement skill = new XElement("Skill");

                    skill.Add(new XAttribute("Skill", sk.Achievement.Id));

                    skill.Add(new XElement("Type", "Achievement"));

                    skill.Add(new XElement("Change", sk.LvlChanging));

                    SkillUps.Add(skill);
                }

                Sections.Add(section);
            }

            result.Add(Sections);

            return result;
        };

        public override string ElementName => "Course";
    }
}
