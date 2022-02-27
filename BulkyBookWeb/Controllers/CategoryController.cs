using BulkyBookWeb.Data;
using BulkyBookWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<CategoryModel> objCategoryList = _db.CategoriesTable.ToList();
            return View(objCategoryList);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryModel objCategory)
        {
            if (objCategory.Name.Equals(objCategory.DisplayOrder.ToString())) 
            {
                ModelState.AddModelError("CustomError", "The DisplayOrder cannot exactly match the Name");
                //In All and where you say for example on error name
                //ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name");
            }
            if (ModelState.IsValid)
            {
                _db.CategoriesTable.Add(objCategory);
                _db.SaveChanges();
                TempData["success"] = "Category has been created!!";
                return RedirectToAction("Index");
            }
            return View(objCategory);
        }

        //GET
        public IActionResult Edit(int? idCategory)
        {
            if(idCategory == null || idCategory == 0)
                return NotFound();

            var categoryFind = _db.CategoriesTable.Find(idCategory);

            if(categoryFind.Equals(null))
                return NotFound();

            return View(categoryFind);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryModel objCategory)
        {
            if (objCategory.Name.Equals(objCategory.DisplayOrder.ToString()))
            {
                ModelState.AddModelError("CustomError", "The DisplayOrder cannot exactly match the Name");
            }
            if (ModelState.IsValid)
            {
                _db.CategoriesTable.Update(objCategory);
                _db.SaveChanges();
                TempData["success"] = "Category has been edited!!";
                return RedirectToAction("Index");
            }
            return View(objCategory);
        }

        //GET
        public IActionResult Delete(int? idCategory)
        {
            if (idCategory == null || idCategory == 0)
                return NotFound();

            var categoryFind = _db.CategoriesTable.Find(idCategory);

            if (categoryFind.Equals(null))
                return NotFound();

            return View(categoryFind);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(CategoryModel objCategory)
        {
            var objToDelete = _db.CategoriesTable.Find(objCategory.Id); 
            if (objToDelete == null)
                return NotFound();
                
            _db.CategoriesTable.Remove(objToDelete);
            _db.SaveChanges();
            TempData["success"] = "Category has been removed!!";
            return RedirectToAction("Index");
        }

    }
}
