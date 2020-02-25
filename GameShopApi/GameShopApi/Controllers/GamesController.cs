using GameShopApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameShopApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        GameShopContext db;

        public GamesController(GameShopContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Game>> GetGamesList()
        {
            return await db.Games.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetGame(int id)
        {
            Game game = await db.Games.FirstOrDefaultAsync(game => game.Id == id);

            if (game == null)
            {
                return NotFound();
            }
            else
            {
                return new JsonResult(game);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddGame(Game game)
        {
            if (game == null)
            {
                return BadRequest();
            }
            else
            {
                db.Games.Add(game);
                await db.SaveChangesAsync();
                return Created(new Uri("https://localhost:44388/api/games"), game);
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateGame(Game game)
        {
            if (game == null)
            {
                return BadRequest();
            }
            if (await db.Games.AnyAsync(g => g.Id == game.Id))
            {
                db.Update(game);
                await db.SaveChangesAsync();
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveGame(int id)
        {
            Game game = await db.Games.FirstOrDefaultAsync(game => game.Id == id);
            if (game == null)
            {
                return NotFound();
            }
            else
            {
                db.Games.Remove(game);
                await db.SaveChangesAsync();
                return NoContent();
            }
        }
    }
}
