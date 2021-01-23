using Infrastructure.UOW;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using XMLDataContext.DataContext;
using XMLDataContext.DataSets;
using EducationProject.Core;

namespace ConsoleInterface
{
    class Program
    {
        static void Main(string[] args)
        {
          //  var uow = new UnitOfWork();

            var t = new XMLDataContext.DataContext.XMLContext();

            
            t.Skills.Create(new Skill()
            {
                MaxValue = 100,
                Title = "dsadsddsdffzsdddssa"
            });

            t.Skills.Delete(t => true);

            t.Skills.Save();

            t.Save();
        }
    }
}
