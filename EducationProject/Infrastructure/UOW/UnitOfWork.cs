using EducationProject.Core;
using EducationProject.DAL;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using XMLDataContext.DataContext;
using EducationProject.Core.DAL;

namespace Infrastructure.UOW
{
    public class UnitOfWork
    {
        private XMLContext _dataContext; 

        private IRepository<Account> _usersRepos;

        private IRepository<AccountCourse> _accountCoursesRepos;

        private IRepository<AccountSkills> _accountSkillsRepos;

        private IRepository<Course> _courseRepos;

        private IRepository<CourseMaterial> _courseMaterialRepos;

        private IRepository<CourseSkill> _courseSkillRepos;

        private IRepository<Material> _materialRepos;

        private IRepository<Skill> _skillRepos;

        public UnitOfWork(XMLContext DataContext)
        {
            _dataContext = DataContext;
        }

        public IRepository<Account> Accounts
        {
            get
            {
                if(_usersRepos == null)
                {
                    _usersRepos = new BaseRepository<Account>(_dataContext.Accounts);
                }
                return _usersRepos;
            }
        }

        public IRepository<AccountCourse> AccountCourses
        {
            get
            {
                if (_accountCoursesRepos == null)
                {
                    _accountCoursesRepos = new BaseRepository<AccountCourse>(_dataContext.AccountCourses);
                }
                return _accountCoursesRepos;
            }
        }

        public IRepository<AccountSkills> AccountSkills
        {
            get
            {
                if (_accountSkillsRepos == null)
                {
                    _accountSkillsRepos = new BaseRepository<AccountSkills>(_dataContext.AccountSkills);
                }
                return _accountSkillsRepos;
            }
        }

        public IRepository<Course> Courses
        {
            get
            {
                if (_courseRepos == null)
                {
                    _courseRepos = new BaseRepository<Course>(_dataContext.Courses);
                }
                return _courseRepos;
            }
        }

        public IRepository<CourseMaterial> CourseMaterials
        {
            get
            {
                if (_courseMaterialRepos == null)
                {
                    _courseMaterialRepos = new BaseRepository<CourseMaterial>(_dataContext.CourseMaterials);
                }
                return _courseMaterialRepos;
            }
        }

        public IRepository<CourseSkill> CourseSkills
        {
            get
            {
                if (_courseSkillRepos == null)
                {
                    _courseSkillRepos = new BaseRepository<CourseSkill>(_dataContext.CourseSkills);
                }
                return _courseSkillRepos;
            }
        }

        public IRepository<Material> Materials
        {
            get
            {
                if (_materialRepos == null)
                {
                    _materialRepos = new BaseRepository<Material>(_dataContext.Materials);
                }
                return _materialRepos;
            }
        }

        public IRepository<Skill> Skills
        {
            get
            {
                if (_skillRepos == null)
                {
                    _skillRepos = new BaseRepository<Skill>(_dataContext.Skills);
                }
                return _skillRepos;
            }
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
