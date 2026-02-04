using System.Collections.Concurrent;
using SeatEats.Application.Interfaces;
using SeatEats.Domain.Entities;
using SeatEats.Domain.Enums;

namespace SeatEats.Web.Infrastructure;

public class InMemoryOrderRepository : IOrderRepository
{
    private readonly ConcurrentDictionary<Guid, Order> _orders = new();

    public Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _orders.TryGetValue(id, out var order);
        return Task.FromResult(order);
    }

    public Task<IReadOnlyList<Order>> GetByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default)
    {
        var orders = _orders.Values.Where(o => o.Status == status).ToList();
        return Task.FromResult<IReadOnlyList<Order>>(orders);
    }

    public Task<IReadOnlyList<Order>> GetActiveOrdersAsync(CancellationToken cancellationToken = default)
    {
        var orders = _orders.Values
            .Where(o => o.Status != OrderStatus.Delivered && o.Status != OrderStatus.Cancelled)
            .OrderBy(o => o.CreatedAt)
            .ToList();
        return Task.FromResult<IReadOnlyList<Order>>(orders);
    }

    public Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        _orders.TryAdd(order.Id, order);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
    {
        _orders[order.Id] = order;
        return Task.CompletedTask;
    }
}
