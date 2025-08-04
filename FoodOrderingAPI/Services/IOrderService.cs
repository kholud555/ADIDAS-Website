using FoodOrderingAPI.DTOs;
using FoodOrderingAPI.Models;

namespace FoodOrderingAPI.Services
{
    public interface IOrderService
    {
        Task<OrderStatusUpdateResponseDto> UpdateOrderStatusAsync(OrderStatusUpdateRequestDto request);
        Task<OrderStatusValidationResultDto> ValidateStatusTransitionAsync(Guid orderId, OrderStatus newStatus);
        Task<bool> IsDeliveryManAuthorizedForOrderAsync(Guid orderId, string deliveryManId);
        Task<IEnumerable<Order>> GetDeliveryManOrdersAsync(string deliveryManId);
    }
}