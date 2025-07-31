using System.ComponentModel.DataAnnotations;

namespace FoodOrderingAPI.Models
{
    public enum OrderStatus
    {
        Preparing = 1,
        OnRoute = 2,
        Delivered = 3
    }

    public static class OrderStatusExtensions
    {
        public static bool CanTransitionTo(this OrderStatus currentStatus, OrderStatus newStatus)
        {
            return (int)newStatus == (int)currentStatus + 1;
        }

        public static bool IsValidNextStatus(this OrderStatus currentStatus, OrderStatus newStatus)
        {
            return currentStatus switch
            {
                OrderStatus.Preparing => newStatus == OrderStatus.OnRoute,
                OrderStatus.OnRoute => newStatus == OrderStatus.Delivered,
                OrderStatus.Delivered => false, // Cannot change from delivered
                _ => false
            };
        }
    }
}