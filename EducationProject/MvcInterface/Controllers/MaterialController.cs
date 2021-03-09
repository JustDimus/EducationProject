using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationProject.BLL.DTO;
using EducationProject.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public MaterialController(
            IMaterialService materialService,
            IServiceResultParser blServiceResultMessageParser)
        {
            this.materialService = materialService;

            this.blMessageParser = blServiceResultMessageParser;

            this.materialTypes.Add("Video", 0);
            this.materialTypes.Add("Book", 1);
            this.materialTypes.Add("Article", 2);
        }

        [HttpGet]
        public IActionResult Create([FromQuery] string materialType = "Article")
        {
            this.materialTypes.TryGetValue(materialType, out int typeId);

            this.ViewBag.MaterialType = typeId;

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateMaterialViewModel materialModel)
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

            return this.RedirectToAction("Index", "Home");
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
                        Title = article.Title
                    };
                case BookMaterialDTO book:
                    return new EditMaterialViewModel()
                    {
                        Id = book.Id,
                        Description = book.Description,
                        Author = book.Author,
                        Type = "Book",
                        Title = book.Title,
                        Pages = book.Pages
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
                        Quality = video.Quality
                    };
                default:
                    return null;
            }
        }
    }
}
