using GameShopApi.Models;
using GameShopApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GameShopApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AccountController : ControllerBase
    {
        GameShopContext db;
        public AccountController(GameShopContext context)
        {
            db = context;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/api/[controller]/singin")]
        public async Task<ActionResult> Signin(AuthenticationRequest authRequest, [FromServices] IJwtSigningEncodingKey signingEncodingKey)
        {
            //checkUser
            bool isUserExists = await db.Users.AnyAsync(u => u.Login == authRequest.Login && u.Password == authRequest.Password);
            if (!isUserExists)
            {
                return Unauthorized();
            }
            //create clame
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier,authRequest.Login) 
            };

            //create JWT
            var token = new JwtSecurityToken(
                issuer:"DemoApp",
                audience: "DemoAppClient",
                claims: claims,
                expires:DateTime.Now.AddMinutes(10),
                signingCredentials: new SigningCredentials(
                    signingEncodingKey.GetKey(),
                    signingEncodingKey.SigningAlgorithm)
                );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return new JsonResult(jwtToken);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/api/[controller]/Registration")]
        public async Task<ActionResult> Registration(AuthenticationRequest authRequest)
        {
            bool isUserExists = await db.Users.AnyAsync(u => u.Login == authRequest.Login);

            if (isUserExists)
            {
                return Conflict();
            }
            else
            {
                return Ok();
            }
        }
        //delete user

    }
}
