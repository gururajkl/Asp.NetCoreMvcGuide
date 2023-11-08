using BulkyWebRazor.Data;
using BulkyWebRazor.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext db;
        public List<Category>? CategoryList { get; set; }

        public IndexModel(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void OnGet()
        {
            CategoryList = db.Categories.ToList();
        }
    }
}
