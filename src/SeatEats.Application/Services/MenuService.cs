using SeatEats.Application.Common;
using SeatEats.Application.DTOs;
using SeatEats.Application.Interfaces;

namespace SeatEats.Application.Services;

public class MenuService
{
    private readonly IMenuRepository _menuRepository;

    public MenuService(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }

    public async Task<ServiceResult<IReadOnlyList<MenuItemResponse>>> GetAvailableMenuAsync(
        CancellationToken cancellationToken = default)
    {
        var items = await _menuRepository.GetAvailableAsync(cancellationToken);
        var response = items.Select(item => new MenuItemResponse(
            item.Id,
            item.Name,
            item.Description,
            item.Price,
            item.Category,
            item.IsAvailable,
            item.ImageUrl
        )).ToList();

        return ServiceResult<IReadOnlyList<MenuItemResponse>>.Success(response);
    }
}
