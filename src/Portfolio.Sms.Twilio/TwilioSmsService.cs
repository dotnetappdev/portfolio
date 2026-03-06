using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Portfolio.Sms.Abstractions;

namespace Portfolio.Sms.Twilio;

/// <summary>
/// Sends SMS messages via the Twilio Programmable Messaging REST API.
/// Docs: https://www.twilio.com/docs/messaging/api/message-resource#create-a-message-resource
/// </summary>
public sealed class TwilioSmsService : ISmsService
{
    // Base URL template — AccountSid is substituted at runtime
    private const string UrlTemplate =
        "https://api.twilio.com/2010-04-01/Accounts/{0}/Messages.json";

    private readonly HttpClient _http;
    private readonly TwilioOptions _options;

    public TwilioSmsService(HttpClient http, TwilioOptions options)
    {
        _http = http;
        _options = options;
    }

    /// <inheritdoc />
    public async Task<SmsResult> SendAsync(SmsMessage message, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_options.AccountSid))
            return SmsResult.Fail("Twilio Account SID is not configured.");
        if (string.IsNullOrWhiteSpace(_options.AuthToken))
            return SmsResult.Fail("Twilio Auth Token is not configured.");
        if (string.IsNullOrWhiteSpace(_options.From))
            return SmsResult.Fail("Twilio From number is not configured.");

        var from = string.IsNullOrWhiteSpace(message.From) ? _options.From : message.From;
        var url = string.Format(UrlTemplate, Uri.EscapeDataString(_options.AccountSid));

        var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["To"]   = message.To,
            ["From"] = from,
            ["Body"] = message.Body
        });

        var credentials = Convert.ToBase64String(
            Encoding.ASCII.GetBytes($"{_options.AccountSid}:{_options.AuthToken}"));

        var request = new HttpRequestMessage(HttpMethod.Post, url) { Content = formContent };
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
            return SmsResult.Fail($"Twilio error {(int)response.StatusCode}: {body}");

        try
        {
            using var doc = JsonDocument.Parse(body);
            var sid = doc.RootElement.GetProperty("sid").GetString();
            return SmsResult.Ok(sid);
        }
        catch
        {
            return SmsResult.Ok();
        }
    }
}
