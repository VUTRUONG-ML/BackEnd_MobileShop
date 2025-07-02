using System.ComponentModel.DataAnnotations;
namespace BackEnd_MobileShop.Models.DTOs
{
    public class SaleRequest
    {
        [Required]
        public string? CustomerName { get; set; }

        [Required]
        public string? MobileNo { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Address { get; set; }

        [Required]
        public string? IMEINo { get; set; }
    }
}
