using Api.Controllers;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_commerceUdemy.Controllers
{
    public class PaymentsController(IPaymentService paymentService,
        IUnitOfWork unit) : BaseApiController
    {
        [Authorize]
        [HttpPost("{cartId}")]
        public async Task<ActionResult<ShoppingCart>> CreateOrUpdatePaymentIntent(string cartId)
        {
            var cart=await paymentService.CreateOrUpdatePaymnetIntent(cartId);
            if (cart == null) return BadRequest("Problem with your cart");
            
            return Ok(cart);
        }
        [HttpGet("delivery-methods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            return Ok(await unit.Repository<DeliveryMethod>().ListAllAsync());
        }

    }
}
