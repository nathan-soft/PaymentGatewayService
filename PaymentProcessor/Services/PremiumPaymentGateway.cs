using PaymentProcessor.Code;
using PaymentProcessor.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentProcessor.Services
{
    public interface IPremiumPaymentGateway
    {
        Task<string> ProcessAsync(PaymentDto payment);
    }

    public class PremiumPaymentGateway : BankingSystem, IPremiumPaymentGateway
    {
        public async Task<string> ProcessAsync(PaymentDto payment)
        {
            await AppHelper.RetryAsync(() => PerformOperation(payment), TimeSpan.FromSeconds(4), 3);

            return $"Your Payment of £{payment.Amount} was successfully proccessed using {nameof(IPremiumPaymentGateway)}";
        }
    }
}
