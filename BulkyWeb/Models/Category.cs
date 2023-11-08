using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyWeb.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage = "Category name is too big")]
        [DisplayName("Category Name")]
        public string? Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "The Display order should be in between 1 and 100")]
        public int DisplayOrder { get; set; }
    }
}
