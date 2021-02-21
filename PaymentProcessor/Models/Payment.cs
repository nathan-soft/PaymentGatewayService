using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentProcessor.Models
{
    public class Payment
    {
        public int Id { get; set; }
        [Required]
        public string CreditCardNumber { get; set; }
        [Required]
        public string CardHolder { get; set; }
        [StringLength(3, MinimumLength = 3)]
        public string SecurityCode { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public DateTime ExpirationDate { get; set; }

        public PaymentState PaymentState { get; set; }
    }
}
