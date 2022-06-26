
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Company> companies = Enumerable.Empty<Company>();
            companies = _unitOfWork.companyRepository.GetAll();

            if (companies.Count() == 0) {
                companies = Enumerable.Empty<Company>();
            }

            return View(companies);
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            Company company = null;
            if (id != null)
            {
                company = _unitOfWork.companyRepository.GetFirstOrDefault(com => com.Id == id);
            }
            else 
            {
                company= new Company();
            }
            return View(company);
        }

        [HttpPost]
        public IActionResult Upsert(Company company)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (company.Id == 0)
                    {
                        _unitOfWork.companyRepository.Add(company);
                        TempData["success"] = "The company created succesfully";
                    }
                    else
                    {
                        _unitOfWork.companyRepository.Update(company);
                        TempData["success"] = "The company edited succesfully";
                    }

                    _unitOfWork.Save();
                    return RedirectToAction("Index");
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception) 
            {
                return View(company);
            }
        }

        #region API CONTROLLERS
        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<Company> companies = _unitOfWork.companyRepository.GetAll();
            return Json(new { success = true, data = companies });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null) 
            {
                return Json(new { success = false, message = "We can\'t found the company" });
            }

            Company company = _unitOfWork.companyRepository.GetFirstOrDefault(com => com.Id == id);
            
            if (company == null)
            {
                return Json(new { success = false, message = "We can\'t found the company" });
            }
            
            _unitOfWork.companyRepository.Remove(company);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted succesfully" });
        }

        #endregion

    }
}
