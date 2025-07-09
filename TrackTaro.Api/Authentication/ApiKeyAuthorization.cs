using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TrackTaro.Api.Authentication;

// This section was done under guidence from https://www.youtube.com/watch?v=GrJJXixjR8M

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAttribute : Attribute, IAsyncAuthorizationFilter
{
    private const string ApiKeyHeaderName = "X-Api-Key";
    private const string ApiKeySectionName = "Authentication:ApiKey";

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var apiKey = config.GetValue<string>(ApiKeySectionName);

        if (string.IsNullOrEmpty(apiKey))
        {
            context.Result = new UnauthorizedObjectResult("API Key missing");
            return;
        }

        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var providedApiKey) || providedApiKey != apiKey)
        {
            context.Result = new UnauthorizedObjectResult("Invalid API Key");
            return;
        }

        await Task.CompletedTask; // Simulate async operation
    }
}