using LifeLongApi.Data.Repositories;
using PaymentProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentProcessor.Data.Repositories
{
    public interface IPaymentStateRepository : IGenericRepository<PaymentState>
    {

    }

    public class PaymentStateRepository : GenericRepository<PaymentState>, IPaymentStateRepository
    {
        public PaymentStateRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
