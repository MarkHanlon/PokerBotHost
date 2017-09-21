using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokerBotHost.Models
{
    public class PlayerContext : DbContext
    {
        public PlayerContext(DbContextOptions<PlayerContext> options)
            : base(options)
        {
            Console.WriteLine("PlayerContext constructed");
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<PokerTable> Tables { get; set; }
    }
}
