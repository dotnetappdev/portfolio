using Microsoft.Extensions.DependencyInjection;
using Portfolio.Sms.Abstractions;

namespace Portfolio.Sms.Twilio;

/// <summary>DI registration extensions for the Twilio SMS provider.</summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="TwilioSmsService"/> as the <see cref="ISmsService"/> implementation.
    /// Use this when Twilio is the only/static provider. For admin-configurable
    /// multi-provider setups, use <c>SmsSender</c> in the host application instead.
    /// </summary>
    public static IServiceCollection AddTwilioSms(
        this IServiceCollection services,
        Action<TwilioOptions> configure)
    {
        var options = new TwilioOptions();
        configure(options);

        services.AddHttpClient();
        services.AddScoped<ISmsService>(sp =>
        {
            var factory = sp.GetRequiredService<IHttpClientFactory>();
            return new TwilioSmsService(factory.CreateClient("Twilio"), options);
        });

        return services;
    }
}
