using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationProject.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcInterface.Models.Models;

namespace MvcInterface.Controllers
{
    [Authorize]
    public class MaterialController : Controller
    {
        private IMaterialService materialService;

        private Dictionary<string, int> materialTypes = new Dictionary<string, int>();

        public MaterialController(
            IMaterialService materialService)
        {
            this.materialService = materialService;

            this.materialTypes.Add("Video", 0);
            this.materialTypes.Add("Book", 1);
            this.materialTypes.Add("Article", 2);
        }

        [HttpGet]
        public IActionResult Create([FromQuery] string materialType = "Article")
        {
            this.materialTypes.TryGetValue(materialType, out int typeId);

            this.ViewBag.MaterialType = typeId;

            return this.View($"Create");
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateMaterialViewModel materialModel)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromQuery] int materialId)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] EditMaterialViewModel materialModel)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromQuery] int materialId)
        {
            throw new NotImplementedException();
        }
    }
}
