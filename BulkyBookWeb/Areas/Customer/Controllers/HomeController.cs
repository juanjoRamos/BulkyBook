using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
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

        public IActionResult Details(int productId)
        {
            IEnumerable<ProductModel> products = _unitOfWork.productRepository.GetAll(includeProperties: "Category,CoverType");

            ShoppingCart cartObj = new()
            {
                Count = 1,
                ProductId = productId,
                Product = products.FirstOrDefault(product => product.Id == productId) ?? new ProductModel()
            };
            return View(cartObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimIdentity = (ClaimsIdentity) User.Identity;
            var nameIdentifier = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = nameIdentifier.Value;


            ShoppingCart ExistShopping = _unitOfWork.shoppingCartRepository
                .GetFirstOrDefault(unit => unit.ApplicationUserId == nameIdentifier.Value && unit.ProductId == shoppingCart.ProductId);

            if (ExistShopping == null)
            {
                _unitOfWork.shoppingCartRepository.Add(shoppingCart);
            }
            else 
            {
                _unitOfWork.shoppingCartRepository.IncrementCount(ExistShopping, shoppingCart.Count);
            }

            _unitOfWork.Save();

            return RedirectToAction("Index");
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