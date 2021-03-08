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
    public class SkillController : Controller
    {
        private ISkillService skillService;

        private IServiceResultParser blMessageParser;

        public SkillController(
            ISkillService skillService,
            IServiceResultParser blServiceResultMessageParser)
        {
            this.skillService = skillService;

            this.blMessageParser = blServiceResultMessageParser;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateSkillViewModel skillModel)
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
                return RedirectToAction("Index", "Home");
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
    }
}
