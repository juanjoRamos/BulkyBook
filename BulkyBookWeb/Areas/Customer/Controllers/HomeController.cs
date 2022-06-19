using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<ProductModel> products = _unitOfWork.productRepository.GetAll(includeProperties: "Category,CoverType");
            return View(products);
        }

        public IActionResult Details(int id)
        {
            IEnumerable<ProductModel> products = _unitOfWork.productRepository.GetAll(includeProperties: "Category,CoverType");

            ShoppingCart cartObj = new()
            {
                Count = 1,
                Product = products.FirstOrDefault(product => product.Id == id) ?? new ProductModel()
            };
            return View(cartObj);
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