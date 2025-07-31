using System.ComponentModel.DataAnnotations;
using FoodOrderingAPI.Models;

namespace FoodOrderingAPI.DTOs
{
    public class OrderStatusUpdateRequestDto
    {
        [Required]
        public Guid OrderId { get; set; }

        [Required]
        [EnumDataType(typeof(OrderStatus))]
        public OrderStatus NewStatus { get; set; }

        [Required]
        public string DeliveryManId { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }

    public class OrderStatusUpdateResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Guid OrderId { get; set; }
        public OrderStatus CurrentStatus { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class OrderStatusValidationResultDto
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
        public OrderStatus CurrentStatus { get; set; }
        public OrderStatus RequestedStatus { get; set; }
    }
}