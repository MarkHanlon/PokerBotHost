using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokerBotHost.Models;
using System.Threading;

namespace PokerBotHost.GameHost
{
    public class PokerGame
    {
        private PokerTable table;

        public PokerGame(PokerTable table)
        {
            this.table = table;
            Console.WriteLine("Starting new Poker Game with {0} players!", table.Players.Count());
        }

        public void Run()
        {
            // Blocks until the game is over, or forced to stop
            // TODO: Add a Cancellation Token to watch
            Thread.Sleep(5000);
        }
    }
}
