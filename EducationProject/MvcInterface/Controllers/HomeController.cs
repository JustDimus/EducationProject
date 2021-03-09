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

        public HomeController(
            ILogger<HomeController> logger,
            ICourseService courseService)
        {
            _logger = logger;

            this.courseService = courseService;
        }

        public IActionResult Index()
        {
            var defaultCoursePage = new PageInfoDTO()
            {
                PageNumber = 0,
                PageSize = 9
            };

            var courses = this.courseService.GetCoursePageAsync(defaultCoursePage);

            this.ViewBag.defaultCourses = courses;

            return View();
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
