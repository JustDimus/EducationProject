using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EducationProject.BLL.DTO;
using EducationProject.BLL.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcInterface.Models.Models;
using MvcInterface.ServiceResultController.Interfaces;

namespace MvcInterface.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IAccountService accountService;

        private IServiceResultParser blMessageParser;

        public AccountController(
            IAccountService accountService,
            IServiceResultParser blServiceResultMessageParser)
        {
            this.accountService = accountService;

            this.blMessageParser = blServiceResultMessageParser;

        }

        [HttpGet]
        public async Task<IActionResult> PassCourse([FromQuery] int courseId)
        {
            var accountCourseDTO = new ChangeAccountCourseDTO()
            {
                CourseId = courseId,
                Status = EducationProject.Core.Models.Enums.ProgressStatus.Passed
            };

            var serviceResult = await this.accountService.ChangeAccountCourseStatusAsync(accountCourseDTO);

            if (!serviceResult.IsSuccessful)
            {
                this.ModelState.AddModelError(
                    string.Empty,
                    this.blMessageParser[serviceResult.MessageCode]);
            }

            return this.RedirectToAction("Show", "Course", new { courseId = courseId });
        }

        [HttpGet]
        public async Task<IActionResult> PassMaterial([FromQuery] int materialId)
        {
            var serviceResult = await this.accountService.AddAccountMaterialAsync(materialId);

            if(!serviceResult.IsSuccessful)
            {
                this.ModelState.AddModelError(
                    string.Empty,
                    this.blMessageParser[serviceResult.MessageCode]);
            }

            return this.RedirectToAction("Show", "Material", new { materialId = materialId });
        }

        [HttpGet]
        public async Task<IActionResult> AddCourse([FromQuery] int courseId)
        {
            var accountCourseDTO = new ChangeAccountCourseDTO()
            {
                CourseId = courseId
            };

            var serviceResult = await this.accountService.AddAccountCourseAsync(accountCourseDTO);

            return this.RedirectToAction("Show", "Course", new { courseId = courseId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var accountInfo = await this.accountService.GetAccountInfoAsync();

            if (!accountInfo.IsSuccessful)
            {
                this.ModelState.AddModelError(
                    string.Empty,
                    this.blMessageParser[accountInfo.MessageCode]);

                return this.View();
            }

            var accountViewModel = new EditAccountInfoViewModel()
            {
                Email = accountInfo.Result.Email,
                FirstName = accountInfo.Result.FirstName,
                Id = accountInfo.Result.Id,
                PhoneNumber = accountInfo.Result.PhoneNumber,
                SecondName = accountInfo.Result.SecondName
            };

            return this.View(accountViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete()
        {
            await this.accountService.DeleteAccountAsync();

            return this.RedirectToAction("LogOut");
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] EditAccountInfoViewModel accountModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(accountModel);
            }

            var accountInfoDTO = new AccountInfoDTO()
            {
                Email = accountModel.Email,
                FirstName = accountModel.FirstName,
                Id = accountModel.Id,
                PhoneNumber = accountModel.PhoneNumber,
                SecondName = accountModel.SecondName
            };

            var updateResult = await this.accountService.UpdateAccountAsync(accountInfoDTO);

            if (!updateResult.IsSuccessful)
            {
                this.ModelState.AddModelError(
                    string.Empty,
                    this.blMessageParser[updateResult.MessageCode]);

                return this.View(accountModel);
            }
            else
            {
                return this.RedirectToAction("Show");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel registerModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(registerModel);
            }

            var registerDTO = new RegisterDTO()
            {
                Email = registerModel.Email,
                Password = registerModel.Password
            };

            var registerResult = await this.accountService.TryRegisterAsync(registerDTO);

            if (!registerResult.IsSuccessful)
            {
                this.ModelState.AddModelError(
                    string.Empty, 
                    this.blMessageParser[registerResult.MessageCode]);

                return this.View(registerModel);
            }
            else
            {
                await this.AuthenticateAsync(
                    registerModel.Email, 
                    registerResult.Result);

                return this.RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult LogIn()
        {
            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn([FromForm] LogInViewModel logInModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(logInModel);
            }

            var logInDTO = new LogInDTO()
            {
                Email = logInModel.Email,
                Password = logInModel.Password
            };

            var logInResult = await this.accountService.TryLogInAsync(logInDTO);

            if (!logInResult.IsSuccessful)
            {
                this.ModelState.AddModelError(
                    string.Empty, 
                    blMessageParser[logInResult.MessageCode]);

                return this.View(logInModel);
            }
            else
            {
                await this.AuthenticateAsync(
                    logInModel.Email, 
                    logInResult.Result);

                return this.RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Show(
            [FromQuery] int? skillPageNumber,
            [FromQuery] int? skillPageSize,
            [FromQuery] int? coursePageNumber,
            [FromQuery] int? coursePageSize)
        {
            var coursePageInfo = new PageInfoDTO()
            {
                PageNumber = coursePageNumber ?? 0,
                PageSize = coursePageSize ?? 10
            };

            var skillPageInfo = new PageInfoDTO()
            {
                PageNumber = skillPageNumber ?? 0,
                PageSize = skillPageSize ?? 10
            };

            var accountInfoServiceResult = await this.accountService.GetAccountFullInfoAsync(
                coursePageInfo,
                skillPageInfo);

            if (!accountInfoServiceResult.IsSuccessful)
            {
                return this.RedirectToAction("Index", "Home");
            }

            return this.View(accountInfoServiceResult.Result);
        }

        private async Task AuthenticateAsync(string email, int accountId)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.NameIdentifier, accountId.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await this.HttpContext.SignInAsync(principal);
        }
    }
}
