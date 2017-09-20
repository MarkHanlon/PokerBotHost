using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using PokerBotHost.GameHost;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace PokerBotHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            host.Start();  //.Run();  // Start without blocking

            Console.WriteLine("Starting TournamentService");

            while (true)
            {
                Thread.Sleep(5 * 1000);
            }
            
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
