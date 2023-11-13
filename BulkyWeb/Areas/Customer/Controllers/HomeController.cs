using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = unitOfWork.Product.GetAll(includeProperties: "Category");
            return View(productList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("Index");
            }

            ShoppingCart shoppingCart = new()
            {
                Product = unitOfWork.Product.Get(p => p.Id == id, "Category"),
                Count = 1,
                ProductId = id
            };
            return View(shoppingCart);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            if (shoppingCart == null)
            {
                return RedirectToAction("Index");
            }

            var claimsIdentity = (ClaimsIdentity)User.Identity!;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            shoppingCart.ApplicationUserId = userId;

            ShoppingCart shoppingCartFromDb = unitOfWork.ShoppingCart.Get(s => s.ApplicationUserId == userId &&
            s.ProductId == shoppingCart.ProductId);

            if (shoppingCartFromDb != null)
            {
                // Cart already exsists.
                shoppingCartFromDb.Count += shoppingCart.Count;
                unitOfWork.ShoppingCart.Update(shoppingCartFromDb);
                unitOfWork.Save();
                TempData["Success"] = "Cart updated successfully";
            }
            else
            {
                unitOfWork.ShoppingCart.Add(shoppingCart);
                unitOfWork.Save();
                TempData["Success"] = "New item added to the cart successfully";
                HttpContext.Session.SetInt32(StaticDetails.SessionCart, (unitOfWork.ShoppingCart.GetAll(s => s.ApplicationUserId == userId).Count()));
            }

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}