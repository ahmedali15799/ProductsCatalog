using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Project.BLL.Interfaces;
using Project.DAL.Models;
using Project.PL.Helper;
using Project.PL.Models;
using Project.PL.ViewModels;

namespace Project.PL.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHostEnvironment _env;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;


        public HomeController(IMapper mapper,IUnitOfWork unitOfWork, IHostEnvironment env
                              , UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _env = env;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string searchinput,int? id)
        {
            ViewData["Categories"] = _unitOfWork.CategoryRepository.GetAll();

            var products = Enumerable.Empty<Product>();
            if (id is null)
            {
                if (string.IsNullOrEmpty(searchinput))
                    products = _unitOfWork.ProductRepository.GetAll();

                else
                    products = await _unitOfWork.ProductRepository.GetByName(searchinput.ToLower());
            }
            else
                products = _unitOfWork.ProductRepository.GetAll().Where(P=>P.CategoryId==id);

            var result = _mapper.Map<IEnumerable<ProductViewModel>>(products);

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Categories"] = _unitOfWork.CategoryRepository.GetAll();

            return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                model.ImageName = DocumentSettings.UploadFile(model.Image, "Images");
                

                var product = new Product
                {
                    Name = model.Name,
                    CategoryId = model.CategoryId,
                    Price = model.Price,
                    StartDate = model.StartDate,
                    Durationdays = model.DurationDays,
                    CreationDate = DateTime.Now, 
                    CreatedByUserId = user?.Id,
                    ImageName=model.ImageName
                };

                _unitOfWork.ProductRepository.Add(product);

                var count = _unitOfWork.Complete();
                if (count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View();
        }





        [HttpGet]
        public IActionResult Details(int? id, string viewname = "Details")
        {

            if (!id.HasValue)
                return BadRequest();

            var product = _unitOfWork.ProductRepository.Get(id.Value);

            if (product is null)
                return NotFound();

            var productViewModel = _mapper.Map<ProductViewModel>(product);

            if (viewname.Equals("Delete", StringComparison.OrdinalIgnoreCase))
                TempData["ImageName"] = productViewModel.ImageName;


            return View(viewname, productViewModel);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ViewData["Categories"] = _unitOfWork.CategoryRepository.GetAll();
            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int? id, ProductViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            model.ImageName = DocumentSettings.UploadFile(model.Image, "Images");

            var product = _mapper.Map<Product>(model);

            if (!ModelState.IsValid)
                return View(product);


            try
            {
                _unitOfWork.ProductRepository.Update(product);
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }


            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error has Occured during Updating the Employee");

                return View(model);
            }
        }


        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int? id, ProductViewModel model) //to prevent any edit from anyone
        {

            model.ImageName = TempData["ImageName"] as string;

            var product = _mapper.Map<Product>(model);

            try
            {
                _unitOfWork.ProductRepository.Delete(product);
                var count = _unitOfWork.Complete();
                if (count > 0)
                {
                    DocumentSettings.DeleteFile(product.ImageName, "Images");
                    return RedirectToAction(nameof(Index));
                }
                return View(model);

            }
            catch (Exception ex)
            {

                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error has Occured during Updating the Employee");

                return View(model);
            }
        }


    }
}
