using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationProject.BLL.DTO;
using EducationProject.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcInterface.Models.Models;
using MvcInterface.ServiceResultController.Interfaces;

namespace MvcInterface.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private ICourseService courseService;

        private IServiceResultParser blMessageParser;

        public CourseController(
            ICourseService courseService,
            IServiceResultParser blServiceResultMessageParser)
        {
            this.courseService = courseService;

            this.blMessageParser = blServiceResultMessageParser;
        }

        [HttpGet]
        public async Task<IActionResult> Publish([FromQuery] int courseId)
        {
            var courseVisibility = new CourseVisibilityDTO()
            {
                CourseId = courseId,
                Visibility = true
            };

            await this.courseService.ChangeCourseVisibilityAsync(courseVisibility);

            return this.RedirectToAction("Show", new { courseId = courseId });
        }

        [HttpGet]
        public async Task<IActionResult> ChangeSkill(
            [FromQuery] int courseId,
            [FromQuery] int skillId)
        {
            var skillServiceResult = await this.courseService.GetCourseSkillAsync(courseId, skillId);

            if(!skillServiceResult.IsSuccessful)
            {
                this.ModelState.AddModelError(
                    string.Empty,
                    this.blMessageParser[skillServiceResult.MessageCode]);

                return View();
            }

            var addSkillToCourseVM = new EditCourseSkillViewModel()
            {
                CourseId = courseId,
                SkillId = skillId,
                ChangeValue = skillServiceResult.Result.SkillChange
            };

            return this.View(addSkillToCourseVM);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeSkill([FromForm] EditCourseSkillViewModel courseSkillModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(courseSkillModel);
            }

            var addSkillToCourseDTO = new ChangeCourseSkillDTO()
            {
                CourseId = courseSkillModel.CourseId,
                SkillId = courseSkillModel.SkillId,
                Change = courseSkillModel.ChangeValue
            };

            var serviceResult = await this.courseService.ChangeCourseSkillAsync(addSkillToCourseDTO);

            if (!serviceResult.IsSuccessful)
            {
                this.ModelState.AddModelError(
                    string.Empty,
                    this.blMessageParser[serviceResult.MessageCode]);

                return this.View(courseSkillModel);
            }
            else
            {
                return this.RedirectToAction("Show", new { courseId = courseSkillModel.CourseId });
            }
        }

        [HttpGet]
        public async Task<IActionResult> RemoveSkill(
            [FromQuery] int courseId,
            [FromQuery] int skillId)
        {
            var courseSkillDTO = new ChangeCourseSkillDTO()
            {
                CourseId = courseId,
                SkillId = skillId
            };

            await this.courseService.RemoveCourseSkillAsync(courseSkillDTO);

            return RedirectToAction("Show", new { courseId = courseId });
        }

        [HttpGet]
        public IActionResult AddSkill(
            [FromQuery] int courseId, 
            [FromQuery] int skillId)
        {
            var addSkillToCourseVM = new AddSkillToCourseViewModel()
            {
                CourseId = courseId,
                SkillId = skillId
            };

            return this.View(addSkillToCourseVM);
        }

        [HttpPost]
        public async Task<IActionResult> AddSkill([FromForm] AddSkillToCourseViewModel addSkillModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(addSkillModel);
            }

            var addSkillToCourseDTO = new ChangeCourseSkillDTO()
            {
                CourseId = addSkillModel.CourseId,
                SkillId = addSkillModel.SkillId,
                Change = addSkillModel.ChangeValue
            };

            var serviceResult = await this.courseService.AddCourseSkillAsync(addSkillToCourseDTO);
            
            if(!serviceResult.IsSuccessful)
            {
                this.ModelState.AddModelError(
                    string.Empty,
                    this.blMessageParser[serviceResult.MessageCode]);

                return this.View(addSkillModel);
            }
            else
            {
                return this.RedirectToAction("Show", new { courseId = addSkillModel.CourseId });
            }
        }

        [HttpGet]
        public async Task<IActionResult> AddMaterial(
            [FromQuery] int courseId,
            [FromQuery] int materialId)
        {
            var courseMaterialChangeDTO = new ChangeCourseMaterialDTO()
            {
                CourseId = courseId,
                MaterialId = materialId
            };

            await this.courseService.AddCourseMaterialAsync(courseMaterialChangeDTO);

            return this.RedirectToAction("Show", new { courseId = courseId });
        }

        [HttpGet] 
        public async Task<IActionResult> RemoveMaterial(
            [FromQuery] int courseId,
            [FromQuery] int materialId)
        {
            var courseMaterialDTO = new ChangeCourseMaterialDTO()
            {
                CourseId = courseId,
                MaterialId = materialId
            };

            await this.courseService.RemoveCourseMaterialAsync(courseMaterialDTO);

            return this.RedirectToAction("Show", new { courseId = courseId });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateCourseViewModel courseModel)
        {
            if(!this.ModelState.IsValid)
            {
                return this.View();
            }

            var courseDTO = new ShortCourseInfoDTO()
            {
                Title = courseModel.Title,
                Description = courseModel.Description
            };

            var serviceResult = await this.courseService.CreateCourseAsync(courseDTO);

            if(!serviceResult.IsSuccessful)
            {
                this.ModelState.AddModelError(
                    string.Empty,
                    this.blMessageParser[serviceResult.MessageCode]);

                return this.View(courseModel);
            }
            else
            {
                return this.RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromQuery] int courseId)
        {
            var courseServiceResult = await this.courseService.GetCourseInfoAsync(courseId);

            if (!courseServiceResult.IsSuccessful)
            {
                this.ModelState.AddModelError(
                    string.Empty,
                    this.blMessageParser[courseServiceResult.MessageCode]);

                return this.View();
            }

            var courseVM = new EditCourseViewModel()
            {
                Id = courseServiceResult.Result.Id,
                Description = courseServiceResult.Result.Description,
                Title = courseServiceResult.Result.Title
            };

            return this.View(courseVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] EditCourseViewModel courseModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(courseModel);
            }

            var courseDTO = new ShortCourseInfoDTO()
            {
                Description = courseModel.Description,
                Id = courseModel.Id,
                Title = courseModel.Title
            };

            var courseUpdateResult = await this.courseService.UpdateCourseAsync(courseDTO);

            if(!courseUpdateResult.IsSuccessful)
            {
                this.ModelState.AddModelError(
                    string.Empty,
                    this.blMessageParser[courseUpdateResult.MessageCode]);

                return View(courseModel);
            }
            else
            {
                return this.RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromQuery] int courseId)
        {
            await this.courseService.DeleteCourseAsync(courseId);

            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Show(
            [FromQuery] int courseId,
            [FromQuery] int? skillPageNumber,
            [FromQuery] int? skillPageSize,
            [FromQuery] int? materialPageNumber,
            [FromQuery] int? materialPageSize)
        {
            var skillPageInfo = new PageInfoDTO()
            {
                PageNumber = skillPageNumber ?? 0,
                PageSize = skillPageSize ?? 4
            };

            var materialPageInfo = new PageInfoDTO()
            {
                PageNumber = materialPageNumber ?? 0,
                PageSize = materialPageSize ?? 6
            };

            var courseInfoServiceResult = await this.courseService.GetFullCourseInfoAsync(
                courseId,
                materialPageInfo,
                skillPageInfo);

            if (!courseInfoServiceResult.IsSuccessful)
            {
                return this.RedirectToAction("Index", "Home");
            }

            return this.View(courseInfoServiceResult.Result);
        }

        [HttpGet]
        public async Task<IActionResult> ShowPage(
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
