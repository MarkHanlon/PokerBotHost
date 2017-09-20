using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokerBotHost.Models
{
    public class PokerTableContext : DbContext
    {
        public PokerTableContext(DbContextOptions<PokerTableContext> options)
            : base(options)
        {
        }

        public DbSet<PokerTable> PokerTables { get; set; }
    }
}
