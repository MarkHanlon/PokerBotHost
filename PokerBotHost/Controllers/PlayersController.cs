using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PokerBotHost.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PokerBotHost.Controllers
{
    [Route("api/[controller]")]
    public class PlayersController : Controller
    {
        private readonly PlayerContext _context;
        private readonly PokerTableContext _tableContext;

        public PlayersController(PlayerContext context, PokerTableContext tableContext, ToDoContext tdc)
        {
            _context = context;
            _tableContext = tableContext;
            
            if (_context.Players.Count() == 0)
            {
                _context.Players.Add(new Player() { Name = "Mark" });
                _context.SaveChanges();
            }
            
        }


        // GET: api/values
        [HttpGet]
        public object Get()
        {            
            return _context.Players.Select(p => new
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
                return _context.Players.Where(p => p.Id == id).Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Table
                }).ToList();
            }
            else
            {
                return _context.Players.SingleOrDefault(p => p.Id == id && p.Token == token);
            }
        }

        // POST api/values
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
            player.Table = _tableContext.PokerTables.Single(t => t.Id == tableId);

            _context.Players.Add(player);
            _context.SaveChanges();
            
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
