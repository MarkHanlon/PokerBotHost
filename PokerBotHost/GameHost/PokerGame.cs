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
        private IServiceProvider _provider;

        public PokerGame(IServiceProvider provider)
        {
            _provider = provider;
            Console.WriteLine("Starting new Poker Game object...");
        }

        public void Run(long pokerTableId)
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
                var table = context.PokerTables.Include(t => t.Players).Single(t => t.Id == pokerTableId);

                // Set dealer
                table.Players[0].isDealer = true;
                context.SaveChanges();

                foreach (Player p in table.Players)
                {
                    p.chipCount = 500;
                }
                context.SaveChanges();

                while (table.Players.Count() > 1)
                {
                    // Take blinds


                }
            }
        }
    }
}
