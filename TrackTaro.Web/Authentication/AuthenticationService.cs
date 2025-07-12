// Given how this is done on JS applications it would be best to somehow store the auth with a session
// Found some info on how to do it with IJSRuntime (https://www.binaryintellect.net/articles/7d88d63b-768c-4e78-a934-cacac486b22a.aspx)
// We will store the key in the headers (since thats what I built the backend to expect)

using Microsoft.JSInterop;

namespace TrackTaro.Web.AuthenticationService;

public class AuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private string? _apiKey;

    public AuthenticationService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }

    // This is what our frontend will call for auth validationx
    public async Task<bool> IsAuthenticatedAsync()
    {
        if (!string.IsNullOrWhiteSpace(_apiKey)) { return true; }

        _apiKey = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "apiKey");
        if (!string.IsNullOrWhiteSpace(_apiKey))
        {
            SetUpHeader();
            return true;
        }
        return false;
    }

    // I don't like that I am hardcoding th header value here, on BE it is stored in the config
    // but perhaps this is better than storing 2 copies in 2 differen configs
    private void SetUpHeader()
    {
        // Set the API key in the HTTP client headers
        if (_httpClient.DefaultRequestHeaders.Contains("X-Api-Key"))
        {
            _httpClient.DefaultRequestHeaders.Remove("X-Api-Key");
        }
        _httpClient.DefaultRequestHeaders.Add("X-Api-Key", _apiKey);
    }

    public async Task LoginAsync(string password)
    {
        // Use our authentication contorller
        var response = await _httpClient.PostAsJsonAsync("api/authentication/token", new { Password = password });
        if (response.IsSuccessStatusCode)
        {
            // Read key from response
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            _apiKey = result?.Token;
            if (!string.IsNullOrWhiteSpace(_apiKey))
            {
                await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "apiKey", _apiKey);
                SetUpHeader();
            }
        }
        else
        {
            throw new Exception("Login failed.");
        }
    }

    public void Logout()
    {
        _apiKey = null;
        _httpClient.DefaultRequestHeaders.Remove("X-Api-Key");
        _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", "apiKey");
    }

    // Again as mentioned on the BE side maybe should be moved to a DTO
    private class AuthResponse { public string? Token { get; set; } }
}