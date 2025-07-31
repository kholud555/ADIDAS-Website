# Food Ordering API - Order Status Update System

This .NET 8 Web API provides a comprehensive system for delivery personnel to update order statuses with proper validation and sequencing.

## Features

- **Status Sequence Validation**: Ensures orders follow the correct status progression: `Preparing` → `OnRoute` → `Delivered`
- **Authorization Checks**: Validates that only assigned delivery personnel can update order status
- **Repository Pattern**: Clean separation of data access logic
- **Service Layer**: Business logic encapsulation with comprehensive validation
- **RESTful API**: Well-structured endpoints with proper HTTP status codes
- **Entity Framework Integration**: Full ORM support with proper relationships

## Project Structure

```
FoodOrderingAPI/
├── Controllers/
│   └── OrderController.cs          # API endpoints
├── Services/
│   ├── IOrderService.cs           # Service interface
│   └── OrderService.cs            # Business logic implementation
├── Repositories/
│   ├── IOrderRepository.cs        # Repository interface
│   └── OrderRepository.cs         # Data access implementation
├── Models/
│   ├── OrderStatus.cs             # Status enum with validation extensions
│   ├── Order.cs                   # Order entity (provided)
│   └── DeliveryMan.cs            # DeliveryMan entity (provided)
├── DTOs/
│   └── OrderStatusUpdateDto.cs    # Request/Response DTOs
├── Data/
│   └── ApplicationDbContext.cs    # Entity Framework context
├── Program.cs                     # Application configuration
├── appsettings.json              # Configuration settings
└── FoodOrderingAPI.csproj        # Project dependencies
```

## Order Status Flow

The system enforces a strict status progression:

1. **Preparing** (Initial status when order is assigned to delivery man)
2. **OnRoute** (Order picked up and in transit)
3. **Delivered** (Order successfully delivered)

### Validation Rules

- Cannot skip statuses (e.g., cannot go directly from `Preparing` to `Delivered`)
- Cannot go backwards in status
- Cannot change status once `Delivered`
- Only assigned delivery personnel can update order status

## API Endpoints

### 1. Update Order Status
```
PUT /api/order/update-status
```

**Request Body:**
```json
{
  "orderId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "newStatus": 2,
  "deliveryManId": "delivery-man-123",
  "latitude": 40.7128,
  "longitude": -74.0060
}
```

**Response:**
```json
{
  "success": true,
  "message": "Order status successfully updated to OnRoute",
  "orderId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "currentStatus": 2,
  "updatedAt": "2024-01-15T10:30:00Z"
}
```

### 2. Validate Status Transition
```
GET /api/order/{orderId}/validate-status-transition?newStatus=2
```

### 3. Get Delivery Man Orders
```
GET /api/order/delivery-man/{deliveryManId}
```

### 4. Check Authorization
```
GET /api/order/{orderId}/check-authorization/{deliveryManId}
```

## Status Enum Values

```csharp
public enum OrderStatus
{
    Preparing = 1,
    OnRoute = 2,
    Delivered = 3
}
```

## Key Components

### OrderService
- **UpdateOrderStatusAsync**: Main method for status updates with full validation
- **ValidateStatusTransitionAsync**: Validates if status change is allowed
- **IsDeliveryManAuthorizedForOrderAsync**: Checks authorization

### OrderRepository
- **GetOrderByIdAsync**: Retrieves order by ID
- **UpdateOrderStatusAsync**: Updates order in database
- **IsOrderAssignedToDeliveryManAsync**: Validates assignment

### Status Validation Extensions
```csharp
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
```

## Error Handling

The API provides comprehensive error handling with descriptive messages:

- **400 Bad Request**: Invalid status transitions, unauthorized access, validation errors
- **404 Not Found**: Order not found
- **500 Internal Server Error**: Unexpected server errors

## Database Configuration

Update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your-server;Database=FoodOrderingDB;Trusted_Connection=true;"
  }
}
```

## Dependencies

- **Microsoft.EntityFrameworkCore.SqlServer**: Database provider
- **Microsoft.AspNetCore.OpenApi**: API documentation
- **Swashbuckle.AspNetCore**: Swagger UI integration

## Usage Example

1. **Start with Preparing Status**: Order is assigned to delivery man
2. **Update to OnRoute**: Delivery man picks up order
   ```bash
   curl -X PUT "https://localhost:7000/api/order/update-status" \
   -H "Content-Type: application/json" \
   -d '{
     "orderId": "order-guid",
     "newStatus": 2,
     "deliveryManId": "delivery-man-id"
   }'
   ```
3. **Update to Delivered**: Order is delivered to customer
   ```bash
   curl -X PUT "https://localhost:7000/api/order/update-status" \
   -H "Content-Type: application/json" \
   -d '{
     "orderId": "order-guid",
     "newStatus": 3,
     "deliveryManId": "delivery-man-id"
   }'
   ```

## Security Considerations

- Authorization validation ensures only assigned delivery personnel can update orders
- Model validation prevents invalid data submission
- Proper error handling prevents information leakage

## Future Enhancements

- JWT authentication for delivery personnel
- Real-time notifications for status updates
- GPS tracking integration
- Order history and analytics
- Push notifications to customers