using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ClothBazar.Entities.Models;
using ClothBazar.Services.Repository.IRepository;
using ClothBazar.Web.Models;

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