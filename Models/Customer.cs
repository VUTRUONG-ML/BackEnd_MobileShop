using System.ComponentModel.DataAnnotations;

namespace BackEnd_MobileShop.Models
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }
        [Required]
        public string? CustomerName { get; set; }
        public string? MobileNo { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}
