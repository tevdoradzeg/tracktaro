using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackTaro.Shared.Mappers;
using TrackTaro.Shared.Dtos;
using TrackTaro.Shared;
using TrackTaro.Api.Authentication;

namespace TrackTaro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly MusicDbContext _context;

    public ItemsController(MusicDbContext context)
    {
        _context = context;
    }

    // GET: api/items
    // Endpoint for searching with parameters
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ItemShortDto>>> GetItems(
        [FromQuery] string? name,
        [FromQuery] string? timeAcquired,
        [FromQuery] string? artistName
    )
    {
        // Build the query
        IQueryable<Item> query = _context.Items.AsQueryable();

        // Apply filter base on item name
        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(item => item.Name.ToLower().Contains(name.ToLower()));
        }

        // A more verbose filter to filter items by time acquired
        if (!string.IsNullOrWhiteSpace(timeAcquired))
        {
            DateTime cutOff = DateTime.UtcNow;
            bool dateFilterValid = true;

            switch (timeAcquired.ToLower())
            {
                case "last_day":
                    cutOff = DateTime.UtcNow.AddDays(-1);
                    break;
                case "last_week":
                    cutOff = DateTime.UtcNow.AddDays(-7);
                    break;
                case "last_month":
                    cutOff = DateTime.UtcNow.AddMonths(-1);
                    break;
                case "last_year":
                    cutOff = DateTime.UtcNow.AddYears(-1);
                    break;
                default:
                    dateFilterValid = false;
                    break;
            }

            if (dateFilterValid) { query = query.Where(item => item.CreatedAt >= cutOff); }
        }

        // Filter items by artist name
        if (!string.IsNullOrWhiteSpace(artistName))
        {
            query = query.Where(item => item.Artists
                .Any(artist => artist.Name.ToLower().Contains(artistName.ToLower())));
        }

        // Get items from the database
        var itemsDb = await query
            .Include(item => item.Artists)
            .ToListAsync();

        // Make them readable, return only things to display / use on FE
        var items = itemsDb.Select(item => item.ToShortDto()).ToList();

        return Ok(items); // 200 OK response with items list
    }

    // GET: api/items/{id}
    // Endpoint for getting a single item by ID
    [HttpGet("{id}")]
    public async Task<ActionResult<ItemDto>> GetItem(int id)
    {
        Item? item = await _context.Items
            .Include(i => i.Artists)
            .Include(i => i.Discs)
                .ThenInclude(d => d.Tracks)
            .Include(i => i.BookletImages)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (item == null) { return NotFound(); } // 404 Not Found if item does not exist

        return Ok(item.ToDto()); // 200 OK response with item details
    }

    // POST: api/items
    // Endpoint for creating a new item
    [HttpPost]
    // [ApiKey]
    public async Task<ActionResult<ItemDto>> CreateItem([FromBody] CreateItemDto itemDto)
    {
        if (itemDto == null) { return BadRequest("Item data is required."); } // 400 Bad Request if item data is null

        // Item object
        Item newItem = new Item
        {
            Name = itemDto.Name,
            Year = itemDto.Year,
            Description = itemDto.Description,
            Publisher = itemDto.Publisher,
            Label = itemDto.Label,
            Type = itemDto.Type,
            CoverImagePath = itemDto.CoverImagePath,
            BackImagePath = itemDto.BackImagePath,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            BookletImages = itemDto.BookletImagePaths
                .Select(path => new BookletImage { ImagePath = path })
                .ToList()
        };

        // Disc objects
        foreach (CreateDiscDto discDto in itemDto.Discs)
        {
            if (discDto.Type == DiscType.CD)
            {
                newItem.Discs.Add(new CDDisc
                {
                    Number = discDto.Number,
                    DiscImagePath = discDto.DiscImagePath,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Tracks = discDto.Tracks.Select(trackDto => new Track
                    {
                        Name = trackDto.Name,
                        Duration = trackDto.Duration,
                        TrackNumber = trackDto.TrackNumber,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    }).ToList()
                });
            }
            else if (discDto.Type == DiscType.Vinyl)
            {
                newItem.Discs.Add(new VinylDisc
                {
                    Number = discDto.Number,
                    DiscImagePath = discDto.DiscImagePath,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Tracks = discDto.Tracks.Select(trackDto => new Track
                    {
                        Name = trackDto.Name,
                        Duration = trackDto.Duration,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    }).ToList()
                });
            }
        }

        _context.Items.Add(newItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetItem), new { id = newItem.Id }, newItem.ToDto()); // 201 Created response with the created item
    }

    // POST: api/items/{itemId}/artists
    [HttpPost("{itemId}/artists")]
    // [ApiKey]
    public async Task<IActionResult> LinkArtistToItem(int itemId, [FromBody] ArtistToItemDto artistToItemDto)
    {
        Item? item = await _context.Items
            .Include(i => i.Artists)
            .FirstOrDefaultAsync(i => i.Id == itemId);

        if (item == null) { return NotFound("Item not found."); } // 404 Not Found if item does not exist

        Artist? artist = await _context.Artists
            .FirstOrDefaultAsync(a => a.Id == artistToItemDto.ArtistId);
        if (artist == null) { return NotFound("Artist not found."); } // 404 Not Found if artist does not exist

        if (item.Artists.Any(a => a.Id == artist.Id))
        {
            return BadRequest("Artist is already linked to this item."); // 400 Bad Request if artist is already linked
        }

        item.Artists.Add(artist);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Artist linked to item successfully." }); // 200 OK response
    }

    // DELETE: api/items/{itemId}/artists/
    [HttpDelete("{itemId}/artists")]
    // [ApiKey]
    public async Task<IActionResult> UnlinkArtistFromItem(int itemId, [FromBody] ArtistToItemDto artistToItemDto)
    {
        Item? item = await _context.Items
            .Include(i => i.Artists)
            .FirstOrDefaultAsync(i => i.Id == itemId);

        if (item == null) { return NotFound("Item not found."); } // 404 Not Found if item does not exist

        Artist? artist = await _context.Artists
            .FirstOrDefaultAsync(a => a.Id == artistToItemDto.ArtistId);
        if (artist == null) { return NotFound("Artist not found."); } // 404 Not Found if artist does not exist

        if (!item.Artists.Any(a => a.Id == artist.Id))
        {
            return BadRequest("Artist is not linked to this item."); // 400 Bad Request if artist is not linked
        }

        item.Artists.Remove(artist);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Artist unlinked from item successfully." }); // 200 OK response
    }

    // DELETE: api/items/{itemId}
    [HttpDelete("{itemId}")]
    // [ApiKey]
    public async Task<IActionResult> DeleteItem(int itemId)
    {
        Item? item = await _context.Items
            .Include(i => i.Artists)
            .Include(i => i.Discs)
                .ThenInclude(d => d.Tracks)
            .Include(i => i.BookletImages)
            .FirstOrDefaultAsync(i => i.Id == itemId);

        if (item == null) { return NotFound("Item not found."); } // 404 Not Found if item does not exist

        _context.Items.Remove(item);
        await _context.SaveChangesAsync();

        return NoContent(); // 204 No Content response
    }

    // PUT: api/items/{itemId}
    // Endpoint for updating an existing item
    [HttpPut("{itemId}")]
    // [ApiKey]
    public async Task<IActionResult> UpdateItem(int itemId, [FromBody] UpdateItemDto itemDto)
    {
        var itemToUpdate = await _context.Items
            .Include(i => i.Artists)
            .Include(i => i.BookletImages)
            .Include(i => i.Discs)
                .ThenInclude(d => d.Tracks)
            .FirstOrDefaultAsync(i => i.Id == itemId);

        if (itemToUpdate == null) { return NotFound("Item not found."); } // 404 Not Found if item does not exist

        itemToUpdate.Name = itemDto.Name;
        itemToUpdate.Year = itemDto.Year;
        itemToUpdate.Description = itemDto.Description;
        itemToUpdate.Publisher = itemDto.Publisher;
        itemToUpdate.Label = itemDto.Label;
        itemToUpdate.Type = itemDto.Type;
        itemToUpdate.CoverImagePath = itemDto.CoverImagePath;
        itemToUpdate.BackImagePath = itemDto.BackImagePath;
        itemToUpdate.UpdatedAt = DateTime.UtcNow;

        // Deal with artists
        var existingArtistIds = itemToUpdate.Artists.Select(a => a.Id).ToList();
        var incomingArtistIds = itemDto.ArtistIds.ToList();

        Console.WriteLine($"Existing Artist IDs: {string.Join(", ", existingArtistIds)}");
        Console.WriteLine($"Incoming Artist IDs: {string.Join(", ", incomingArtistIds)}");

        var artistsToRemove = itemToUpdate.Artists
            .Where(a => !incomingArtistIds.Contains(a.Id))
            .ToList();

        var artistIdsToAdd = incomingArtistIds.Except(existingArtistIds).ToList();
        var artistsToAdd = await _context.Artists
            .Where(a => artistIdsToAdd.Contains(a.Id))
            .ToListAsync();

        foreach (var artist in artistsToRemove)
        {
            itemToUpdate.Artists.Remove(artist);
        }
        foreach (var artist in artistsToAdd)
        {
            itemToUpdate.Artists.Add(artist);
        }

        // Sort through discs
        var incomingDiscIds = itemDto.Discs.Select(d => d.Id).ToList();
        var discsToRemove = itemToUpdate.Discs
            .Where(d => !incomingDiscIds.Contains(d.Id))
            .ToList();

        _context.Discs.RemoveRange(discsToRemove); // Remove discs that are extra now

        foreach (var discDto in itemDto.Discs)
        {
            var existingDisc = itemToUpdate.Discs.FirstOrDefault(d => d.Id == discDto.Id);
            if (existingDisc != null)
            {
                existingDisc.Number = discDto.Number;
                existingDisc.DiscImagePath = discDto.DiscImagePath;

                var incomingTrackIds = discDto.Tracks.Select(t => t.Id).ToList();
                var tracksToRemove = existingDisc.Tracks
                    .Where(t => !incomingTrackIds.Contains(t.Id))
                    .ToList();

                _context.Tracks.RemoveRange(tracksToRemove); // Remove tracks that are extra now (same as discs)
                foreach (var trackDto in discDto.Tracks)
                {
                    var existingTrack = existingDisc.Tracks.FirstOrDefault(t => t.Id == trackDto.Id);
                    if (existingTrack != null)
                    {
                        existingTrack.Name = trackDto.Name;
                        existingTrack.Duration = trackDto.Duration;
                        existingTrack.TrackNumber = trackDto.TrackNumber;
                        existingTrack.UpdatedAt = DateTime.UtcNow;
                    }
                    else
                    {
                        // Add new track if it doesn't exist
                        existingDisc.Tracks.Add(new Track
                        {
                            Name = trackDto.Name,
                            Duration = trackDto.Duration,
                            TrackNumber = trackDto.TrackNumber,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }
            }
            else
            {
                // Add new disc if it doesn't exist
                if (discDto.Type == DiscType.CD)
                {
                    itemToUpdate.Discs.Add(new CDDisc
                    {
                        Number = discDto.Number,
                        DiscImagePath = discDto.DiscImagePath,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        Tracks = discDto.Tracks.Select(trackDto => new Track
                        {
                            Name = trackDto.Name,
                            Duration = trackDto.Duration,
                            TrackNumber = trackDto.TrackNumber,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        }).ToList()
                    });
                }
                else if (discDto.Type == DiscType.Vinyl)
                {
                    itemToUpdate.Discs.Add(new VinylDisc
                    {
                        Number = discDto.Number,
                        DiscImagePath = discDto.DiscImagePath,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        Tracks = discDto.Tracks.Select(trackDto => new Track
                        {
                            Name = trackDto.Name,
                            Duration = trackDto.Duration,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        }).ToList()
                    });
                }
            }
        }

        // Update booklet images
        itemToUpdate.BookletImages = itemDto.BookletImagePaths
                .Select(path => new BookletImage { ImagePath = path })
                .ToList();

        await _context.SaveChangesAsync();
        return Ok(itemToUpdate.ToDto()); // 200 OK response with updated item
    }
}