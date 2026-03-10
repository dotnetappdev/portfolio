namespace Portfolio.Web.Services;

public class AppSettingsService(PortfolioApiService apiService)
{
    public Task<Portfolio.Shared.Models.AppSettingsDto?> GetAsync(string adminToken)
        => apiService.GetAppSettingsAsync(adminToken);

    public Task<(bool Success, string? Error)> SaveAsync(Portfolio.Shared.Models.AppSettingsDto dto, string adminToken)
        => apiService.SaveAppSettingsAsync(dto, adminToken);
}
