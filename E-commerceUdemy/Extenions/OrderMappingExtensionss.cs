using Core.Entities.OrderAggregate;
using E_commerceUdemy.DTOs;

namespace E_commerceUdemy.Extenions
{
    public static class OrderMappingExtensionss
    {
        public static OrderDto ToDto(this Order order)
        {
            return new OrderDto
            {

                Id = order.Id,
                BuyerEmail = order.BuyerEmail,
                OrderDate = order.OrderDate,
                ShippingAddress = order.ShippingAddress,
                PaymentSummary = order.PaymentSummary,
                DeliveryMethod = order.DeliveryMethod.Description,
                ShippingPrice = order.DeliveryMethod.Price,
                OrderItems = order.OrderItems.Select(x=>x.ToDto()).ToList(),
                Total=order.GetTotal(),
                Subtotal = order.Subtotal,
                Status = order.Status.ToString(),
                PaymentIntentId=order.PaymentIntentId,
            };
        }

        public static OrderItemDto ToDto(this OrderItem orderItem)
        {
            return new OrderItemDto
            {
                ProductId = orderItem.ItemOrdered.ProductId,
                ProductName = orderItem.ItemOrdered.ProductName,
                PictureUrl = orderItem.ItemOrdered.PictureUrl,
                Price = orderItem.Price,
                Quantity = orderItem.Quantity,
            };
        }
    }
}
