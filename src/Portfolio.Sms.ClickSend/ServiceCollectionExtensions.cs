using Microsoft.Extensions.DependencyInjection;
using Portfolio.Sms.Abstractions;

namespace Portfolio.Sms.ClickSend;

/// <summary>DI registration extensions for the ClickSend SMS provider.</summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="ClickSendSmsService"/> as the <see cref="ISmsService"/> implementation.
    /// Use this when ClickSend is the only/static provider. For admin-configurable
    /// multi-provider setups, use <c>SmsSender</c> in the host application instead.
    /// </summary>
    public static IServiceCollection AddClickSendSms(
        this IServiceCollection services,
        Action<ClickSendOptions> configure)
    {
        var options = new ClickSendOptions();
        configure(options);

        services.AddHttpClient();
        services.AddScoped<ISmsService>(sp =>
        {
            var factory = sp.GetRequiredService<IHttpClientFactory>();
            return new ClickSendSmsService(factory.CreateClient("ClickSend"), options);
        });

        return services;
    }
}
