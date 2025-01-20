using App.Core.Dto;
using App.Core.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EComApplicationBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ISaleService _saleService;


        public PaymentController(IPaymentService paymentService, ISaleService saleService)
        {
            _paymentService = paymentService;
            _saleService = saleService;
        }

        [HttpPost("card-details")]
        public async Task<IActionResult> verifyCardDetails(CardDto cardDto)
        {
            var result = await _paymentService.verifyCard(cardDto);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("generate-invoice")]
        public async Task<IActionResult> invoice(InvoiceDto invoiceDto)
        {
            if(invoiceDto != null)
            {
                await _saleService.populateSalesMasterAndSalesDetails(invoiceDto);
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet("generate-reciept")]
        public async Task<IActionResult> generateReciept(int userId)
        {
            if(userId !=0 || userId != null)
            {
                var result = await _saleService.generateReciept(userId);
                return Ok(result);
            }
            return BadRequest();
        }
    }
}