using System.ComponentModel.DataAnnotations;
namespace BackEnd_MobileShop.Models
{
    public class Company
    {
        [Key]
        public int CompanyID { get; set; }  

        [Required]
        public string? CompanyName { get; set; }
    }
}
