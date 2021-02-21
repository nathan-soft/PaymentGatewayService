using LifeLongApi.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using PaymentProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentProcessor.Data.Repositories
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {

    }
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(ApplicationDbContext context) : base(context)
        {

        }

        //public async Task<Payment> GetByNameAsync(string fieldOfInterest)
        //{
        //    return await context.Set<Payment>().FirstOrDefaultAsync(f => f.Name.ToLower().Contains(fieldOfInterest.ToLower()));
        //}
    }
}
