using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd_MobileShop.Models
{
    public class Transaction
    {
        [Key]
        public int TransID { get; set; }

        [ForeignKey("Model")]
        public int ModelID { get; set; }

        public int Quantity { get; set; }

        public decimal Amount { get; set; }

        public DateTime TransDate { get; set; }

        public Model? Model { get; set; } // Navigation

    }
}
