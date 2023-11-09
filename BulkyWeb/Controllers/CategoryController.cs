using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            List<Category> categoriesList = categoryRepository.GetAll().ToList();
            return View(categoriesList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            /* For Custom Validation (Use ModelState.AddModelError()). */

            if (ModelState.IsValid)
            {
                categoryRepository.Add(category);
                categoryRepository.Save();

                // Using TempData to show the tost on the UI.
                TempData["Success"] = "Category created successfully";
                return RedirectToAction("Index", "Category");
            }
            return View();
        }

        public IActionResult Edit(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            else
            {
                Category? categoryFromDb = categoryRepository.Get(c => c.Id == Id);
                if (categoryFromDb == null)
                {
                    return NotFound();
                }
                return View(categoryFromDb);
            }
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.Update(category);
                categoryRepository.Save();
                TempData["Success"] = "Category updated successfully";
                return RedirectToAction("Index", "Category");
            }
            return View();
        }

        public IActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            else
            {
                Category? categoryFromDb = categoryRepository.Get(c => c.Id == Id);
                if (categoryFromDb == null)
                {
                    return NotFound();
                }
                return View(categoryFromDb);
            }
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? Id)
        {
            Category? category = categoryRepository.Get(c => c.Id == Id);

            if (category == null)
            {
                return NotFound();
            }

            categoryRepository.Remove(category);
            categoryRepository.Save();
            TempData["Success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
