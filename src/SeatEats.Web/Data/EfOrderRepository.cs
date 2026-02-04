using Microsoft.EntityFrameworkCore;
using SeatEats.Application.Interfaces;
using SeatEats.Domain.Entities;
using SeatEats.Domain.Enums;

namespace SeatEats.Web.Data;

public class EfOrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public EfOrderRepository(AppDbContext context) => _context = context;

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Orders.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Order>> GetByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default)
        => await _context.Orders.Where(o => o.Status == status).OrderBy(o => o.CreatedAt).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Order>> GetActiveOrdersAsync(CancellationToken cancellationToken = default)
        => await _context.Orders.Where(o => o.Status != OrderStatus.Delivered && o.Status != OrderStatus.Cancelled)
            .OrderBy(o => o.CreatedAt).ToListAsync(cancellationToken);

    public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        await _context.Orders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
