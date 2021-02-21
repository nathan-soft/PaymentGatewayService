using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentProcessor.Models
{
    public class PaymentState
    {
        public int Id { get; set; }
        public int PaymentId { get; set; }
        [Required]
        public string State { get; set; }
        public Payment Payment { get; set; }
    }

    public enum PaymentStateEnum
    {
        PENDING,
        FAILED,
        PROCESSESD
    }
}
