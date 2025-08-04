using FoodOrderingAPI.DTOs;
using FoodOrderingAPI.Models;
using FoodOrderingAPI.Repositories;

namespace FoodOrderingAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderStatusUpdateResponseDto> UpdateOrderStatusAsync(OrderStatusUpdateRequestDto request)
        {
            try
            {
                // Validate delivery man authorization
                var isAuthorized = await IsDeliveryManAuthorizedForOrderAsync(request.OrderId, request.DeliveryManId);
                if (!isAuthorized)
                {
                    return new OrderStatusUpdateResponseDto
                    {
                        Success = false,
                        Message = "Delivery man is not authorized to update this order",
                        OrderId = request.OrderId,
                        UpdatedAt = DateTime.Now
                    };
                }

                // Validate status transition
                var validationResult = await ValidateStatusTransitionAsync(request.OrderId, request.NewStatus);
                if (!validationResult.IsValid)
                {
                    return new OrderStatusUpdateResponseDto
                    {
                        Success = false,
                        Message = validationResult.ErrorMessage,
                        OrderId = request.OrderId,
                        CurrentStatus = validationResult.CurrentStatus,
                        UpdatedAt = DateTime.Now
                    };
                }

                // Get the order
                var order = await _orderRepository.GetOrderByIdAsync(request.OrderId);
                if (order == null)
                {
                    return new OrderStatusUpdateResponseDto
                    {
                        Success = false,
                        Message = "Order not found",
                        OrderId = request.OrderId,
                        UpdatedAt = DateTime.Now
                    };
                }

                // Update order status
                order.Status = request.NewStatus.ToString();
                
                // If status is delivered, set delivery timestamp
                if (request.NewStatus == OrderStatus.Delivered)
                {
                    order.DeliveredAt = DateTime.Now;
                }

                // Update delivery man location if provided
                if (request.Latitude.HasValue && request.Longitude.HasValue)
                {
                    // You might want to update delivery man location in a separate table or service
                    // This is a placeholder for location update logic
                }

                var updateResult = await _orderRepository.UpdateOrderStatusAsync(order);
                if (!updateResult)
                {
                    return new OrderStatusUpdateResponseDto
                    {
                        Success = false,
                        Message = "Failed to update order status",
                        OrderId = request.OrderId,
                        UpdatedAt = DateTime.Now
                    };
                }

                var saveResult = await _orderRepository.SaveChangesAsync();
                if (!saveResult)
                {
                    return new OrderStatusUpdateResponseDto
                    {
                        Success = false,
                        Message = "Failed to save changes",
                        OrderId = request.OrderId,
                        UpdatedAt = DateTime.Now
                    };
                }

                return new OrderStatusUpdateResponseDto
                {
                    Success = true,
                    Message = $"Order status successfully updated to {request.NewStatus}",
                    OrderId = request.OrderId,
                    CurrentStatus = request.NewStatus,
                    UpdatedAt = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                return new OrderStatusUpdateResponseDto
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}",
                    OrderId = request.OrderId,
                    UpdatedAt = DateTime.Now
                };
            }
        }

        public async Task<OrderStatusValidationResultDto> ValidateStatusTransitionAsync(Guid orderId, OrderStatus newStatus)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return new OrderStatusValidationResultDto
                {
                    IsValid = false,
                    ErrorMessage = "Order not found",
                    RequestedStatus = newStatus
                };
            }

            // Parse current status
            if (!Enum.TryParse<OrderStatus>(order.Status, out var currentStatus))
            {
                return new OrderStatusValidationResultDto
                {
                    IsValid = false,
                    ErrorMessage = "Invalid current order status",
                    RequestedStatus = newStatus
                };
            }

            // Check if transition is valid
            if (!currentStatus.IsValidNextStatus(newStatus))
            {
                return new OrderStatusValidationResultDto
                {
                    IsValid = false,
                    ErrorMessage = $"Cannot transition from {currentStatus} to {newStatus}. Status must follow the sequence: Preparing → OnRoute → Delivered",
                    CurrentStatus = currentStatus,
                    RequestedStatus = newStatus
                };
            }

            return new OrderStatusValidationResultDto
            {
                IsValid = true,
                ErrorMessage = string.Empty,
                CurrentStatus = currentStatus,
                RequestedStatus = newStatus
            };
        }

        public async Task<bool> IsDeliveryManAuthorizedForOrderAsync(Guid orderId, string deliveryManId)
        {
            return await _orderRepository.IsOrderAssignedToDeliveryManAsync(orderId, deliveryManId);
        }

        public async Task<IEnumerable<Order>> GetDeliveryManOrdersAsync(string deliveryManId)
        {
            return await _orderRepository.GetOrdersByDeliveryManAsync(deliveryManId);
        }
    }
}