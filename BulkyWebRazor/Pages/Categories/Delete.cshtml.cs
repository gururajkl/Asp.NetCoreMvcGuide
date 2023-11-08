using BulkyWebRazor.Data;
using BulkyWebRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor.Pages.Categories
{
    [BindProperties]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext db;
        public Category Category { get; set; } = new Category();

        public DeleteModel(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void OnGet(int Id)
        {
            Category = db.Categories.Find(Id)!;
        }

        public IActionResult OnPost()
        {
            db.Categories.Remove(Category);
            db.SaveChanges();
            TempData["Success"] = "Delete Success";
            return RedirectToPage("Index");
        }
    }
}
