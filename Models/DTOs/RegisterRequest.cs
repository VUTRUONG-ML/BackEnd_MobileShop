using System.ComponentModel.DataAnnotations;

namespace BackEnd_MobileShop.Models.DTOs
{
    public class RegisterRequest
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;
    }
}
