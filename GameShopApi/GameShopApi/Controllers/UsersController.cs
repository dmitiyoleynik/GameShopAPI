using GameShopApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameShopApi.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GameShopApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController: ControllerBase
    {
        GameShopContext db;

        public UsersController(GameShopContext context)
        {
            db = context;
        }

        [HttpPut]
        public async Task<ActionResult> CorrectUserData(ShopUser shopUser)
        {
            var nameIndefiter = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);



            ShopUser user = await db.Users.FirstOrDefaultAsync(u => u.Login == shopUser.Login);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                shopUser.Id = user.Id;
                db.Update(shopUser);
                await db.SaveChangesAsync();

                return NoContent();
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteUser(string Login)
        {
            ShopUser user = await db.Users.FirstOrDefaultAsync(u=>u.Login == Login);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                db.Users.Remove(user);
                await db.SaveChangesAsync();

                return NoContent();
            }
        }
    }
}
