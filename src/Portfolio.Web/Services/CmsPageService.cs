namespace Portfolio.Web.Services;

public class CmsPageService(PortfolioApiService apiService)
{
    public async Task<Portfolio.Shared.Models.CmsPageDto?> GetBySlugAsync(string slug)
        => await apiService.GetCmsPageBySlugAsync(slug);

    public async Task<List<Portfolio.Shared.Models.CmsPageDto>> GetAllForAdminAsync(string adminToken)
        => await apiService.GetCmsPagesForAdminAsync(adminToken);

    public async Task<Portfolio.Shared.Models.CmsPageDto> CreateAsync(Portfolio.Shared.Models.CmsPageDto page, string adminToken)
    {
        var (result, _) = await apiService.CreateCmsPageAsync(page, adminToken);
        return result ?? page;
    }

    public async Task UpdateAsync(Portfolio.Shared.Models.CmsPageDto page, string adminToken)
        => await apiService.UpdateCmsPageAsync(page.Id, page, adminToken);

    public async Task DeleteAsync(int id, string adminToken)
        => await apiService.DeleteCmsPageAsync(id, adminToken);
}
