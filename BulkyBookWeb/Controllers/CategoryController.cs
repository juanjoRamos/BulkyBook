using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BulkyBook.DataAccess.Data;
using BulkyBook.Models;

namespace BulkyBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.CategoriesTable.ToList());
        }

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryModel categoryModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoryModel);
                await _context.SaveChangesAsync();
                TempData["success"] = "Successfully created";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Something is wrong";
            return View();
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? idCategory)
        {
            try
            {
                if (idCategory.HasValue)
                {
                    IEnumerable<CategoryModel> categoryList = await _context.CategoriesTable.ToListAsync();
                    CategoryModel category = categoryList.FirstOrDefault(c => c.Id == idCategory);
                    return View(category);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                TempData["error"] = "Something is wrong";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Edit(CategoryModel categoryModel)
        {
            try
            {
                if (categoryModel.Name == categoryModel.DisplayOrder.ToString()) 
                {
                    ModelState.AddModelError("", "The display order cannot exactly match the name");
                }
                if (ModelState.IsValid)
                {
                    _context.CategoriesTable.Update(categoryModel);
                    _context.SaveChanges();
                    TempData["success"] = "Successfully edited";
                    return RedirectToAction("Index");
                }
                else 
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                TempData["error"] = "Something is wrong";
                return View();
            }
        }

        #endregion

        #region Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int? idCategory)
        {
            try
            {
                IEnumerable<CategoryModel> categoryList = await _context.CategoriesTable.ToListAsync();
                CategoryModel category = categoryList.Where(c => c.Id == idCategory).FirstOrDefault();
                return View(category);
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

                var categoryModel = await _context.CategoriesTable
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (categoryModel == null)
                {
                    throw new Exception();
                }

                _context.CategoriesTable.Remove(categoryModel);
                _context.SaveChanges();
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

    }
}
