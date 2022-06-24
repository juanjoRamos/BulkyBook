using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork=unitOfWork;
        }

        public IActionResult Index()
        {
            return View(_unitOfWork.coverTypeRepository.GetAll());
        }

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CoverTypeModel coverTypeModel) 
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.coverTypeRepository.Add(coverTypeModel);
                    _unitOfWork.Save();
                    TempData["success"] = "Cover created succesfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception) 
            {
                TempData["error"] = "Cover cannot created";
                return View();
            }

        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    TempData["error"] = "You must select a cover type";
                    return View();
                }
                else 
                {
                    CoverTypeModel coverBD = _unitOfWork.coverTypeRepository.GetFirstOrDefault(covertype => covertype.Id == id);
                    if (ModelState.IsValid)
                    {
                        return View(coverBD);
                    }
                    else 
                    {
                        TempData["error"] = "Cover type don't exists";
                        return View();
                    }
                }

            }
            catch (Exception)
            {

            }
            return View();
        }

        [HttpPost]
        public IActionResult Edit(CoverTypeModel coverTypeModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.coverTypeRepository.Update(coverTypeModel);
                    _unitOfWork.Save();
                    TempData["success"] = "Cover edited succesfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                TempData["error"] = "Cover cannot created";
                return View();
            }

        }
        #endregion
    }
}
