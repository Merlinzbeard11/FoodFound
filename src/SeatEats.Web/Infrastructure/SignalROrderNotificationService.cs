using Microsoft.AspNetCore.SignalR;
using SeatEats.Application.Interfaces;
using SeatEats.Domain.Enums;
using SeatEats.Web.Hubs;

namespace SeatEats.Web.Infrastructure;

public class SignalROrderNotificationService : IOrderNotificationService
{
    private readonly IHubContext<OrderHub, IOrderHubClient> _hubContext;

    public SignalROrderNotificationService(IHubContext<OrderHub, IOrderHubClient> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyOrderStatusChangedAsync(Guid orderId, OrderStatus newStatus, CancellationToken cancellationToken = default)
    {
        await _hubContext.Clients.Group($"order-{orderId}").OrderStatusChanged(orderId, newStatus);
        
        if (newStatus == OrderStatus.Ready)
        {
            await _hubContext.Clients.Group("runners").OrderStatusChanged(orderId, newStatus);
        }
        
        await _hubContext.Clients.Group("kitchen").OrderStatusChanged(orderId, newStatus);
    }

    public async Task NotifyNewOrderAsync(Guid orderId, string seatLocation, CancellationToken cancellationToken = default)
    {
        await _hubContext.Clients.Group("kitchen").NewOrderReceived(orderId, seatLocation);
    }

    public async Task NotifyOrderReadyAsync(Guid orderId, string seatLocation, CancellationToken cancellationToken = default)
    {
        await _hubContext.Clients.Group("runners").OrderReady(orderId, seatLocation);
    }

    public async Task NotifyOrderClaimedAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        await _hubContext.Clients.Group($"order-{orderId}").OrderClaimed(orderId);
        await _hubContext.Clients.Group("runners").OrderClaimed(orderId);
    }
}
