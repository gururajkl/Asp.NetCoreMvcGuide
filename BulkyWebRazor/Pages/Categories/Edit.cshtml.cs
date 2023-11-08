using BulkyWebRazor.Data;
using BulkyWebRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor.Pages.Categories
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext db;
        public Category Category { get; set; } = new Category();

        public EditModel(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void OnGet(int Id)
        {
            Category = db.Categories.Find(Id)!;
        }

        public IActionResult OnPost()
        {
            db.Categories.Update(Category);
            db.SaveChanges();
            TempData["Success"] = "Update Success";
            return RedirectToPage("Index");
        }
    }
}
