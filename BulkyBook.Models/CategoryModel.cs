using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Models
{
    public class CategoryModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [DisplayName("Display order"), Range(1, 100, ErrorMessage = "Display order must be between 1 and 100 only!")]
        public int DisplayOrder { get; set; }

        public DateTime CreatedDatetime { get; set; } = DateTime.Now;

    }
}
