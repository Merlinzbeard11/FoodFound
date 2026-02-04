namespace SeatEats.Domain.Entities;

public class MenuItem
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public string Category { get; private set; } = string.Empty;
    public bool IsAvailable { get; private set; }
    public string ImageUrl { get; private set; } = string.Empty;

    private MenuItem() { }

    public static MenuItem Create(string name, string description, decimal price, string category, string imageUrl = "")
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required", nameof(name));
        if (price < 0)
            throw new ArgumentException("Price cannot be negative", nameof(price));

        return new MenuItem
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            Price = price,
            Category = category,
            IsAvailable = true,
            ImageUrl = imageUrl
        };
    }

    public void SetAvailability(bool isAvailable) => IsAvailable = isAvailable;
}
