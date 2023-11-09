using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Product> products = unitOfWork.Product.GetAll().ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Product.Add(product);
                unitOfWork.Save();
                TempData["Success"] = "Product created successfully";
                return RedirectToAction("Index", "Product");
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
                Product productFromDb = unitOfWork.Product.Get(p => p.Id == Id);
                if (productFromDb == null)
                {
                    return NotFound();
                }
                return View(productFromDb);
            }
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Product.Update(product);
                unitOfWork.Save();
                TempData["Success"] = "Product updated successfully";
                return RedirectToAction("Index", "Product");
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
                Product productFromDb = unitOfWork.Product.Get(p => p.Id == Id);
                if (productFromDb == null)
                {
                    return NotFound();
                }
                return View(productFromDb);
            }
        }

        [HttpPost]
        public IActionResult Delete(int Id)
        {
            Product? product = unitOfWork.Product.Get(p => p.Id == Id);

            if (product == null)
            {
                return NotFound();
            }

            unitOfWork.Product.Remove(product);
            unitOfWork.Save();
            TempData["Success"] = "Product deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
