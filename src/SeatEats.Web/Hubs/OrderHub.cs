using Microsoft.AspNetCore.SignalR;
using SeatEats.Domain.Enums;

namespace SeatEats.Web.Hubs;

public interface IOrderHubClient
{
    Task OrderStatusChanged(Guid orderId, OrderStatus newStatus);
    Task NewOrderReceived(Guid orderId, string seatLocation);
    Task OrderReady(Guid orderId, string seatLocation);
    Task OrderClaimed(Guid orderId);
}

public class OrderHub : Hub<IOrderHubClient>
{
    public async Task JoinOrderGroup(string orderId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"order-{orderId}");
    }

    public async Task LeaveOrderGroup(string orderId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"order-{orderId}");
    }

    public async Task JoinKitchenGroup()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "kitchen");
    }

    public async Task JoinRunnerGroup()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "runners");
    }
}
