using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationProject.BLL.DTO;
using EducationProject.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MvcInterface.Models.Models;
using MvcInterface.ServiceResultController.Interfaces;

namespace MvcInterface.Controllers
{
    [Authorize]
    public class SkillController : Controller
    {
        private ISkillService skillService;

        private IServiceResultParser blMessageParser;

        private int defaultPageNumber;

        private int defaultPageSize;

        public SkillController(
            ISkillService skillService,
            IServiceResultParser blServiceResultMessageParser,
            IConfiguration configuration)
        {
            this.skillService = skillService;

            this.blMessageParser = blServiceResultMessageParser;

            this.defaultPageNumber = int.Parse(configuration["DefaultControllerValues:DefaultPageNumber"]);

            this.defaultPageSize = int.Parse(configuration["DefaultControllerValues:DefaultPageSize"]);
        }

        [HttpGet]
        public IActionResult Create([FromQuery] int? addToCourseId)
        {
            this.ViewBag.addToCourseId = addToCourseId;

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromQuery] int? addToCourseId,
            [FromForm] CreateSkillViewModel skillModel)
        {
            if(!this.ModelState.IsValid)
            {
                return this.View(skillModel);
            }

            var skillDTO = new SkillDTO()
            {
                Title = skillModel.Title,
                Description = skillModel.Description,
                MaxValue = skillModel.MaxValue
            };

            var serviceResult = await this.skillService.CreateSkillAsync(skillDTO);

            if(!serviceResult.IsSuccessful)
            {
                this.ModelState.AddModelError(
                    string.Empty,
                    this.blMessageParser[serviceResult.MessageCode]);

                return this.View(skillModel);
            }
            else
            {
                if (addToCourseId.HasValue)
                {
                    return RedirectToAction(
                        "AddSkill", 
                        "Course", 
                        new { courseId = addToCourseId.Value, 
                            skillId = serviceResult.Result });
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromQuery] int skillId)
        {
            var getSkillServiceResult = await this.skillService.GetSkillAsync(skillId);

            if (!getSkillServiceResult.IsSuccessful)
            {
                this.ModelState.AddModelError(
                    string.Empty,
                    this.blMessageParser[getSkillServiceResult.MessageCode]);

                return this.View();
            }


            var skillVM = new EditSkillViewModel()
            {
                Description = getSkillServiceResult.Result.Description,
                Id = getSkillServiceResult.Result.Id,
                MaxValue = getSkillServiceResult.Result.MaxValue,
                Title = getSkillServiceResult.Result.Title
            };

            return this.View(skillVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] EditSkillViewModel editSkillModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var skillDTO = new SkillDTO()
            {
                Description = editSkillModel.Description,
                Id = editSkillModel.Id,
                MaxValue = editSkillModel.MaxValue,
                Title = editSkillModel.Title
            };

            var serviceResult = await this.skillService.UpdateSkillAsync(skillDTO);

            if(!serviceResult.IsSuccessful)
            {
                this.ModelState.AddModelError(
                    string.Empty,
                    this.blMessageParser[serviceResult.MessageCode]);

                return this.View(editSkillModel);
            }
            else
            {
                return this.RedirectToAction("Index", "Home");
            }    
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromQuery] int skillId)
        {
            await this.skillService.DeleteSkillAsync(skillId);

            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Show([FromQuery] int skillId)
        {
            var skillDTO = await this.skillService.GetSkillAsync(skillId);

            return this.View(skillDTO.Result);
        }

        [HttpGet]
        public async Task<IActionResult> ShowPage(
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize,
            [FromQuery] int? addToCourseId)
        {
            var pageInfo = new PageInfoDTO()
            {
                PageNumber = pageNumber ?? defaultPageNumber,
                PageSize = pageSize ?? defaultPageSize
            };

            var skillPageServiceResult = await this.skillService.GetSkillPageAsync(pageInfo);

            this.ViewBag.addToCourseId = addToCourseId;

            return this.View(skillPageServiceResult.Result);
        }

        [HttpGet]
        public async Task<IActionResult> AddToCourse([FromQuery] int courseId)
        {
            return this.RedirectToAction("showPage", new { addToCourseId = courseId});
        }
    }
}
