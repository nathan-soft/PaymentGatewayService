using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentProcessor.Dtos;
using PaymentProcessor.Services;

namespace PaymentProcessor.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {

        private readonly IPaymentGateway _paymentGateway;
        public PaymentsController(IPaymentGateway paymentGateway)
        {
            _paymentGateway = paymentGateway;
        }

        /// <summary>
        /// Processes payments
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     POST /Payments
        ///     {
        ///        "CreditCardNumber": "3400 0000 0000 009",
        ///        "CardHolder": "Blessing Joel",
        ///        "Amount": 501,
        ///        "ExpirationDate": "2021-12-01"
        ///     }
        ///
        /// </remarks>
        /// <param name="payment"></param>
        /// <returns></returns>
        /// <response code="200">Returns a success message</response>
        /// <response code="400">If the item is null or invalid parameter(s) was sent.</response>
        /// <response code="500">If an unhandled exception occured.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ProcessPaymentAsync(PaymentDto payment)
        {
            try
            {
                var result = await _paymentGateway.ProcessAsync(payment);
                return Ok(new { Message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message});
            }
        }
    }
}
