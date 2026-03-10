namespace Portfolio.Web.Services;

public class MenuService(PortfolioApiService apiService)
{
    public async Task<List<Portfolio.Shared.Models.MenuItemDto>> GetVisibleItemsAsync()
        => await apiService.GetMenuItemsAsync();

    public async Task<List<Portfolio.Shared.Models.MenuItemDto>> GetAllForAdminAsync(string adminToken)
        => await apiService.GetMenuItemsForAdminAsync(adminToken);

    public async Task<Portfolio.Shared.Models.MenuItemDto> CreateAsync(Portfolio.Shared.Models.MenuItemDto item, string adminToken)
    {
        var (result, _) = await apiService.CreateMenuItemAsync(item, adminToken);
        return result ?? item;
    }

    public async Task UpdateAsync(Portfolio.Shared.Models.MenuItemDto item, string adminToken)
        => await apiService.UpdateMenuItemAsync(item.Id, item, adminToken);

    public async Task DeleteAsync(int id, string adminToken)
        => await apiService.DeleteMenuItemAsync(id, adminToken);
}
