
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
                    productViewModel.productModel = _unitOfWork.productRepository
                        .GetFirstOrDefault(p => p.Id == id);
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

                        if (productViewModel.productModel.ImageUrl != null) 
                        {
                            var oldImagePath = Path.Combine(wwwRootPath, productViewModel.productModel.ImageUrl.TrimStart('\\'));
                            if (System.IO.File.Exists(oldImagePath)) 
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName+extension)
                            , FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        productViewModel.productModel.ImageUrl = @"\images\products\" + fileName + extension;

                    }

                    if (productViewModel.productModel.Id == 0)
                    {
                        _unitOfWork.productRepository.Add(productViewModel.productModel);
                    }
                    else 
                    {
                        _unitOfWork.productRepository.Update(productViewModel.productModel);
                    }

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

        #region API CONTROLLERS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.productRepository.GetAll(includeProperties: "Category,CoverType");
            return Json(new { success = true, data = productList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
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
                    return Json(new { success = false, message = "Error while deleting" });
                }

                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productModel.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                _unitOfWork.productRepository.Remove(productModel);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Delete successful" });
            }
            catch (Exception)
            {
                TempData["error"] = "Something is wrong";
                return View();
            }

        }

        #endregion
    }
}
