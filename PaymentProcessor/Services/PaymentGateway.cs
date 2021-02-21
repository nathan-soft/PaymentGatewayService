using AutoMapper;
using PaymentProcessor.Data.Repositories;
using PaymentProcessor.Dtos;
using PaymentProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentProcessor.Services
{
    public interface IPaymentGateway
    {
        Task<string> ProcessAsync(PaymentDto payment);
    }

    public class PaymentGateway : IPaymentGateway
    {
        private readonly ICheapPaymentGateway _cheapPaymentGateway;
        private readonly IExpensivePaymentGateway _expensivePaymentGateway;
        private readonly IPremiumPaymentGateway _premiumPaymentGateway;
        private readonly IPaymentRepository _paymentRepo;
        private readonly IPaymentStateRepository _paymentStateRepo;
        private readonly IMapper _mapper;

        public PaymentGateway(ICheapPaymentGateway cheapPaymentGateway, IExpensivePaymentGateway expensivePaymentGateway, IPremiumPaymentGateway premiumPaymentGateway, IPaymentRepository paymentRepo, IPaymentStateRepository paymentStateRepo, IMapper mapper)
        {
            _cheapPaymentGateway = cheapPaymentGateway;
            _expensivePaymentGateway = expensivePaymentGateway;
            _premiumPaymentGateway = premiumPaymentGateway;
            _paymentRepo = paymentRepo;
            _paymentStateRepo = paymentStateRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Processes a payment after validating few cases. CHECKS FOR INNVALID CREDIT CARD AND VALID DATE ARE DONE 
        /// AT THE CONTROLLER/DTO LEVEL.
        /// </summary>
        /// <param name="payment">The payment to be processed</param>
        /// <returns>A task that resolves to a string.</returns>
        public async Task<string> ProcessAsync(PaymentDto payment)
        {
            if (payment == null)
                throw new ArgumentNullException(nameof(payment));

            if (payment.Amount <= 0)
                throw new ArgumentException($"£{nameof(payment.Amount)} is not a valid amount");

            if (payment.Amount >= 20 && payment.Amount < 21)
                throw new Exception($"Cannot process payment between £20.01 to £20.99");

            PaymentState paymentState = null;
            string result = "";

            try
            {
                //Insert  into Payment Entity
                var newPayment = _mapper.Map<Payment>(payment);
                await _paymentRepo.InsertAsync(newPayment);
                paymentState = new PaymentState
                {
                    PaymentId = newPayment.Id,
                    State = nameof(PaymentStateEnum.PENDING)
                };
                await _paymentStateRepo.InsertAsync(paymentState);

                //proceed to process payment...
                if (payment.Amount < 20)
                {
                    //use ICheapPaymentGateway to process the payment.
                    result = _cheapPaymentGateway.Process(payment);
                }
                else if (payment.Amount >= 21 && payment.Amount <= 500)
                {
                    //use IExpensivePaymentGateway to process the payment.
                    result =  await _expensivePaymentGateway.ProcessAsync(payment);
                }
                else
                {
                    //use IPremiumPaymentGateway to process the payment.
                    result =  await _premiumPaymentGateway.ProcessAsync(payment);
                }
                
                return result;
            }
            finally
            {
                //paymentState could be null if the repository method never runs or
                //it threw an unexpected exception.
                //if that's the case, no need for cleanup.
                if(paymentState != null)
                {
                    if (result.Length > 1)
                    {
                        //there was no error
                        //update Payment State Entity to processed.
                        paymentState.State = nameof(PaymentStateEnum.PROCESSESD);
                    }
                    else
                    {
                        // update Payment State Entity
                        paymentState.State = nameof(PaymentStateEnum.FAILED);
                    }

                    await _paymentStateRepo.UpdateAsync(paymentState);
                }
                
            }
            
        }
    }
}
