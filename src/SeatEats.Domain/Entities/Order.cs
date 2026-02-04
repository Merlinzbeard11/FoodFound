using SeatEats.Domain.Enums;
using SeatEats.Domain.ValueObjects;

namespace SeatEats.Domain.Entities;

public class Order
{
    public Guid Id { get; private set; }
    public SeatLocation SeatLocation { get; private set; } = null!;
    public OrderStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? EstimatedDeliveryTime { get; private set; }
    public Guid? RunnerId { get; private set; }
    public decimal TotalAmount => _items.Sum(i => i.TotalPrice);

    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    private Order() { }

    public static Order Create(SeatLocation seatLocation)
    {
        return new Order
        {
            Id = Guid.NewGuid(),
            SeatLocation = seatLocation,
            Status = OrderStatus.Received,
            CreatedAt = DateTime.UtcNow,
            EstimatedDeliveryTime = DateTime.UtcNow.AddMinutes(15)
        };
    }

    public void AddItem(MenuItem menuItem, int quantity)
    {
        if (Status != OrderStatus.Received)
            throw new InvalidOperationException("Cannot add items to an order that is already being processed");

        var existingItem = _items.FirstOrDefault(i => i.MenuItemId == menuItem.Id);
        if (existingItem != null)
        {
            _items.Remove(existingItem);
        }

        _items.Add(OrderItem.Create(Id, menuItem, quantity));
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsPreparing()
    {
        if (Status != OrderStatus.Received)
            throw new InvalidOperationException("Cannot mark order as preparing from status " + Status);

        Status = OrderStatus.Preparing;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsReady()
    {
        if (Status != OrderStatus.Preparing)
            throw new InvalidOperationException("Cannot mark order as ready from status " + Status);

        Status = OrderStatus.Ready;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ClaimByRunner(Guid runnerId)
    {
        if (Status != OrderStatus.Ready)
            throw new InvalidOperationException("Cannot claim order from status " + Status);

        RunnerId = runnerId;
        Status = OrderStatus.EnRoute;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsDelivered()
    {
        if (Status != OrderStatus.EnRoute)
            throw new InvalidOperationException("Cannot mark order as delivered from status " + Status);

        Status = OrderStatus.Delivered;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status >= OrderStatus.EnRoute)
            throw new InvalidOperationException("Cannot cancel order that is already out for delivery");

        Status = OrderStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }
}
