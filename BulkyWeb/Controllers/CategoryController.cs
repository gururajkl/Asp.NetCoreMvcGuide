using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext db;

        public CategoryController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            List<Category> categoriesList = db.Categories.ToList();
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
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index", "Category");
            }
            return View();
        }

        public IActionResult Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            else
            {
                Category? categoryFromDb = db.Categories.Find(Id);
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
                db.Categories.Update(category);
                db.SaveChanges();
                return RedirectToAction("Index", "Category");
            }
            return View();
        }
    }
}
