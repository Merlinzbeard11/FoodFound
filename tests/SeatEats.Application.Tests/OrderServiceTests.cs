using FluentAssertions;
using SeatEats.Application.DTOs;
using SeatEats.Application.Interfaces;
using SeatEats.Application.Services;
using SeatEats.Domain.Entities;
using SeatEats.Domain.Enums;

namespace SeatEats.Application.Tests;

public class OrderServiceTests
{
    private readonly TestMenuRepository _menuRepository;
    private readonly TestOrderRepository _orderRepository;
    private readonly OrderService _sut;

    public OrderServiceTests()
    {
        _menuRepository = new TestMenuRepository();
        _orderRepository = new TestOrderRepository();
        _sut = new OrderService(_orderRepository, _menuRepository);
    }

    [Fact]
    public async Task CreateOrder_WithValidSeatAndItems_ReturnsOrderConfirmation()
    {
        // Arrange
        var menuItem = MenuItem.Create("Big Mac", "Burger", 5.99m, "Burgers");
        _menuRepository.Add(menuItem);

        var request = new CreateOrderRequest(
            Section: "101",
            Row: "A",
            SeatNumber: 15,
            Items: new List<OrderItemRequest>
            {
                new(menuItem.Id, 2)
            }
        );

        // Act
        var result = await _sut.CreateOrderAsync(request);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.SeatLocation.Should().Contain("101");
        result.Value.Status.Should().Be(OrderStatus.Received);
        result.Value.TotalAmount.Should().Be(11.98m);
        result.Value.EstimatedDeliveryTime.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateOrder_WithInvalidSeat_ReturnsValidationError()
    {
        // Arrange
        var menuItem = MenuItem.Create("Big Mac", "Burger", 5.99m, "Burgers");
        _menuRepository.Add(menuItem);

        var request = new CreateOrderRequest(
            Section: "",
            Row: "A",
            SeatNumber: 15,
            Items: new List<OrderItemRequest>
            {
                new(menuItem.Id, 1)
            }
        );

        // Act
        var result = await _sut.CreateOrderAsync(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be("INVALID_SEAT");
    }

    [Fact]
    public async Task CreateOrder_WithEmptyCart_ReturnsValidationError()
    {
        // Arrange
        var request = new CreateOrderRequest(
            Section: "101",
            Row: "A",
            SeatNumber: 15,
            Items: new List<OrderItemRequest>()
        );

        // Act
        var result = await _sut.CreateOrderAsync(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be("EMPTY_CART");
    }

    [Fact]
    public async Task GetOrder_WithValidId_ReturnsOrder()
    {
        // Arrange
        var menuItem = MenuItem.Create("Big Mac", "Burger", 5.99m, "Burgers");
        _menuRepository.Add(menuItem);

        var createRequest = new CreateOrderRequest(
            Section: "101",
            Row: "A",
            SeatNumber: 15,
            Items: new List<OrderItemRequest> { new(menuItem.Id, 1) }
        );
        var createResult = await _sut.CreateOrderAsync(createRequest);
        var orderId = createResult.Value!.Id;

        // Act
        var result = await _sut.GetOrderAsync(orderId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Id.Should().Be(orderId);
    }

    [Fact]
    public async Task UpdateOrderStatus_Preparing_UpdatesStatus()
    {
        // Arrange
        var menuItem = MenuItem.Create("Big Mac", "Burger", 5.99m, "Burgers");
        _menuRepository.Add(menuItem);

        var createRequest = new CreateOrderRequest(
            Section: "101",
            Row: "A",
            SeatNumber: 15,
            Items: new List<OrderItemRequest> { new(menuItem.Id, 1) }
        );
        var createResult = await _sut.CreateOrderAsync(createRequest);
        var orderId = createResult.Value!.Id;

        // Act
        var result = await _sut.UpdateOrderStatusAsync(orderId, OrderStatus.Preparing);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Status.Should().Be(OrderStatus.Preparing);
    }
}

// Test doubles
public class TestMenuRepository : IMenuRepository
{
    private readonly Dictionary<Guid, MenuItem> _items = new();

    public void Add(MenuItem item) => _items[item.Id] = item;

    public Task<IReadOnlyList<MenuItem>> GetAllAsync(CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<MenuItem>>(_items.Values.ToList());

    public Task<IReadOnlyList<MenuItem>> GetAvailableAsync(CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<MenuItem>>(_items.Values.Where(x => x.IsAvailable).ToList());

    public Task<MenuItem?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult(_items.GetValueOrDefault(id));

    public Task<IReadOnlyList<MenuItem>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<MenuItem>>(_items.Values.Where(x => ids.Contains(x.Id)).ToList());
}

public class TestOrderRepository : IOrderRepository
{
    private readonly Dictionary<Guid, Order> _orders = new();

    public Task<Order?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult(_orders.GetValueOrDefault(id));

    public Task<IReadOnlyList<Order>> GetByStatusAsync(OrderStatus status, CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<Order>>(_orders.Values.Where(x => x.Status == status).ToList());

    public Task<IReadOnlyList<Order>> GetActiveOrdersAsync(CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<Order>>(_orders.Values.ToList());

    public Task AddAsync(Order order, CancellationToken ct = default)
    {
        _orders[order.Id] = order;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Order order, CancellationToken ct = default)
    {
        _orders[order.Id] = order;
        return Task.CompletedTask;
    }
}
