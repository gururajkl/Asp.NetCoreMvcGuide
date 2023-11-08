using BulkyWebRazor.Data;
using BulkyWebRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext db;

        [BindProperty]
        public Category Category { get; set; } = new Category();

        public CreateModel(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            db.Categories.Add(Category);
            db.SaveChanges();
            TempData["Success"] = "Create Success";
            return RedirectToPage("Index");
        }
    }
}
