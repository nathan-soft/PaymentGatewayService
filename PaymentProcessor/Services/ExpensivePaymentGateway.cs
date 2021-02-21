using PaymentProcessor.Code;
using PaymentProcessor.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PaymentProcessor.Services
{
    public interface IExpensivePaymentGateway
    {
        Task<string> ProcessAsync(PaymentDto payment);
    }

    public class ExpensivePaymentGateway : BankingSystem, IExpensivePaymentGateway
    {
        private readonly ICheapPaymentGateway _cheapPaymentGateway;
        public ExpensivePaymentGateway(ICheapPaymentGateway cheapPaymentGateway)
        {
            _cheapPaymentGateway = cheapPaymentGateway;
        }
        public async Task<string> ProcessAsync(PaymentDto payment)
        {
            try
            {
                PerformOperation(payment);
                return $"Your payment of £{payment.Amount} was successfully proccessed using {nameof(IExpensivePaymentGateway)}";
            }
            catch (WebException)
            {
                //log exception.
                string result = "";
                //Retry once using ICheapPaymentGateway.
                //exception will be thrown if failed.
                await AppHelper.RetryAsync(
                     () => { result = _cheapPaymentGateway.Process(payment); },
                    TimeSpan.FromSeconds(4),
                    1);
                //return the result of using cheapPaymentGateway.
                return result;
            }

        }
    }
}
