using Azure;
using ClothBazar.Entities.Models;
using ClothBazar.Entities.ViewModels;
using ClothBazar.Services.Repository;
using ClothBazar.Services.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.Common;
using System.Security.Claims;

namespace ClothBazar.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartViewModels ShoppingCartViewModels { get; set; }
        public ShoppingCartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [Authorize]
        public async Task<IActionResult> Index()
        {
            try
            {
                var claimIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                ShoppingCartViewModels = new()
                {
                    ListShoppingCart = await _unitOfWork.ShoppingCartRepository.GetAllAsync(x => x.ApplicationUserId == userId, 
                    includeProperties: "Product")
                };
                foreach (var cart in ShoppingCartViewModels.ListShoppingCart)
                {
                    ShoppingCartViewModels.OrderTotal += (cart.Product.Price * cart.Count);
                }
                return View(ShoppingCartViewModels);
            }
            catch (Exception ex)
            {
                return View(new ShoppingCartViewModels());
            }
        }

        public async Task<IActionResult> Plus(int cartId)
        {
            var cartDb = await _unitOfWork.ShoppingCartRepository.GetAsync(x => x.Id == cartId);
            cartDb.Count += 1;
            await _unitOfWork.ShoppingCartRepository.UpdateAsync(cartDb);
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Minus(int cartId)
        {
            var cartDb = await _unitOfWork.ShoppingCartRepository.GetAsync(x => x.Id == cartId);
            if (cartDb.Count <= 1)
            {
                await _unitOfWork.ShoppingCartRepository.DeleteAsync(cartDb);
            }
            else
            {
                cartDb.Count -= 1;
                await _unitOfWork.ShoppingCartRepository.UpdateAsync(cartDb);
            }
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Remove(int cartId)
        {
            var cartDb = await _unitOfWork.ShoppingCartRepository.GetAsync(x => x.Id == cartId);
            await _unitOfWork.ShoppingCartRepository.DeleteAsync(cartDb);
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ShoppingCart shoppingCart)
        {
            if (ModelState.IsValid)
            {
                if (shoppingCart != null)
                {
                    await _unitOfWork.ShoppingCartRepository.AddAsync(shoppingCart);
                    await _unitOfWork.SaveAsync();
                    TempData["success"] = "ShoppingCart created successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = "Error encountered.";
                    return View(shoppingCart);
                }
            }
            TempData["error"] = "Error encountered.";
            return View(shoppingCart);
        }

        

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            
            if (id != 0)
            {
                var shoppingCart = await _unitOfWork.ShoppingCartRepository.GetAsync(x => x.Id == id, false);
                return View(shoppingCart);
            }
            TempData["error"] = "Error encountered.";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ShoppingCart shoppingCart)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ShoppingCart dbPoduct = await _unitOfWork.ShoppingCartRepository.GetAsync(x => x.Id == shoppingCart.Id, false);
                    if (dbPoduct != null)
                    {
                        await _unitOfWork.ShoppingCartRepository.UpdateAsync(dbPoduct);
                        await _unitOfWork.SaveAsync();
                        TempData["success"] = "ShoppingCart updated successfully";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["error"] = "Error encountered.";
                        return View(shoppingCart);
                    }
                }
            }
            catch (Exception)
            {
                TempData["error"] = "Error encountered.";
                return View(shoppingCart);
            }
            TempData["error"] = "Error encountered.";
            return View(shoppingCart);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {

            if (id != 0)
            {
                var shoppingCart = await _unitOfWork.ShoppingCartRepository.GetAsync(x => x.Id == id, false);
                return View(shoppingCart);
            }
            TempData["error"] = "Error encountered.";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ShoppingCart shoppingCart)
        {
            try
            {
                ShoppingCart dbPoduct = await _unitOfWork.ShoppingCartRepository.GetAsync(x => x.Id == shoppingCart.Id);
                if (dbPoduct != null)
                {
                    await _unitOfWork.ShoppingCartRepository.DeleteAsync(dbPoduct);
                    await _unitOfWork.SaveAsync();
                    TempData["success"] = "ShoppingCart deleted successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = "Error encountered.";
                    return View(shoppingCart);
                }
            }
            catch (Exception)
            {
                TempData["error"] = "Error encountered.";
                return View(shoppingCart);
            }
            TempData["error"] = "Error encountered.";
            return View(shoppingCart);
        }
    }
}
