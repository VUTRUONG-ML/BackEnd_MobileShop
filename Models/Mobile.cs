using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BackEnd_MobileShop.Models
{
    public class Mobile
    {
        [Key]
        public string? IMEINo { get; set; }
        [ForeignKey("Model")]
        public int ModelID { get; set; }
        public Model? Model { get; set; }
        [Required]
        public string? Status { get; set; }  // Available, Sold, etc.

        public decimal Price { get; set; }
    }
}
