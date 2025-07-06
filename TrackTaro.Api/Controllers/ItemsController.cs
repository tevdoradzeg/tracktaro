using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackTaro.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

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
    public async Task<ActionResult<IEnumerable<Item>>> GetItems()
    {
        var items = await _context.Items
            .Include(item => item.Artists)
            .Include(item => item.Discs)
                .ThenInclude(disc => disc.Tracks)
            .Include(item => item.BookletImages)
            .ToListAsync();

        return Ok(items); // 200 OK response with items list
    }
}