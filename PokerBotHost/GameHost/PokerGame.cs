using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokerBotHost.Models;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace PokerBotHost.GameHost
{
    public class PokerGame
    {
        private PokerTable _table;
        private IServiceProvider _provider;

        public PokerGame(PokerTable table, IServiceProvider provider)
        {
            this._table = table;
            this._provider = provider;
            Console.WriteLine("Starting new Poker Game with {0} players!", table.Players.Count());
        }

        public void Run()
        {
            // Blocks until the game is over, or forced to stop
            // TODO: Add a Cancellation Token to watch
            // TODO: Implement State Machine here for game of poker

            // Allocate chips to players
            using (IServiceScope scope = _provider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                //var context = _provider.GetService<PokerTableContext>();                    
                var context = scope.ServiceProvider.GetService<PokerTableContext>();

                // Get this table
                var table = context.PokerTables.Include(t => t.Players).Single(t => t.Id == _table.Id);

                // Set dealer
                table.Players[0].isDealer = true;
                context.SaveChanges();

                foreach (Player p in table.Players)
                {
                    p.chipCount = 500;
                }
                context.SaveChanges();

                while (_table.Players.Count() > 1)
                {
                    // Take blinds


                }
            }
        }
    }
}
