using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EducationProject.BLL.DTO;
using EducationProject.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcInterface.Models;

namespace MvcInterface.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private ICourseService courseService;

        private IMaterialService materialService;

        public HomeController(
            ILogger<HomeController> logger,
            ICourseService courseService,
            IMaterialService materialService)
        {
            _logger = logger;

            this.courseService = courseService;

            this.materialService = materialService;
        }

        public async Task<IActionResult> Index()
        {
            var defaultCoursePage = new PageInfoDTO()
            {
                PageNumber = 0,
                PageSize = 9
            };

            var defaultMaterialPage = new PageInfoDTO()
            {
                PageNumber = 0,
                PageSize = 15
            };

            var courses = await this.courseService.GetCoursePageAsync(defaultCoursePage);

            var materials = await this.materialService.GetMaterialPageAsync(defaultMaterialPage);

            this.ViewBag.materials = materials.Result.Entities;

            return View(courses.Result.Entities.ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
