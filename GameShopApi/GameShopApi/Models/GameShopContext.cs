using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameShopApi.Models
{
    public class GameShopContext :DbContext
    {
        public DbSet<Game> Games { get; set; }

        public GameShopContext(DbContextOptions<GameShopContext> options)
            : base(options)
        {

        }
    }
}
