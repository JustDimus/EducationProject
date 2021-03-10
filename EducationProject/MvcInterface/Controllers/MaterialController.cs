using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationProject.BLL.DTO;
using EducationProject.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using MvcInterface.Models.Models;
using MvcInterface.ServiceResultController.Interfaces;

namespace MvcInterface.Controllers
{
    [Authorize]
    public class MaterialController : Controller
    {
        private IMaterialService materialService;

        private IServiceResultParser blMessageParser;

        private Dictionary<string, int> materialTypes = new Dictionary<string, int>();

        private int defaultPageNumber;

        private int defaultPageSize;

        public MaterialController(
            IMaterialService materialService,
            IServiceResultParser blServiceResultMessageParser,
            IConfiguration configuration)
        {
            this.materialService = materialService;

            this.blMessageParser = blServiceResultMessageParser;

            this.materialTypes.Add("Video", 0);
            this.materialTypes.Add("Book", 1);
            this.materialTypes.Add("Article", 2);

            this.defaultPageNumber = int.Parse(configuration["DefaultControllerValues:DefaultPageNumber"]);

            this.defaultPageSize = int.Parse(configuration["DefaultControllerValues:DefaultPageSize"]);
        }

        [HttpGet]
        public IActionResult Create(
            [FromQuery] int? addToCourseId,
            [FromQuery] string materialType = "Article")
        {
            this.materialTypes.TryGetValue(materialType, out int typeId);

            this.ViewBag.MaterialType = typeId;

            this.ViewBag.addToCourseId = addToCourseId;

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromQuery] int? addToCourseId,
            [FromForm] CreateMaterialViewModel materialModel)
        {
            this.materialTypes.TryGetValue(materialModel.Type, out int typeId);

            if (!ModelState.IsValid)
            {
                this.ViewBag.MaterialType = typeId;

                return this.View();
            }

            MaterialDTO materialToCreate = this.GenerateMaterial(materialModel, typeId);

            if (materialToCreate == null)
            {
                this.ModelState.AddModelError(
                    string.Empty,
                    "Невозможно создать объект");

                return this.View(materialModel);
            }

            var createMaterialServiceResult = await this.materialService.CreateMaterialAsync(materialToCreate);

            if (!createMaterialServiceResult.IsSuccessful)
            {
                this.ModelState.AddModelError(
                    string.Empty,
                    this.blMessageParser[createMaterialServiceResult.MessageCode]);

                return this.View(materialModel);
            }
            else
            {
                if (addToCourseId.HasValue)
                {
                    return this.RedirectToAction(
                        "AddMaterial",
                        "Course",
                        new
                        {
                            materialId = createMaterialServiceResult.Result,
                            courseId = addToCourseId.Value
                        });
                }

                return this.RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromQuery] int materialId)
        {
            var materialDTO = await this.materialService.GetMaterialAsync(materialId);

            if(!materialDTO.IsSuccessful)
            {
                this.ModelState.AddModelError(
                    string.Empty,
                    this.blMessageParser[materialDTO.MessageCode]);

                return this.View();
            }

            var materialVM = this.CreateViewModelMaterial(materialDTO.Result);

            return this.View(materialVM);
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] EditMaterialViewModel materialModel)
        {
            this.materialTypes.TryGetValue(materialModel.Type, out int typeId);

            if (!this.ModelState.IsValid)
            {
                this.ViewBag.MaterialType = typeId;

                return this.View(materialModel);
            }

            MaterialDTO materialToCreate = this.GenerateMaterial(materialModel, typeId);

            if (materialToCreate == null)
            {
                this.ModelState.AddModelError(
                    string.Empty,
                    "Невозможно создать объект");

                return this.View(materialModel);
            }

            var updateMaterialServiceResult = await this.materialService.UpdateMaterialAsync(materialToCreate);

            if(!updateMaterialServiceResult.IsSuccessful)
            {
                this.ModelState.AddModelError(
                    string.Empty,
                    this.blMessageParser[updateMaterialServiceResult.MessageCode]);

                return this.View(materialModel);
            }
            else
            {
                return this.RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromQuery] int materialId)
        {
            await this.materialService.DeleteMaterialAsync(materialId);

            return this.RedirectToAction("ShowPage");
        }

        [HttpGet]
        public async Task<IActionResult> Show([FromQuery] int materialId)
        {
            var getMaterialResult = await this.materialService.GetMaterialAsync(materialId);

            if (!getMaterialResult.IsSuccessful)
            {
                this.ModelState.AddModelError(
                    string.Empty,
                    this.blMessageParser[getMaterialResult.MessageCode]);

                return this.View();
            }

            return this.View(this.CreateViewModelMaterial(getMaterialResult.Result));
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
                PageSize = pageNumber ?? defaultPageSize
            };

            var materialPageServiceResult = await this.materialService.GetMaterialPageAsync(pageInfo);

            this.ViewBag.addToCourseId = addToCourseId;

            return this.View(materialPageServiceResult.Result);
        }

        [HttpGet]
        public IActionResult AddToCourse([FromQuery] int courseId)
        {
            return this.RedirectToAction("ShowPage", new { addToCourseId = courseId });
        }

        private MaterialDTO GenerateMaterial(CreateMaterialViewModel materialModel, int typeId)
        {
            switch (typeId)
            {
                case 0:
                    return new VideoMaterialDTO()
                    {
                        Title = materialModel.Title,
                        Description = materialModel.Description,
                        Duration = materialModel.Duration,
                        Quality = materialModel.Quality,
                        Type = EducationProject.Core.Models.Enums.MaterialType.VideoMaterial,
                        URI = materialModel.URI
                    };
                case 1:
                    return new BookMaterialDTO()
                    {
                        Title = materialModel.Title,
                        Description = materialModel.Description,
                        Author = materialModel.Author,
                        Type = EducationProject.Core.Models.Enums.MaterialType.BookMaterial,
                        Pages = materialModel.Pages
                    };
                case 2:
                    return new ArticleMaterialDTO()
                    {
                        Title = materialModel.Title,
                        Description = materialModel.Description,
                        PublicationDate = materialModel.PublicationDate,
                        URI = materialModel.URI,
                        Type = EducationProject.Core.Models.Enums.MaterialType.ArticleMaterial,
                    };
                default:
                    return null;
            }
        }

        private MaterialDTO GenerateMaterial(EditMaterialViewModel materialModel, int typeId)
        {
            switch (typeId)
            {
                case 0:
                    return new VideoMaterialDTO()
                    {
                        Id = materialModel.Id,
                        Title = materialModel.Title,
                        Description = materialModel.Description,
                        Duration = materialModel.Duration,
                        Quality = materialModel.Quality,
                        Type = EducationProject.Core.Models.Enums.MaterialType.VideoMaterial,
                        URI = materialModel.URI
                    };
                case 1:
                    return new BookMaterialDTO()
                    {
                        Id = materialModel.Id,
                        Title = materialModel.Title,
                        Description = materialModel.Description,
                        Author = materialModel.Author,
                        Type = EducationProject.Core.Models.Enums.MaterialType.BookMaterial,
                        Pages = materialModel.Pages
                    };
                case 2:
                    return new ArticleMaterialDTO()
                    {
                        Id = materialModel.Id,
                        Title = materialModel.Title,
                        Description = materialModel.Description,
                        PublicationDate = materialModel.PublicationDate,
                        URI = materialModel.URI,
                        Type = EducationProject.Core.Models.Enums.MaterialType.ArticleMaterial,
                    };
                default:
                    return null;
            }
        }

        private EditMaterialViewModel CreateViewModelMaterial(MaterialDTO material)
        {
            switch(material)
            {
                case ArticleMaterialDTO article:
                    return new EditMaterialViewModel()
                    {
                        Id = article.Id,
                        URI = article.URI,
                        Description = article.Description,
                        PublicationDate = article.PublicationDate,
                        Type = "Article",
                        Title = article.Title,
                        IsAccountPassed = article.IsAccountPassed
                    };
                case BookMaterialDTO book:
                    return new EditMaterialViewModel()
                    {
                        Id = book.Id,
                        Description = book.Description,
                        Author = book.Author,
                        Type = "Book",
                        Title = book.Title,
                        Pages = book.Pages,
                        IsAccountPassed = book.IsAccountPassed
                    };
                case VideoMaterialDTO video:
                    return new EditMaterialViewModel()
                    {
                        Id = video.Id,
                        Description = video.Description,
                        Type = "Video",
                        Title = video.Title,
                        URI = video.URI,
                        Duration = video.Duration,
                        Quality = video.Quality,
                        IsAccountPassed = video.IsAccountPassed
                    };
                default:
                    return null;
            }
        }
    }
}
