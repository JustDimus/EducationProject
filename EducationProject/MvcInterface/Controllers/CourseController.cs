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
