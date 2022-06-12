using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork=unitOfWork;
            _webHostEnvironment=webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Edit
        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            ProductViewModel productViewModel = new ProductViewModel()
            {
                productModel = new(),
                CategoryList = _unitOfWork.categoryRepository
                .GetAll()
                .Select(category => new SelectListItem()
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                }),
                CovertTypeList = _unitOfWork.coverTypeRepository
                .GetAll()
                .Select(coverType => new SelectListItem()
                {
                    Text = coverType.Name,
                    Value = coverType.Id.ToString()
                })
            };

            try
            {
                if (id == null || id == 0)
                {

                    //Create
                    return View(productViewModel);
                }
                else
                {
                    productViewModel.productModel = _unitOfWork.productRepository.GetFirstOrDefault(p => p.Id == id);
                    return View(productViewModel);
                    //Update
                }
            }
            catch (Exception)
            {

            }
            return View(productViewModel);
        }

        [HttpPost]
        public IActionResult Upsert(ProductViewModel productViewModel, IFormFile file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    if (file != null)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(wwwRootPath, @"images\products");
                        var extension = Path.GetExtension(file.FileName);

                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName+extension)
                            , FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        productViewModel.productModel.ImageUrl = @"\images\product" + fileName + extension;

                    }
                    _unitOfWork.productRepository.Add(productViewModel.productModel);
                    _unitOfWork.Save();
                    TempData["success"] = "The product edited succesfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                TempData["error"] = "The product cannot created";
                return View();
            }

        }
        #endregion

        #region Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                ProductModel product = _unitOfWork.productRepository.GetFirstOrDefault(product => product.Id == id);
                if (product == null)
                {
                    throw new Exception();
                }
                return View(product);
            }
            catch (Exception)
            {
                TempData["error"] = "Something is wrong";
                return RedirectToAction("Index");
            }
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception();
                }

                ProductModel productModel = _unitOfWork.productRepository.GetFirstOrDefault(m => m.Id == id);

                if (productModel == null)
                {
                    throw new Exception();
                }

                _unitOfWork.productRepository.Remove(productModel);
                _unitOfWork.Save();
                TempData["success"] = "Successfully deleted";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["error"] = "Something is wrong";
                return View();
            }

        }

        #endregion

        #region API CONTROLLERS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.productRepository.GetAll(includeProperties: "Category,CoverType");
            return Json(new { success = true, data = productList });
        }

        #endregion
    }
}
