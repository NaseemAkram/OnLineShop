using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace OnLineShop.Models
{
    public class Order
    {

        public Order()
        {
            orderDetails = new List<OrderDetails>();
        }
        [Key]
        public int Id { get; set; }
        [Display(Name = "Order no")]
        public string OrderNo { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Phone No")]
        public string PhoneNo { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        public DateTime OrderDate { get; set; }

        public virtual List<OrderDetails> orderDetails { get; set; }
    }
}
