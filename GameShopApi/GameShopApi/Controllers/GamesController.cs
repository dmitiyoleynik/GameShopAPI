using GameShopApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<Game>> GetGamesList()
        {
            return await db.Games.ToListAsync();
        }

        [AllowAnonymous]
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
            if (!isAdmin(HttpContext.User.Claims))
            {
                return Forbid();
            }

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
            if (!isAdmin(HttpContext.User.Claims))
            {
                return Forbid();
            }

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
            if (!isAdmin(HttpContext.User.Claims))
            {
                return Forbid();
            }

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
   
        private bool isAdmin(IEnumerable<Claim> claims)
        {
            string userRole = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;

            return userRole == UserRole.Admin.ToString();
        }
    }
}
