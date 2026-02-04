using System.Collections.Concurrent;
using SeatEats.Application.Interfaces;
using SeatEats.Domain.Entities;

namespace SeatEats.Web.Infrastructure;

public class InMemoryMenuRepository : IMenuRepository
{
    private readonly ConcurrentDictionary<Guid, MenuItem> _menuItems;

    public InMemoryMenuRepository()
    {
        _menuItems = new ConcurrentDictionary<Guid, MenuItem>();
        SeedData();
    }

    private void SeedData()
    {
        var items = new[]
        {
            MenuItem.Create("Big Mac", "Two all-beef patties, special sauce, lettuce, cheese, pickles, onions on a sesame seed bun", 5.99m, "Burgers", "/images/bigmac.png"),
            MenuItem.Create("Quarter Pounder with Cheese", "Fresh beef patty with cheese, onions, pickles, ketchup, and mustard", 6.49m, "Burgers", "/images/quarterpounder.png"),
            MenuItem.Create("McChicken", "Crispy chicken patty with mayo and shredded lettuce", 4.49m, "Chicken", "/images/mcchicken.png"),
            MenuItem.Create("10 Piece McNuggets", "Tender chicken McNuggets with your choice of dipping sauce", 6.99m, "Chicken", "/images/nuggets.png"),
            MenuItem.Create("Large Fries", "Golden, crispy world-famous fries", 3.99m, "Sides", "/images/fries.png"),
            MenuItem.Create("Medium Fries", "Golden, crispy world-famous fries", 2.99m, "Sides", "/images/fries.png"),
            MenuItem.Create("Coca-Cola Large", "Ice-cold Coca-Cola", 2.99m, "Drinks", "/images/coke.png"),
            MenuItem.Create("Sprite Large", "Ice-cold Sprite", 2.99m, "Drinks", "/images/sprite.png"),
            MenuItem.Create("McFlurry Oreo", "Creamy vanilla soft serve with Oreo cookie pieces", 4.49m, "Desserts", "/images/mcflurry.png"),
            MenuItem.Create("Apple Pie", "Warm, crispy pie with sweet apple filling", 2.49m, "Desserts", "/images/applepie.png")
        };

        foreach (var item in items)
        {
            _menuItems.TryAdd(item.Id, item);
        }
    }

    public Task<IReadOnlyList<MenuItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyList<MenuItem>>(_menuItems.Values.ToList());
    }

    public Task<IReadOnlyList<MenuItem>> GetAvailableAsync(CancellationToken cancellationToken = default)
    {
        var available = _menuItems.Values.Where(m => m.IsAvailable).ToList();
        return Task.FromResult<IReadOnlyList<MenuItem>>(available);
    }

    public Task<MenuItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _menuItems.TryGetValue(id, out var item);
        return Task.FromResult(item);
    }

    public Task<IReadOnlyList<MenuItem>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        var idSet = ids.ToHashSet();
        var items = _menuItems.Values.Where(m => idSet.Contains(m.Id)).ToList();
        return Task.FromResult<IReadOnlyList<MenuItem>>(items);
    }
}
