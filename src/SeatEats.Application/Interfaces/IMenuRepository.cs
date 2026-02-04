using SeatEats.Domain.Entities;

namespace SeatEats.Application.Interfaces;

public interface IMenuRepository
{
    Task<IReadOnlyList<MenuItem>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<MenuItem>> GetAvailableAsync(CancellationToken cancellationToken = default);
    Task<MenuItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<MenuItem>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
}
