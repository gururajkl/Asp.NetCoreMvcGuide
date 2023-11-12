using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Company> companies = unitOfWork.Company.GetAll().ToList();
            return View(companies);
        }

        public IActionResult Upsert(int? Id)
        {
            if (Id == null)
            {
                return View(new Company());
            }
            else
            {
                Company company = unitOfWork.Company.Get(p => p.Id == Id);
                return View(company);
            }
        }

        [HttpPost]
        public IActionResult Upsert(Company company)
        {
            if (ModelState.IsValid)
            {
                if (company.Id == 0)
                {
                    unitOfWork.Company.Add(company);
                }
                else
                {
                    unitOfWork.Company.Update(company);
                }
                unitOfWork.Save();
                TempData["Success"] = "Company created successfully";
                return RedirectToAction("Index", "Company");
            }
            else
            {
                return View(company);
            }
        }

        #region API CALLS
        /*
         * Just an Api function which returns products data in Json format.
         */
        public IActionResult GetAll()
        {
            List<Company> companyList = unitOfWork.Company.GetAll().ToList();
            return Json(new { data = companyList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            Company? company = unitOfWork.Company.Get(p => p.Id == id);
            if (company == null)
            {
                return Json(new { success = false, message = "Error while deleteing" });
            }

            unitOfWork.Company.Remove(company);
            unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successfull" });
        }
        #endregion
    }
}
