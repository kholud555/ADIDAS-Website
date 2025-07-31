using Microsoft.AspNetCore.Mvc;
using FoodOrderingAPI.DTOs;
using FoodOrderingAPI.Services;
using FoodOrderingAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace FoodOrderingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Update order status by delivery man
        /// </summary>
        /// <param name="request">Order status update request</param>
        /// <returns>Order status update response</returns>
        [HttpPut("update-status")]
        public async Task<ActionResult<OrderStatusUpdateResponseDto>> UpdateOrderStatus([FromBody] OrderStatusUpdateRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new OrderStatusUpdateResponseDto
                {
                    Success = false,
                    Message = "Invalid request data",
                    OrderId = request?.OrderId ?? Guid.Empty,
                    UpdatedAt = DateTime.Now
                });
            }

            var result = await _orderService.UpdateOrderStatusAsync(request);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Validate if a status transition is allowed
        /// </summary>
        /// <param name="orderId">Order ID</param>
        /// <param name="newStatus">New status to transition to</param>
        /// <returns>Validation result</returns>
        [HttpGet("{orderId}/validate-status-transition")]
        public async Task<ActionResult<OrderStatusValidationResultDto>> ValidateStatusTransition(
            Guid orderId, 
            [FromQuery] OrderStatus newStatus)
        {
            var result = await _orderService.ValidateStatusTransitionAsync(orderId, newStatus);
            
            if (!result.IsValid)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Get all orders assigned to a delivery man
        /// </summary>
        /// <param name="deliveryManId">Delivery man ID</param>
        /// <returns>List of orders</returns>
        [HttpGet("delivery-man/{deliveryManId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetDeliveryManOrders(string deliveryManId)
        {
            if (string.IsNullOrWhiteSpace(deliveryManId))
            {
                return BadRequest("Delivery man ID is required");
            }

            var orders = await _orderService.GetDeliveryManOrdersAsync(deliveryManId);
            return Ok(orders);
        }

        /// <summary>
        /// Check if delivery man is authorized to update a specific order
        /// </summary>
        /// <param name="orderId">Order ID</param>
        /// <param name="deliveryManId">Delivery man ID</param>
        /// <returns>Authorization status</returns>
        [HttpGet("{orderId}/check-authorization/{deliveryManId}")]
        public async Task<ActionResult<bool>> CheckDeliveryManAuthorization(Guid orderId, string deliveryManId)
        {
            if (string.IsNullOrWhiteSpace(deliveryManId))
            {
                return BadRequest("Delivery man ID is required");
            }

            var isAuthorized = await _orderService.IsDeliveryManAuthorizedForOrderAsync(orderId, deliveryManId);
            return Ok(new { IsAuthorized = isAuthorized });
        }
    }
}