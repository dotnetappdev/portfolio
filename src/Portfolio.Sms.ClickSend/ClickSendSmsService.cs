using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Portfolio.Sms.Abstractions;

namespace Portfolio.Sms.ClickSend;

/// <summary>
/// Sends SMS messages via the ClickSend REST API v3.
/// Docs: https://developers.clicksend.com/docs/rest/v3/?csharp#send-sms
/// </summary>
public sealed class ClickSendSmsService : ISmsService
{
    private const string BaseUrl = "https://rest.clicksend.com/v3/sms/send";

    private readonly HttpClient _http;
    private readonly ClickSendOptions _options;

    public ClickSendSmsService(HttpClient http, ClickSendOptions options)
    {
        _http = http;
        _options = options;
    }

    /// <inheritdoc />
    public async Task<SmsResult> SendAsync(SmsMessage message, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_options.Username))
            return SmsResult.Fail("ClickSend username is not configured.");
        if (string.IsNullOrWhiteSpace(_options.ApiKey))
            return SmsResult.Fail("ClickSend API key is not configured.");

        var from = string.IsNullOrWhiteSpace(message.From)
            ? (string.IsNullOrWhiteSpace(_options.From) ? "Portfolio" : _options.From)
            : message.From;

        var payload = new
        {
            messages = new[]
            {
                new { to = message.To, body = message.Body, from }
            }
        };

        var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl)
        {
            Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
        };

        var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_options.Username}:{_options.ApiKey}"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);

        HttpResponseMessage response;
        try
        {
            response = await _http.SendAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            return SmsResult.Fail($"HTTP error: {ex.Message}");
        }

        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
            return SmsResult.Fail($"ClickSend error {(int)response.StatusCode}: {body}");

        // Parse the message_id from the first item in the response
        try
        {
            using var doc = JsonDocument.Parse(body);
            var messageId = doc.RootElement
                .GetProperty("data")
                .GetProperty("messages")[0]
                .GetProperty("message_id")
                .GetString();
            return SmsResult.Ok(messageId);
        }
        catch
        {
            return SmsResult.Ok();
        }
    }
}
