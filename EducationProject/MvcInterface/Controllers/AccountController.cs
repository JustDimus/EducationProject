using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationProject.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MvcInterface.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IAccountService accountService;

        public AccountController(
            IAccountService accountService)
        {
            this.accountService = accountService;

        }

        [HttpGet]
        public async Task<IActionResult> LogIn()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn()
        {

        }

    }
}
