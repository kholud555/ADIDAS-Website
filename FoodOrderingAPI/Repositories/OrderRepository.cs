using FoodOrderingAPI.Models;
using FoodOrderingAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingAPI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            return await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderID == orderId);
        }

        public async Task<Order> GetOrderByIdWithDeliveryManAsync(Guid orderId)
        {
            return await _context.Orders
                .Include(o => o.DeliveryMan)
                .FirstOrDefaultAsync(o => o.OrderID == orderId);
        }

        public async Task<bool> UpdateOrderStatusAsync(Order order)
        {
            try
            {
                _context.Orders.Update(order);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsOrderAssignedToDeliveryManAsync(Guid orderId, string deliveryManId)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderID == orderId);
            
            return order != null && order.DeliveryManID == deliveryManId;
        }

        public async Task<IEnumerable<Order>> GetOrdersByDeliveryManAsync(string deliveryManId)
        {
            return await _context.Orders
                .Where(o => o.DeliveryManID == deliveryManId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}