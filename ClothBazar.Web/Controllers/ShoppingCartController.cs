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
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var claimIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                // Retrieve shopping cart items for the logged-in user
                var shoppingCartItems = await _unitOfWork.ShoppingCartRepository.GetAllAsync(x => x.ApplicationUserId == userId, includeProperties: "Product");

                // Calculate total order value
                decimal totalOrderValue = shoppingCartItems.Sum(x => x.Product.Price * x.Count);

                var model = new ShoppingCartViewModels
                {
                    ListShoppingCart = shoppingCartItems,
                    OrderTotal = totalOrderValue,
                    Coupon = new Coupon() // Initialize an empty list for coupons
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while fetching your cart.";
                return View(new ShoppingCartViewModels());
            }
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Index(ShoppingCartViewModels model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Coupon.Code))
                {
                    TempData["ErrorMessage"] = "No valid coupon codes provided.";
                    return View(model);
                }

                string validationMessage = await _unitOfWork.ShoppingCartRepository.ValidateCouponAsync(model.Coupon.Code);
                if (validationMessage != "Coupon is valid.")
                {
                    TempData["ErrorMessage"] = validationMessage;
                    return RedirectToAction("Index");
                }

                var claimIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                // Retrieve shopping cart items for the logged-in user
                var shoppingCartItems = await _unitOfWork.ShoppingCartRepository.GetAllAsync(x => x.ApplicationUserId == userId, includeProperties: "Product");

                // Calculate total order value
                decimal totalOrderValue = shoppingCartItems.Sum(x => x.Product.Price * x.Count);

                model.ListShoppingCart = shoppingCartItems;
                model.OrderTotal = totalOrderValue;

                // Apply the discount based on coupon type
                decimal discount = await _unitOfWork.ShoppingCartRepository.ApplyCouponAsync(model.Coupon.Code, model);
                model.OrderTotal -= discount;
                model.Coupon.DiscountValue = discount;

                return View(model);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while applying the coupon.";
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

    }
}
