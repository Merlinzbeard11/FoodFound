using SeatEats.Application.Common;
using SeatEats.Application.DTOs;
using SeatEats.Application.Interfaces;
using SeatEats.Domain.Entities;
using SeatEats.Domain.Enums;
using SeatEats.Domain.ValueObjects;

namespace SeatEats.Application.Services;

public class OrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMenuRepository _menuRepository;
    private readonly IOrderNotificationService? _notificationService;

    public OrderService(
        IOrderRepository orderRepository, 
        IMenuRepository menuRepository, 
        IOrderNotificationService? notificationService = null)
    {
        _orderRepository = orderRepository;
        _menuRepository = menuRepository;
        _notificationService = notificationService;
    }

    public async Task<ServiceResult<OrderResponse>> CreateOrderAsync(
        CreateOrderRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Items == null || request.Items.Count == 0)
            return ServiceResult<OrderResponse>.Failure("Order must contain at least one item", "EMPTY_CART");

        SeatLocation seatLocation;
        try
        {
            seatLocation = SeatLocation.Create(request.Section, request.Row, request.SeatNumber);
        }
        catch (ArgumentException ex)
        {
            return ServiceResult<OrderResponse>.Failure(ex.Message, "INVALID_SEAT");
        }

        var menuItemIds = request.Items.Select(i => i.MenuItemId).ToList();
        var menuItems = await _menuRepository.GetByIdsAsync(menuItemIds, cancellationToken);

        if (menuItems.Count != menuItemIds.Count)
            return ServiceResult<OrderResponse>.Failure("One or more menu items not found", "INVALID_ITEMS");

        var unavailableItems = menuItems.Where(m => !m.IsAvailable).ToList();
        if (unavailableItems.Any())
            return ServiceResult<OrderResponse>.Failure(
                "Items not available: " + string.Join(", ", unavailableItems.Select(m => m.Name)),
                "ITEMS_UNAVAILABLE");

        var order = Order.Create(seatLocation);

        foreach (var itemRequest in request.Items)
        {
            var menuItem = menuItems.First(m => m.Id == itemRequest.MenuItemId);
            order.AddItem(menuItem, itemRequest.Quantity);
        }

        await _orderRepository.AddAsync(order, cancellationToken);

        if (_notificationService != null)
        {
            await _notificationService.NotifyNewOrderAsync(order.Id, seatLocation.DisplayText, cancellationToken);
        }

        return ServiceResult<OrderResponse>.Success(MapToResponse(order));
    }

    public async Task<ServiceResult<OrderResponse>> GetOrderAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
        if (order == null)
            return ServiceResult<OrderResponse>.Failure("Order not found", "NOT_FOUND");

        return ServiceResult<OrderResponse>.Success(MapToResponse(order));
    }

    public async Task<ServiceResult<IReadOnlyList<OrderResponse>>> GetOrdersByStatusAsync(
        OrderStatus status,
        CancellationToken cancellationToken = default)
    {
        var orders = await _orderRepository.GetByStatusAsync(status, cancellationToken);
        return ServiceResult<IReadOnlyList<OrderResponse>>.Success(
            orders.Select(MapToResponse).ToList());
    }

    public async Task<ServiceResult<OrderResponse>> UpdateOrderStatusAsync(
        Guid orderId,
        OrderStatus newStatus,
        Guid? runnerId = null,
        CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
        if (order == null)
            return ServiceResult<OrderResponse>.Failure("Order not found", "NOT_FOUND");

        try
        {
            switch (newStatus)
            {
                case OrderStatus.Preparing:
                    order.MarkAsPreparing();
                    break;
                case OrderStatus.Ready:
                    order.MarkAsReady();
                    break;
                case OrderStatus.EnRoute:
                    if (!runnerId.HasValue)
                        return ServiceResult<OrderResponse>.Failure("Runner ID required", "RUNNER_REQUIRED");
                    order.ClaimByRunner(runnerId.Value);
                    break;
                case OrderStatus.Delivered:
                    order.MarkAsDelivered();
                    break;
                case OrderStatus.Cancelled:
                    order.Cancel();
                    break;
                default:
                    return ServiceResult<OrderResponse>.Failure("Invalid status transition", "INVALID_STATUS");
            }
        }
        catch (InvalidOperationException ex)
        {
            return ServiceResult<OrderResponse>.Failure(ex.Message, "INVALID_TRANSITION");
        }

        await _orderRepository.UpdateAsync(order, cancellationToken);

        if (_notificationService != null)
        {
            await _notificationService.NotifyOrderStatusChangedAsync(order.Id, newStatus, cancellationToken);
            
            if (newStatus == OrderStatus.Ready)
            {
                await _notificationService.NotifyOrderReadyAsync(order.Id, order.SeatLocation.DisplayText, cancellationToken);
            }
            else if (newStatus == OrderStatus.EnRoute)
            {
                await _notificationService.NotifyOrderClaimedAsync(order.Id, cancellationToken);
            }
        }

        return ServiceResult<OrderResponse>.Success(MapToResponse(order));
    }

    private static OrderResponse MapToResponse(Order order)
    {
        return new OrderResponse(
            order.Id,
            order.SeatLocation.DisplayText,
            order.Status,
            order.TotalAmount,
            order.CreatedAt,
            order.EstimatedDeliveryTime,
            order.Items.Select(i => new OrderItemResponse(
                i.Id,
                i.MenuItemName,
                i.Quantity,
                i.UnitPrice,
                i.TotalPrice
            )).ToList()
        );
    }
}
