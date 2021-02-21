using PaymentProcessor.Code;
using PaymentProcessor.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentProcessor.Services
{
    public interface ICheapPaymentGateway
    {
        string Process(PaymentDto payment);
    }

    public class CheapPaymentGateway : BankingSystem, ICheapPaymentGateway
    {
        public string Process(PaymentDto payment)
        {
            PerformOperation(payment);
            return $"Your Payment of £{payment.Amount} was successfully proccessed using {nameof(ICheapPaymentGateway)}";
        }
    }
}
