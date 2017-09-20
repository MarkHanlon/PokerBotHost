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
    public class TablesController : Controller
    {
        private readonly PokerTableContext _context;

        public TablesController(PokerTableContext context)
        {
            _context = context;

            if (_context.PokerTables.Count() == 0)
            {
                _context.PokerTables.Add(new PokerTable() { TableState = TableStates.Registering });
                _context.SaveChanges();
            }

        }


        // GET: api/tables
        [HttpGet]
        public IEnumerable<PokerTable> Get()
        {
            return _context.PokerTables.ToList();
        }

        // GET api/tables/5
        [HttpGet("{id}", Name = "GetPokerTable")]
        public IActionResult GetById(long id)
        {
            var item = _context.PokerTables.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        // POST api/tables ** Not allowed by normal user **
        [HttpPost]
        public IActionResult Create([FromBody] PokerTable item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _context.PokerTables.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetPokerTable", new { id = item.Id }, item);
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
