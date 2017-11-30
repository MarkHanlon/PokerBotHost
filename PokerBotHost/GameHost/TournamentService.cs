using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PokerBotHost.Models;
using System;
using System.Collections.Concurrent;
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
        private Timer timer = null;
        private AutoResetEvent autoEvent = new AutoResetEvent(true);
        private static IServiceProvider _provider;
        ConcurrentBag<Thread> gameThreads = new ConcurrentBag<Thread>();

        public TournamentService(IServiceProvider provider)
        {
            Console.WriteLine("Creating TournamentService");

            _provider = provider;
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

                // At the top of a minute, so clean up empty tables, start a new one and turn registering
                // tables into game playing tables if there are any players...
                int cleanedCount = 0;

                // Get DB context
                using (IServiceScope scope = _provider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    //var context = _provider.GetService<PokerTableContext>();                    
                    var context = scope.ServiceProvider.GetService<PokerTableContext>();

                    // Find all closed tables
                    var closedTables = context.PokerTables.Where(t => t.TableState == TableStates.Closed);
                    foreach (PokerTable t in closedTables)
                    {
                        context.PokerTables.Remove(t);
                        cleanedCount++;
                    }

                    // Find all empty tables
                    var emptyTables = context.PokerTables.Where(t => t.Players.Any() == false);
                    foreach (PokerTable t in emptyTables)
                    {
                        context.PokerTables.Remove(t);
                        cleanedCount++;
                    }

                    context.SaveChanges();
                    Console.WriteLine("Cleaned {0} tables", cleanedCount);

                    // Any registering table with players can now start
                    // Note: There should be at most one!
                    var registeringTable = context.PokerTables.Include(t => t.Players).SingleOrDefault(t => t.TableState == TableStates.Registering);
                    if (registeringTable != null)
                    {
                        if (registeringTable.Players == null || registeringTable.Players.Count() < 2)
                        {
                            Console.WriteLine("Cleaning registering table with {0} players", registeringTable.Players.Count());
                            context.PokerTables.Remove(registeringTable);
                            context.SaveChanges();
                        }
                        else
                        {
                            // Start this table playing
                            registeringTable.TableState = TableStates.Playing;
                            Console.WriteLine("Starting table {0} with {1} players", registeringTable.Id, registeringTable.Players.Count());
                            context.SaveChanges();
                            Thread pokerGameThread = new Thread(StartPokerGame);
                            gameThreads.Add(pokerGameThread);
                            pokerGameThread.Start(registeringTable);
                        }
                    }

                    Console.WriteLine("Creating new table");
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

        private void StartPokerGame(object gameTable)
        {
            PokerTable table = (PokerTable)gameTable;

            PokerGame game = _provider.GetService<PokerGame>();
            game.Run(table.Id); // blocks until the game is complete
        }

    }
}
