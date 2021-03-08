using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using ConsoleInterface.Validators;
using System.Threading.Tasks;

namespace ConsoleInterface.Implementations.Commands
{
    public class AddSkillToCourseCommand : BaseCommand
    {
        private ICourseService courseService;

        private ChangeCourseSkillValidator changeCourseSkillValidator;

        public AddSkillToCourseCommand(
            ICourseService courseService,
            ChangeCourseSkillValidator changeCourseSkillValidator,
            string commandName)
            : base(commandName)
        {
            this.courseService = courseService;

            this.changeCourseSkillValidator = changeCourseSkillValidator;
        }

        public async override Task Run(int accountId)
        {
            Console.WriteLine("Adding skill to course");

            Console.Write("Course ID: ");

            if (!int.TryParse(Console.ReadLine(), out int courseId))
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            Console.Write("Skill ID: ");

            if (!int.TryParse(Console.ReadLine(), out int skillId))
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }
            
            Console.Write("Skill change: ");

            if (!int.TryParse(Console.ReadLine(), out int change))
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            var changeCourseSkill = new ChangeCourseSkillDTO()
            {
                Change = change,
                AccountId = accountId,
                CourseId = courseId,
                SkillId = skillId
            };

            if (!this.ValidateEntity(changeCourseSkill))
            {
                return;
            }

            var actionResult = await this.courseService.AddCourseSkillAsync(changeCourseSkill);

            if (!actionResult.IsSuccessful)
            {
                Console.WriteLine("Error");
                Console.WriteLine(actionResult.ResultMessage);
            }
            else
            {
                Console.WriteLine("Successful");
            }

            Console.WriteLine();
        }

        private bool ValidateEntity(ChangeCourseSkillDTO changeCourseSkill)
        {
            var validationresult = this.changeCourseSkillValidator.Validate(changeCourseSkill);

            if (!validationresult.IsValid)
            {
                Console.WriteLine(string.Join("\n", validationresult.Errors));
                Console.WriteLine();
                return false;
            }

            return true;
        }
    }
}
