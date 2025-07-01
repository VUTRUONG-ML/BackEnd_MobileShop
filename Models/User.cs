using System.ComponentModel.DataAnnotations;

namespace BackEnd_MobileShop.Models
{
    public class User
    {
        [Key]
        public string? UserName { get; set; }

        [Required]
        public string? Password { get; set; }

        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }

        public string? Role { get; set; }
    }
}
