using FoodOrderingAPI.Models;

namespace FoodOrderingAPI.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderByIdAsync(Guid orderId);
        Task<Order> GetOrderByIdWithDeliveryManAsync(Guid orderId);
        Task<bool> UpdateOrderStatusAsync(Order order);
        Task<bool> IsOrderAssignedToDeliveryManAsync(Guid orderId, string deliveryManId);
        Task<IEnumerable<Order>> GetOrdersByDeliveryManAsync(string deliveryManId);
        Task<bool> SaveChangesAsync();
    }
}