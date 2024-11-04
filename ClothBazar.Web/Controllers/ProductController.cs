using Azure;
using ClothBazar.Entities.Models;
using ClothBazar.Services.Repository;
using ClothBazar.Services.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.Common;

namespace ClothBazar.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<IActionResult> Index()
        {
            try
            {
                IEnumerable<Product> products = await _unitOfWork.ProductRepository.GetAllAsync();
                if (products == null)
                {
                    return View(new Product());
                }
                return View(products);
            }
            catch (Exception ex)
            {
                return View(new Product());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                if (product != null)
                {
                    await _unitOfWork.ProductRepository.AddAsync(product);
                    await _unitOfWork.SaveAsync();
                    TempData["success"] = "Product created successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = "Error encountered.";
                    return View(product);
                }
            }
            TempData["error"] = "Error encountered.";
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            
            if (id != 0)
            {
                var product = await _unitOfWork.ProductRepository.GetAsync(x => x.Id == id, false);
                return View(product);
            }
            TempData["error"] = "Error encountered.";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Product dbPoduct = await _unitOfWork.ProductRepository.GetAsync(x => x.Id == product.Id, false);
                    if (dbPoduct != null)
                    {
                        dbPoduct.Name = product.Name;
                        dbPoduct.Description = product.Description;
                        dbPoduct.Price = product.Price;
                        await _unitOfWork.ProductRepository.UpdateAsync(dbPoduct);
                        await _unitOfWork.SaveAsync();
                        TempData["success"] = "Product updated successfully";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["error"] = "Error encountered.";
                        return View(product);
                    }
                }
            }
            catch (Exception)
            {
                TempData["error"] = "Error encountered.";
                return View(product);
            }
            TempData["error"] = "Error encountered.";
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {

            if (id != 0)
            {
                var product = await _unitOfWork.ProductRepository.GetAsync(x => x.Id == id, false);
                return View(product);
            }
            TempData["error"] = "Error encountered.";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Product product)
        {
            try
            {
                Product dbPoduct = await _unitOfWork.ProductRepository.GetAsync(x => x.Id == product.Id);
                if (dbPoduct != null)
                {
                    await _unitOfWork.ProductRepository.DeleteAsync(dbPoduct);
                    await _unitOfWork.SaveAsync();
                    TempData["success"] = "Product deleted successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = "Error encountered.";
                    return View(product);
                }
            }
            catch (Exception)
            {
                TempData["error"] = "Error encountered.";
                return View(product);
            }
            TempData["error"] = "Error encountered.";
            return View(product);
        }
    }
}
