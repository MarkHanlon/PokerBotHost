using PokerBotHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PokerBotHost.GameHost
{
    /// <summary>
    /// Responsible for creating new tables for players to register on
    /// </summary>
    public class TournamentService
    {
        private readonly PokerTableContext _context;

        public TournamentService(PokerTableContext context)
        {
            //var allTables = context.PokerTables.AsEnumerable();
            Console.WriteLine("Creating TournamentService");

            _context = context;
        }

        public void Run()
        {
            Console.WriteLine("Starting TournamentService");
            bool running = true;
            while (running)
            {
                // Wait until the start of the next tournament, based
                // on real clock time
                DateTime t = DateTime.UtcNow;
                if (t.Second < 2)
                {
                    // At the top of a minute, so start a new table
                    Console.WriteLine("Starting new table");

                    PokerTable newTable = new PokerTable() { TableState = TableStates.Registering };
                    _context.Add(newTable);
                    _context.SaveChanges();

                    Thread.Sleep(2000);
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }

    }
}
