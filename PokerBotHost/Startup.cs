using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using PokerBotHost.Models;
using PokerBotHost.GameHost;

namespace PokerBotHost
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {            
            services.AddDbContext<PokerTableContext>(opt => opt.UseInMemoryDatabase("PokerTable"));
            //services.AddDbContext<PlayerContext>(opt => opt.UseInMemoryDatabase("Players"));

            // My TournamentService background worker. Register it for DI
            services.AddScoped<TournamentService>();

            services.AddMvc();

            //services.AddLogging();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, TournamentService tournamentService, PokerTableContext tableContext)
        {
            app.UseMvc();

            // Set up some test data
            AddTestData(tableContext);

            tournamentService.Start();
        }

        private static void AddTestData(PokerTableContext context)
        {
            var table = new PokerTable
            {
                TableState = TableStates.Registering
            };

            context.PokerTables.Add(table);
            context.SaveChanges();

            var player = new Player
            {
                Name = "Player1",
                Token = Guid.NewGuid(),
                HoleCard1 = "As",
                HoleCard2 = "Ad",
                TableId = table.Id
            };

            context.Players.Add(player);
            context.SaveChanges();


        }
    }
}
