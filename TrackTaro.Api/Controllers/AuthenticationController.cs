using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace TrackTaro.Api.Controllers;

// Technically should go with other DTOs but kept here for simplicity
// Its short and if auth ever gets too serious I will move it
public class TokenRequest
{
    public string? Password { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IConfiguration _configuration;
    
    public AuthenticationController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // POST: /api/authentication/token
    [HttpPost("token")]
    public IActionResult GetToken([FromBody] TokenRequest request)
    {
        if (string.IsNullOrEmpty(request.Password)) { return BadRequest("Password is required."); }

        string? adminPassword = _configuration["Authentication:AdminPassword"];
        string? apiKey = _configuration["Authentication:ApiKey"];
        
        // Validate the passwords
        if (request.Password == adminPassword)
        {
            return Ok(new { Token = apiKey });
        }

        return Unauthorized();
    }
}