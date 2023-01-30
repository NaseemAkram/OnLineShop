using System.ComponentModel.DataAnnotations;

namespace OnLineShop.Models
{
    public class ProductTypes
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Product Types")]
        public string ProductType { get; set; }
    }
}
