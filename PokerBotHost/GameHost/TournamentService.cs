using Microsoft.Extensions.DependencyInjection;
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
        private Timer timer = null;
        private AutoResetEvent autoEvent = new AutoResetEvent(true);
        private static IServiceProvider _provider;

        public TournamentService(PokerTableContext context, IServiceProvider provider)
        {
            Console.WriteLine("Creating TournamentService");

            _provider = provider;
            _context = context;
        }

        public void Start()
        {
            Console.WriteLine("Starting TournamentService");
            if (timer != null)
            {
                Console.WriteLine("Warning: TournamentService already started. Now stopping...");
                Stop();
            }

            timer = new Timer(ManageTables, null, 1000, Timeout.Infinite);
            Console.WriteLine("Started TournamentService");
        }

        public void Stop()
        {
            if (!autoEvent.WaitOne(5000))
            {
                Console.WriteLine("Timed out waiting TournamentService to stop being busy. Aborting.");
            }

            // Stop the timer
            timer.Change(Timeout.Infinite, Timeout.Infinite);
            timer.Dispose();
            timer = null;
            Console.WriteLine("Stopped TournamentService");
        }

        private void ManageTables(object state)
        {
            autoEvent.Reset(); // Make sure the Stop() method blocks until we are done here
            int sleepTime = 1000;

            try
            {
                // Check if it's time to start a new tournament
                DateTime time = DateTime.UtcNow;
                if (time.Second > 1)
                {
                    // Nope, we're done
                    return;
                }

                // At the top of a minute, so clean up empty tables and start a new one
                int cleanedCount = 0;

                // Get DB context
                using (IServiceScope scope = _provider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = _provider.GetService<PokerTableContext>();

                    // Find all closed tables
                    var closedTables = context.PokerTables.Where(t => t.TableState == TableStates.Closed);
                    foreach (PokerTable t in closedTables)
                    {
                        context.PokerTables.Remove(t);
                        cleanedCount++;
                    }

                    // Find all empty Registering tables
                    var emptyTables = context.PokerTables.Where(t => t.Players.Any() == false);
                    foreach (PokerTable t in emptyTables)
                    {
                        context.PokerTables.Remove(t);
                        cleanedCount++;
                    }

                    context.SaveChanges();
                    Console.WriteLine("Cleaned {0} tables", cleanedCount);

                    Console.WriteLine("Starting new table");

                    PokerTable newTable = new PokerTable() { TableState = TableStates.Registering };
                    context.Add(newTable);
                    context.SaveChanges();
                }

                sleepTime = Math.Max(1, 55 - DateTime.UtcNow.Second) * 1000;  // Sleep from 1 to 55 seconds
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in TournamentService worker: " + ex.Message);
            }
            finally
            {
                // Restart the timer, waiting until we might be close to the next start time
                timer.Change(sleepTime, Timeout.Infinite);
                autoEvent.Set();  // Stop() method can complete now, if called
            }
                
        }

    }
}
