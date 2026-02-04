namespace SeatEats.Application.DTOs;

public record MenuItemResponse(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string Category,
    bool IsAvailable,
    string ImageUrl
);
