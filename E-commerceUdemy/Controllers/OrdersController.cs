using Api.Controllers;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using E_commerceUdemy.DTOs;
using E_commerceUdemy.Extenions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace E_commerceUdemy.Controllers
{
    [Authorize]
    public class OrdersController(ICartService cartService,IUnitOfWork unit) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult<Core.Entities.OrderAggregate.Order>> CreateOrder(CreateOrderDTO orderDTO)
        {
            var email=User.GetEmail();

            var cart = await cartService.GetCartAsync(orderDTO.CartId);
            if (cart == null) return BadRequest("Cart not found");

            if (cart.PaymentIntentId == null) return BadRequest("No Paymnet intent for this order ");

            var items=new List<OrderItem>();

            foreach (var item in cart.Items)
            {
                var productItem=await unit.Repository<Product>().GetByIdAsync(item.ProductId);
                if (productItem == null) return BadRequest("Problem with the order");
                var itemOrderd=new ProductItemOrdered 
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    PictureUrl = productItem.PictureUrl ,
                };

                var orderItem = new OrderItem
                {
                    ItemOrdered = itemOrderd,
                    Price = productItem.Price,
                    Quantity = item.Quantity
                };
                items.Add(orderItem);


            }

            var deliveryMethod=await unit.Repository<DeliveryMethod>().GetByIdAsync(orderDTO.DeliveryMethodId);

            var order = new Core.Entities.OrderAggregate.Order
            {
                OrderItems = items,
                DeliveryMethod = deliveryMethod,
                ShippingAddress = orderDTO.ShippingAddress,
                Subtotal = items.Sum(x => x.Price * x.Quantity),
                PaymentSummary = orderDTO.PaymentSummary,
                PaymentIntentId = cart.PaymentIntentId,
                BuyerEmail = email
            };

            unit.Repository<Order>().Add(order);
            if(await unit.Complete())
            {
                return order;
            }

            return BadRequest("Problem creating order");

        }


        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrdersForUser()
        {
            var spec = new OrderSpecification(User.GetEmail());
            var orders=await unit.Repository<Order>().ListAsync(spec);

            var ordersToReturn=orders.Select(o=>o.ToDto()).ToList();

            return Ok(ordersToReturn);
        }

        [HttpGet("{id:int}")]   
        public async Task<ActionResult<OrderDto>> GetOrderById(int id)
        {
            var spec=new OrderSpecification(User.GetEmail(),id);
            var order=await unit.Repository<Order>().GetEntityWithSpec(spec);

            if (order == null) return NotFound(); 
            
        return order.ToDto();

        }





    }
}
