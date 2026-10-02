using System.Collections.Concurrent;
using System.Net;
using System.Text.Json;

namespace IbrahimPortfolio.WebUI.Services;

// Scoped to the current page request; safe for parallel dashboard calls.
public sealed class ApiRequestState(ILogger<ApiRequestState> logger, IHttpContextAccessor context)
{
    private readonly ConcurrentDictionary<string, byte> _failures = new();
    public bool HasErrors => !_failures.IsEmpty;
    public bool Failed(string path) => _failures.ContainsKey(path);

    public async Task<T?> GetAsync<T>(HttpClient client, string path) where T : class
    {
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, path);
            AddAdminKey(request);
            using var response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
        {
            Record(path, ex);
            return null;
        }
    }

    public async Task<bool> SendAsync(HttpClient client, HttpMethod method, string path, object? body = null)
    {
        try
        {
            using var request = new HttpRequestMessage(method, path);
            if (body != null) request.Content = JsonContent.Create(body);
            // Only authenticated admin requests may use the server-to-server credential.
            AddAdminKey(request);
            using var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode) return true;
            Record(path, new HttpRequestException($"API status: {(int)response.StatusCode}"));
            return false;
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            Record(path, ex);
            return false;
        }
    }

    private void Record(string path, Exception exception)
    {
        _failures.TryAdd(path, 0);
        logger.LogWarning(exception, "API request failed: {Path}", path);
    }

    private void AddAdminKey(HttpRequestMessage request)
    {
        if (context.HttpContext?.User.IsInRole("Admin") != true) return;
        var key = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>()["ApiSettings:AdminApiKey"];
        if (!string.IsNullOrWhiteSpace(key)) request.Headers.Add("X-Admin-Api-Key", key);
    }
}
