using PaymentProcessor.DataAnnotation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentProcessor.Dtos
{
    public class PaymentDto
    {
        [Required, CreditCard]
        public string CreditCardNumber { get; set; }
        [Required]
        public string CardHolder { get; set; }
        [StringLength(3, MinimumLength = 3)]
        public string SecurityCode { get; set; }
        [DataType("decimal")]
        [Range(typeof(decimal), "0.1", "79228162514264337593543950335")]
        public decimal Amount { get; set; }
        [DataType(DataType.Date)]
        [ExpirationDate]
        public DateTime ExpirationDate { get; set; }
    }
}
