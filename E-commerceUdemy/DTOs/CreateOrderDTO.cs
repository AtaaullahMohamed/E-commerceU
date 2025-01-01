using Core.Entities.OrderAggregate;
using System.ComponentModel.DataAnnotations;

namespace E_commerceUdemy.DTOs
{
    public class CreateOrderDTO
    {
        [Required]
        public string CartId { get; set; }
        [Required]
        public int DeliveryMethodId { get; set; }
        [Required]
        public ShippingAddress ShippingAddress { get; set; } = null!;
        [Required]
        public PaymentSummary PaymentSummary { get; set; }=null!;
    }
}
