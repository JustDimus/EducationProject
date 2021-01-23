using XMLDataContext.DataSets;
using XMLDataContext.Interfaces;
using System.Configuration;
using XMLDataContext.Parsers;
using System.Xml.Linq;
using EducationProject.Core;

namespace XMLDataContext.DataContext
{
    public class XMLContext
    {
        private IDbSet<Skill> _skills;

        public IDbSet<Skill> Skills 
        { 
            get
            {
                if (_skills == null)
                {
                    _skills = new BaseDbSet<Skill>(new BaseXMLParser<Skill>(), _document);
                }

                return _skills;
            } 
        }

        private IDbSet<CourseSkill> _courseSkills;

        public IDbSet<CourseSkill> CourseSkills
        {
            get
            {
                if (_courseSkills == null)
                {
                    _courseSkills = new BaseDbSet<CourseSkill>(new BaseXMLParser<CourseSkill>(), _document);
                }

                return _courseSkills;
            }
        }

        private IDbSet<AccountSkills> _accountSkills;

        public IDbSet<AccountSkills> AccountSkills
        {
            get
            {
                if (_accountSkills == null)
                {
                    _accountSkills = new BaseDbSet<AccountSkills>(new BaseXMLParser<AccountSkills>(), _document);
                }

                return _accountSkills;
            }
        }

        private IDbSet<Material> _materials;

        public IDbSet<Material> Materials
        {
            get
            {
                if (_materials == null)
                {
                    _materials = new BaseDbSet<Material>(new BaseXMLParser<Material>(), _document);
                }

                return _materials;
            }
        }

        private IDbSet<CourseMaterial> _courseMaterials;

        public IDbSet<CourseMaterial> CourseMaterials
        {
            get
            {
                if (_courseMaterials == null)
                {
                    _courseMaterials = new BaseDbSet<CourseMaterial>(new BaseXMLParser<CourseMaterial>(), _document);
                }

                return _courseMaterials;
            }
        }

        private IDbSet<Course> _course;

        public IDbSet<Course> Courses
        {
            get
            {
                if (_course == null)
                {
                    _course = new BaseDbSet<Course>(new BaseXMLParser<Course>(), _document);
                }

                return _course;
            }
        }

        private IDbSet<AccountCourse> _accountCourse;

        public IDbSet<AccountCourse> AccountCourses
        {
            get
            {
                if (_accountCourse == null)
                {
                    _accountCourse = new BaseDbSet<AccountCourse>(new BaseXMLParser<AccountCourse>(), _document);
                }

                return _accountCourse;
            }
        }

        private IDbSet<Account> _accounts;

        public IDbSet<Account> Accounts
        {
            get
            {
                if (_accounts == null)
                {
                    _accounts = new BaseDbSet<Account>(new BaseXMLParser<Account>(), _document);
                }

                return _accounts;
            }
        }

        private string _fileName;

        private XDocument _document;

        private string filePath { get; set; }

        public XMLContext()
        {
            _fileName = ConfigurationManager.AppSettings.Get("XMLFile");
            _document = new FileCheckers.FileChecker(_fileName).Get();
        }

        public void Save()
        {
            Accounts.Save();
            AccountCourses.Save();
            AccountSkills.Save();
            Courses.Save();
            Skills.Save();
            CourseSkills.Save();
            CourseMaterials.Save();
            Materials.Save();

            _document.Save(_fileName);
        }
    }
}
