using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;
namespace OnLineShop.Areas.Admin.Models
{
    public class RoleUserVm
    {

        [Required]
        [Display(Name ="User")]
        public string UserId { get; set; }
        [Display(Name = "Role")]
        [Required]


        public string RoleId { get; set; }
    }
}
