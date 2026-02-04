namespace SeatEats.Application.DTOs;

public record CreateOrderRequest(
    string Section,
    string Row,
    int SeatNumber,
    List<OrderItemRequest> Items
);

public record OrderItemRequest(
    Guid MenuItemId,
    int Quantity
);
