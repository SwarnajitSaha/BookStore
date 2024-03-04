using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        [DisplayName("Category Name")]
        public string CategoryName {  get; set; }

        [Required]
        [Range(1, 1000)]
        public int DisplayOrder { get; set; }  

    }
}
