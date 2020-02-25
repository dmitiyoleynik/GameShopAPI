using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameShopApi.Models
{
    public class Game
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Genre { get; set; }

        public int Price { get; set; }

        public string Description { get; set; }

        //picture

    }
}
