using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackTaro.Shared.Mappers;
using TrackTaro.Shared.Dtos;
using TrackTaro.Api.Authentication;
using TrackTaro.Api.Helpers;

namespace TrackTaro.Api.Controllers;

// From huge help of https://stackoverflow.com/a/75027994
// And https://www.youtube.com/watch?v=6-FNejMrVuk
[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    public readonly IWebHostEnvironment _env;

    public FilesController(IWebHostEnvironment env)
    {
        _env = env;
    }

    [HttpPost("upload")]
    // [ApiKey]
    public async Task<IActionResult> UploadImage(IFormFile file, string uploadType)
    {
        // Check for file reqs
        string filenameResult = new UploadHandler().Upload(file);
        if (filenameResult.StartsWith("Invalid"))
        {
            return BadRequest(filenameResult); // 400 Bad Request if upload is invalid
        }

        // Determine folder
        string subfolder;
        switch (uploadType.ToLower())
        {
            case "cover":
                subfolder = "covers";
                break;
            case "back":
                subfolder = "backs";
                break;
            case "disc":
                subfolder = "discs";
                break;
            case "booklet":
                subfolder = "booklets";
                break;
            default:
                subfolder = "miscuploads";
                break;
        }

        // Path management
        string uploadsFolderPath = Path.Combine(_env.WebRootPath, "uploads", subfolder);
        if (!Directory.Exists(uploadsFolderPath)) { Directory.CreateDirectory(uploadsFolderPath); }
        string filePath = Path.Combine(uploadsFolderPath, filenameResult);

        // Save file
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        // Return public path so we can use it
        string publicPath = $"/uploads/{subfolder}/{filenameResult}";

        return Ok(new { FilePath = publicPath }); // 200 OK response with file path
    }
}