using Microsoft.AspNetCore.Mvc;
using SeatEats.Application.DTOs;
using SeatEats.Application.Services;
using SeatEats.Domain.Enums;

namespace SeatEats.Web.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderApiController : ControllerBase
{
    private readonly OrderService _orderService;
    private readonly MenuService _menuService;

    public OrderApiController(OrderService orderService, MenuService menuService)
    {
        _orderService = orderService;
        _menuService = menuService;
    }

    [HttpGet("menu")]
    public async Task<IActionResult> GetMenu()
    {
        var result = await _menuService.GetAvailableMenuAsync();
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var result = await _orderService.CreateOrderAsync(request);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(OrderStatus status)
    {
        var result = await _orderService.GetOrdersByStatusAsync(status);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        var result = await _orderService.GetOrderAsync(id);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateStatusRequest request)
    {
        var result = await _orderService.UpdateOrderStatusAsync(id, request.Status, request.RunnerId);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}

public record UpdateStatusRequest(OrderStatus Status, Guid? RunnerId = null);
