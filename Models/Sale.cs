using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BackEnd_MobileShop.Models
{
    public class Sale
    {
        [Key]
        public int SaleID { get; set; }

        [ForeignKey("Mobile")]
        public string? IMEINo { get; set; }

        public DateTime SaleDate { get; set; }

        public decimal Price { get; set; }

        [ForeignKey("Customer")]
        public int CustomerID { get; set; }

        public Mobile? Mobile { get; set; }
        public Customer? Customer { get; set; }
    }
}
