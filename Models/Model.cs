using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BackEnd_MobileShop.Models
{
    public class Model
    {
        [Key]
        public int ModelID { get; set; }
        [Required]
        public string? ModelName { get; set; }

        public int AvailableQty { get; set; }

        [ForeignKey("Company")]
        public int CompanyID { get; set; }

        public Company? Company { get; set; }
    }
}
