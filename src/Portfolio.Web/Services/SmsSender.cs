namespace Portfolio.Web.Services;

public sealed class SmsSender(PortfolioApiService apiService)
{
    public Task<bool> SendTestAsync(string adminToken)
        => apiService.SendTestSmsAsync(adminToken);
}
