using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ClothBazar.Entities.Models;
using ClothBazar.Services.Repository.IRepository;
using ClothBazar.Web.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ClothBazar.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
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
        public async Task<IActionResult> Details(int productId)
        {
            ShoppingCart cart = new()
            {
                Product = await _unitOfWork.ProductRepository.GetAsync(x => x.Id == productId),
                Count = 1,
                ProductId = productId
            };
            return View(cart);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Details(ShoppingCart shoppingCart)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;

            ShoppingCart cartdb = await _unitOfWork.ShoppingCartRepository.GetAsync(x => x.ApplicationUserId == userId && x.ProductId == shoppingCart.ProductId);
            if (cartdb !=null)
            {
                // cart exists.
                cartdb.Count += shoppingCart.Count;
                await _unitOfWork.ShoppingCartRepository.UpdateAsync(cartdb);
            }
            else
            {
                // create new
                await _unitOfWork.ShoppingCartRepository.AddAsync(shoppingCart);
            }
            await _unitOfWork.SaveAsync();

            TempData["success"] = "Cart updated successfully";
            return RedirectToAction(nameof(Index));
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