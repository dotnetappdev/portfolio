namespace Portfolio.Web.Services;

public sealed class EmailSender(PortfolioApiService apiService)
{
    public Task<bool> SendTestAsync(string to, string adminToken)
        => apiService.SendTestEmailAsync(to, adminToken);
}
