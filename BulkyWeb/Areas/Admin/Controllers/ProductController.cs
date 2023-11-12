using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            this.unitOfWork = unitOfWork;
            this.webHostEnvironment = webHostEnvironment;

        }

        public IActionResult Index()
        {
            List<Product> products = unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
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
                string wwwRootPath = webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"Images\Product");

                    // Update the old image if new image url is found.
                    if (!string.IsNullOrEmpty(productViewModel.Product!.ImageUrl))
                    {
                        string oldImagePath = Path.Combine(wwwRootPath, productViewModel.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productViewModel.Product!.ImageUrl = @"Images\Product\" + fileName;
                }

                if (productViewModel.Product!.Id == 0)
                {
                    unitOfWork.Product.Add(productViewModel.Product!);
                }
                else
                {
                    unitOfWork.Product.Update(productViewModel.Product);
                }
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

        #region API CALLS
        /*
         * Just an Api function which returns products data in Json format.
         */
        public IActionResult GetAll()
        {
            List<Product> productList = unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = productList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            Product? product = unitOfWork.Product.Get(p => p.Id == id);
            if (product == null)
            {
                return Json(new { success = false, message = "Error while deleteing" });
            }
            string oldImagePath = Path.Combine(webHostEnvironment.WebRootPath, product.ImageUrl!.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            unitOfWork.Product.Remove(product);
            unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successfull" });
        }
        #endregion
    }
}
