using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PokerBotHost.Models;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PokerBotHost.Controllers
{
    [Route("api/[controller]")]
    public class PlayersController : Controller
    {
        //private readonly PlayerContext _playerContext;
        private readonly PokerTableContext _tableContext;

        public PlayersController(PokerTableContext tableContext)
        {
            //_playerContext = playerContext;
            _tableContext = tableContext;                        
        }


        // GET: api/players
        [HttpGet]
        public object Get()
        {
            var allPlayers = _tableContext.Players.Include(t => t.Table.Players);
            return allPlayers.Select(p => new
            {
                p.Id,
                p.Name,
                p.Table
            }).ToList();
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetPlayer")]
        public object Get(long id, Guid token)
        {
            if (token == null)
            {
                return _tableContext.Players.Where(p => p.Id == id).Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Table
                }).ToList();
            }
            else
            {
                return _tableContext.Players.SingleOrDefault(p => p.Id == id && p.Token == token);
            }
        }

        // POST api/players?tableId={tableId}
        [HttpPost]
        public IActionResult Post([FromBody]Player player, int tableId)
        {
            if (player == null || tableId == 0)
            {
                return BadRequest();
            }

            if (!_tableContext.PokerTables.Any( t => t.Id == tableId))
            {
                return BadRequest();    // This table Id doesn't exist
            }
            
            player.Token = Guid.NewGuid();
            player.TableId = tableId; //_tableContext.PokerTables.Single(t => t.Id == tableId);

            //_playerContext.Players.Add(player);
            //_playerContext.SaveChanges();
            _tableContext.Players.Add(player);
            _tableContext.SaveChanges();

            return CreatedAtRoute("GetPlayer", new { id = player.Id }, player);

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
