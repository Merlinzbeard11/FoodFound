using Microsoft.EntityFrameworkCore;
using SeatEats.Application.Interfaces;
using SeatEats.Domain.Entities;

namespace SeatEats.Web.Data;

public class EfMenuRepository : IMenuRepository
{
    private readonly AppDbContext _context;

    public EfMenuRepository(AppDbContext context) => _context = context;

    public async Task<IReadOnlyList<MenuItem>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.MenuItems.ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<MenuItem>> GetAvailableAsync(CancellationToken cancellationToken = default)
        => await _context.MenuItems.Where(m => m.IsAvailable).ToListAsync(cancellationToken);

    public async Task<MenuItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.MenuItems.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

    public async Task<IReadOnlyList<MenuItem>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        => await _context.MenuItems.Where(m => ids.Contains(m.Id)).ToListAsync(cancellationToken);
}
