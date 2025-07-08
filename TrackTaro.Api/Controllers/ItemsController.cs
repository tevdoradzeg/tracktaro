using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackTaro.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackTaro.Shared.Dtos;
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
    public async Task<ActionResult<IEnumerable<Item>>> GetItems()
    {
        // Get items from the database
        var itemsDb = await _context.Items
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