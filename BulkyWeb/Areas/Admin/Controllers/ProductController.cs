using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public IActionResult Upsert(int? Id)
        {
            IEnumerable<SelectListItem> categoryList = unitOfWork.Category.GetAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
            });

            ProductViewModel productViewModel = new()
            {
                Product = new Product(),
                CategoryList = categoryList
            };

            if (Id == null)
            {
                return View(productViewModel);
            }
            else
            {
                productViewModel.Product = unitOfWork.Product.Get(p => p.Id == Id);
                return View(productViewModel);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductViewModel productViewModel, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Product.Add(productViewModel.Product!);
                unitOfWork.Save();
                TempData["Success"] = "Product created successfully";
                return RedirectToAction("Index", "Product");
            }
            else
            {
                productViewModel.CategoryList = unitOfWork.Category.GetAll().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                });

                return View(productViewModel);
            }
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
