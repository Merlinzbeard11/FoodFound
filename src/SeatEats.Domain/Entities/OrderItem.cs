namespace SeatEats.Domain.Entities;

public class OrderItem
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid MenuItemId { get; private set; }
    public string MenuItemName { get; private set; } = string.Empty;
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal TotalPrice => Quantity * UnitPrice;

    private OrderItem() { }

    public static OrderItem Create(Guid orderId, MenuItem menuItem, int quantity)
    {
        if (quantity < 1)
            throw new ArgumentException("Quantity must be at least 1", nameof(quantity));

        return new OrderItem
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            MenuItemId = menuItem.Id,
            MenuItemName = menuItem.Name,
            Quantity = quantity,
            UnitPrice = menuItem.Price
        };
    }
}
