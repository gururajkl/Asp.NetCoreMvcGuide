using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        [BindProperty]
        public OrderViewModel? OrderViewModel { get; set; }

        public OrderController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int orderId)
        {
            OrderViewModel = new()
            {
                OrderHeader = unitOfWork.OrderHeader.Get(u => u.Id == orderId, includeProperties: "ApplicationUser"),
                OrderDetail = unitOfWork.OrderDetail.GetAll(u => u.OrderIdHeaderId == orderId, includeProperties: "Product")
            };

            return View(OrderViewModel);
        }

        [Authorize(Roles = StaticDetails.Role_Admin + "," + StaticDetails.Role_Employee)]
        [HttpPost]
        public IActionResult UpdateOrderDetail()
        {
            var orderHeaderFromDb = unitOfWork.OrderHeader.Get(o => o.Id == OrderViewModel!.OrderHeader!.Id);

            orderHeaderFromDb.Name = OrderViewModel!.OrderHeader!.Name;
            orderHeaderFromDb.PhoneNumber = OrderViewModel!.OrderHeader!.PhoneNumber;
            orderHeaderFromDb.StreetAddress = OrderViewModel!.OrderHeader!.StreetAddress;
            orderHeaderFromDb.City = OrderViewModel!.OrderHeader!.City;
            orderHeaderFromDb.State = OrderViewModel!.OrderHeader!.State;
            orderHeaderFromDb.PostalCode = OrderViewModel!.OrderHeader!.PostalCode;
            if (!string.IsNullOrEmpty(OrderViewModel.OrderHeader.Carrier))
            {
                orderHeaderFromDb.Carrier = OrderViewModel!.OrderHeader!.Carrier;
            }
            if (!string.IsNullOrEmpty(OrderViewModel.OrderHeader.TrackingNumber))
            {
                orderHeaderFromDb.TrackingNumber = OrderViewModel!.OrderHeader!.TrackingNumber;
            }

            unitOfWork.OrderHeader.Update(orderHeaderFromDb);
            unitOfWork.Save();

            TempData["Success"] = "Order details updated succesfully";

            return RedirectToAction(nameof(Details), new { orderId = orderHeaderFromDb.Id });
        }

        [Authorize(Roles = StaticDetails.Role_Admin + "," + StaticDetails.Role_Employee)]
        [HttpPost]
        public IActionResult StartProcessing()
        {
            unitOfWork.OrderHeader.UpdateStatus(OrderViewModel!.OrderHeader!.Id, StaticDetails.StatusInProcess);
            unitOfWork.Save();

            TempData["Success"] = "Order details updated succesfully";

            return RedirectToAction(nameof(Details), new { orderId = OrderViewModel.OrderHeader.Id });
        }

        [Authorize(Roles = StaticDetails.Role_Admin + "," + StaticDetails.Role_Employee)]
        [HttpPost]
        public IActionResult ShipOrder()
        {
            var orderHeader = unitOfWork.OrderHeader.Get(o => o.Id == OrderViewModel!.OrderHeader!.Id);
            orderHeader.TrackingNumber = OrderViewModel!.OrderHeader!.TrackingNumber;
            orderHeader.Carrier = OrderViewModel.OrderHeader.Carrier;
            orderHeader.OrderStatus = StaticDetails.StatusShipped;
            orderHeader.ShippingDate = DateTime.Now;
            if (orderHeader.PaymentStatus == StaticDetails.PaymentStatusDelayedPayment)
            {
                orderHeader.PaymentDueDate = DateTime.Now.AddDays(30);
            }

            unitOfWork.OrderHeader.Update(orderHeader);
            unitOfWork.Save();

            TempData["Success"] = "Order Shipped Successfully.";

            return RedirectToAction(nameof(Details), new { orderId = OrderViewModel.OrderHeader.Id });
        }

        #region API CALLS
        /*
         * Just an Api function which returns products data in Json format.
         */
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> objOrderHeaders;

            var claimsIdentity = (ClaimsIdentity)User.Identity!;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            if (User.IsInRole(StaticDetails.Role_Admin) || User.IsInRole(StaticDetails.Role_Employee))
            {
                objOrderHeaders = unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser").ToList();
            }
            else
            {
                objOrderHeaders = unitOfWork.OrderHeader.GetAll(o => o.ApplicationUserId == userId, includeProperties: "ApplicationUser").ToList();
            }

            switch (status)
            {
                case "pending":
                    objOrderHeaders = objOrderHeaders.Where(u => u.PaymentStatus == StaticDetails.PaymentStatusDelayedPayment);
                    break;
                case "inprocess":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == StaticDetails.StatusInProcess);
                    break;
                case "completed":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == StaticDetails.StatusShipped);
                    break;
                case "approved":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == StaticDetails.StatusApproved);
                    break;
                default:
                    break;

            }

            return Json(new { data = objOrderHeaders });
        }
        #endregion
    }
}
