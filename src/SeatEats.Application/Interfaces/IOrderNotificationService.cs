using SeatEats.Domain.Enums;

namespace SeatEats.Application.Interfaces;

public interface IOrderNotificationService
{
    Task NotifyOrderStatusChangedAsync(Guid orderId, OrderStatus newStatus, CancellationToken cancellationToken = default);
    Task NotifyNewOrderAsync(Guid orderId, string seatLocation, CancellationToken cancellationToken = default);
    Task NotifyOrderReadyAsync(Guid orderId, string seatLocation, CancellationToken cancellationToken = default);
    Task NotifyOrderClaimedAsync(Guid orderId, CancellationToken cancellationToken = default);
}
