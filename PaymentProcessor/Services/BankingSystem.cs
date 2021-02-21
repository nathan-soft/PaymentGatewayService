using PaymentProcessor.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PaymentProcessor.Services
{
    public abstract class BankingSystem
    {
        /// <summary>
        /// Simulates a transient process that could fail or pass.
        /// </summary>
        /// <returns></returns>
        protected void PerformOperation(PaymentDto payment) {
            //simulate error
            Random rnd = new Random();
            var rndNumber = rnd.Next(1, 4);
            if (rndNumber == 3)
                throw new WebException("Cannot connect to host.");
        }
    }

}
