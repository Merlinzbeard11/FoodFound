using SeatEats.Domain.Enums;

namespace SeatEats.Application.DTOs;

public record OrderResponse(
    Guid Id,
    string SeatLocation,
    OrderStatus Status,
    decimal TotalAmount,
    DateTime CreatedAt,
    DateTime? EstimatedDeliveryTime,
    List<OrderItemResponse> Items
);

public record OrderItemResponse(
    Guid Id,
    string Name,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice
);
