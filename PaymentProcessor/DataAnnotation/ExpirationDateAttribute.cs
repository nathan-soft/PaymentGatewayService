using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentProcessor.DataAnnotation
{
    public class ExpirationDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var expirationDate = ((DateTime)value).Date;
            
            if (expirationDate.CompareTo(DateTime.Today.Date) < 0)
                return new ValidationResult("Expiration date is required and should not be in the past.");

            return ValidationResult.Success;
        }
    }
}
