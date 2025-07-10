using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackTaro.Shared.Mappers;
using TrackTaro.Shared.Dtos;
using TrackTaro.Shared;
using TrackTaro.Api.Authentication;

namespace TrackTaro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArtistsController : ControllerBase
{
    private readonly MusicDbContext _context;

    public ArtistsController(MusicDbContext context)
    {
        _context = context;
    }

    // GET: api/artists
    // Endpoint for searching with parameters
    [HttpGet]
    // [ApiKey]
    public async Task<ActionResult<IEnumerable<ArtistDto>>> GetArtists(
        [FromQuery] string? name,
        [FromQuery] string? country,
        [FromQuery] string? member
    )
    {
        // Build the query
        IQueryable<Artist> query = _context.Artists.AsQueryable();

        // Apply filter based on artist name
        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(artist => artist.Name.ToLower().Contains(name.ToLower()));
        }

        // Apply filter based on artist country
        if (!string.IsNullOrWhiteSpace(country))
        {
            query = query.Where(artist => artist.Country.ToLower().Contains(country.ToLower()));
        }

        // Apply filter based on member name
        if (!string.IsNullOrWhiteSpace(member))
        {
            query = query.Where(artist => artist.Members.Any(m => m.Name.ToLower().Contains(member.ToLower())));
        }

        var artistsDb = await query
            .Include(artist => artist.Items)
            .Include(artist => artist.Members)
            .ToListAsync();

        var artists = artistsDb.Select(artist => artist.ToDto()).ToList();
        return Ok(artists); // Return a 200 OK response with the list of artists
    }

    // GET: api/artists/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ArtistDto>> GetArtist(int id)
    {
        var artist = await _context.Artists
            .Include(a => a.Items)
            .Include(a => a.Members)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (artist == null)
        {
            return NotFound(); // Return 404 Not Found if the artist does not exist
        }

        return Ok(artist.ToDto()); // Return the artist details as DTO
    }
}