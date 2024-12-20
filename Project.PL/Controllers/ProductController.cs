using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Project.BLL.Interfaces;
using Project.DAL.Models;
using Project.PL.Helper;
using Project.PL.ViewModels;

namespace Project.PL.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IMapper mapper,IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(string searchinput)
        {
            var products = Enumerable.Empty<Product>();
            if (string.IsNullOrEmpty(searchinput))
                products = _unitOfWork.ProductRepository.GetAll();

            else
                products = await _unitOfWork.ProductRepository.GetByName(searchinput.ToLower());

            var result = _mapper.Map<IEnumerable<ProductViewModel>>(products);
            return View(result);
        }

        //[HttpGet]
        //public IActionResult Create()
        //{
        //    ViewData["Customers"] = _unitOfWork.CustomerRepository.GetAll();
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(ProductViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var Product = _mapper.Map<Product>(model);
        //        _unitOfWork.ProductRepository.Add(Product);

        //        var customer = _unitOfWork.CustomerRepository.Get(Product.CustomerId.Value);
        //        customer.Salary = customer.Salary + ((int)Product.price * Product.Quantity);
        //        _unitOfWork.CustomerRepository.Update(customer);


        //        var count = _unitOfWork.Complete();
        //        if (count > 0)
        //            return RedirectToAction(nameof(Index));
        //    }
        //    return View();
        //}


        //public IActionResult Details(int? id, string viewname = "Details")
        //{
        //    ViewData["Customers"] = _unitOfWork.CustomerRepository.GetAll();

        //    if (id is null)
        //        return BadRequest();

        //    var product = _unitOfWork.ProductRepository.Get(id.Value);

        //    var ProductViewModel = _mapper.Map<ProductViewModel>(product);


        //    if (product is null)
        //        return NotFound();
        //    return View(viewname, ProductViewModel);
        //}

        //[HttpGet]
        //public IActionResult Edit(int? id)
        //{
        //    ViewData["Customers"] = _unitOfWork.CustomerRepository.GetAll();
        //    return Details(id, "Edit");
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit([FromRoute] int? id, ProductViewModel model) //to prevent any edit from anyone
        //{
        //    if (id != model.Id)
        //        return BadRequest();


        //    var product = _mapper.Map<Product>(model);


        //    if (ModelState.IsValid) // to can use data validation
        //    {
        //        var old = _unitOfWork.ProductRepository.Get(id.Value);

        //        old.price = model.price;
        //        old.priceToCST = model.priceToCST;
        //        old.CustomerId = model.CustomerId;
        //        old.Colors = model.Colors;
        //        old.description = model.description;
        //        old.name = model.name;
        //        old.Quantity = model.Quantity;
        //        old.Sizes = model.Sizes;
        //        old.Incomes=model.Incomes;

        //        var customers = _unitOfWork.CustomerRepository.GetAll();
        //        var incomes = _unitOfWork.IncomesRepository.GetAll();

        //            foreach (var cus in customers)
        //            {
        //                        cus.Salary =_unitOfWork.ProductRepository.GetAll().Where(p => p.Customer.Name == cus.Name)
        //                                    .Sum(p => p.Quantity * (int)p.price) -
        //                                    _unitOfWork.IncomesRepository.GetAll().Where(I=>I.Customer.Name ==cus.Name)
        //                                    .Sum(I=>I.IncomeAmount);
        //            _unitOfWork.CustomerRepository.Update(cus);
        //            }

        //        var count = _unitOfWork.Complete();

        //        if (count > 0)
        //        {
        //            return RedirectToAction("Index");
        //        }
        //    }
        //    return View();
        //}


        //[HttpGet]
        //public IActionResult Delete(int? id)
        //{
        //    return Details(id, "Delete");
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Delete([FromRoute] int? id, ProductViewModel model) //to prevent any edit from anyone
        //{
        //    ViewData["Customers"] = _unitOfWork.CustomerRepository.GetAll();

        //    if (id != model.Id)
        //        return BadRequest();

        //    var product = _mapper.Map<Product>(model);

        //    if (ModelState.IsValid) // to can use data validation
        //    {
        //        var customer = _unitOfWork.CustomerRepository.Get(product.CustomerId.Value);
        //        customer.Salary = customer.Salary - ((int)product.price * product.Quantity);
        //        _unitOfWork.CustomerRepository.Update(customer);


        //        _unitOfWork.ProductRepository.Delete(product);
        //        var count = _unitOfWork.Complete();
        //        if (count > 0)
        //            return RedirectToAction("Index");
        //    }
        //    return View();
        //}

    }
}
