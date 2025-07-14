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
    private readonly IHttpClientFactory _httpClientFactory;

    public ArtistsController(MusicDbContext context, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
    }

    // GET: api/artists
    // Endpoint for searching with parameters
    [HttpGet]
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
                .ThenInclude(i => i.Discs)
                    .ThenInclude(d => d.Tracks)
            .Include(a => a.Members)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (artist == null)
        {
            return NotFound(); // Return 404 Not Found if the artist does not exist
        }

        return Ok(artist.ToDto()); // Return the artist details as DTO
    }

    // POST: api/artists
    [HttpPost]
    public async Task<ActionResult<ArtistDto>> CreateArtist([FromBody] CreateArtistDto artistDto)
    {
        if (artistDto == null) { return BadRequest("Artist data is required."); }

        Artist newArtist = new Artist
        {
            Name = artistDto.Name,
            Country = artistDto.Country,
            Members = artistDto.Members.Select(name => new Member { Name = name }).ToList()
        };

        _context.Artists.Add(newArtist);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetArtist), new { id = newArtist.Id }, newArtist.ToDto()); // 201 for success
    }

    // GET: api/artists/search-external
    // Endpoint for searching artists in MusicBrainz
    [HttpGet("search-external")]
    public async Task<IActionResult> SearchMusicBrainz([FromQuery] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("Artist name is required.");
        }

        HttpClient client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("User-Agent", "TrackTaro/1.0 ( tevdoradzege@gmail.com )");

        string ulr = $"https://musicbrainz.org/ws/2/artist/?query=artist:{Uri.EscapeDataString(name)}&fmt=json";
        try
        {
            HttpResponseMessage response = await client.GetAsync(ulr);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<MusicBrainzSearchResult>();

                // Empty list if no artists found
                if (result?.Artists == null)
                {
                    return Ok(new List<ArtistSuggestionDto>());
                }

                var suggestions = result.Artists.Select(artist => new ArtistSuggestionDto
                {
                    Name = artist.Name,
                    Country = artist.Area?.Name ?? artist.CountryCode ?? "N/A",
                    Disambiguation = artist.Disambiguation ?? ""
                }).ToList();

                return Ok(suggestions);
            }
            return StatusCode((int)response.StatusCode, "Error fetching data from MusicBrainz.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // DELETE: api/artists/{id}
    [HttpDelete("{id}")]
    // [ApiKey]
    public async Task<IActionResult> DeleteArtist(int id)
    {
        var artist = await _context.Artists
            .Include(a => a.Items)
                .ThenInclude(i => i.Discs)
                    .ThenInclude(d => d.Tracks)
            .Include(a => a.Members)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (artist == null)
        {
            return NotFound("Artist not found.");
        }

        _context.Artists.Remove(artist);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}