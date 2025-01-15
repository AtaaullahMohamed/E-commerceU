using Api.Controllers;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using E_commerceUdemy.Extenions;
using E_commerceUdemy.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Stripe;

namespace E_commerceUdemy.Controllers
{
    public class PaymentsController(IPaymentService paymentService,
     IUnitOfWork unit, ILogger<PaymentsController> logger,
     IConfiguration config , IHubContext<NotificationHub> hubContext) : BaseApiController
    {
        private readonly string _whSecret = config["StripeSettings:WhSecret"]!;

        [Authorize]
        [HttpPost("{cartId}")]
        public async Task<ActionResult<ShoppingCart>> CreateOrUpdatePaymentIntent(string cartId)
        {
            var cart = await paymentService.CreateOrUpdatePaymnetIntent(cartId);

            if (cart == null) return BadRequest("Problem with your cart");

            return Ok(cart);
        }

        [HttpGet("delivery-methods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            return Ok(await unit.Repository<DeliveryMethod>().ListAllAsync());
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = ConstructStripeEvent(json);

                // Handle only specific event types
                if (stripeEvent.Type == "payment_intent.succeeded")
                {
                    var intent = stripeEvent.Data.Object as PaymentIntent;
                    if (intent == null)
                    {
                        logger.LogWarning("Invalid payment intent data");
                        return BadRequest("Invalid payment intent data");
                    }

                    await HandlePaymentIntentSucceeded(intent);
                    return Ok();
                }

                logger.LogInformation($"Unhandled event type: {stripeEvent.Type}");
                return Ok(); // Acknowledge other events without action
            }
            catch (StripeException ex)
            {
                logger.LogError(ex, "Stripe webhook error");
                return StatusCode(StatusCodes.Status500InternalServerError, "Stripe webhook error");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
            }
        }

        private async Task HandlePaymentIntentSucceeded(PaymentIntent intent)
        {
            if (intent.Status != "succeeded")
            {
                logger.LogWarning("Payment intent is not in a succeeded state");
                return;
            }

            var spec = new OrderSpecification(intent.Id, true);
            var order = await unit.Repository<Order>().GetEntityWithSpec(spec);

            if (order == null)
            {
                logger.LogError($"Order not found for PaymentIntent ID: {intent.Id}");
                throw new Exception("Order not found");
            }

            var orderTotalInCents = (long)Math.Round(order.GetTotal() * 100, MidpointRounding.AwayFromZero);

            if (orderTotalInCents != intent.Amount)
            {
                order.Status = OrderStatus.PaymentMismatch;
                logger.LogWarning($"Payment mismatch: Expected {orderTotalInCents}, Received {intent.Amount}");
            }
            else
            {
                order.Status = OrderStatus.PaymentReceived;
                logger.LogInformation($"Payment received for order {order.Id}");
            }

            await unit.Complete();

            var connectionId = NotificationHub.GetConnectionIdByEmail(order.BuyerEmail);

            if (!string.IsNullOrEmpty(connectionId))
            {
                await hubContext.Clients.Client(connectionId).SendAsync("OrderCompleteNotification",order.ToDto());
            }

        }

        private Event ConstructStripeEvent(string json)
        {
            try
            {
                return EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _whSecret);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to construct Stripe event");
                throw new StripeException("Invalid signature");
            }
        }
    }
}