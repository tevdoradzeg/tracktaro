using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackTaro.Shared;
using TrackTaro.Api.Authentication;
using TrackTaro.Shared.Mappers;

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
    [HttpGet]
    [ApiKey]
    public async Task<ActionResult<IEnumerable<Item>>> GetItems(
        [FromQuery] string? name,
        [FromQuery] string? timeAcquired,
        [FromQuery] string? artistName
    )
    {
        // Build the query
        var query = _context.Items.AsQueryable();

        // Apply filter base on item name
        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(item => item.Name.ToLower().Contains(name.ToLower()));
        }

        // A more verbose filter to filter items by time acquired
        if (!string.IsNullOrWhiteSpace(timeAcquired))
        {
            var cutOff = DateTime.UtcNow;
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
            .Include(item => item.Discs)
                .ThenInclude(disc => disc.Tracks)
            .Include(item => item.BookletImages)
            .ToListAsync();

        // Make them readable, return only things to display / use on FE
        var items = itemsDb.Select(item => item.ToDto()).ToList();

        return Ok(items); // 200 OK response with items list
    }
}