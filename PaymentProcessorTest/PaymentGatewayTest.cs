using AutoMapper;
using LifeLongApi.Codes;
using Microsoft.EntityFrameworkCore;
using PaymentProcessor.Data;
using PaymentProcessor.Data.Repositories;
using PaymentProcessor.Dtos;
using PaymentProcessor.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PaymentProcessorTest
{
    public class PaymentGatewayTest
    {
        private readonly ICheapPaymentGateway _cheapPaymentGateway;
        private readonly IExpensivePaymentGateway _expensivePaymentGateway;
        private readonly IPremiumPaymentGateway _premiumPaymentGateway;
        private readonly IPaymentRepository _paymentRepo;
        private readonly IPaymentStateRepository _paymentStateRepo;
        private readonly IMapper _mapper;
        private readonly IPaymentGateway _paymentGateway;

        private readonly ApplicationDbContext _context;

        public PaymentGatewayTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer("Data Source=(local)\\SQLEXPRESS;Initial Catalog=PaymentGateway;Integrated Security=True;").Options;
            _context = new ApplicationDbContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new AutoMapperProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }

            _cheapPaymentGateway = new CheapPaymentGateway();
            _expensivePaymentGateway = new ExpensivePaymentGateway(_cheapPaymentGateway);
            _premiumPaymentGateway = new PremiumPaymentGateway();
            _paymentRepo = new PaymentRepository(_context);
            _paymentStateRepo = new PaymentStateRepository(_context);
            _paymentGateway = new PaymentGateway(_cheapPaymentGateway, _expensivePaymentGateway, _premiumPaymentGateway, _paymentRepo, _paymentStateRepo, _mapper);
        }

        [Fact]
        public async void Should_Not_Process_Null_Value()
        {
            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>_paymentGateway.ProcessAsync(null));
        }

        [Fact]
        public async void Should_Not_Process_0_Or_Negative_Amount()
        {
            var newPayment = new PaymentDto()
            {
                CreditCardNumber = "3400 0000 0000 009",
                CardHolder = "Ifeoma Joel",
                Amount = -200,
                ExpirationDate = DateTime.Parse("01-07-2022")
            };

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _paymentGateway.ProcessAsync(newPayment));
        }

        [Fact]
        public async void Should_Not_Process_Amount_Equals_To_Or_Greater_Than_20_But_Less_Than_21()
        {
            var newPayment = new PaymentDto()
            {
                CreditCardNumber = "3400 0000 0000 009",
                CardHolder = "Blessing Joel",
                Amount = 20.999m,
                ExpirationDate = DateTime.Parse("01-07-2022")
            };

            // Assert
            await Assert.ThrowsAsync<Exception>(() => _paymentGateway.ProcessAsync(newPayment));
        }

        [Fact]
        public async void Should_Send_Amount_Less_Than_20_To_CheapPaymentGateway_For_Processing()
        {
            var newPayment = new PaymentDto()
            {
                CreditCardNumber = "3400 0000 0000 009",
                CardHolder = "Blessing Joel",
                Amount = 16,
                ExpirationDate = DateTime.Parse("01-07-2022")
            };
            string result = await _paymentGateway.ProcessAsync(newPayment);
            // Assert
            Assert.Contains($"was successfully proccessed using {nameof(ICheapPaymentGateway)}", result);
        }

        [Fact]
        public async void Should_Send_Amount_Between_21And500_To_ExpensivePaymentGateway_For_ProcessingAsync()
        {
            var newPayment = new PaymentDto()
            {
                CreditCardNumber = "3400 0000 0000 009",
                CardHolder = "Blessing Joel",
                Amount = 78,
                ExpirationDate = DateTime.Parse("01-07-2022")
            };
            string result = await _paymentGateway.ProcessAsync(newPayment);
            List<string> resultOptions = new List<string> { $"was successfully proccessed using {nameof(IExpensivePaymentGateway)}", $"was successfully proccessed using {nameof(ICheapPaymentGateway)}" };

            // Assert
            Assert.Contains(resultOptions, (x)=> { return result.Contains(x); });
        }

        [Fact]
        public async void Should_Send_Amount_Greater_Than_500_To_IPremiumPaymentGateway_For_Processing()
        {
            var newPayment = new PaymentDto()
            {
                CreditCardNumber = "3400 0000 0000 009",
                CardHolder = "Blessing Emmanuel",
                Amount = 509,
                ExpirationDate = DateTime.Parse("01-07-2022")
            };
            string result = await _paymentGateway.ProcessAsync(newPayment);
            // Assert
            Assert.Contains($"was successfully proccessed using {nameof(IPremiumPaymentGateway)}", result);
        }
    }
}
